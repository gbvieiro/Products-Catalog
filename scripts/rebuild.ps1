<#
.SYNOPSIS
    Reconstroi as imagens do Products Catalog do zero e sobe o ambiente local.

.DESCRIPTION
    Util sempre que uma mudanca no codigo (Api ou frontend) nao aparecer depois
    de um "docker compose up" normal - isso costuma significar que o Docker
    reaproveitou uma imagem antiga em cache. Este script forca tudo a ser
    reconstruido do zero:
      1. docker compose down -v   -> para os containers e apaga o volume do
         Postgres (o schema e recriado na proxima subida - ver README.md,
         secao "Se voce ja rodou o projeto antes desta mudanca").
      2. docker compose build --no-cache -> builda as imagens ignorando
         qualquer cache de camada.
      3. docker compose up -> sobe tudo com as imagens novas.

.PARAMETER Service
    Nome de um servico especifico do docker-compose.yml para reconstruir
    (ex: "api", "client"). Sem esse parametro, reconstroi todos os servicos
    padrao (postgres/api/client).

.EXAMPLE
    ./scripts/rebuild.ps1
    Reconstroi tudo do zero (Postgres + Api + frontend).

.EXAMPLE
    ./scripts/rebuild.ps1 -Service api
    Reconstroi so a imagem da Api (mais rapido quando so o backend mudou).
#>
param(
    [string]$Service
)

$ErrorActionPreference = "Stop"

# Roda sempre a partir da raiz do repo (onde esta o docker-compose.yml),
# nao importa de onde o script foi chamado.
$repoRoot = Split-Path -Parent $PSScriptRoot
Push-Location $repoRoot
try {
    Write-Host "==> Parando containers e apagando o volume do Postgres (down -v)..." -ForegroundColor Cyan
    docker compose down -v
    if ($LASTEXITCODE -ne 0) { throw "docker compose down -v falhou (exit code $LASTEXITCODE)." }

    if ($Service) {
        Write-Host "==> Reconstruindo a imagem de '$Service' sem cache..." -ForegroundColor Cyan
        docker compose build --no-cache $Service
    } else {
        Write-Host "==> Reconstruindo todas as imagens sem cache..." -ForegroundColor Cyan
        docker compose build --no-cache
    }
    if ($LASTEXITCODE -ne 0) { throw "docker compose build falhou (exit code $LASTEXITCODE) - veja o erro de build acima." }

    Write-Host "==> Subindo o ambiente (Ctrl+C para parar)..." -ForegroundColor Cyan
    docker compose up
}
finally {
    Pop-Location
}
