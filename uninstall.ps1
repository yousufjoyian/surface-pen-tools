# Uninstall script for Surface Pen Tools

$installDir = $env:USERPROFILE

Write-Host "Uninstalling Surface Pen Tools..." -ForegroundColor Cyan

# Stop service
Stop-Process -Name PenEnterService -Force -ErrorAction SilentlyContinue

# Remove from startup
reg delete "HKCU\Software\Microsoft\Windows\CurrentVersion\Run" /v PenEnterService /f 2>$null

# Remove executables
$exes = @("PenEnter.exe", "PenEnterService.exe", "VoiceTyping.exe", "PenNumPad.exe")
foreach ($exe in $exes) {
    if (Test-Path "$installDir\$exe") {
        Remove-Item "$installDir\$exe" -Force
        Write-Host "  Removed $exe"
    }
}

# Remove signal file
Remove-Item "$installDir\.pen_enter_signal" -Force -ErrorAction SilentlyContinue

Write-Host "`nUninstall complete." -ForegroundColor Green
Write-Host "Remember to reset pen buttons in Settings > Bluetooth & devices > Pen & Windows Ink."
