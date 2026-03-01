using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

/// <summary>
/// Floating dark-themed number pad.
/// Sends number keystrokes to the previously active window.
/// Press Escape to close.
/// </summary>
public class PenNumPad : Form
{
    [DllImport("user32.dll")]
    static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    static extern bool SetForegroundWindow(IntPtr hWnd);

    private IntPtr previousWindow;

    public PenNumPad()
    {
        previousWindow = GetForegroundWindow();
        this.Text = "Num";
        this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.TopMost = true;
        this.ShowInTaskbar = false;
        this.Width = 200;
        this.Height = 280;
        this.BackColor = Color.FromArgb(30, 30, 30);

        string[] labels = { "7", "8", "9", "4", "5", "6", "1", "2", "3", "0", ".", "<-" };

        for (int i = 0; i < 12; i++)
        {
            Button btn = new Button();
            btn.Text = labels[i];
            btn.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            btn.ForeColor = Color.White;
            btn.BackColor = Color.FromArgb(55, 55, 55);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = Color.FromArgb(80, 80, 80);
            int col = i % 3;
            int row = i / 3;
            btn.SetBounds(10 + col * 60, 10 + row * 60, 55, 55);
            string lbl = labels[i];
            btn.Click += (s, e) =>
            {
                SetForegroundWindow(previousWindow);
                System.Threading.Thread.Sleep(50);
                if (lbl == "<-")
                {
                    keybd_event(0x08, 0, 0, UIntPtr.Zero);
                    keybd_event(0x08, 0, 2, UIntPtr.Zero);
                }
                else if (lbl == ".")
                {
                    keybd_event(0xBE, 0, 0, UIntPtr.Zero);
                    keybd_event(0xBE, 0, 2, UIntPtr.Zero);
                }
                else
                {
                    byte k = (byte)(0x30 + int.Parse(lbl));
                    keybd_event(k, 0, 0, UIntPtr.Zero);
                    keybd_event(k, 0, 2, UIntPtr.Zero);
                }
                this.Activate();
            };
            this.Controls.Add(btn);
        }

        this.KeyPreview = true;
        this.KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) this.Close(); };
    }

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.Run(new PenNumPad());
    }
}
