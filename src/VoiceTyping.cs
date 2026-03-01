using System;
using System.Runtime.InteropServices;

/// <summary>
/// Sends Win+H keystroke to toggle Windows Voice Typing.
/// Works globally regardless of which window is focused.
/// </summary>
class VoiceTyping
{
    [DllImport("user32.dll")]
    static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

    const byte VK_LWIN = 0x5B;
    const byte VK_H = 0x48;
    const uint KEYEVENTF_KEYUP = 0x0002;

    static void Main()
    {
        keybd_event(VK_LWIN, 0, 0, UIntPtr.Zero);
        keybd_event(VK_H, 0, 0, UIntPtr.Zero);
        keybd_event(VK_H, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
        keybd_event(VK_LWIN, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
    }
}
