namespace OxipngGui.Forms;

sealed partial class MainForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.RootTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ButtonsTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.AddFilesButton = new System.Windows.Forms.Button();
            this.AddFolderButton = new System.Windows.Forms.Button();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.ClearButton = new System.Windows.Forms.Button();
            this.AboutButton = new System.Windows.Forms.Button();
            this.FilesPanel = new System.Windows.Forms.Panel();
            this.DropHintLabel = new System.Windows.Forms.Label();
            this.FilesListView = new System.Windows.Forms.ListView();
            this.FileColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.OriginalColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.OptimizedColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.SavedColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.StatusColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.FilesContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ShowInFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RemoveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SettingsTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.OptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.LevelLabel = new System.Windows.Forms.Label();
            this.LevelTrackBar = new System.Windows.Forms.TrackBar();
            this.LevelValueLabel = new System.Windows.Forms.Label();
            this.MetadataLabel = new System.Windows.Forms.Label();
            this.MetadataComboBox = new System.Windows.Forms.ComboBox();
            this.ZopfliCheckBox = new System.Windows.Forms.CheckBox();
            this.AlphaCheckBox = new System.Windows.Forms.CheckBox();
            this.KeepDatesCheckBox = new System.Windows.Forms.CheckBox();
            this.OutputGroupBox = new System.Windows.Forms.GroupBox();
            this.OverwriteRadioButton = new System.Windows.Forms.RadioButton();
            this.SaveToFolderRadioButton = new System.Windows.Forms.RadioButton();
            this.OutputFolderTextBox = new System.Windows.Forms.TextBox();
            this.OutputFolderSelectButton = new System.Windows.Forms.Button();
            this.OutputHintLabel = new System.Windows.Forms.Label();
            this.AdvancedCheckBox = new System.Windows.Forms.CheckBox();
            this.AdvancedGroupBox = new System.Windows.Forms.GroupBox();
            this.InterlaceLabel = new System.Windows.Forms.Label();
            this.InterlaceComboBox = new System.Windows.Forms.ComboBox();
            this.TimeoutLabel = new System.Windows.Forms.Label();
            this.TimeoutNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.TimeoutUnitLabel = new System.Windows.Forms.Label();
            this.FixChecksumsCheckBox = new System.Windows.Forms.CheckBox();
            this.ReduceTo8BitCheckBox = new System.Windows.Forms.CheckBox();
            this.RunTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.OptimizeButton = new System.Windows.Forms.Button();
            this.OptimizeProgressBar = new System.Windows.Forms.ProgressBar();
            this.SummaryLabel = new System.Windows.Forms.Label();
            this.HelpToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.RootTableLayoutPanel.SuspendLayout();
            this.ButtonsTableLayoutPanel.SuspendLayout();
            this.FilesPanel.SuspendLayout();
            this.FilesContextMenuStrip.SuspendLayout();
            this.SettingsTableLayoutPanel.SuspendLayout();
            this.OptionsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LevelTrackBar)).BeginInit();
            this.OutputGroupBox.SuspendLayout();
            this.AdvancedGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TimeoutNumericUpDown)).BeginInit();
            this.RunTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            //
            // RootTableLayoutPanel
            //
            this.RootTableLayoutPanel.ColumnCount = 1;
            this.RootTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RootTableLayoutPanel.Controls.Add(this.ButtonsTableLayoutPanel, 0, 0);
            this.RootTableLayoutPanel.Controls.Add(this.FilesPanel, 0, 1);
            this.RootTableLayoutPanel.Controls.Add(this.SettingsTableLayoutPanel, 0, 2);
            this.RootTableLayoutPanel.Controls.Add(this.RunTableLayoutPanel, 0, 3);
            this.RootTableLayoutPanel.Controls.Add(this.SummaryLabel, 0, 4);
            this.RootTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RootTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.RootTableLayoutPanel.Name = "RootTableLayoutPanel";
            this.RootTableLayoutPanel.Padding = new System.Windows.Forms.Padding(6);
            this.RootTableLayoutPanel.RowCount = 5;
            this.RootTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.RootTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RootTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.RootTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.RootTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.RootTableLayoutPanel.Size = new System.Drawing.Size(820, 640);
            this.RootTableLayoutPanel.TabIndex = 0;
            //
            // ButtonsTableLayoutPanel
            //
            this.ButtonsTableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonsTableLayoutPanel.AutoSize = true;
            this.ButtonsTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ButtonsTableLayoutPanel.ColumnCount = 6;
            this.ButtonsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.ButtonsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.ButtonsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.ButtonsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.ButtonsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ButtonsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.ButtonsTableLayoutPanel.Controls.Add(this.AddFilesButton, 0, 0);
            this.ButtonsTableLayoutPanel.Controls.Add(this.AddFolderButton, 1, 0);
            this.ButtonsTableLayoutPanel.Controls.Add(this.RemoveButton, 2, 0);
            this.ButtonsTableLayoutPanel.Controls.Add(this.ClearButton, 3, 0);
            this.ButtonsTableLayoutPanel.Controls.Add(this.AboutButton, 5, 0);
            this.ButtonsTableLayoutPanel.Location = new System.Drawing.Point(6, 6);
            this.ButtonsTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonsTableLayoutPanel.Name = "ButtonsTableLayoutPanel";
            this.ButtonsTableLayoutPanel.RowCount = 1;
            this.ButtonsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.ButtonsTableLayoutPanel.Size = new System.Drawing.Size(808, 29);
            this.ButtonsTableLayoutPanel.TabIndex = 0;
            //
            // AddFilesButton
            //
            this.AddFilesButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AddFilesButton.Location = new System.Drawing.Point(3, 3);
            this.AddFilesButton.Name = "AddFilesButton";
            this.AddFilesButton.Size = new System.Drawing.Size(100, 23);
            this.AddFilesButton.TabIndex = 0;
            this.AddFilesButton.Text = "Add Files...";
            this.HelpToolTip.SetToolTip(this.AddFilesButton, "Add PNG files to the list. You can also drop files and folders onto the list.");
            this.AddFilesButton.UseVisualStyleBackColor = true;
            this.AddFilesButton.Click += new System.EventHandler(this.AddFilesButton_Click);
            //
            // AddFolderButton
            //
            this.AddFolderButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AddFolderButton.Location = new System.Drawing.Point(109, 3);
            this.AddFolderButton.Name = "AddFolderButton";
            this.AddFolderButton.Size = new System.Drawing.Size(100, 23);
            this.AddFolderButton.TabIndex = 1;
            this.AddFolderButton.Text = "Add Folder...";
            this.HelpToolTip.SetToolTip(this.AddFolderButton, "Add every PNG file in a folder and its subfolders.");
            this.AddFolderButton.UseVisualStyleBackColor = true;
            this.AddFolderButton.Click += new System.EventHandler(this.AddFolderButton_Click);
            //
            // RemoveButton
            //
            this.RemoveButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.RemoveButton.Enabled = false;
            this.RemoveButton.Location = new System.Drawing.Point(215, 3);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(75, 23);
            this.RemoveButton.TabIndex = 2;
            this.RemoveButton.Text = "Remove";
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            //
            // ClearButton
            //
            this.ClearButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ClearButton.Enabled = false;
            this.ClearButton.Location = new System.Drawing.Point(296, 3);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(75, 23);
            this.ClearButton.TabIndex = 3;
            this.ClearButton.Text = "Clear";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            //
            // AboutButton
            //
            this.AboutButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AboutButton.Location = new System.Drawing.Point(730, 3);
            this.AboutButton.Name = "AboutButton";
            this.AboutButton.Size = new System.Drawing.Size(75, 23);
            this.AboutButton.TabIndex = 4;
            this.AboutButton.Text = "About";
            this.AboutButton.UseVisualStyleBackColor = true;
            this.AboutButton.Click += new System.EventHandler(this.AboutButton_Click);
            //
            // FilesPanel
            //
            this.FilesPanel.Controls.Add(this.DropHintLabel);
            this.FilesPanel.Controls.Add(this.FilesListView);
            this.FilesPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FilesPanel.Location = new System.Drawing.Point(9, 38);
            this.FilesPanel.Name = "FilesPanel";
            this.FilesPanel.Size = new System.Drawing.Size(802, 247);
            this.FilesPanel.TabIndex = 1;
            //
            // DropHintLabel
            //
            this.DropHintLabel.AllowDrop = true;
            this.DropHintLabel.BackColor = System.Drawing.SystemColors.Window;
            this.DropHintLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.DropHintLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DropHintLabel.ForeColor = System.Drawing.SystemColors.GrayText;
            this.DropHintLabel.Location = new System.Drawing.Point(0, 0);
            this.DropHintLabel.Name = "DropHintLabel";
            this.DropHintLabel.Size = new System.Drawing.Size(802, 247);
            this.DropHintLabel.TabIndex = 1;
            this.DropHintLabel.Text = "Drop PNG files or folders here\r\n\r\nor use Add Files... and Add Folder...";
            this.DropHintLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.DropHintLabel.DragDrop += new System.Windows.Forms.DragEventHandler(this.Files_DragDrop);
            this.DropHintLabel.DragEnter += new System.Windows.Forms.DragEventHandler(this.Files_DragEnter);
            //
            // FilesListView
            //
            this.FilesListView.AllowDrop = true;
            this.FilesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.FileColumnHeader,
            this.OriginalColumnHeader,
            this.OptimizedColumnHeader,
            this.SavedColumnHeader,
            this.StatusColumnHeader});
            this.FilesListView.ContextMenuStrip = this.FilesContextMenuStrip;
            this.FilesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FilesListView.FullRowSelect = true;
            this.FilesListView.HideSelection = false;
            this.FilesListView.Location = new System.Drawing.Point(0, 0);
            this.FilesListView.Name = "FilesListView";
            this.FilesListView.ShowItemToolTips = true;
            this.FilesListView.Size = new System.Drawing.Size(802, 247);
            this.FilesListView.TabIndex = 0;
            this.FilesListView.UseCompatibleStateImageBehavior = false;
            this.FilesListView.View = System.Windows.Forms.View.Details;
            this.FilesListView.SelectedIndexChanged += new System.EventHandler(this.FilesListView_SelectedIndexChanged);
            this.FilesListView.DragDrop += new System.Windows.Forms.DragEventHandler(this.Files_DragDrop);
            this.FilesListView.DragEnter += new System.Windows.Forms.DragEventHandler(this.Files_DragEnter);
            this.FilesListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FilesListView_KeyDown);
            this.FilesListView.Resize += new System.EventHandler(this.FilesListView_Resize);
            //
            // FileColumnHeader
            //
            this.FileColumnHeader.Text = "File";
            this.FileColumnHeader.Width = 400;
            //
            // OriginalColumnHeader
            //
            this.OriginalColumnHeader.Text = "Original";
            this.OriginalColumnHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.OriginalColumnHeader.Width = 80;
            //
            // OptimizedColumnHeader
            //
            this.OptimizedColumnHeader.Text = "Optimized";
            this.OptimizedColumnHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.OptimizedColumnHeader.Width = 80;
            //
            // SavedColumnHeader
            //
            this.SavedColumnHeader.Text = "Saved";
            this.SavedColumnHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.SavedColumnHeader.Width = 60;
            //
            // StatusColumnHeader
            //
            this.StatusColumnHeader.Text = "Status";
            this.StatusColumnHeader.Width = 160;
            //
            // FilesContextMenuStrip
            //
            this.FilesContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ShowInFolderToolStripMenuItem,
            this.RemoveToolStripMenuItem});
            this.FilesContextMenuStrip.Name = "FilesContextMenuStrip";
            this.FilesContextMenuStrip.Size = new System.Drawing.Size(151, 48);
            this.FilesContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.FilesContextMenuStrip_Opening);
            //
            // ShowInFolderToolStripMenuItem
            //
            this.ShowInFolderToolStripMenuItem.Name = "ShowInFolderToolStripMenuItem";
            this.ShowInFolderToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.ShowInFolderToolStripMenuItem.Text = "Show in folder";
            this.ShowInFolderToolStripMenuItem.Click += new System.EventHandler(this.ShowInFolderToolStripMenuItem_Click);
            //
            // RemoveToolStripMenuItem
            //
            this.RemoveToolStripMenuItem.Name = "RemoveToolStripMenuItem";
            this.RemoveToolStripMenuItem.ShortcutKeyDisplayString = "Del";
            this.RemoveToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.RemoveToolStripMenuItem.Text = "Remove";
            this.RemoveToolStripMenuItem.Click += new System.EventHandler(this.RemoveToolStripMenuItem_Click);
            //
            // SettingsTableLayoutPanel
            //
            this.SettingsTableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SettingsTableLayoutPanel.AutoSize = true;
            this.SettingsTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.SettingsTableLayoutPanel.ColumnCount = 2;
            this.SettingsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.SettingsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.SettingsTableLayoutPanel.Controls.Add(this.OptionsGroupBox, 0, 0);
            this.SettingsTableLayoutPanel.Controls.Add(this.OutputGroupBox, 1, 0);
            this.SettingsTableLayoutPanel.Controls.Add(this.AdvancedCheckBox, 0, 1);
            this.SettingsTableLayoutPanel.Controls.Add(this.AdvancedGroupBox, 0, 2);
            this.SettingsTableLayoutPanel.Location = new System.Drawing.Point(6, 288);
            this.SettingsTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.SettingsTableLayoutPanel.Name = "SettingsTableLayoutPanel";
            this.SettingsTableLayoutPanel.RowCount = 3;
            this.SettingsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.SettingsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.SettingsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.SettingsTableLayoutPanel.SetColumnSpan(this.AdvancedCheckBox, 2);
            this.SettingsTableLayoutPanel.SetColumnSpan(this.AdvancedGroupBox, 2);
            this.SettingsTableLayoutPanel.Size = new System.Drawing.Size(808, 285);
            this.SettingsTableLayoutPanel.TabIndex = 2;
            //
            // OptionsGroupBox
            //
            this.OptionsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OptionsGroupBox.Controls.Add(this.LevelLabel);
            this.OptionsGroupBox.Controls.Add(this.LevelTrackBar);
            this.OptionsGroupBox.Controls.Add(this.LevelValueLabel);
            this.OptionsGroupBox.Controls.Add(this.MetadataLabel);
            this.OptionsGroupBox.Controls.Add(this.MetadataComboBox);
            this.OptionsGroupBox.Controls.Add(this.ZopfliCheckBox);
            this.OptionsGroupBox.Controls.Add(this.AlphaCheckBox);
            this.OptionsGroupBox.Controls.Add(this.KeepDatesCheckBox);
            this.OptionsGroupBox.Location = new System.Drawing.Point(3, 3);
            this.OptionsGroupBox.Name = "OptionsGroupBox";
            this.OptionsGroupBox.Size = new System.Drawing.Size(398, 170);
            this.OptionsGroupBox.TabIndex = 0;
            this.OptionsGroupBox.TabStop = false;
            this.OptionsGroupBox.Text = "Options";
            //
            // LevelLabel
            //
            this.LevelLabel.AutoSize = true;
            this.LevelLabel.Location = new System.Drawing.Point(9, 27);
            this.LevelLabel.Name = "LevelLabel";
            this.LevelLabel.Size = new System.Drawing.Size(33, 13);
            this.LevelLabel.TabIndex = 0;
            this.LevelLabel.Text = "Level";
            //
            // LevelTrackBar
            //
            this.LevelTrackBar.LargeChange = 1;
            this.LevelTrackBar.Location = new System.Drawing.Point(72, 19);
            this.LevelTrackBar.Maximum = 6;
            this.LevelTrackBar.Name = "LevelTrackBar";
            this.LevelTrackBar.Size = new System.Drawing.Size(160, 45);
            this.LevelTrackBar.TabIndex = 1;
            this.HelpToolTip.SetToolTip(this.LevelTrackBar, "Higher levels try more compression strategies: slower, usually a little smaller. 2 is oxipng\'s default.");
            this.LevelTrackBar.Value = 2;
            this.LevelTrackBar.ValueChanged += new System.EventHandler(this.Option_Changed);
            //
            // LevelValueLabel
            //
            this.LevelValueLabel.AutoSize = true;
            this.LevelValueLabel.Location = new System.Drawing.Point(238, 27);
            this.LevelValueLabel.Name = "LevelValueLabel";
            this.LevelValueLabel.Size = new System.Drawing.Size(59, 13);
            this.LevelValueLabel.TabIndex = 2;
            this.LevelValueLabel.Text = "2 – default";
            //
            // MetadataLabel
            //
            this.MetadataLabel.AutoSize = true;
            this.MetadataLabel.Location = new System.Drawing.Point(9, 71);
            this.MetadataLabel.Name = "MetadataLabel";
            this.MetadataLabel.Size = new System.Drawing.Size(52, 13);
            this.MetadataLabel.TabIndex = 3;
            this.MetadataLabel.Text = "Metadata";
            //
            // MetadataComboBox
            //
            this.MetadataComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MetadataComboBox.FormattingEnabled = true;
            this.MetadataComboBox.Items.AddRange(new object[] {
            "Keep all",
            "Strip safely",
            "Strip all"});
            this.MetadataComboBox.Location = new System.Drawing.Point(75, 68);
            this.MetadataComboBox.Name = "MetadataComboBox";
            this.MetadataComboBox.Size = new System.Drawing.Size(157, 21);
            this.MetadataComboBox.TabIndex = 4;
            this.HelpToolTip.SetToolTip(this.MetadataComboBox, "Keep all: only the compression changes.\r\nStrip safely: removes text, EXIF and other metadata that doesn\'t change how the image looks;\r\ncolor profiles stay.\r\nStrip all: removes every optional chunk; colors can shift in color-managed viewers.");
            this.MetadataComboBox.SelectedIndexChanged += new System.EventHandler(this.Option_Changed);
            //
            // ZopfliCheckBox
            //
            this.ZopfliCheckBox.AutoSize = true;
            this.ZopfliCheckBox.Location = new System.Drawing.Point(12, 98);
            this.ZopfliCheckBox.Name = "ZopfliCheckBox";
            this.ZopfliCheckBox.Size = new System.Drawing.Size(196, 17);
            this.ZopfliCheckBox.TabIndex = 5;
            this.ZopfliCheckBox.Text = "Zopfli (smaller files, much slower)";
            this.HelpToolTip.SetToolTip(this.ZopfliCheckBox, "Uses the much slower Zopfli compressor, which usually saves a few more percent.");
            this.ZopfliCheckBox.UseVisualStyleBackColor = true;
            this.ZopfliCheckBox.CheckedChanged += new System.EventHandler(this.Option_Changed);
            //
            // AlphaCheckBox
            //
            this.AlphaCheckBox.AutoSize = true;
            this.AlphaCheckBox.Location = new System.Drawing.Point(12, 121);
            this.AlphaCheckBox.Name = "AlphaCheckBox";
            this.AlphaCheckBox.Size = new System.Drawing.Size(186, 17);
            this.AlphaCheckBox.TabIndex = 6;
            this.AlphaCheckBox.Text = "Optimize fully transparent pixels";
            this.HelpToolTip.SetToolTip(this.AlphaCheckBox, "Changes the hidden color of fully transparent pixels so they compress better.\r\nThe image looks the same, but its pixel data changes.");
            this.AlphaCheckBox.UseVisualStyleBackColor = true;
            this.AlphaCheckBox.CheckedChanged += new System.EventHandler(this.Option_Changed);
            //
            // KeepDatesCheckBox
            //
            this.KeepDatesCheckBox.AutoSize = true;
            this.KeepDatesCheckBox.Location = new System.Drawing.Point(12, 144);
            this.KeepDatesCheckBox.Name = "KeepDatesCheckBox";
            this.KeepDatesCheckBox.Size = new System.Drawing.Size(97, 17);
            this.KeepDatesCheckBox.TabIndex = 7;
            this.KeepDatesCheckBox.Text = "Keep file dates";
            this.HelpToolTip.SetToolTip(this.KeepDatesCheckBox, "Keeps each file\'s modification date instead of setting it to now.");
            this.KeepDatesCheckBox.UseVisualStyleBackColor = true;
            this.KeepDatesCheckBox.CheckedChanged += new System.EventHandler(this.Option_Changed);
            //
            // OutputGroupBox
            //
            this.OutputGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OutputGroupBox.Controls.Add(this.OverwriteRadioButton);
            this.OutputGroupBox.Controls.Add(this.SaveToFolderRadioButton);
            this.OutputGroupBox.Controls.Add(this.OutputFolderTextBox);
            this.OutputGroupBox.Controls.Add(this.OutputFolderSelectButton);
            this.OutputGroupBox.Controls.Add(this.OutputHintLabel);
            this.OutputGroupBox.Location = new System.Drawing.Point(407, 3);
            this.OutputGroupBox.Name = "OutputGroupBox";
            this.OutputGroupBox.Size = new System.Drawing.Size(398, 170);
            this.OutputGroupBox.TabIndex = 1;
            this.OutputGroupBox.TabStop = false;
            this.OutputGroupBox.Text = "Output";
            //
            // OverwriteRadioButton
            //
            this.OverwriteRadioButton.AutoSize = true;
            this.OverwriteRadioButton.Checked = true;
            this.OverwriteRadioButton.Location = new System.Drawing.Point(12, 24);
            this.OverwriteRadioButton.Name = "OverwriteRadioButton";
            this.OverwriteRadioButton.Size = new System.Drawing.Size(139, 17);
            this.OverwriteRadioButton.TabIndex = 0;
            this.OverwriteRadioButton.TabStop = true;
            this.OverwriteRadioButton.Text = "Overwrite original files";
            this.HelpToolTip.SetToolTip(this.OverwriteRadioButton, "oxipng is lossless: an optimized file shows exactly the same pixels as the original.");
            this.OverwriteRadioButton.UseVisualStyleBackColor = true;
            this.OverwriteRadioButton.CheckedChanged += new System.EventHandler(this.Option_Changed);
            //
            // SaveToFolderRadioButton
            //
            this.SaveToFolderRadioButton.AutoSize = true;
            this.SaveToFolderRadioButton.Location = new System.Drawing.Point(12, 47);
            this.SaveToFolderRadioButton.Name = "SaveToFolderRadioButton";
            this.SaveToFolderRadioButton.Size = new System.Drawing.Size(95, 17);
            this.SaveToFolderRadioButton.TabIndex = 1;
            this.SaveToFolderRadioButton.Text = "Save to folder:";
            this.HelpToolTip.SetToolTip(this.SaveToFolderRadioButton, "Writes optimized copies to a folder and leaves the originals untouched.");
            this.SaveToFolderRadioButton.UseVisualStyleBackColor = true;
            this.SaveToFolderRadioButton.CheckedChanged += new System.EventHandler(this.SaveToFolderRadioButton_CheckedChanged);
            //
            // OutputFolderTextBox
            //
            this.OutputFolderTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OutputFolderTextBox.Location = new System.Drawing.Point(30, 70);
            this.OutputFolderTextBox.Name = "OutputFolderTextBox";
            this.OutputFolderTextBox.ReadOnly = true;
            this.OutputFolderTextBox.Size = new System.Drawing.Size(281, 20);
            this.OutputFolderTextBox.TabIndex = 2;
            //
            // OutputFolderSelectButton
            //
            this.OutputFolderSelectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.OutputFolderSelectButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.OutputFolderSelectButton.Location = new System.Drawing.Point(317, 68);
            this.OutputFolderSelectButton.Name = "OutputFolderSelectButton";
            this.OutputFolderSelectButton.Size = new System.Drawing.Size(75, 23);
            this.OutputFolderSelectButton.TabIndex = 3;
            this.OutputFolderSelectButton.Text = "...";
            this.HelpToolTip.SetToolTip(this.OutputFolderSelectButton, "Choose the folder for the optimized files.");
            this.OutputFolderSelectButton.UseVisualStyleBackColor = true;
            this.OutputFolderSelectButton.Click += new System.EventHandler(this.OutputFolderSelectButton_Click);
            //
            // OutputHintLabel
            //
            this.OutputHintLabel.AutoSize = true;
            this.OutputHintLabel.ForeColor = System.Drawing.SystemColors.GrayText;
            this.OutputHintLabel.Location = new System.Drawing.Point(27, 97);
            this.OutputHintLabel.Name = "OutputHintLabel";
            this.OutputHintLabel.Size = new System.Drawing.Size(237, 13);
            this.OutputHintLabel.TabIndex = 4;
            this.OutputHintLabel.Text = "Files from added folders keep their subfolders.";
            //
            // AdvancedCheckBox
            //
            this.AdvancedCheckBox.AutoSize = true;
            this.AdvancedCheckBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AdvancedCheckBox.Location = new System.Drawing.Point(6, 179);
            this.AdvancedCheckBox.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.AdvancedCheckBox.Name = "AdvancedCheckBox";
            this.AdvancedCheckBox.Size = new System.Drawing.Size(108, 17);
            this.AdvancedCheckBox.TabIndex = 2;
            this.AdvancedCheckBox.Text = "Advanced options";
            this.AdvancedCheckBox.UseVisualStyleBackColor = true;
            this.AdvancedCheckBox.CheckedChanged += new System.EventHandler(this.AdvancedCheckBox_CheckedChanged);
            //
            // AdvancedGroupBox
            //
            this.AdvancedGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AdvancedGroupBox.Controls.Add(this.InterlaceLabel);
            this.AdvancedGroupBox.Controls.Add(this.InterlaceComboBox);
            this.AdvancedGroupBox.Controls.Add(this.TimeoutLabel);
            this.AdvancedGroupBox.Controls.Add(this.TimeoutNumericUpDown);
            this.AdvancedGroupBox.Controls.Add(this.TimeoutUnitLabel);
            this.AdvancedGroupBox.Controls.Add(this.FixChecksumsCheckBox);
            this.AdvancedGroupBox.Controls.Add(this.ReduceTo8BitCheckBox);
            this.AdvancedGroupBox.Location = new System.Drawing.Point(3, 202);
            this.AdvancedGroupBox.Name = "AdvancedGroupBox";
            this.AdvancedGroupBox.Size = new System.Drawing.Size(802, 80);
            this.AdvancedGroupBox.TabIndex = 3;
            this.AdvancedGroupBox.TabStop = false;
            this.AdvancedGroupBox.Text = "Advanced";
            this.AdvancedGroupBox.Visible = false;
            //
            // InterlaceLabel
            //
            this.InterlaceLabel.AutoSize = true;
            this.InterlaceLabel.Location = new System.Drawing.Point(9, 25);
            this.InterlaceLabel.Name = "InterlaceLabel";
            this.InterlaceLabel.Size = new System.Drawing.Size(59, 13);
            this.InterlaceLabel.TabIndex = 0;
            this.InterlaceLabel.Text = "Interlacing";
            //
            // InterlaceComboBox
            //
            this.InterlaceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.InterlaceComboBox.FormattingEnabled = true;
            this.InterlaceComboBox.Items.AddRange(new object[] {
            "Remove (default)",
            "Keep",
            "Add (Adam7)"});
            this.InterlaceComboBox.Location = new System.Drawing.Point(90, 22);
            this.InterlaceComboBox.Name = "InterlaceComboBox";
            this.InterlaceComboBox.Size = new System.Drawing.Size(140, 21);
            this.InterlaceComboBox.TabIndex = 1;
            this.HelpToolTip.SetToolTip(this.InterlaceComboBox, "Remove: smaller files, oxipng\'s default.\r\nKeep: leaves interlacing as it is.\r\nAdd: progressive loading in browsers, but usually larger; skipped where it would grow the file.");
            this.InterlaceComboBox.SelectedIndexChanged += new System.EventHandler(this.Option_Changed);
            //
            // TimeoutLabel
            //
            this.TimeoutLabel.AutoSize = true;
            this.TimeoutLabel.Location = new System.Drawing.Point(262, 25);
            this.TimeoutLabel.Name = "TimeoutLabel";
            this.TimeoutLabel.Size = new System.Drawing.Size(84, 13);
            this.TimeoutLabel.TabIndex = 2;
            this.TimeoutLabel.Text = "Timeout per file";
            //
            // TimeoutNumericUpDown
            //
            this.TimeoutNumericUpDown.Location = new System.Drawing.Point(355, 23);
            this.TimeoutNumericUpDown.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.TimeoutNumericUpDown.Name = "TimeoutNumericUpDown";
            this.TimeoutNumericUpDown.Size = new System.Drawing.Size(60, 20);
            this.TimeoutNumericUpDown.TabIndex = 3;
            this.HelpToolTip.SetToolTip(this.TimeoutNumericUpDown, "Stops trying new strategies on a file after this many seconds and keeps the best result so far.");
            this.TimeoutNumericUpDown.ValueChanged += new System.EventHandler(this.Option_Changed);
            //
            // TimeoutUnitLabel
            //
            this.TimeoutUnitLabel.AutoSize = true;
            this.TimeoutUnitLabel.Location = new System.Drawing.Point(421, 25);
            this.TimeoutUnitLabel.Name = "TimeoutUnitLabel";
            this.TimeoutUnitLabel.Size = new System.Drawing.Size(58, 13);
            this.TimeoutUnitLabel.TabIndex = 4;
            this.TimeoutUnitLabel.Text = "s (0 = off)";
            //
            // FixChecksumsCheckBox
            //
            this.FixChecksumsCheckBox.AutoSize = true;
            this.FixChecksumsCheckBox.Location = new System.Drawing.Point(12, 52);
            this.FixChecksumsCheckBox.Name = "FixChecksumsCheckBox";
            this.FixChecksumsCheckBox.Size = new System.Drawing.Size(170, 17);
            this.FixChecksumsCheckBox.TabIndex = 5;
            this.FixChecksumsCheckBox.Text = "Fix files with bad checksums";
            this.HelpToolTip.SetToolTip(this.FixChecksumsCheckBox, "Repairs files with broken chunk checksums instead of rejecting them.");
            this.FixChecksumsCheckBox.UseVisualStyleBackColor = true;
            this.FixChecksumsCheckBox.CheckedChanged += new System.EventHandler(this.Option_Changed);
            //
            // ReduceTo8BitCheckBox
            //
            this.ReduceTo8BitCheckBox.AutoSize = true;
            this.ReduceTo8BitCheckBox.Location = new System.Drawing.Point(265, 52);
            this.ReduceTo8BitCheckBox.Name = "ReduceTo8BitCheckBox";
            this.ReduceTo8BitCheckBox.Size = new System.Drawing.Size(178, 17);
            this.ReduceTo8BitCheckBox.TabIndex = 6;
            this.ReduceTo8BitCheckBox.Text = "Reduce 16-bit to 8-bit (lossy)";
            this.HelpToolTip.SetToolTip(this.ReduceTo8BitCheckBox, "Converts 16-bit images to 8-bit. This changes pixel values.");
            this.ReduceTo8BitCheckBox.UseVisualStyleBackColor = true;
            this.ReduceTo8BitCheckBox.CheckedChanged += new System.EventHandler(this.Option_Changed);
            //
            // RunTableLayoutPanel
            //
            this.RunTableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RunTableLayoutPanel.AutoSize = true;
            this.RunTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.RunTableLayoutPanel.ColumnCount = 2;
            this.RunTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.RunTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.RunTableLayoutPanel.Controls.Add(this.OptimizeButton, 0, 0);
            this.RunTableLayoutPanel.Controls.Add(this.OptimizeProgressBar, 1, 0);
            this.RunTableLayoutPanel.Location = new System.Drawing.Point(6, 573);
            this.RunTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.RunTableLayoutPanel.Name = "RunTableLayoutPanel";
            this.RunTableLayoutPanel.RowCount = 1;
            this.RunTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.RunTableLayoutPanel.Size = new System.Drawing.Size(808, 29);
            this.RunTableLayoutPanel.TabIndex = 3;
            //
            // OptimizeButton
            //
            this.OptimizeButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.OptimizeButton.Enabled = false;
            this.OptimizeButton.Location = new System.Drawing.Point(3, 3);
            this.OptimizeButton.Name = "OptimizeButton";
            this.OptimizeButton.Size = new System.Drawing.Size(100, 23);
            this.OptimizeButton.TabIndex = 0;
            this.OptimizeButton.Text = "OPTIMIZE";
            this.OptimizeButton.UseVisualStyleBackColor = true;
            this.OptimizeButton.Click += new System.EventHandler(this.OptimizeButton_Click);
            //
            // OptimizeProgressBar
            //
            this.OptimizeProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.OptimizeProgressBar.ForeColor = System.Drawing.Color.Green;
            this.OptimizeProgressBar.Location = new System.Drawing.Point(109, 3);
            this.OptimizeProgressBar.Name = "OptimizeProgressBar";
            this.OptimizeProgressBar.Size = new System.Drawing.Size(696, 23);
            this.OptimizeProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.OptimizeProgressBar.TabIndex = 1;
            //
            // SummaryLabel
            //
            this.SummaryLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.SummaryLabel.AutoEllipsis = true;
            this.SummaryLabel.Location = new System.Drawing.Point(9, 605);
            this.SummaryLabel.Name = "SummaryLabel";
            this.SummaryLabel.Size = new System.Drawing.Size(802, 15);
            this.SummaryLabel.TabIndex = 4;
            this.SummaryLabel.Text = "Add PNG files to get started.";
            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(820, 640);
            this.Controls.Add(this.RootTableLayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(720, 600);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "oxipng GUI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.RootTableLayoutPanel.ResumeLayout(false);
            this.RootTableLayoutPanel.PerformLayout();
            this.ButtonsTableLayoutPanel.ResumeLayout(false);
            this.FilesPanel.ResumeLayout(false);
            this.FilesContextMenuStrip.ResumeLayout(false);
            this.SettingsTableLayoutPanel.ResumeLayout(false);
            this.SettingsTableLayoutPanel.PerformLayout();
            this.OptionsGroupBox.ResumeLayout(false);
            this.OptionsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LevelTrackBar)).EndInit();
            this.OutputGroupBox.ResumeLayout(false);
            this.OutputGroupBox.PerformLayout();
            this.AdvancedGroupBox.ResumeLayout(false);
            this.AdvancedGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TimeoutNumericUpDown)).EndInit();
            this.RunTableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel RootTableLayoutPanel;
    private System.Windows.Forms.TableLayoutPanel ButtonsTableLayoutPanel;
    private System.Windows.Forms.Button AddFilesButton;
    private System.Windows.Forms.Button AddFolderButton;
    private System.Windows.Forms.Button RemoveButton;
    private System.Windows.Forms.Button ClearButton;
    private System.Windows.Forms.Button AboutButton;
    private System.Windows.Forms.Panel FilesPanel;
    private System.Windows.Forms.Label DropHintLabel;
    private System.Windows.Forms.ListView FilesListView;
    private System.Windows.Forms.ColumnHeader FileColumnHeader;
    private System.Windows.Forms.ColumnHeader OriginalColumnHeader;
    private System.Windows.Forms.ColumnHeader OptimizedColumnHeader;
    private System.Windows.Forms.ColumnHeader SavedColumnHeader;
    private System.Windows.Forms.ColumnHeader StatusColumnHeader;
    private System.Windows.Forms.ContextMenuStrip FilesContextMenuStrip;
    private System.Windows.Forms.ToolStripMenuItem ShowInFolderToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem RemoveToolStripMenuItem;
    private System.Windows.Forms.TableLayoutPanel SettingsTableLayoutPanel;
    private System.Windows.Forms.GroupBox OptionsGroupBox;
    private System.Windows.Forms.Label LevelLabel;
    private System.Windows.Forms.TrackBar LevelTrackBar;
    private System.Windows.Forms.Label LevelValueLabel;
    private System.Windows.Forms.Label MetadataLabel;
    private System.Windows.Forms.ComboBox MetadataComboBox;
    private System.Windows.Forms.CheckBox ZopfliCheckBox;
    private System.Windows.Forms.CheckBox AlphaCheckBox;
    private System.Windows.Forms.CheckBox KeepDatesCheckBox;
    private System.Windows.Forms.GroupBox OutputGroupBox;
    private System.Windows.Forms.RadioButton OverwriteRadioButton;
    private System.Windows.Forms.RadioButton SaveToFolderRadioButton;
    private System.Windows.Forms.TextBox OutputFolderTextBox;
    private System.Windows.Forms.Button OutputFolderSelectButton;
    private System.Windows.Forms.Label OutputHintLabel;
    private System.Windows.Forms.CheckBox AdvancedCheckBox;
    private System.Windows.Forms.GroupBox AdvancedGroupBox;
    private System.Windows.Forms.Label InterlaceLabel;
    private System.Windows.Forms.ComboBox InterlaceComboBox;
    private System.Windows.Forms.Label TimeoutLabel;
    private System.Windows.Forms.NumericUpDown TimeoutNumericUpDown;
    private System.Windows.Forms.Label TimeoutUnitLabel;
    private System.Windows.Forms.CheckBox FixChecksumsCheckBox;
    private System.Windows.Forms.CheckBox ReduceTo8BitCheckBox;
    private System.Windows.Forms.TableLayoutPanel RunTableLayoutPanel;
    private System.Windows.Forms.Button OptimizeButton;
    private System.Windows.Forms.ProgressBar OptimizeProgressBar;
    private System.Windows.Forms.Label SummaryLabel;
    private System.Windows.Forms.ToolTip HelpToolTip;
}
