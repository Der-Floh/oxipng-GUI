using System.Diagnostics;

namespace OxipngGui.Utils;

public static class IOHelper
{
    public static string? SelectFolder(string description, string? selectedPath = null)
    {
        using var dialog = new FolderBrowserDialog
        {
            Description = description
        };

        if (!string.IsNullOrEmpty(selectedPath) && Directory.Exists(selectedPath))
            dialog.SelectedPath = selectedPath;

        return dialog.ShowDialog() == DialogResult.OK ? dialog.SelectedPath : null;
    }

    public static string[] SelectFiles(string title, string filter, string? initialDirectory = null)
    {
        using var dialog = new OpenFileDialog
        {
            Title = title,
            Filter = filter,
            CheckFileExists = true,
            Multiselect = true,
        };

        if (!string.IsNullOrEmpty(initialDirectory) && Directory.Exists(initialDirectory))
            dialog.InitialDirectory = initialDirectory;
        else
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

        return dialog.ShowDialog() == DialogResult.OK ? dialog.FileNames : [];
    }

    public static void ShowInExplorer(string path) => Process.Start("explorer.exe", $"/select,\"{path}\"")?.Dispose();

    public static void OpenUrl(string url) => Process.Start(url)?.Dispose();
}
