[![Build status](https://ci.appveyor.com/api/projects/status/github/bytedev/ByteDev.DotNet?branch=master&svg=true)](https://ci.appveyor.com/project/bytedev/ByteDev-DotNet/branch/master)
[![NuGet Package](https://img.shields.io/nuget/v/ByteDev.DotNet.svg)](https://www.nuget.org/packages/ByteDev.DotNet)
[![Codacy Badge](https://api.codacy.com/project/badge/Grade/d7286791f04843528797e2a48e72a070)](https://www.codacy.com/manual/ByteDev/ByteDev.DotNet?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=ByteDev/ByteDev.DotNet&amp;utm_campaign=Badge_Grade)

# ByteDev.DotNet

Set of classes for reading .NET solution and project files.

## Installation

ByteDev.DotNet has been written as a .NET Standard 2.0 library, so you can consume it from a .NET Core or .NET Framework 4.6.1 (or greater) application.

ByteDev.DotNet is hosted as a package on nuget.org.  To install from the Package Manager Console in Visual Studio run:

`Install-Package ByteDev.DotNet`

Further details can be found on the [nuget page](https://www.nuget.org/packages/ByteDev.DotNet/).

## Release Notes

Releases follow semantic versioning.

Full details of the release notes can be viewed on [GitHub](https://github.com/ByteDev/ByteDev.DotNet/blob/master/docs/RELEASE-NOTES.md).

## Usage

There are two main classes in the project: `DotNetSolution` and `DotNetProject`.

### DotNetSolution

Use `DotNetSolution` from the namespace: `ByteDev.DotNet.Solution`.

Load a .NET solution file:

```csharp
DotNetSolution dotNetSolution = DotNetSolution.Load(@"C:\mysolution.sln");

Console.WriteLine(dotNetSolution.VisualStudioVersion);
Console.WriteLine(dotNetSolution.Projects.Count);
```

`DotNetSolution` contains the following properties:
- FormatVersion
- MajorVersion
- VisualStudioVersion
- MinimumVisualStudioVersion
- Projects
- GlobalSections


### DotNetProject

Use `DotNetProject` from the namespace: `ByteDev.DotNet.Project`.

Load a .NET project file:

```csharp
DotNetProject dotNetProject = DotNetProject.Load(@"C:\myproj.csproj");

Console.WriteLine(dotNetProject.ProjectTargets.Single());
Console.WriteLine(dotNetProject.Format);
```

`DotNetProject` contains the following properties:
- IsMultiTarget
- ProjectTargets
- Format
- ProjectReferences
- PackageReferences
- AssemblyInfo
- NugetMetaData
