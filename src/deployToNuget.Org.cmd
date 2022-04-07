del *.nupkg

dotnet pack .\Nehta.Xsp\Nehta.Xsp.csproj -c Release -o .

pause

forfiles /m *.nupkg /c "cmd /c NuGet.exe push @FILE -Source https://www.nuget.org/api/v2/package"

