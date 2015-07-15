@echo off
cd ..\\.nuget
rm UOKO.WebAPI.Tools.*.nupkg
call .\nuget.exe pack ..\UOKO.WebAPI.Tools\UOKO.WebAPI.Tools.csproj -Build
.\nuget.exe push .\UOKO.WebAPI.Tools.*.nupkg bc14f17c129b45a2acc2eff1fffb495f