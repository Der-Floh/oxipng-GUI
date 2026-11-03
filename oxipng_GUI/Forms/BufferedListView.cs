using System.ComponentModel;
using System.Runtime.InteropServices;

namespace OxipngGui.Forms;

/// <summary>
/// A list view that draws off-screen first. WinForms repaints the whole list whenever an item's color changes,
/// which flickers without double buffering. It also selects or deselects every item with one message, where WinForms
/// would change a virtual list's items one by one.
/// </summary>
[ToolboxItem(true)]
public sealed class BufferedListView : ListView
{
    private const int SetItemStateMessage = 0x102B;
    private const int SelectedState = 0x0002;
    private const int AllItems = -1;

    public BufferedListView()
    {
        DoubleBuffered = true;
    }

    public void SelectAll() => SetAllSelected(true);

    public void DeselectAll() => SetAllSelected(false);

    private void SetAllSelected(bool selected)
    {
        var item = new NativeListViewItem { StateMask = SelectedState, State = selected ? SelectedState : 0 };
        SendMessage(Handle, SetItemStateMessage, (IntPtr)AllItems, ref item);
    }

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern IntPtr SendMessage(IntPtr window, int message, IntPtr wParam, ref NativeListViewItem lParam);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct NativeListViewItem
    {
        public int Mask;
        public int Item;
        public int SubItem;
        public int State;
        public int StateMask;
        public IntPtr Text;
        public int TextMax;
        public int Image;
        public IntPtr Param;
        public int Indent;
        public int GroupId;
        public int Columns;
        public IntPtr ColumnIndexes;
        public IntPtr ColumnFormats;
        public int Group;
    }
}
