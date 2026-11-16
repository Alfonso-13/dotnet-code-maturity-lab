param(
    [string]$OutputPath = "artifacts/github-pages",
    [string]$RepositoryName = "dotnet-code-maturity-lab"
)

$ErrorActionPreference = "Stop"
$projectRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$publishPath = Join-Path $projectRoot $OutputPath
$projectPath = Join-Path $projectRoot "GitHubPages/DotnetCodeMaturityLab.GitHubPages.csproj"

dotnet publish $projectPath -c Release -o $publishPath

$indexPath = Join-Path $publishPath "wwwroot/index.html"
$index = Get-Content -LiteralPath $indexPath -Raw
$pagesBase = '<base href="/{0}/" />' -f $RepositoryName
$index = $index.Replace('<base href="/" />', $pagesBase)
Set-Content -LiteralPath $indexPath -Value $index -NoNewline

Write-Host "GitHub Pages preparado en: $publishPath/wwwroot"
Write-Host "Base path configurado: /$RepositoryName/"
