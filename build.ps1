# Build script for Surface Pen Tools
# Compiles all C# source files into WindowsApplication executables

$ErrorActionPreference = "Stop"
$outDir = "$PSScriptRoot\bin"
$srcDir = "$PSScriptRoot\src"

if (!(Test-Path $outDir)) { New-Item -ItemType Directory -Path $outDir | Out-Null }

Write-Host "Building Surface Pen Tools..." -ForegroundColor Cyan

# PenEnter.exe - signal sender (no UI)
Write-Host "  Compiling PenEnter.exe..."
Add-Type -TypeDefinition (Get-Content "$srcDir\PenEnter.cs" -Raw) -OutputAssembly "$outDir\PenEnter.exe" -OutputType WindowsApplication

# PenEnterService.exe - background service
Write-Host "  Compiling PenEnterService.exe..."
Add-Type -TypeDefinition (Get-Content "$srcDir\PenEnterService.cs" -Raw) -OutputAssembly "$outDir\PenEnterService.exe" -OutputType WindowsApplication -ReferencedAssemblies System.Windows.Forms

# VoiceTyping.exe - Win+H toggle
Write-Host "  Compiling VoiceTyping.exe..."
Add-Type -TypeDefinition (Get-Content "$srcDir\VoiceTyping.cs" -Raw) -OutputAssembly "$outDir\VoiceTyping.exe" -OutputType WindowsApplication

# PenNumPad.exe - floating number pad
Write-Host "  Compiling PenNumPad.exe..."
Add-Type -TypeDefinition (Get-Content "$srcDir\PenNumPad.cs" -Raw) -OutputAssembly "$outDir\PenNumPad.exe" -OutputType WindowsApplication -ReferencedAssemblies System.Windows.Forms,System.Drawing

Write-Host "`nBuild complete! Executables are in: $outDir" -ForegroundColor Green
Get-ChildItem "$outDir\*.exe" | ForEach-Object { Write-Host "  $($_.Name) ($([math]::Round($_.Length/1KB)) KB)" }
