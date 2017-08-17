@echo off
echo *** Building Microsoft.Bot.Builder.Extensions.Telemetry.DebugWriter
setlocal
setlocal enabledelayedexpansion
setlocal enableextensions
set errorlevel=0
mkdir ..\nuget
erase /s ..\nuget\Microsoft.Bot.Builder.Extensions.Telemetry.DebugWriter*nupkg
msbuild /property:Configuration=release Microsoft.Bot.Builder.Extensions.Telemetry.DebugWriter.csproj 
for /f %%v in ('powershell -noprofile "(Get-Command .\bin\release\Microsoft.Bot.Builder.Extensions.Telemetry.dll).FileVersionInfo.FileVersion"') do set telemetry=%%v
for /f %%v in ('powershell -noprofile "(Get-Command .\bin\release\Microsoft.Bot.Builder.Extensions.Telemetry.DebugWriter.dll).FileVersionInfo.FileVersion"') do set version=%%v
..\..\packages\NuGet.CommandLine.4.1.0\tools\NuGet.exe pack Microsoft.Bot.Builder.Extensions.Telemetry.DebugWriter.nuspec -symbols -properties version=%version%;telemetry=%telemetry% -OutputDirectory ..\nuget
echo *** Finished building Microsoft.Bot.Builder.Extensions.Telemetry.DebugWriter

