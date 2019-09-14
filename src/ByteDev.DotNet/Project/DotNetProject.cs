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
        private Lazy<bool> _isPackable;
        private Lazy<string> _packageVersion;
        private Lazy<string> _packageId;
        private Lazy<string> _title;
        private Lazy<string> _packageDescription;
        private Lazy<bool> _packageRequireLicenseAcceptance;
        private Lazy<string> _packageLicenseExpression;

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
        /// Project URL applicable to the package.
        /// </summary>
        public string PackageProjectUrl => _packageProjectUrl.Value;

        /// <summary>
        /// Collection of references to other projects.
        /// </summary>
        public IEnumerable<ProjectReference> ProjectReferences { get; private set; }

        /// <summary>
        /// Collection of references to external packages.  Will return empty unless the project
        /// is in the new format (old format package references are typically in a packages.config file).
        /// </summary>
        public IEnumerable<PackageReference> PackageReferences { get; private set; }

        #region Assembly properties

        /// <summary>
        /// Company name for the project.
        /// </summary>
        public string Company => _company.Value;

        #endregion

        #region Package Metadata
        
        /// <summary>
        /// An SPDX license identifier or expression. For example, Apache-2.0.
        /// </summary>
        public string PackageLicenseExpression => _packageLicenseExpression.Value;

        /// <summary>
        /// Specifies whether the client must prompt the consumer to accept the package license before installing the package. The default is false.
        /// </summary>
        public bool PackageRequireLicenseAcceptance => _packageRequireLicenseAcceptance.Value;

        /// <summary>
        /// A long description of the package for UI display.
        /// </summary>
        public string PackageDescription => _packageDescription.Value;

        /// <summary>
        /// A human-friendly title of the package, typically used in UI displays as on nuget.org and the Package Manager in Visual Studio. If not specified, the package ID is used instead.
        /// </summary>
        public string Title => _title.Value;

        /// <summary>
        /// Specifies the name for the resulting package. If not specified, the pack operation will default to using the AssemblyName or directory name as the name of the package.
        /// </summary>
        public string PackageId => _packageId.Value;

        /// <summary>
        /// Specifies the version that the resulting package will have. Accepts all forms of NuGet version string.
        /// </summary>
        public string PackageVersion => _packageVersion.Value;

        /// <summary>
        /// Specifies whether the project can be packed. The default value is true.
        /// </summary>
        public bool IsPackable => _isPackable.Value;

        /// <summary>
        /// A semicolon-separated list of packages authors, matching the profile names on nuget.org.
        /// These are displayed in the NuGet Gallery on nuget.org and are used to cross-reference packages by the same authors.
        /// </summary>
        public string Authors => _authors.Value;

        /// <summary>
        /// A long description for the assembly.
        /// If PackageDescription is not specified then this property is also used as the description of the package.
        /// </summary>
        public string Description => _description.Value;

        /// <summary>
        /// Copyright details for the package.
        /// </summary>
        public string Copyright => _copyright.Value;

        /// <summary>
        /// An URL to the license that is applicable to the package. (deprecated since Visual Studio 15.9.4, .NET SDK 2.1.502 and 2.2.101).
        /// </summary>
        public string PackageLicenseUrl => _packageLicenseUrl.Value;

        /// <summary>
        /// A URL for a 64x64 image used as the icon for the package in UI display.
        /// </summary>
        public string PackageIconUrl => _packageIconUrl.Value;

        /// <summary>
        /// Release notes for the package.
        /// </summary>
        public string PackageReleaseNotes => _packageReleaseNotes.Value;

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
        /// Specifies the URL for the repository where the source code for the package resides and/or from which it's being built.
        /// </summary>
        public string RepositoryUrl => _repositoryUrl.Value;

        /// <summary>
        /// Specifies the type of the repository. For example: "git".
        /// </summary>
        public string RepositoryType => _repositoryType.Value;

        #endregion
        
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

            SetAssemblyProperties(propertyGroups);
            SetPackageProperties(propertyGroups);
        }

        private void SetAssemblyProperties(PropertyGroupCollection propertyGroups)
        {
            _company = new Lazy<string>(() => propertyGroups.GetElementValue("Company"));
        }

        private void SetPackageProperties(PropertyGroupCollection propertyGroups)
        {
            _isPackable = new Lazy<bool>(() =>
            {
                var value = propertyGroups.GetElementValue("IsPackable");

                return value == null || Convert.ToBoolean(value);
            });

            _packageVersion = new Lazy<string>(() => propertyGroups.GetElementValue("PackageVersion"));
            _packageId = new Lazy<string>(() => propertyGroups.GetElementValue("PackageId"));
            _title = new Lazy<string>(() => propertyGroups.GetElementValue("Title"));
            _packageDescription = new Lazy<string>(() => propertyGroups.GetElementValue("PackageDescription"));
            _packageRequireLicenseAcceptance = new Lazy<bool>(() => Convert.ToBoolean(propertyGroups.GetElementValue("PackageRequireLicenseAcceptance")));
            _packageLicenseExpression = new Lazy<string>(() => propertyGroups.GetElementValue("PackageLicenseExpression"));

            _description = new Lazy<string>(() => propertyGroups.GetElementValue("Description"));
            _authors = new Lazy<string>(() => propertyGroups.GetElementValue("Authors"));
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