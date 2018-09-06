using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ByteDev.DotNet.Project.Parsers;

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

        private XElement GetTargetFrameworkElement(XDocument xDocument)
        {
            var propertyGroups = GetPropertyGroups(xDocument).ToList();

            var targetFrameworkElement = PropertyGroupXmlParser.GetOldStyleTargetFrameworkElement(propertyGroups);

            if (targetFrameworkElement == null)
            {
                targetFrameworkElement = PropertyGroupXmlParser.GetNewStyleTargetFrameworkElement(propertyGroups);
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

        private static IList<XElement> GetPropertyGroups(XDocument xDocument)
        {
            var propertyGroups = ProjectXmlParser.GetPropertyGroups(xDocument)?.ToList();

            if (propertyGroups == null || !propertyGroups.Any())
                throw new InvalidDotNetProjectException("Project document contains no PropertyGroup elements.");

            return propertyGroups;
        }
    }
}