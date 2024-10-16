# Downloads the lastest version of nuget.exe

$sourceNugetExe = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"
$targetNugetPath = ".\tools"
$targetNugetExe =  $targetNugetPath + "\nuget.exe"

# Create dir if not exist
if (Test-Path $targetNugetPath) 
{
	Write-Output "$targetNugetPath path exists"
}
else
{
	Write-Output "Creating path: $targetNugetPath"
    New-Item $targetNugetPath -ItemType Directory
}

Write-Output "Downloading nuget.exe ..."
Invoke-WebRequest $sourceNugetExe -OutFile $targetNugetExe
Write-Output "Downloaded latest nuget.exe"

# Print the version details of nuget.exe
& $targetNugetExe help | select -First 1

Write-Output ""