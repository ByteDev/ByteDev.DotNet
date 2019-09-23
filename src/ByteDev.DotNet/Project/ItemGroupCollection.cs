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

            return projectRefElements.Select(CreateProjectReferenceFor);
        }

        public IEnumerable<PackageReference> GetPackageReferences()
        {
            var packageRefElements = ItemGroupElements.Descendants().Where(e => e.Name.LocalName == "PackageReference");

            return packageRefElements.Select(CreatePackageReferenceFor);
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
                FilePath = projectReferenceElement.Attribute("Include")?.Value
            };
        }

        private static PackageReference CreatePackageReferenceFor(XElement packageReferenceElement)
        {
            return new PackageReference
            {
                Name = packageReferenceElement.Attribute("Include")?.Value,
                Version = packageReferenceElement.Attribute("Version")?.Value,
                InclueAssets = packageReferenceElement.Attribute("IncludeAssets")?.Value.SplitOnSemiColon(),
                ExcludeAssets = packageReferenceElement.Attribute("ExcludeAssets")?.Value.SplitOnSemiColon(),
                PrivateAssets = packageReferenceElement.Attribute("PrivateAssets")?.Value.SplitOnSemiColon()
            };
        }
    }
}