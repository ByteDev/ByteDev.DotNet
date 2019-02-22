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

        private readonly Lazy<string> _description;
        private readonly Lazy<string> _authors;
        private readonly Lazy<string> _company;
        private readonly Lazy<string> _packageTags;

        public DotNetProject(XDocument xDocument)
        {
            if(xDocument == null)
                throw new ArgumentNullException(nameof(xDocument));

            var propertyGroups = new PropertyGroupCollection(xDocument);

            SetFormatAndTargets(propertyGroups.PropertyGroupElements);

            _description = new Lazy<string>(() => propertyGroups.GetElementValue("Description"));
            _authors = new Lazy<string>(() => propertyGroups.GetElementValue("Authors"));
            _company = new Lazy<string>(() => propertyGroups.GetElementValue("Company"));
            _packageTags = new Lazy<string>(() => propertyGroups.GetElementValue("PackageTags"));

            var itemGroups = new ItemGroupCollection(xDocument);

            ProjectReferences = itemGroups.GetProjectReferences();
        }

        public bool IsMultiTarget => ProjectTargets?.Count() > 1;

        public IEnumerable<DotNetProjectTarget> ProjectTargets { get; private set; }

        public ProjectFormat Format { get; private set; }

        public string Description => _description.Value;

        public string Authors => _authors.Value;

        public string Company => _company.Value;

        public IEnumerable<string> PackageTags => string.IsNullOrEmpty(_packageTags.Value) ? new string[0] : _packageTags.Value.Split(PackageTagsDelimiter);

        public IEnumerable<ProjectReference> ProjectReferences { get; set; }

        public static DotNetProject Load(string projFilePath)
        {
            var xDoc = XDocument.Load(projFilePath);

            return new DotNetProject(xDoc);
        }
        
        private void SetFormatAndTargets(IList<XElement> propertyGroups)
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
    }
}