# Install script for Surface Pen Tools
# Copies executables to user profile and sets up auto-start

$ErrorActionPreference = "Stop"
$binDir = "$PSScriptRoot\bin"
$installDir = $env:USERPROFILE

if (!(Test-Path "$binDir\PenEnter.exe")) {
    Write-Host "Executables not found. Run build.ps1 first." -ForegroundColor Red
    exit 1
}

Write-Host "Installing Surface Pen Tools..." -ForegroundColor Cyan

# Stop existing service
Stop-Process -Name PenEnterService -Force -ErrorAction SilentlyContinue
Start-Sleep 1

# Copy executables
$exes = @("PenEnter.exe", "PenEnterService.exe", "VoiceTyping.exe", "PenNumPad.exe")
foreach ($exe in $exes) {
    Copy-Item "$binDir\$exe" "$installDir\$exe" -Force
    Write-Host "  Copied $exe to $installDir"
}

# Add PenEnterService to startup
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Run" /v PenEnterService /t REG_SZ /d "$installDir\PenEnterService.exe" /f | Out-Null
Write-Host "  Added PenEnterService to startup"

# Start the service
Start-Process "$installDir\PenEnterService.exe"
Write-Host "  Started PenEnterService"

Write-Host "`nInstallation complete!" -ForegroundColor Green
Write-Host "`nConfigure pen buttons in Settings > Bluetooth & devices > Pen & Windows Ink:"
Write-Host "  Single click  -> $installDir\PenEnter.exe"
Write-Host "  Double click  -> $installDir\VoiceTyping.exe"
Write-Host "  Long press    -> $installDir\PenNumPad.exe"
