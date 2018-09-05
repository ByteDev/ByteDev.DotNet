# ByteDev.DotNet

Set of classes for reading .NET solution and project files.

## Installation

ByteDev.DotNet has been written as a .NET Standard 2.0 library, so you can consume it from a .NET Core or .NET Framework 4.6.1 (or greater) application.

## Code

The repo can be cloned from git bash:

`git clone https://github.com/ByteDev/ByteDev.DotNet`

## Usage

Read in the plan text from a .NET solution file and pass it into a DotNetSolution object:

```c#
string slnText = File.ReadAllText(@"C:\mysolution.sln");

var dotNetSolution = new DotNetSolution(slnText);

Console.WriteLine(dotNetSolution.VisualStudioVersion);
```

Read in the XML from a .NET project file and pass it into a DotNetProject object:

```c#
string projXml = XDocument.Load(@"C:\myproj.csproj");

var dotNetProject = new DotNetProject(projXml);

Console.WriteLine(dotNetProject.ProjectTarget.TargetValue);
```