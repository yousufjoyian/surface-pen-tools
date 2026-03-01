# Surface Pen Tools

Custom utilities for Surface Pen button actions on Windows 11. Lightweight C# executables that extend the pen's functionality beyond what Windows offers natively.

## Tools

| Tool | Description |
|------|-------------|
| **PenEnter** | Sends Enter keystroke to the active window via a background service |
| **VoiceTyping** | Toggles Windows Voice Typing (Win+H) - dictate text anywhere |
| **PenNumPad** | Floating dark-themed number pad for quick numeric input |

## Pen Button Mapping

| Action | Function |
|--------|----------|
| Single click | Enter key |
| Double click | Voice Typing (Win+H) |
| Long press | Floating Number Pad |

## How It Works

- **VoiceTyping** and **PenNumPad** are standalone executables that run and exit
- **PenEnter** uses a two-part architecture because launching an exe from the pen steals window focus:
  - `PenEnterService.exe` — invisible background service that polls for a signal file every 50ms and fires an Enter keystroke globally
  - `PenEnter.exe` — tiny app that creates the signal file and exits immediately

## Setup

### Build

```powershell
powershell -ExecutionPolicy Bypass -File build.ps1
```

### Install

```powershell
powershell -ExecutionPolicy Bypass -File install.ps1
```

This copies executables to your user profile and adds `PenEnterService` to Windows startup.

### Configure Pen Buttons

Open **Settings > Bluetooth & devices > Pen & Windows Ink** and set:
- Single click → `C:\Users\<username>\PenEnter.exe`
- Double click → `C:\Users\<username>\VoiceTyping.exe`
- Long press → `C:\Users\<username>\PenNumPad.exe`

### Uninstall

```powershell
powershell -ExecutionPolicy Bypass -File uninstall.ps1
```

## Requirements

- Windows 10/11
- Surface device with Surface Pen (or compatible stylus)
- .NET Framework (included with Windows)

## Built With

Compiled using PowerShell's `Add-Type` with inline C# — no Visual Studio or .NET SDK required.
