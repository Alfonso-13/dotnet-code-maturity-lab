param(
    [string]$OutputPath = "artifacts/github-pages",
    [string]$RepositoryName = "dotnet-code-maturity-lab"
)

$ErrorActionPreference = "Stop"
$projectRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$publishPath = [System.IO.Path]::GetFullPath((Join-Path $projectRoot $OutputPath))
$stagingPath = "$publishPath.staging"
$projectPath = Join-Path $projectRoot "GitHubPages/DotnetCodeMaturityLab.GitHubPages.csproj"
$pathSeparators = [char[]]@(
    [System.IO.Path]::DirectorySeparatorChar,
    [System.IO.Path]::AltDirectorySeparatorChar
)
$projectRootPrefix = [System.IO.Path]::GetFullPath($projectRoot).TrimEnd($pathSeparators) +
    [System.IO.Path]::DirectorySeparatorChar

foreach ($path in @($publishPath, $stagingPath)) {
    if (-not $path.StartsWith($projectRootPrefix, [System.StringComparison]::OrdinalIgnoreCase)) {
        throw "La ruta de publicación debe permanecer dentro del repositorio: $path"
    }
}

if (Test-Path -LiteralPath $stagingPath) {
    Remove-Item -LiteralPath $stagingPath -Recurse -Force
}

dotnet publish $projectPath -c Release -o $stagingPath --nologo
if ($LASTEXITCODE -ne 0) {
    throw "dotnet publish terminó con el código $LASTEXITCODE. No se reemplazó el artefacto anterior."
}

$webRoot = Join-Path $stagingPath "wwwroot"
$requiredFiles = @("index.html", "404.html", ".nojekyll", "_framework/blazor.webassembly.js")

foreach ($relativePath in $requiredFiles) {
    $requiredPath = Join-Path $webRoot $relativePath
    if (-not (Test-Path -LiteralPath $requiredPath -PathType Leaf)) {
        throw "El artefacto de GitHub Pages no contiene el archivo requerido: $relativePath"
    }
}

$indexPath = Join-Path $webRoot "index.html"
$index = Get-Content -LiteralPath $indexPath -Raw
$pagesBase = '<base href="/{0}/" />' -f $RepositoryName
$defaultBase = '<base href="/" />'

if (-not $index.Contains($defaultBase, [System.StringComparison]::Ordinal)) {
    throw "No se encontró el base href esperado en index.html."
}

$index = $index.Replace($defaultBase, $pagesBase)
Set-Content -LiteralPath $indexPath -Value $index -NoNewline

if (Test-Path -LiteralPath $publishPath) {
    Remove-Item -LiteralPath $publishPath -Recurse -Force
}

Move-Item -LiteralPath $stagingPath -Destination $publishPath

Write-Host "GitHub Pages preparado en: $publishPath/wwwroot"
Write-Host "Base path configurado: /$RepositoryName/"
