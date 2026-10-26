using OxipngGui.Models;
using OxipngGui.Utils;

namespace OxipngGui.Forms;

public sealed partial class AboutForm : Form
{
    private const string OxipngProjectUrl = "https://github.com/oxipng/oxipng";

    public AboutForm(AboutInfo aboutInfo)
    {
        if (aboutInfo is null)
            throw new ArgumentNullException(nameof(aboutInfo));

        InitializeComponent();
        ProductLabel.Text = $"oxipng GUI {aboutInfo.AppVersion}";
        OxipngLinkLabel.Text = $"Powered by oxipng {aboutInfo.OxipngVersion}: {OxipngProjectUrl}";
        LicensesTextBox.Text = aboutInfo.LicenseText;
    }

    private void OxipngLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => IOHelper.OpenUrl(OxipngProjectUrl);
}
