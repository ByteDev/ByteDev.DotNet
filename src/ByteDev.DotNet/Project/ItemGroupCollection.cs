using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ByteDev.DotNet.Project.Parsers;

namespace ByteDev.DotNet.Project
{
    internal class ItemGroupCollection
    {
        public IList<XElement> ItemGroupElements { get; }

        public ItemGroupCollection(XDocument xDocument)
        {
            ItemGroupElements = GetItemGroups(xDocument).ToList();
        }

        public IEnumerable<ProjectReference> GetProjectReferences()
        {
            var projectRefElements = ItemGroupElements.Descendants().Where(e => e.Name.LocalName == "ProjectReference");

            return projectRefElements.Select(projectRefElement => CreateProjectReferenceFor(projectRefElement));
        }

        private static IList<XElement> GetItemGroups(XDocument xDocument)
        {
            var itemGroups = ProjectXmlParser.GetItemGroups(xDocument)?.ToList();

            return itemGroups ?? new List<XElement>();
        }

        private static ProjectReference CreateProjectReferenceFor(XElement projectReferenceElement)
        {
            return new ProjectReference
            {
                Include = projectReferenceElement.Attribute("Include")?.Value
            };
        }
    }
}