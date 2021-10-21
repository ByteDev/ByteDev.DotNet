using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ByteDev.DotNet.Project.Parsers;

namespace ByteDev.DotNet.Project
{
    internal class ItemGroupCollection
    {
        private IList<XElement> ItemGroupElements { get; }

        public ItemGroupCollection(XDocument xDocument)
        {
            ItemGroupElements = GetItemGroups(xDocument).ToList();
        }

        public IEnumerable<ProjectReference> GetProjectReferences()
        {
            return ItemGroupElements
                .Descendants()
                .Where(e => e.Name.LocalName == "ProjectReference")
                .Select(CreateProjectReferenceFor);
        }

        public IEnumerable<PackageReference> GetPackageReferences()
        {
            return ItemGroupElements
                .Descendants()
                .Where(e => e.Name.LocalName == "PackageReference")
                .Select(CreatePackageReferenceFor);
        }

        public IEnumerable<ProjectItem> GetExcludedItems(ProjectFormat format)
        {
            if (format == ProjectFormat.Old)
                return Enumerable.Empty<ProjectItem>();

            return ItemGroupElements
                .Descendants()
                .Where(e => BuildAction.IsValid(e.Name.LocalName) && e.Attribute("Remove") != null)
                .Select(CreateExcludedItemFor);
        }

        public IEnumerable<ProjectItem> GetIncludedItems(ProjectFormat format)
        {
            if (format == ProjectFormat.Old)
                return Enumerable.Empty<ProjectItem>();

            return ItemGroupElements
                .Descendants()
                .Where(e => BuildAction.IsValid(e.Name.LocalName) && e.Attribute("Include") != null)
                .Select(CreateIncludedItemFor);
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

        private static ProjectItem CreateExcludedItemFor(XElement itemElement)
        {
            return new ProjectItem
            {
                BuildAction = itemElement.Name.LocalName,
                Path = itemElement.Attribute("Remove")?.Value
            };
        }        
        
        private static ProjectItem CreateIncludedItemFor(XElement itemElement)
        {
            return new ProjectItem
            {
                BuildAction = itemElement.Name.LocalName,
                Path = itemElement.Attribute("Include")?.Value
            };
        }
    }
}