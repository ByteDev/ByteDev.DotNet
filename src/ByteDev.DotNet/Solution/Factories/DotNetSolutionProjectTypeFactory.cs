using System;
using System.Collections.Generic;

namespace ByteDev.DotNet.Solution.Factories
{
    public class DotNetSolutionProjectTypeFactory : IDotNetSolutionProjectTypeFactory
    {
        private static readonly Dictionary<Guid, string> Types = new Dictionary<Guid, string>
        {
            {ProjectTypeIds.SolutionFolder, "Solution Folder"},
            {ProjectTypeIds.ProjectFolder, "Project Folder"},
            {ProjectTypeIds.Test, "Test"},
            {ProjectTypeIds.CSharp, "C#"},
            {ProjectTypeIds.CSharpNewFormat, "C#"},
            {ProjectTypeIds.FSharp, "F#"},
            {ProjectTypeIds.JSharp, "J#"},
            {ProjectTypeIds.CPlusPlus, "C++"},
            {ProjectTypeIds.VbNet, "VB.NET"},
            {ProjectTypeIds.WebSite, "Web Site"},
            {ProjectTypeIds.AspNet5, "ASP.NET 5"},
            {ProjectTypeIds.AspNetMvc1, "ASP.NET MVC 1"},
            {ProjectTypeIds.AspNetMvc2, "ASP.NET MVC 2"},
            {ProjectTypeIds.AspNetMvc3, "ASP.NET MVC 3"},
            {ProjectTypeIds.AspNetMvc4, "ASP.NET MVC 4"},
            {ProjectTypeIds.AspNetMvc5, "ASP.NET MVC 5"},
            {ProjectTypeIds.Database, "Database"},
            {ProjectTypeIds.DatabaseOther, "Database (other project types)"},
            {ProjectTypeIds.DeploymentCab, "Deployment Cab"},
            {ProjectTypeIds.DeploymentMergeModule, "Deployment Merge Module"},
            {ProjectTypeIds.DeploymentSetup, "Deployment Setup"},
            {ProjectTypeIds.DeploymentSmartDeviceCab, "Deployment Smart Device Cab"},
            {ProjectTypeIds.DistributedSystem, "Distributed System"},
            {ProjectTypeIds.Dynamics2012AxCSharpInAot, "Dynamics 2012 AX C# in AOT"},
            {ProjectTypeIds.CSharpLegacy2003SmartDevice, "Legacy (2003) Smart Device (C#)"},
            {ProjectTypeIds.VbNetLegacy2003SmartDevice, "egacy (2003) Smart Device (VB.NET)"},
            {ProjectTypeIds.MicroFramework, "Micro Framework"},
            {ProjectTypeIds.MonoAndroid, "Mono/Xamarin Android"},
            {ProjectTypeIds.MonoTouch, "Mono/iOS Touch"},
            {ProjectTypeIds.MonoTouchBinding, "Mono Touch Binding"},
            {ProjectTypeIds.PortableClassLibrary, "Portable Class Library"},
            {ProjectTypeIds.SharePointCSharp, "SharePoint (C#)"},
            {ProjectTypeIds.SharePointVbNet, "SharePoint (VB.NET)"},
            {ProjectTypeIds.SharePointWorkflow, "SharePoint Workflow"},
            {ProjectTypeIds.Silverlight, "Silverlight"},
            {ProjectTypeIds.SmartDeviceCSharp, "Smart Device (C#)"},
            {ProjectTypeIds.SmartDeviceVbNet, "Smart Device (VB.NET)"},
            {ProjectTypeIds.UniversalWindowsClassLibrary, "Universal Windows Class Library"},
            {ProjectTypeIds.VisualDatabaseTools, "Visual Database Tools"},
            {ProjectTypeIds.VisualStudio2015InstallerProjectExtension, "Visual Studio 2015 Installer Project Extension"},
            {ProjectTypeIds.VisualStudioToolsForApplications, "Visual Studio Tools for Applications (VSTA)"},
            {ProjectTypeIds.VisualStudioToolsForOffice, "Visual Studio Tools for Office (VSTO)"},
            {ProjectTypeIds.Wcf, "Windows Communication Foundation (WCF)"},
            {ProjectTypeIds.Wpf, "Windows Presentation Foundation (WPF)"},
            {ProjectTypeIds.WorkflowCSharp, "Workflow (C#)"},
            {ProjectTypeIds.WorkflowVbNet, "Workflow (VB.NET)"},
            {ProjectTypeIds.WorkflowFoundation, "Workflow Foundation"},
            {ProjectTypeIds.WindowsPhone, "Windows Phone 8/8.1 Blank/Hub/Webview App"},
            {ProjectTypeIds.WindowsPhoneAppCSharp, "Windows Phone 8/8.1 App (C#)"},
            {ProjectTypeIds.WindowsPhoneAppVbNet, "Windows Phone 8/8.1 App (VB.NET)"},
            {ProjectTypeIds.WindowsStoreAppsAndComponents, "Windows Store (Metro) Apps & Components"},
            {ProjectTypeIds.XnaWindows, "XNA (Windows)"},
            {ProjectTypeIds.XnaXBox, "XNA (XBox)"},
            {ProjectTypeIds.XnaZune, "XNA (Zune)"}
        };

        public DotNetSolutionProjectType Create(Guid projectTypeId)
        {
            try
            {
                var description = Types[projectTypeId];

                return new DotNetSolutionProjectType(projectTypeId, description);
            }
            catch (KeyNotFoundException)
            {
                return new UnknownDotNetSolutionProjectType(projectTypeId);
            }
        }
    }
}