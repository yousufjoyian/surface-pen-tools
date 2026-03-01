using System;
using System.IO;

/// <summary>
/// Signal sender for PenEnterService.
/// Creates a signal file that the background service watches for.
/// When detected, the service fires an Enter keystroke to the active window.
/// </summary>
class PenEnter
{
    static void Main()
    {
        string signalFile = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".pen_enter_signal"
        );
        File.WriteAllText(signalFile, "go");
    }
}
