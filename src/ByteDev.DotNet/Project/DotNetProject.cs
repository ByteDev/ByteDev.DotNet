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

        public ProjectFormat Format { get; private set; }

        private static IEnumerable<XElement> GetPropertyGroups(XDocument xDocument)
        {
            var propertyGroups = xDocument.Root?.Descendants().Where(d => d.Name.LocalName == "PropertyGroup");

            if (propertyGroups == null || !propertyGroups.Any())
                throw new InvalidDotNetProjectException("Project document contains no PropertyGroup elements.");

            return propertyGroups;
        }

        private XElement GetTargetFrameworkElement(XDocument xDocument)
        {
            var propertyGroups = GetPropertyGroups(xDocument);

            var targetFrameworkElement = GetOldStyleTargetFrameworkElement(propertyGroups);

            if (targetFrameworkElement == null)
            {
                targetFrameworkElement = GetNewStyleTargetFrameworkElement(propertyGroups);
                Format = ProjectFormat.New;
            }
            else
            {
                Format = ProjectFormat.Old;
            }

            if (targetFrameworkElement == null)
                throw new InvalidDotNetProjectException("Project document contains no TargetFramework.");

            return targetFrameworkElement;
        }

        private static XElement GetNewStyleTargetFrameworkElement(IEnumerable<XElement> propertyGroups)
        {
            const string name = "TargetFramework";

            return propertyGroups.SingleOrDefault(pg => pg.Elements().SingleOrDefault()?.Name.LocalName == name)?
                .Elements()
                .SingleOrDefault(pg => pg.Name.LocalName == name);
        }

        private static XElement GetOldStyleTargetFrameworkElement(IEnumerable<XElement> propertyGroups)
        {
            const string name = "TargetFrameworkVersion";

            return propertyGroups.SingleOrDefault(pg => pg.Elements().SingleOrDefault()?.Name.LocalName == name)?
                .Elements()
                .SingleOrDefault(pg => pg.Name.LocalName == name);
        }
    }
}