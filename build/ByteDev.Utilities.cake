string GetBuildConfiguration()
{
	if(HasArgument("Configuration"))
	{
		return Argument<string>("Configuration");
	}

	return EnvironmentVariable("Configuration") != null ? 
		EnvironmentVariable("Configuration") : 
		"Release";
}

string GetNuGetVersion()
{
	var settings = new GitVersionSettings
	{
		OutputType = GitVersionOutput.Json
	};

	GitVersion versionInfo = GitVersion(settings);

	Information("GitVersion:");
	Information(versionInfo.Dump<GitVersion>());

	return versionInfo.NuGetVersion;
}

// -----

void CleanObjDirectories()
{
	CleanDirectories(GetDirectories("../src/**/obj"));
	CleanDirectories(GetDirectories("../tests/**/obj"));
}

void CleanBinDirectories()
{	
	CleanDirectories(GetDirectories("../src/**/bin"));
	CleanDirectories(GetDirectories("../tests/**/bin"));
}

// -----

FilePathCollection GetUnitTestProjFiles()
{
	return GetFiles("../tests/*.UnitTests/**/*.csproj");
}

FilePathCollection GetIntTestProjFiles()
{
	return GetFiles("../tests/*.IntTests/**/*.csproj");
}

FilePathCollection GetPackageTestProjFiles()
{
	return GetFiles("../tests/*.PackageTests/*.csproj");
}

// -----

void DotNetTests(FilePathCollection projects, DotNetTestSettings settings)
{
	foreach(var project in projects)
	{
		DotNetTest(project.FullPath, settings);
	}
}

void DotNetUnitTests(DotNetTestSettings settings)
{
	var projects = GetUnitTestProjFiles();

	DotNetTests(projects, settings);
}

void DotNetIntTests(DotNetTestSettings settings)
{
	var projects = GetIntTestProjFiles();

	DotNetTests(projects, settings);
}

void DotNetPackageTests(DotNetTestSettings settings)
{
	var projects = GetPackageTestProjFiles();

	DotNetTests(projects, settings);
}

// -----

void NetFrameworkUnitTests(string configuration)
{
	var assemblies = GetFiles($"../tests/*UnitTests/bin/{configuration}/**/*.UnitTests.dll");
		
	NUnit3(assemblies);
}

void NetFrameworkIntTests(string configuration)
{
	var assemblies = GetFiles($"../tests/*IntTests/bin/{configuration}/**/*.IntTests.dll");
		
	NUnit3(assemblies);
}

void NetFrameworkPackageTests(string configuration)
{
	var assemblies = GetFiles($"../tests/*.Net4*.PackageTests/bin/{configuration}/**/*.PackageTests.dll");
		
	NUnit3(assemblies);
}