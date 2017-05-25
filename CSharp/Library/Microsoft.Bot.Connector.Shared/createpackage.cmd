@echo off
echo *** Building Microsoft.Bot.Connector.Common
setlocal
setlocal enabledelayedexpansion
setlocal enableextensions
set errorlevel=0
mkdir ..\nuget
erase /s ..\nuget\Microsoft.Bot.Connector.Common*nupkg
dotnet build --configuration Release Microsoft.Bot.Connector.Common.csproj
for /f %%v in ('powershell -noprofile "(Get-Command .\bin\release\Microsoft.Bot.Connector.Common.dll).FileVersionInfo.FileVersion"') do set version=%%v
dotnet pack Microsoft.Bot.Connector.Common.csproj --include-symbols -properties version=%version% --output ..\nuget
echo *** Finished building Microsoft.Bot.Connector.Common
