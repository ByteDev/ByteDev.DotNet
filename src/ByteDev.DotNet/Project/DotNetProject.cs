using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ByteDev.DotNet.Project.Parsers;

namespace ByteDev.DotNet.Project
{
    public class DotNetProject
    {
        private const char PackageTagsDelimiter = ' ';
        private const char ProjectTargetDelimiter = ';';

        private Lazy<string> _description;
        private Lazy<string> _authors;
        private Lazy<string> _company;
        private Lazy<string> _packageTags;

        public DotNetProject(XDocument xDocument)
        {
            if(xDocument == null)
                throw new ArgumentNullException(nameof(xDocument));

            SetItemGroupProperties(xDocument);
            SetPropertyGroupProperties(xDocument);
        }

        public bool IsMultiTarget => ProjectTargets?.Count() > 1;

        public IEnumerable<DotNetProjectTarget> ProjectTargets { get; private set; }

        public ProjectFormat Format { get; private set; }

        public string Description => _description.Value;

        public string Authors => _authors.Value;

        public string Company => _company.Value;

        public IEnumerable<string> PackageTags => string.IsNullOrEmpty(_packageTags.Value) ? new string[0] : _packageTags.Value.Split(PackageTagsDelimiter);

        public IEnumerable<ProjectReference> ProjectReferences { get; private set; }

        public IEnumerable<PackageReference> PackageReferences { get; private set; }

        public static DotNetProject Load(string projFilePath)
        {
            var xDoc = XDocument.Load(projFilePath);

            return new DotNetProject(xDoc);
        }

        private void SetItemGroupProperties(XDocument xDocument)
        {
            var itemGroups = new ItemGroupCollection(xDocument);

            ProjectReferences = itemGroups.GetProjectReferences();
            PackageReferences = itemGroups.GetPackageReferences();
        }

        private void SetPropertyGroupProperties(XDocument xDocument)
        {
            var propertyGroups = new PropertyGroupCollection(xDocument);

            SetFormatAndProjectTargets(propertyGroups);

            _description = new Lazy<string>(() => propertyGroups.GetElementValue("Description"));
            _authors = new Lazy<string>(() => propertyGroups.GetElementValue("Authors"));
            _company = new Lazy<string>(() => propertyGroups.GetElementValue("Company"));
            _packageTags = new Lazy<string>(() => propertyGroups.GetElementValue("PackageTags"));
        }

        private void SetFormatAndProjectTargets(PropertyGroupCollection propertyGroups)
        {
            var targetElement = PropertyGroupXmlParser.GetOldStyleTargetElement(propertyGroups.PropertyGroupElements);

            if (targetElement == null)
            {
                targetElement = PropertyGroupXmlParser.GetNewStyleTargetElement(propertyGroups.PropertyGroupElements);
                Format = ProjectFormat.New;
            }
            else
            {
                Format = ProjectFormat.Old;
            }

            if (targetElement == null)
                throw new InvalidDotNetProjectException("Project document contains no target framework.");

            ProjectTargets = targetElement
                .Value
                .Split(ProjectTargetDelimiter)
                .Select(value => new DotNetProjectTarget(value));
        }
    }
}