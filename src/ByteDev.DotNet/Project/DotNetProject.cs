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

            SetFormatAndTarget(xDocument);
        }

        public bool IsMultiTarget => ProjectTargets?.Count() > 1;

        public IEnumerable<DotNetProjectTarget> ProjectTargets { get; private set; }

        public ProjectFormat Format { get; private set; }

        private void SetFormatAndTarget(XDocument xDocument)
        {
            var propertyGroups = GetPropertyGroups(xDocument).ToList();

            var targetElement = PropertyGroupXmlParser.GetOldStyleTargetElement(propertyGroups);

            if (targetElement == null)
            {
                targetElement = PropertyGroupXmlParser.GetNewStyleTargetElement(propertyGroups);
                Format = ProjectFormat.New;
            }
            else
            {
                Format = ProjectFormat.Old;
            }

            if (targetElement == null)
                throw new InvalidDotNetProjectException("Project document contains no target framework.");

            ProjectTargets = targetElement.Value
                .Split(';')
                .Select(value => new DotNetProjectTarget(value));
        }

        private static IEnumerable<XElement> GetPropertyGroups(XDocument xDocument)
        {
            var propertyGroups = ProjectXmlParser.GetPropertyGroups(xDocument)?.ToList();

            if (propertyGroups == null || !propertyGroups.Any())
                throw new InvalidDotNetProjectException("Project document contains no PropertyGroup elements.");

            return propertyGroups;
        }
    }
}