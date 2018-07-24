using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ByteDev.DotNet
{
    public class DotNetProject
    {
        public DotNetProject(XDocument xDocument)
        {
            if(xDocument == null)
                throw new ArgumentNullException(nameof(xDocument));

            TargetFramework = GetTargetFrameworkElement(xDocument).Value;
        }

        public string TargetFramework { get; set; }

        private static IEnumerable<XElement> GetPropertyGroups(XDocument xDocument)
        {
            var propertyGroups = xDocument.Root?.Elements("PropertyGroup");

            if (propertyGroups == null)
                throw new InvalidDotNetProjectException("Project document contains no PropertyGroup.");

            return propertyGroups;
        }

        private static XElement GetTargetFrameworkElement(XDocument xDocument)
        {
            var propertyGroups = GetPropertyGroups(xDocument);

            var targetFrameworkElement = propertyGroups.Single(pg => pg.Element("TargetFramework") != null).Element("TargetFramework");

            if (targetFrameworkElement == null)
                throw new InvalidDotNetProjectException("Project document contains no TargetFramework.");

            return targetFrameworkElement;
        }
    }
}