[CmdletBinding()]
param(
    [string]$Url = "http://localhost:5187"
)

$ErrorActionPreference = "Stop"
$projectPath = Join-Path $PSScriptRoot "DotnetCodeMaturityLab.csproj"

if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
    throw "No se encontro el SDK de .NET. Instala .NET 10 y vuelve a intentarlo."
}

Write-Host "Iniciando .NET Code Maturity Lab..." -ForegroundColor Red
Write-Host "URL: $Url"
Write-Host "Presiona Ctrl+C para detener la aplicacion.`n" -ForegroundColor DarkGray

$browserJob = Start-Job -ScriptBlock {
    param($TargetUrl)

    for ($attempt = 1; $attempt -le 60; $attempt++) {
        try {
            Invoke-WebRequest -Uri $TargetUrl -UseBasicParsing -TimeoutSec 1 | Out-Null
            Start-Process $TargetUrl
            return
        }
        catch {
            Start-Sleep -Milliseconds 500
        }
    }
} -ArgumentList $Url

try {
    $env:ASPNETCORE_ENVIRONMENT = "Development"
    dotnet run --project $projectPath --no-launch-profile --urls $Url
}
finally {
    Stop-Job -Job $browserJob -ErrorAction SilentlyContinue
    Remove-Job -Job $browserJob -Force -ErrorAction SilentlyContinue
}
