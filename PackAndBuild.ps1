Remove-Item .\nuget-packages\* -Recurse -Force

dotnet pack --configuration Release --output .\nuget-packages --version-suffix (Get-Date -Format "yyMMddhhmmss")

Copy-Item -Path "C:\Users\Evers\source\repos\NDK\nuget-packages\*" -Destination "C:\PrivateFeed" -Recurse -Force

