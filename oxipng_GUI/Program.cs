using OxipngGui.Forms;
using OxipngGui.Services;
using OxipngGui.Utils;

namespace OxipngGui;

internal static class Program
{
    private const string AppDataFolderName = "oxipng_GUI";
    private const string OxipngFolderName = "oxipng";

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        Application.ThreadException += (_, e) => ShowUnexpectedError(e.Exception);
        AppDomain.CurrentDomain.UnhandledException += (_, e) => ShowUnexpectedError(e.ExceptionObject as Exception);

        using var job = new KillOnCloseJob();
        var processRunner = new ProcessRunner(job);
        using var oxipng = new EmbeddedOxipngBinary(OxipngCacheDirectory(), processRunner);
        var batchOptimizer = new BatchOptimizer(oxipng, new OxipngOptimizer(oxipng, processRunner));
        var settingsStore = new SettingsStore(Properties.Settings.Default);
        var aboutInfo = AboutInfoFactory.Create(oxipng.Version);

        Application.Run(new MainForm(batchOptimizer, new PngFileCollector(), settingsStore, aboutInfo, args));
    }

    private static string OxipngCacheDirectory() =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppDataFolderName, OxipngFolderName);

    private static void ShowUnexpectedError(Exception? exception) =>
        MessageBox.Show($"An unexpected error occurred: {exception?.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
}
