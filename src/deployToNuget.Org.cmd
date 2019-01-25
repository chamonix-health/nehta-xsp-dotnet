del *.nupkg

msbuild /p:Configuration=Release

NuGet.exe pack Nehta.Xsp\Nehta.Xsp.csproj -Properties Configuration=Release

pause

forfiles /m *.nupkg /c "cmd /c NuGet.exe push @FILE -Source https://www.nuget.org/api/v2/package"

