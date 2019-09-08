using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ByteDev.DotNet.Project.Parsers;

namespace ByteDev.DotNet.Project
{
    public class DotNetProject
    {
        private const char ProjectTargetDelimiter = ';';

        private Lazy<string> _description;
        private Lazy<string> _authors;
        private Lazy<string> _company;
        private Lazy<string> _packageTags;
        private Lazy<string> _packageLicenseUrl;
        private Lazy<string> _packageProjectUrl;
        private Lazy<string> _packageIconUrl;
        private Lazy<string> _repositoryUrl;
        private Lazy<string> _repositoryType;
        private Lazy<string> _packageReleaseNotes;
        private Lazy<string> _copyright;

        public DotNetProject(XDocument xDocument)
        {
            if(xDocument == null)
                throw new ArgumentNullException(nameof(xDocument));

            SetPropertyGroupProperties(xDocument);
            SetItemGroupProperties(xDocument);
        }

        /// <summary>
        /// Whether the project have more than one target.
        /// </summary>
        public bool IsMultiTarget => ProjectTargets?.Count() > 1;

        /// <summary>
        /// The project's targets.
        /// </summary>
        public IEnumerable<DotNetProjectTarget> ProjectTargets { get; private set; }

        /// <summary>
        /// Whether the project in the new or old format.
        /// </summary>
        public ProjectFormat Format { get; private set; }

        /// <summary>
        /// Project description.
        /// </summary>
        public string Description => _description.Value;

        /// <summary>
        /// Authors of the project.
        /// </summary>
        public string Authors => _authors.Value;
        
        /// <summary>
        /// Company name for the project.
        /// </summary>
        public string Company => _company.Value;

        /// <summary>
        /// A URL to the license that is applicable to the package.
        /// </summary>
        public string PackageLicenseUrl => _packageLicenseUrl.Value;

        /// <summary>
        /// Project URL applicable to the package.
        /// </summary>
        public string PackageProjectUrl => _packageProjectUrl.Value;

        /// <summary>
        /// A URL for a 64x64 image used as the icon for the package in UI display.
        /// </summary>
        public string PackageIconUrl => _packageIconUrl.Value;

        /// <summary>
        /// Specifies the URL for the repository where the source code for the package resides and/or from which it's being built.
        /// </summary>
        public string RepositoryUrl => _repositoryUrl.Value;

        /// <summary>
        /// Specifies the type of the repository. For example: "git".
        /// </summary>
        public string RepositoryType => _repositoryType.Value;

        /// <summary>
        /// Release notes for the package.
        /// </summary>
        public string PackageReleaseNotes => _packageReleaseNotes.Value;

        /// <summary>
        /// Copy right information for the project.
        /// </summary>
        public string Copyright => _copyright.Value;

        
        /// <summary>
        /// Collection of tags that designates the package.
        /// </summary>
        public IEnumerable<string> PackageTags
        {
            get
            {
                if (string.IsNullOrEmpty(_packageTags.Value))
                    return Enumerable.Empty<string>();

                if (_packageTags.Value.Contains(','))
                    return _packageTags.Value.Split(',').Select(tag => tag.Trim());

                return _packageTags.Value.Split(' ');
            }
        }

        /// <summary>
        /// Collection of references to other projects.
        /// </summary>
        public IEnumerable<ProjectReference> ProjectReferences { get; private set; }

        /// <summary>
        /// Collection of references to external packages.  Will return empty unless the project
        /// is in the new format (old format package references are typically in a packages.config file).
        /// </summary>
        public IEnumerable<PackageReference> PackageReferences { get; private set; }

        /// <summary>
        /// Loads the DotNetProject from the specified file path.
        /// </summary>
        /// <param name="projFilePath">Project file path.</param>
        /// <returns>DotNetProject</returns>
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
            _packageLicenseUrl = new Lazy<string>(() => propertyGroups.GetElementValue("PackageLicenseUrl"));
            _packageProjectUrl = new Lazy<string>(() => propertyGroups.GetElementValue("PackageProjectUrl"));
            _packageIconUrl = new Lazy<string>(() => propertyGroups.GetElementValue("PackageIconUrl"));
            _repositoryUrl = new Lazy<string>(() => propertyGroups.GetElementValue("RepositoryUrl"));
            _repositoryType = new Lazy<string>(() => propertyGroups.GetElementValue("RepositoryType"));
            _packageReleaseNotes = new Lazy<string>(() => propertyGroups.GetElementValue("PackageReleaseNotes"));
            _copyright = new Lazy<string>(() => propertyGroups.GetElementValue("Copyright"));
        }

        private void SetFormatAndProjectTargets(PropertyGroupCollection propertyGroups)
        {
            var targetElement = PropertyGroupXmlParser.GetOldStyleTargetElement(propertyGroups.PropertyGroupElements);

            if (targetElement == null)
            {
                targetElement = PropertyGroupXmlParser.GetNewStyleTargetElement(propertyGroups.PropertyGroupElements);
                
                if (targetElement == null)
                    throw new InvalidDotNetProjectException("Project document contains no target framework.");

                Format = ProjectFormat.New;
            }
            else
            {
                Format = ProjectFormat.Old;
            }
            
            ProjectTargets = targetElement
                .Value
                .Split(ProjectTargetDelimiter)
                .Select(value => new DotNetProjectTarget(value));
        }
    }
}