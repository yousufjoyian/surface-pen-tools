using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.IO;

/// <summary>
/// Background service that watches for a signal file and sends an Enter keystroke
/// globally to wherever the cursor is currently active.
/// Runs invisibly with no taskbar icon or window.
/// </summary>
class PenEnterService : Form
{
    [DllImport("user32.dll")]
    static extern void keybd_event(byte vk, byte scan, uint flags, UIntPtr extra);

    static string signalFile = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        ".pen_enter_signal"
    );

    PenEnterService()
    {
        this.ShowInTaskbar = false;
        this.WindowState = FormWindowState.Minimized;
        this.FormBorderStyle = FormBorderStyle.None;
        this.Opacity = 0;

        var checker = new System.Windows.Forms.Timer();
        checker.Interval = 50;
        checker.Tick += (s, e) =>
        {
            if (File.Exists(signalFile))
            {
                try { File.Delete(signalFile); } catch { return; }
                // Fire Enter globally to wherever cursor is active
                keybd_event(0x0D, 0x1C, 0, UIntPtr.Zero);
                keybd_event(0x0D, 0x1C, 2, UIntPtr.Zero);
            }
        };
        checker.Start();
    }

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.Run(new PenEnterService());
    }
}
