#addin "nuget:?package=Cake.Incubator&version=2.0.0"
#tool "nuget:?package=xunit.runner.console"

var nugetSources = new[] {"https://api.nuget.org/v3/index.json"};

var target = Argument("target", "Default");

var solutionFilePath = "../src/ByteDev.DotNet.sln";

var artifactsDirectory = Directory("../artifacts");
var nugetDirectory = artifactsDirectory + Directory("NuGet");
	
// Configuration - The build configuration (Debug/Release) to use.
// 1. If command line parameter parameter passed, use that.
// 2. Otherwise if an Environment variable exists, use that.
var configuration = 
    HasArgument("Configuration") ? Argument<string>("Configuration") :
    EnvironmentVariable("Configuration") != null ? EnvironmentVariable("Configuration") : "Release";
	
Information("Configurtion: " + configuration);


Task("Clean")
    .Does(() =>
{
    CleanDirectory(artifactsDirectory);
	
	//var binDirectoriesToClean = GetDirectories("../src/**/bin");
	//var objDirectoriesToClean = GetDirectories("../src/**/obj");

	//CleanDirectories(binDirectoriesToClean);
	//CleanDirectories(objDirectoriesToClean);
});

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
    {
		var settings = new NuGetRestoreSettings
		{
			Source = nugetSources
		};

		NuGetRestore(solutionFilePath, settings);
    });

Task("Build")
	.IsDependentOn("Restore")
    .Does(() =>
	{	
        DotNetCoreBuild(
            solutionFilePath,
            new DotNetCoreBuildSettings()
            {
                Configuration = configuration
            });
	});

Task("UnitTests")
    .IsDependentOn("Build")
    .Does(() =>
	{
		var projects = GetFiles("../src/*UnitTests/**/*.csproj");
		
		foreach(var project in projects)
		{
			DotNetCoreTest(
				project.FullPath,
				new DotNetCoreTestSettings()
				{
					Configuration = configuration,
					NoBuild = true
				});
		}
	});
	
Task("IntegrationTests")
    .IsDependentOn("UnitTests")
    .Does(() =>
	{
		var projects = GetFiles("../src/*IntTests/**/*.csproj");
		
		foreach(var project in projects)
		{
			DotNetCoreTest(
				project.FullPath,
				new DotNetCoreTestSettings()
				{
					Configuration = configuration,
					NoBuild = true
				});
		}
	});
	
Task("CreateNuGetPackages")
    .IsDependentOn("IntegrationTests")
    .Does(() =>
    {
        var settings = new DotNetCorePackSettings()
		{
			Configuration = configuration,
			OutputDirectory = nugetDirectory
		};
                
		DotNetCorePack("../src/ByteDev.DotNet/ByteDev.DotNet.csproj", settings);
    });

   
Task("Default")
    .IsDependentOn("CreateNuGetPackages");

RunTarget(target);