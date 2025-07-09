@echo off
REM This script requires several tools to be installed for it to work:

REM Nerdbank.GitVersioning (nbgv): dotnet tool install --global nbgv
REM C++ Build Tools, typically installed via "Desktop development with C++" workload.

setlocal enabledelayedexpansion

if "%~1"=="" (
    echo Version number is required.
    echo Usage: build.bat [version] [extra_args...]
    exit /b 1
)

cd %~dp0..\..\..\



REM echo.
REM echo Building Velopack Vpk
REM dotnet build src/vpk/Velopack.Vpk/Velopack.Vpk.csproj
REM if errorlevel 1 exit /b 1

cd %~dp0..
set "version=%~1"

echo.
echo Compiling VelopackCSharpAvalonia with dotnet...
dotnet publish GetStartedApp.csproj -c Release --self-contained -r win-x64 -o publish -p:UseLocalVelopack=true
if errorlevel 1 exit /b 1

echo.
echo Building Velopack Release v%version%
vpk pack -u GetStartedApp -o releases -p publish -v %*
if errorlevel 1 exit /b 1