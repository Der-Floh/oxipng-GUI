namespace OxipngGui.Forms;

public sealed partial class CommandLineHelpForm : Form
{
    public CommandLineHelpForm(string helpText)
    {
        if (helpText is null)
            throw new ArgumentNullException(nameof(helpText));

        InitializeComponent();
        Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        HelpTextBox.Text = helpText.Replace("\r\n", "\n").Replace("\n", Environment.NewLine);
    }
}
