using OxipngGui.Forms;
using OxipngGui.Models;
using OxipngGui.Services;
using OxipngGui.Utils;

namespace OxipngGui;

internal static class Program
{
    private const string AppTitle = "oxipng GUI";
    private const string AppDataFolderName = "oxipng_GUI";
    private const string OxipngFolderName = "oxipng";
    private const int SuccessExitCode = 0;
    private const int InvalidArgumentsExitCode = 2;

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static int Main(string[] args)
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        Application.ThreadException += (_, e) => ShowUnexpectedError(e.Exception);
        AppDomain.CurrentDomain.UnhandledException += (_, e) => ShowUnexpectedError(e.ExceptionObject as Exception);

        var launch = new LaunchArgumentsParser().Parse(args);
        var aboutInfo = AboutInfoFactory.Create(EmbeddedResources.Metadata(EmbeddedResources.OxipngVersionKey));
        if (launch.Errors.Count > 0)
            return ShowInvalidArguments(launch.Errors);
        if (launch.HelpText is not null)
            return ShowHelp(launch.HelpText);
        if (launch.ShowVersion)
            return ShowVersion(aboutInfo);

        using var job = new KillOnCloseJob();
        var processRunner = new ProcessRunner(job);
        using var oxipng = new EmbeddedOxipngBinary(OxipngCacheDirectory(), processRunner);
        var batchOptimizer = new BatchOptimizer(oxipng, new OxipngOptimizer(oxipng, processRunner));
        ISettingsStore settingsStore = new SettingsStore(Properties.Settings.Default);
        if (launch.OverridesSettings)
            settingsStore = new LaunchSettingsStore(settingsStore, launch);

        Application.Run(new MainForm(batchOptimizer, () => new BatchEstimator(), new PngFileCollector(), settingsStore, aboutInfo, launch.Request));
        return SuccessExitCode;
    }

    private static int ShowInvalidArguments(IReadOnlyList<string> errors)
    {
        var hint = $"Run {Path.GetFileName(Application.ExecutablePath)} --help to see all options.";
        MessageBox.Show(string.Join(Environment.NewLine, errors) + Environment.NewLine + Environment.NewLine + hint, $"{AppTitle}: invalid command line", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return InvalidArgumentsExitCode;
    }

    private static int ShowHelp(string helpText)
    {
        using var help = new CommandLineHelpForm(helpText);
        help.ShowDialog();
        return SuccessExitCode;
    }

    private static int ShowVersion(AboutInfo aboutInfo)
    {
        MessageBox.Show(aboutInfo.ToString(), AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
        return SuccessExitCode;
    }

    private static string OxipngCacheDirectory() =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppDataFolderName, OxipngFolderName);

    private static void ShowUnexpectedError(Exception? exception) =>
        MessageBox.Show($"An unexpected error occurred: {exception?.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
}
