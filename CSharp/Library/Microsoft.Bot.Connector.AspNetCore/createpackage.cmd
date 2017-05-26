@echo off
setlocal
setlocal enabledelayedexpansion
setlocal enableextensions
set errorlevel=0
mkdir ..\nuget
erase /s ..\nuget\Microsoft.Bot.Connector.AspNetCore*nupkg
dotnet build --configuration Release Microsoft.Bot.Connector.AspNetCore.csproj
for /f %%v in ('powershell -noprofile "(Get-Command .\bin\release\Microsoft.Bot.Connector.AspNetCore.dll).FileVersionInfo.FileVersion"') do set version=%%v
for /f %%v in ('powershell -noprofile "(Get-Command .\bin\release\Microsoft.Bot.Connector.Common.dll).FileVersionInfo.FileVersion"') do set connectorCommonVersion=%%v
dotnet pack Microsoft.Bot.Connector.AspNetCore.csproj --include-symbols -properties version=%version%;connectorCommon=%connectorCommonVersion% --output ..\nuget
