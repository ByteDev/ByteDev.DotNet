#addin "nuget:?package=Cake.Incubator&version=8.0.0"
#tool "nuget:?package=NUnit.ConsoleRunner&version=3.16.3"
#tool "nuget:?package=GitVersion.CommandLine&version=5.12.0"
#load "ByteDev.Utilities.cake"

var solutionName = "ByteDev.DotNet";
var projName = "ByteDev.DotNet";

var solutionFilePath = "../" + solutionName + ".sln";
var nuspecFilePath = projName + ".nuspec";

var target = Argument("target", "Default");

var artifactsDirectory = Directory("../artifacts");
var nugetDirectory = artifactsDirectory + Directory("NuGet");

var configuration = GetBuildConfiguration();

Information("Configurtion: " + configuration);

Task("Clean")
    .Does(() =>
	{
		CleanDirectory(artifactsDirectory);
	
		CleanBinDirectories();
		CleanObjDirectories();
	});

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
    {
		var settings = new NuGetRestoreSettings
		{
			Source = new[] { "https://api.nuget.org/v3/index.json" }
		};

		NuGetRestore(solutionFilePath, settings);
    });

Task("Build")
	.IsDependentOn("Restore")
    .Does(() =>
	{	
		var settings = new DotNetBuildSettings
        {
            Configuration = configuration
        };

        DotNetBuild(solutionFilePath, settings);
	});

Task("UnitTests")
    .IsDependentOn("Build")
    .Does(() =>
	{
		var settings = new DotNetTestSettings
		{
			Configuration = configuration,
			NoBuild = true
		};

		DotNetUnitTests(settings);
	});

Task("IntegrationTests")
    .IsDependentOn("UnitTests")
    .Does(() =>
	{
		var settings = new DotNetTestSettings()
		{
			Configuration = configuration,
			NoBuild = true
		};

		DotNetIntTests(settings);
	});
	
Task("CreateNuGetPackages")
    .IsDependentOn("IntegrationTests")
    .Does(() =>
    {
		var nugetVersion = GetNuGetVersion();

		var nugetSettings = new NuGetPackSettings 
		{
			Version = nugetVersion,
			OutputDirectory = nugetDirectory
		};
                
		NuGetPack(nuspecFilePath, nugetSettings);
    });

   
Task("Default")
    .IsDependentOn("CreateNuGetPackages");

RunTarget(target);
