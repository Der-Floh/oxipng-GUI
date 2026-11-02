using System.ComponentModel;

using OxipngGui.Models;
using OxipngGui.Services;
using OxipngGui.Utils;

namespace OxipngGui.Forms;

public sealed partial class MainForm : Form
{
    private const string PngFileFilter = "PNG images (*.png; *.apng)|*.png;*.apng|All files (*.*)|*.*";
    private const string VcRedistDownloadUrl = "https://aka.ms/vc14/vc_redist.x64.exe";
    private const string OptimizeText = "OPTIMIZE";
    private const string StopText = "STOP";
    private const string GetStartedText = "Add PNG files to get started.";
    private const string CommandLineFlagsText = "Also passed to oxipng, from the command line: ";
    private const string SignatureSeparator = "|";
    private const int MinFileColumnWidth = 150;

    private static readonly string[] LevelDescriptions =
    [
        "0 – fastest",
        "1 – fast",
        "2 – default",
        "3 – better",
        "4 – strong",
        "5 – stronger",
        "6 – maximum",
    ];

    private readonly IBatchOptimizer _batchOptimizer;
    private readonly IPngFileCollector _fileCollector;
    private readonly ISettingsStore _settingsStore;
    private readonly AboutInfo _aboutInfo;
    private readonly LaunchRequest _launch;
    private readonly Dictionary<string, ListViewItem> _rows = new(StringComparer.OrdinalIgnoreCase);

    private CancellationTokenSource? _runCts;
    private Task _runTask = Task.CompletedTask;
    private bool _closePending;
    private bool _applyingSettings = true;
    private string? _lastRunSignature;
    private string _lastBrowseFolder = string.Empty;
    private ExtraOxipngOptions _extraOptions = ExtraOxipngOptions.None;
    private bool _forceWrite;
    private BatchSummary _summary = BatchSummary.Empty;
    private int _runFileCount;

    public MainForm(IBatchOptimizer batchOptimizer, IPngFileCollector fileCollector, ISettingsStore settingsStore, AboutInfo aboutInfo, LaunchRequest launch)
    {
        _batchOptimizer = batchOptimizer ?? throw new ArgumentNullException(nameof(batchOptimizer));
        _fileCollector = fileCollector ?? throw new ArgumentNullException(nameof(fileCollector));
        _settingsStore = settingsStore ?? throw new ArgumentNullException(nameof(settingsStore));
        _aboutInfo = aboutInfo ?? throw new ArgumentNullException(nameof(aboutInfo));
        _launch = launch ?? throw new ArgumentNullException(nameof(launch));
        InitializeComponent();
    }

    private bool IsBusy => _runCts is not null;

    private async void MainForm_Load(object sender, EventArgs e)
    {
        ApplySettings(_settingsStore.Load());
        FitFileColumn();
        UpdateControls();
        await AddPathsAsync(_launch.Paths);
        if (_launch.OptimizeOnStart && FilesListView.Items.Count > 0)
            await StartOptimizationAsync();
    }

    private async void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (!IsBusy)
            return;

        e.Cancel = true;
        if (_closePending || (e.CloseReason == CloseReason.UserClosing && !ConfirmStopAndClose()))
            return;

        StopForClosing();
        await _runTask;
        // Deferred, since this still runs inside FormClosing when the run finished during the prompt.
        BeginInvoke(new Action(Close));
    }

    private static bool ConfirmStopAndClose() =>
        MessageBox.Show(
            "Files are still being optimized. Stopping leaves every file that isn't finished as it was."
                + Environment.NewLine + Environment.NewLine + "Stop and close the app?",
            "Still working", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes;

    private void StopForClosing()
    {
        _closePending = true;
        Text += " - Stopping...";
        UseWaitCursor = true;
        RootTableLayoutPanel.Enabled = false;
        _runCts?.Cancel();
    }

    private async void AddFilesButton_Click(object sender, EventArgs e)
    {
        var files = IOHelper.SelectFiles("Add PNG files", PngFileFilter, _lastBrowseFolder);
        if (files.Length == 0)
            return;

        RememberBrowseFolder(Path.GetDirectoryName(files[0]));
        await AddPathsAsync(files);
    }

    private async void AddFolderButton_Click(object sender, EventArgs e)
    {
        var folder = IOHelper.SelectFolder("Add a folder with PNG files (subfolders included)", _lastBrowseFolder);
        if (folder is null)
            return;

        RememberBrowseFolder(folder);
        await AddPathsAsync([folder]);
    }

    private void RemoveButton_Click(object sender, EventArgs e) => RemoveSelectedRows();

    private void RemoveToolStripMenuItem_Click(object sender, EventArgs e) => RemoveSelectedRows();

    private void ClearButton_Click(object sender, EventArgs e)
    {
        if (IsBusy)
            return;

        FilesListView.Items.Clear();
        _rows.Clear();
        ShowSummary(GetStartedText, SystemColors.ControlText);
        UpdateControls();
    }

    private void AboutButton_Click(object sender, EventArgs e)
    {
        using var about = new AboutForm(_aboutInfo);
        about.ShowDialog(this);
    }

    private void Files_DragEnter(object sender, DragEventArgs e) =>
        e.Effect = !IsBusy && e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;

    private async void Files_DragDrop(object sender, DragEventArgs e)
    {
        if (e.Data.GetData(DataFormats.FileDrop) is string[] paths)
            await AddPathsAsync(paths);
    }

    private void FilesListView_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Delete)
            RemoveSelectedRows();
        else if (e.Control && e.KeyCode == Keys.A)
            SelectAllRows();
        else
            return;

        e.Handled = true;
    }

    private void FilesListView_SelectedIndexChanged(object sender, EventArgs e) => UpdateControls();

    private void FilesListView_Resize(object sender, EventArgs e) => FitFileColumn();

    private void FilesContextMenuStrip_Opening(object sender, CancelEventArgs e)
    {
        var selectedCount = FilesListView.SelectedItems.Count;
        ShowInFolderToolStripMenuItem.Enabled = selectedCount == 1;
        RemoveToolStripMenuItem.Enabled = selectedCount > 0 && !IsBusy;
        e.Cancel = selectedCount == 0;
    }

    private void ShowInFolderToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (FilesListView.SelectedItems.Count == 1)
            IOHelper.ShowInExplorer(ResultOf(FilesListView.SelectedItems[0]).File.FullPath);
    }

    private void Option_Changed(object sender, EventArgs e)
    {
        LevelValueLabel.Text = LevelDescriptions[LevelTrackBar.Value];
        SaveSettings();
    }

    private void SaveToFolderRadioButton_CheckedChanged(object sender, EventArgs e)
    {
        if (!_applyingSettings && SaveToFolderRadioButton.Checked && OutputFolderTextBox.Text.Length == 0 && !SelectOutputFolder())
            OverwriteRadioButton.Checked = true;

        OutputFolderTextBox.Enabled = SaveToFolderRadioButton.Checked;
        SaveSettings();
    }

    private void OutputFolderSelectButton_Click(object sender, EventArgs e)
    {
        if (!SelectOutputFolder())
            return;

        SaveToFolderRadioButton.Checked = true;
        SaveSettings();
    }

    private void AdvancedCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        AdvancedGroupBox.Visible = AdvancedCheckBox.Checked;
        SaveSettings();
    }

    private async void OptimizeButton_Click(object sender, EventArgs e)
    {
        if (IsBusy)
        {
            _runCts?.Cancel();
            UpdateControls();
            return;
        }

        await StartOptimizationAsync();
    }

    private Task StartOptimizationAsync()
    {
        _runTask = RunOptimizationAsync();
        return _runTask;
    }

    private async Task RunOptimizationAsync()
    {
        var settings = ReadSettings();
        var files = PrepareRun(settings);
        if (files.Count == 0)
        {
            ShowSummary("All files are already optimized with these settings.", SystemColors.ControlText);
            return;
        }

        try
        {
            if (await RunBatchAsync(files, settings) && _summary.Failed > 0)
                ShowFailuresWarning();
        }
        catch (OxipngUnavailableException ex)
        {
            ShowOxipngUnavailable(ex);
        }
    }

    /// <returns><see langword="false"/> if the batch was stopped.</returns>
    private async Task<bool> RunBatchAsync(IReadOnlyList<PngFile> files, UserSettings settings)
    {
        using var cts = new CancellationTokenSource();
        _runCts = cts;
        BeginRun(files.Count);
        try
        {
            await _batchOptimizer.RunAsync(files, settings.Options, settings.Output, new Progress<FileResult>(ShowProgress), cts.Token);
            ShowFinished();
            return true;
        }
        catch (OperationCanceledException)
        {
            ShowSummary(StoppedText(), SystemColors.ControlText);
            return false;
        }
        finally
        {
            _runCts = null;
            UpdateControls();
        }
    }

    /// <summary>
    /// Resets the rows that need to run and returns their files: all rows when the settings differ from the last
    /// run, otherwise only those that aren't finished yet.
    /// </summary>
    private List<PngFile> PrepareRun(UserSettings settings)
    {
        var signature = RunSignature(settings);
        var rerunAll = !string.Equals(signature, _lastRunSignature, StringComparison.Ordinal);
        _lastRunSignature = signature;

        List<PngFile> files = [];
        foreach (ListViewItem row in FilesListView.Items)
        {
            var result = ResultOf(row);
            if (result.IsFinished && !rerunAll)
                continue;

            ShowResult(row, FileResult.Waiting(result.File));
            files.Add(result.File);
        }

        return files;
    }

    private void BeginRun(int fileCount)
    {
        _summary = BatchSummary.Empty;
        _runFileCount = fileCount;
        OptimizeProgressBar.Maximum = fileCount;
        OptimizeProgressBar.Value = 0;
        ShowSummary(ProgressText(), SystemColors.ControlText);
        UpdateControls();
    }

    private void ShowProgress(FileResult result)
    {
        if (!_rows.TryGetValue(result.File.FullPath, out var row))
            return;

        ShowResult(row, result);
        if (result.Status is FileStatus.Optimizing or FileStatus.Stopped)
            return;

        _summary = _summary.Add(result);
        OptimizeProgressBar.Value = Math.Min(OptimizeProgressBar.Maximum, _summary.Done);
        SummaryLabel.Text = ProgressText();
    }

    private void ShowFinished()
    {
        OptimizeProgressBar.Value = OptimizeProgressBar.Maximum;
        ShowSummary(FinishedText(), _summary.Failed > 0 ? Color.Firebrick : Color.Green);
    }

    private void ShowFailuresWarning() =>
        MessageBox.Show($"Finished, but {FileCount(_summary.Failed)} couldn't be optimized. The Status column shows why.", "Finished with errors", MessageBoxButtons.OK, MessageBoxIcon.Warning);

    private void ShowOxipngUnavailable(OxipngUnavailableException exception)
    {
        ShowSummary("oxipng couldn't be started.", Color.Firebrick);
        if (!exception.IsRuntimeMissing)
        {
            MessageBox.Show(exception.Message, "oxipng couldn't be started", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        var answer = MessageBox.Show(
            exception.Message + Environment.NewLine + Environment.NewLine + "Download it from Microsoft now? Once it's installed, click OPTIMIZE again.",
            "Visual C++ Runtime missing", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (answer == DialogResult.Yes)
            IOHelper.OpenUrl(VcRedistDownloadUrl);
    }

    private async Task AddPathsAsync(IReadOnlyCollection<string> paths)
    {
        if (paths.Count == 0 || IsBusy)
            return;

        ShowSummary("Looking for PNG files...", SystemColors.ControlText);
        var collected = await Task.Run(() => _fileCollector.Collect(paths));
        var added = AddRows(collected.Files);
        ShowSummary(AddedText(added, collected.Files.Count - added, collected.SkippedCount), SystemColors.ControlText);
        UpdateControls();
    }

    private int AddRows(IReadOnlyList<PngFile> files)
    {
        var added = 0;
        FilesListView.BeginUpdate();
        try
        {
            foreach (var file in files)
            {
                if (_rows.TryGetValue(file.FullPath, out var existing))
                {
                    ShowResult(existing, FileResult.Waiting(file));
                    continue;
                }

                var row = new ListViewItem([file.RelativeOutputPath, string.Empty, string.Empty, string.Empty, string.Empty]);
                ShowResult(row, FileResult.Waiting(file));
                FilesListView.Items.Add(row);
                _rows.Add(file.FullPath, row);
                added++;
            }
        }
        finally
        {
            FilesListView.EndUpdate();
        }

        return added;
    }

    private void RemoveSelectedRows()
    {
        if (IsBusy || FilesListView.SelectedItems.Count == 0)
            return;

        FilesListView.BeginUpdate();
        try
        {
            foreach (var row in FilesListView.SelectedItems.Cast<ListViewItem>().ToList())
            {
                _rows.Remove(ResultOf(row).File.FullPath);
                FilesListView.Items.Remove(row);
            }
        }
        finally
        {
            FilesListView.EndUpdate();
        }

        UpdateControls();
    }

    private void SelectAllRows()
    {
        FilesListView.BeginUpdate();
        foreach (ListViewItem row in FilesListView.Items)
            row.Selected = true;
        FilesListView.EndUpdate();
    }

    private void ShowResult(ListViewItem row, FileResult result)
    {
        row.Tag = result;
        row.SubItems[FileColumnHeader.Index].Text = result.File.RelativeOutputPath;
        row.SubItems[OriginalColumnHeader.Index].Text = ByteSize.Format(result.OriginalSize);
        row.SubItems[OptimizedColumnHeader.Index].Text = result.OptimizedSize is long optimizedSize ? ByteSize.Format(optimizedSize) : string.Empty;
        row.SubItems[SavedColumnHeader.Index].Text = result.IsFinished ? SavedPercent(result) : string.Empty;
        row.SubItems[StatusColumnHeader.Index].Text = StatusText(result);
        row.ForeColor = StatusColor(result.Status);
        row.ToolTipText = result.Error is null ? result.File.FullPath : result.File.FullPath + Environment.NewLine + result.Error;
    }

    private void FitFileColumn()
    {
        var otherColumnsWidth = OriginalColumnHeader.Width + OptimizedColumnHeader.Width + SavedColumnHeader.Width + StatusColumnHeader.Width;
        var availableWidth = FilesListView.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - otherColumnsWidth;
        FileColumnHeader.Width = Math.Max(MinFileColumnWidth, availableWidth);
    }

    private void UpdateControls()
    {
        var busy = IsBusy;
        var hasRows = FilesListView.Items.Count > 0;
        AddFilesButton.Enabled = !busy;
        AddFolderButton.Enabled = !busy;
        RemoveButton.Enabled = !busy && FilesListView.SelectedItems.Count > 0;
        ClearButton.Enabled = !busy && hasRows;
        OptionsGroupBox.Enabled = !busy;
        OutputGroupBox.Enabled = !busy;
        AdvancedGroupBox.Enabled = !busy;
        FilesListView.AllowDrop = !busy;
        DropHintLabel.AllowDrop = !busy;
        DropHintLabel.Visible = !hasRows;
        OptimizeButton.Text = busy ? StopText : OptimizeText;
        OptimizeButton.Enabled = busy ? _runCts?.IsCancellationRequested == false : hasRows;
    }

    private void ShowSummary(string text, Color color)
    {
        SummaryLabel.Text = text;
        SummaryLabel.ForeColor = color;
    }

    private void ApplySettings(UserSettings settings)
    {
        _applyingSettings = true;
        try
        {
            var options = settings.Options;
            LevelTrackBar.Value = options.Level;
            LevelValueLabel.Text = LevelDescriptions[options.Level];
            MetadataComboBox.SelectedIndex = (int)options.Metadata;
            ZopfliCheckBox.Checked = options.UseZopfli;
            AlphaCheckBox.Checked = options.OptimizeAlpha;
            KeepDatesCheckBox.Checked = options.KeepFileDates;
            InterlaceComboBox.SelectedIndex = (int)options.Interlace;
            TimeoutNumericUpDown.Value = options.TimeoutSeconds;
            FixChecksumsCheckBox.Checked = options.FixChecksums;
            ReduceTo8BitCheckBox.Checked = options.ReduceTo8Bit;
            OutputFolderTextBox.Text = settings.OutputFolder;
            SaveToFolderRadioButton.Checked = settings.SaveToFolder && settings.OutputFolder.Length > 0;
            OverwriteRadioButton.Checked = !SaveToFolderRadioButton.Checked;
            OutputFolderTextBox.Enabled = SaveToFolderRadioButton.Checked;
            AdvancedCheckBox.Checked = settings.ShowAdvanced;
            AdvancedGroupBox.Visible = settings.ShowAdvanced;
            _lastBrowseFolder = settings.LastBrowseFolder;
            ApplyCommandLineOptions(options);
        }
        finally
        {
            _applyingSettings = false;
        }
    }

    /// <summary>
    /// Keeps the oxipng options that came from the command line, shows them, and locks the controls whose own flags
    /// would contradict them.
    /// </summary>
    private void ApplyCommandLineOptions(OptimizationOptions options)
    {
        _extraOptions = options.Extra;
        _forceWrite = options.ForceWrite;
        var flags = OxipngArguments.CommandLineFlags(options);
        CommandLineLabel.Visible = flags.Count > 0;
        CommandLineLabel.Text = CommandLineFlagsText + string.Join(" ", flags);
        HelpToolTip.SetToolTip(CommandLineLabel, CommandLineLabel.Text);
        MetadataComboBox.Enabled = !_extraOptions.ControlsMetadata;
        ZopfliCheckBox.Enabled = _extraOptions.RequiredZopfli is null;
    }

    private UserSettings ReadSettings() => new(
        new OptimizationOptions(
            level: LevelTrackBar.Value,
            metadata: (MetadataMode)MetadataComboBox.SelectedIndex,
            useZopfli: ZopfliCheckBox.Checked,
            optimizeAlpha: AlphaCheckBox.Checked,
            keepFileDates: KeepDatesCheckBox.Checked,
            interlace: (InterlaceMode)InterlaceComboBox.SelectedIndex,
            timeoutSeconds: (int)TimeoutNumericUpDown.Value,
            fixChecksums: FixChecksumsCheckBox.Checked,
            reduceTo8Bit: ReduceTo8BitCheckBox.Checked,
            extra: _extraOptions,
            forceWrite: _forceWrite),
        saveToFolder: SaveToFolderRadioButton.Checked,
        outputFolder: OutputFolderTextBox.Text,
        showAdvanced: AdvancedCheckBox.Checked,
        lastBrowseFolder: _lastBrowseFolder);

    private void SaveSettings()
    {
        if (!_applyingSettings)
            _settingsStore.Save(ReadSettings());
    }

    private void RememberBrowseFolder(string? folder)
    {
        _lastBrowseFolder = folder ?? string.Empty;
        SaveSettings();
    }

    private bool SelectOutputFolder()
    {
        var current = OutputFolderTextBox.Text.Length > 0 ? OutputFolderTextBox.Text : _lastBrowseFolder;
        var folder = IOHelper.SelectFolder("Select the folder for the optimized files", current);
        if (folder is null)
            return false;

        OutputFolderTextBox.Text = folder;
        return true;
    }

    private string ProgressText() => $"{_summary.Done} / {_runFileCount} files · {SavedText(_summary)}";

    private string FinishedText()
    {
        var failed = _summary.Failed > 0 ? $", {_summary.Failed} failed" : string.Empty;
        return $"Done: {_summary.Optimized} optimized, {_summary.AlreadyOptimized} already optimized{failed} · {SavedText(_summary)}";
    }

    private string StoppedText() => $"Stopped after {FileCount(_summary.Done)} · {SavedText(_summary)}";

    private static string RunSignature(UserSettings settings) =>
        string.Join(" ", OxipngArguments.OptionFlags(settings.Options)) + SignatureSeparator + settings.Output.Folder;

    private static FileResult ResultOf(ListViewItem row) => (FileResult)row.Tag;

    private static string SavedText(BatchSummary summary) => $"saved {ByteSize.Format(summary.SavedBytes)} ({summary.SavedRatio:P1})";

    private static string SavedPercent(FileResult result) =>
        result.OriginalSize == 0 ? string.Empty : ((double)result.SavedBytes / result.OriginalSize).ToString("P1");

    private static string FileCount(int count) => count == 1 ? "1 file" : $"{count} files";

    private static string AddedText(int added, int alreadyListed, int skipped)
    {
        List<string> parts = [added > 0 ? $"Added {FileCount(added)}." : "No new PNG files."];
        if (alreadyListed > 0)
            parts.Add($"{FileCount(alreadyListed)} already in the list.");
        if (skipped == 1)
            parts.Add("Skipped 1 item that isn't a PNG file or can't be read.");
        else if (skipped > 1)
            parts.Add($"Skipped {skipped} items that aren't PNG files or can't be read.");

        return string.Join(" ", parts);
    }

    private static string StatusText(FileResult result) => result.Status switch
    {
        FileStatus.Waiting => "Waiting",
        FileStatus.Optimizing => "Optimizing...",
        FileStatus.Optimized => "Optimized",
        FileStatus.AlreadyOptimized => "Already optimized",
        FileStatus.Failed => $"Failed: {result.Error}",
        FileStatus.Stopped => "Stopped",
        _ => throw new NotSupportedException($"Unsupported file status: {result.Status}"),
    };

    private static Color StatusColor(FileStatus status) => status switch
    {
        FileStatus.Optimized => Color.Green,
        FileStatus.Failed => Color.Firebrick,
        FileStatus.AlreadyOptimized or FileStatus.Stopped => SystemColors.GrayText,
        FileStatus.Waiting or FileStatus.Optimizing => SystemColors.WindowText,
        _ => throw new NotSupportedException($"Unsupported file status: {status}"),
    };
}
