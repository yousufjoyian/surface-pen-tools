using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

/// <summary>
/// Background service that watches for a signal file and sends an Enter keystroke
/// to the last active user window. Runs invisibly with no taskbar icon or window.
///
/// Architecture: PenEnter.exe creates a signal file when the pen button is clicked.
/// This service detects the file, restores focus to the user's last active window,
/// and sends the Enter keystroke. This two-part design is needed because launching
/// any exe from the pen button briefly steals window focus.
/// </summary>
class PenEnterService : Form
{
    [DllImport("user32.dll")]
    static extern void keybd_event(byte vk, byte scan, uint flags, UIntPtr extra);

    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    static extern bool SetForegroundWindow(IntPtr h);

    [DllImport("user32.dll")]
    static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

    [DllImport("user32.dll")]
    static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint pid);

    [DllImport("kernel32.dll")]
    static extern uint GetCurrentThreadId();

    static IntPtr lastUserWindow = IntPtr.Zero;
    static IntPtr myHandle = IntPtr.Zero;
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
        myHandle = this.Handle;

        // Track the foreground window every 100ms
        // Ignores our own window and PenEnter.exe
        var tracker = new System.Windows.Forms.Timer();
        tracker.Interval = 100;
        tracker.Tick += (s, e) =>
        {
            IntPtr fg = GetForegroundWindow();
            if (fg != IntPtr.Zero && fg != myHandle)
            {
                uint pid;
                GetWindowThreadProcessId(fg, out pid);
                try
                {
                    Process p = Process.GetProcessById((int)pid);
                    string name = p.ProcessName.ToLower();
                    if (name != "penenterservice" && name != "penenter")
                    {
                        lastUserWindow = fg;
                    }
                }
                catch
                {
                    lastUserWindow = fg;
                }
            }
        };
        tracker.Start();

        // Check for signal file
        var checker = new System.Windows.Forms.Timer();
        checker.Interval = 50;
        checker.Tick += (s, e) =>
        {
            if (File.Exists(signalFile))
            {
                try { File.Delete(signalFile); } catch { return; }

                if (lastUserWindow != IntPtr.Zero)
                {
                    // Wait for PenEnter.exe to fully exit and focus to settle
                    Thread.Sleep(400);

                    // ALT trick to bypass Windows' foreground window restriction
                    keybd_event(0xA4, 0, 0, UIntPtr.Zero);
                    keybd_event(0xA4, 0, 2, UIntPtr.Zero);

                    // Attach to target thread for reliable focus switch
                    uint pid;
                    uint targetThread = GetWindowThreadProcessId(lastUserWindow, out pid);
                    uint myThread = GetCurrentThreadId();
                    AttachThreadInput(myThread, targetThread, true);

                    // Restore focus to user's window, then send Enter
                    SetForegroundWindow(lastUserWindow);
                    Thread.Sleep(100);
                    keybd_event(0x0D, 0x1C, 0, UIntPtr.Zero);
                    keybd_event(0x0D, 0x1C, 2, UIntPtr.Zero);

                    AttachThreadInput(myThread, targetThread, false);
                }
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
