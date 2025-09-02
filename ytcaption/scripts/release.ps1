param(
    [Parameter(Mandatory = $true)][string]$Version,
    [string]$Runtime = 'win-x64',
    [string]$Configuration = 'Release'
)

$ErrorActionPreference = 'Stop'

$project = Join-Path $PSScriptRoot '..' 'YtCaption.Wpf' 'YtCaption.Wpf.csproj'
$repoRoot = Resolve-Path (Join-Path $PSScriptRoot '..')
$publishDir = Join-Path $repoRoot "YtCaption.Wpf/bin/$Configuration/net8.0-windows/$Runtime/publish"
$distDir = Join-Path $repoRoot 'dist'

Write-Host "[1/3] Publishing SCD single-file ($Runtime) ..." -ForegroundColor Cyan
dotnet publish $project -c $Configuration -r $Runtime --self-contained true `
  -p:PublishSingleFile=true -p:PublishReadyToRun=true -p:PublishTrimmed=false `
  -p:DebugType=None -p:DebugSymbols=false `
  -p:Version=$Version -p:FileVersion=$Version -p:AssemblyVersion=$Version

if (-not (Test-Path $publishDir)) {
    throw "Publish directory not found: $publishDir"
}

$exePath = Join-Path $publishDir 'YtCaption.Wpf.exe'
if (-not (Test-Path $exePath)) {
    throw "Executable not found: $exePath"
}

if (-not (Test-Path $distDir)) { New-Item -ItemType Directory -Path $distDir | Out-Null }
$distReadme = Join-Path $distDir 'README.txt'
if (-not (Test-Path $distReadme)) {
    Write-Warning "dist/README.txt not found; creating a minimal placeholder."
    @(
      'YT Caption Utility — Distribution Notes',
      'Run YtCaption.Wpf.exe; settings at %AppData%/YtCaption.Wpf/settings.json'
    ) | Set-Content -Path $distReadme -Encoding UTF8
}

$zipName = "ytcaption-v$Version-$Runtime-scd-single.zip"
$zipPath = Join-Path $repoRoot $zipName

Write-Host "[2/3] Packaging ZIP -> $zipPath" -ForegroundColor Cyan
if (Test-Path $zipPath) { Remove-Item $zipPath -Force }
Compress-Archive -Path $exePath,$distReadme -DestinationPath $zipPath

Write-Host "[3/3] Done" -ForegroundColor Green
Write-Host "Output: $zipPath"

# Optional: Code signing step (uncomment and configure certificate)
# & signtool sign /tr http://timestamp.digicert.com /td sha256 /fd sha256 /a "$exePath"

