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

            List<XElement> propertyGroups = xDocument.GetPropertyGroups().ToList();

            SetFormatAndTargets(propertyGroups);

            Description = GetPropertyGroupElement(propertyGroups, "Description");
            Authors = GetPropertyGroupElement(propertyGroups, "Authors");
            Company = GetPropertyGroupElement(propertyGroups, "Company");
            PackageTags = GetPropertyGroupElement(propertyGroups, "PackageTags");
        }

        public bool IsMultiTarget => ProjectTargets?.Count() > 1;

        public IEnumerable<DotNetProjectTarget> ProjectTargets { get; private set; }

        public ProjectFormat Format { get; private set; }

        public string Description { get; }

        public string Authors { get; }

        public string Company { get; }

        public string PackageTags { get; }

        private void SetFormatAndTargets(List<XElement> propertyGroups)
        {
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

        private static string GetPropertyGroupElement(IEnumerable<XElement> propertyGroups, string elementName)
        {
            var xElement = propertyGroups.SingleOrDefault(pg => pg.Element(elementName) != null)?
                .Element(elementName);

            return xElement?.Value;
        }
    }
}