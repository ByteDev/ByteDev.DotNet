using System.Collections.Generic;

namespace ByteDev.DotNet.Project
{
    internal static class BuildAction
    {
        /// <summary>
        /// List of build action values:
        /// https://docs.microsoft.com/en-us/visualstudio/ide/build-actions?view=vs-2019
        /// </summary>
        private static readonly HashSet<string> Values = new HashSet<string>
        {
            "AdditionalFiles",
            "ApplicationDefinition",
            "CodeAnalysisDictionary",
            "Compile",
            "Content",
            "DesignData",
            "DesignDataWithDesignTimeCreateable",
            "EmbeddedResource",
            "EntityDeploy",
            "Fakes",
            "None", 
            "Page",
            "Resource",
            "Shadow",
            "SplashScreen",
            "XamlAppDef"
        };

        public static bool IsValid(string value)
        {
            return Values.Contains(value);
        }
    }
}