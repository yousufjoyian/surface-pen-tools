using System;
using System.IO;
using System.Threading;

/// <summary>
/// Waits for focus to return to the user's app, then creates a signal file
/// that PenEnterService watches for. The delay happens here (inside the
/// pen-launched exe) so that by the time the signal is sent, this process
/// has already taken and released focus, and the user's window is active again.
/// </summary>
class PenEnter
{
    static void Main()
    {
        // Wait for Windows to finish launching this process and return
        // focus to the previous window
        Thread.Sleep(400);

        string signalFile = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".pen_enter_signal"
        );
        File.WriteAllText(signalFile, "go");
    }
}
