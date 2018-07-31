using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ByteDev.DotNet.Project
{
    public class DotNetProject
    {
        public DotNetProject(XDocument xDocument)
        {
            if(xDocument == null)
                throw new ArgumentNullException(nameof(xDocument));

            ProjectTarget = new DotNetProjectTarget(GetTargetFrameworkElement(xDocument).Value);
        }

        public DotNetProjectTarget ProjectTarget { get; }

        private static IEnumerable<XElement> GetPropertyGroups(XDocument xDocument)
        {
            var propertyGroups = xDocument.Root?.Descendants().Where(d => d.Name.LocalName == "PropertyGroup");

            if (propertyGroups == null || !propertyGroups.Any())
                throw new InvalidDotNetProjectException("Project document contains no PropertyGroup elements.");

            return propertyGroups;
        }

        private static XElement GetTargetFrameworkElement(XDocument xDocument)
        {
            var propertyGroups = GetPropertyGroups(xDocument);

            // .NET Core / Standard
            var targetFrameworkElement = propertyGroups.SingleOrDefault(pg => pg.Elements().SingleOrDefault()?.Name.LocalName == "TargetFramework")?
                .Elements().SingleOrDefault(pg => pg.Name.LocalName == "TargetFramework");

            if (targetFrameworkElement == null)
            {
                // .NET Framework
                targetFrameworkElement = propertyGroups.SingleOrDefault(pg => pg.Elements().SingleOrDefault()?.Name.LocalName == "TargetFrameworkVersion")?
                    .Elements().SingleOrDefault(pg => pg.Name.LocalName == "TargetFrameworkVersion");
            }

            if (targetFrameworkElement == null)
                throw new InvalidDotNetProjectException("Project document contains no TargetFramework.");

            return targetFrameworkElement;
        }
    }
}