# ByteDev.DotNet

Set of classes for reading .NET solution and project files.

## Installation

ByteDev.DotNet has been written as a .NET Standard 2.0 library, so you can consume it from a .NET Core or .NET Framework 4.6.1 (or greater) application.

ByteDev.DotNet is hosted as a package on nuget.org.  To install from the Package Manager Console in Visual Studio run:

`Install-Package ByteDev.DotNet`

Further details can be found on the [nuget page](https://www.nuget.org/packages/ByteDev.DotNet/).

## Code

The repo can be cloned from git bash:

`git clone https://github.com/ByteDev/ByteDev.DotNet`

## Usage

There are two main classes in the project: DotNetSolution and DotNetProject.

### DotNetSolution

Read in a .NET solution file and pass it into a DotNetSolution object:

```c#
string slnText = File.ReadAllText(@"C:\mysolution.sln");

var dotNetSolution = new DotNetSolution(slnText);

Console.WriteLine(dotNetSolution.VisualStudioVersion);
Console.WriteLine(dotNetSolution.Projects.Count);
```

### DotNetProject

Read in a .NET project file and pass it into a DotNetProject object:

```c#
string projXml = XDocument.Load(@"C:\myproj.csproj");

var dotNetProject = new DotNetProject(projXml);

Console.WriteLine(dotNetProject.ProjectTargets.Single());
Console.WriteLine(dotNetProject.Format);
```