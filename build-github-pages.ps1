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

# Both deletion targets are derived from command arguments. Resolve and constrain them
# before any recursive operation so the script cannot remove data outside the repository.
foreach ($path in @($publishPath, $stagingPath)) {
    if (-not $path.StartsWith($projectRootPrefix, [System.StringComparison]::OrdinalIgnoreCase)) {
        throw "La ruta de publicación debe permanecer dentro del repositorio: $path"
    }
}

if (Test-Path -LiteralPath $stagingPath) {
    Remove-Item -LiteralPath $stagingPath -Recurse -Force
}

# Build in staging and keep the previous artifact intact until every contract below passes.
dotnet publish $projectPath -c Release -o $stagingPath --nologo
if ($LASTEXITCODE -ne 0) {
    throw "dotnet publish terminó con el código $LASTEXITCODE. No se reemplazó el artefacto anterior."
}

$webRoot = Join-Path $stagingPath "wwwroot"
$requiredFiles = @(
    "index.html",
    "404.html",
    ".nojekyll",
    "_framework/blazor.webassembly.js",
    "robots.txt",
    "sitemap.xml",
    "site.webmanifest",
    "images/dotnet-code-maturity-lab-social.jpg"
)

foreach ($relativePath in $requiredFiles) {
    $requiredPath = Join-Path $webRoot $relativePath
    if (-not (Test-Path -LiteralPath $requiredPath -PathType Leaf)) {
        throw "El artefacto de GitHub Pages no contiene el archivo requerido: $relativePath"
    }
}

$fallbackPath = Join-Path $webRoot "404.html"
$fallback = Get-Content -LiteralPath $fallbackPath -Raw
# Direct Pages routes depend on this fallback. Validate its known failure mode and its
# redirect marker before publishing an artifact that would turn refreshes into 404s.
if ($fallback.Contains("const location = window.location;", [System.StringComparison]::Ordinal)) {
    throw "404.html redeclara el global location y rompería la recuperación de rutas profundas."
}
if (-not $fallback.Contains("currentLocation.replace(", [System.StringComparison]::Ordinal)) {
    throw "404.html no contiene la redirección esperada para rutas profundas."
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

# Search engines and link unfurlers do not execute Blazor. The published HTML must
# therefore carry complete canonical and social metadata before the artifact ships.
$seoContracts = @(
    'rel="canonical" href="https://alfonso-13.github.io/dotnet-code-maturity-lab/"',
    'name="twitter:card" content="summary_large_image"',
    'property="og:image" content="https://alfonso-13.github.io/dotnet-code-maturity-lab/images/dotnet-code-maturity-lab-social.jpg"',
    'property="og:image:width" content="1200"',
    'property="og:image:height" content="630"'
)

foreach ($contract in $seoContracts) {
    if (-not $index.Contains($contract, [System.StringComparison]::Ordinal)) {
        throw "index.html no cumple el contrato SEO requerido: $contract"
    }
}

$socialImagePath = Join-Path $webRoot "images/dotnet-code-maturity-lab-social.jpg"
if ((Get-Item -LiteralPath $socialImagePath).Length -lt 50000) {
    throw "La imagen social parece incompleta o excesivamente comprimida."
}

# Only a validated staging build is allowed to replace the last usable artifact.
if (Test-Path -LiteralPath $publishPath) {
    Remove-Item -LiteralPath $publishPath -Recurse -Force
}

Move-Item -LiteralPath $stagingPath -Destination $publishPath

Write-Host "GitHub Pages preparado en: $publishPath/wwwroot"
Write-Host "Base path configurado: /$RepositoryName/"
