using System.ComponentModel;

namespace OxipngGui.Forms;

/// <summary>
/// A list view that draws off-screen first. WinForms repaints the whole list whenever an item's color changes,
/// which flickers without double buffering.
/// </summary>
[ToolboxItem(true)]
public sealed class BufferedListView : ListView
{
    public BufferedListView()
    {
        DoubleBuffered = true;
    }
}
