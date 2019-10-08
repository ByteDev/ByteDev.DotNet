using System;
using System.Collections.Generic;
using System.Linq;

namespace ByteDev.DotNet.Project
{
    /// <summary>
    /// Represents Nuget meta data properties in the .NET project file.
    /// </summary>
    public class NugetMetaDataProperties
    {
        private const string ContentTargetFoldersDefaultValue = "content;contentFiles";

        private readonly Lazy<bool> _isPackable;
        private readonly Lazy<string> _packageVersion;
        private readonly Lazy<string> _packageId;
        private readonly Lazy<string> _title;
        private readonly Lazy<string> _packageDescription;
        private readonly Lazy<string> _description;
        private readonly Lazy<string> _packageLicenseUrl;
        private readonly Lazy<string> _packageIconUrl;
        private readonly Lazy<string> _repositoryUrl;
        private readonly Lazy<string> _repositoryType;
        private readonly Lazy<string> _packageReleaseNotes;
        private readonly Lazy<string> _copyright;
        private readonly Lazy<bool> _packageRequireLicenseAcceptance;
        private readonly Lazy<string> _packageLicenseExpression;
        private readonly Lazy<string> _packageOutputPath;
        private readonly Lazy<bool> _includeSymbols;
        private readonly Lazy<string> _symbolPackageFormat;
        private readonly Lazy<bool> _includeSource;
        private readonly Lazy<bool> _isTool;
        private readonly Lazy<bool> _noPackageAnalysis;
        private readonly Lazy<bool> _includeBuildOutput;
        private readonly Lazy<bool> _includeContentInPack;
        private readonly Lazy<string> _buildOutputTargetFolder;
        private readonly Lazy<string> _minClientVersion;
        private readonly Lazy<string> _nuspecFile;
        private readonly Lazy<string> _nuspecBasePath;
        private readonly Lazy<string> _packageLicenseFile;

        private readonly Lazy<IEnumerable<string>> _packageTags;
        private readonly Lazy<IEnumerable<string>> _contentTargetFolders;
        private readonly Lazy<IEnumerable<string>> _authors;

        private readonly Lazy<Dictionary<string, string>> _nuspecProperties;
        
        internal NugetMetaDataProperties(PropertyGroupCollection propertyGroups)
        {
            if(propertyGroups == null)
                throw new ArgumentNullException(nameof(propertyGroups));

            _isPackable = new Lazy<bool>(() => propertyGroups.GetElementBoolValue("IsPackable", true));
            _packageVersion = new Lazy<string>(() => propertyGroups.GetElementValue("PackageVersion"));
            _packageId = new Lazy<string>(() => propertyGroups.GetElementValue("PackageId"));
            _title = new Lazy<string>(() => propertyGroups.GetElementValue("Title"));
            _authors = new Lazy<IEnumerable<string>>(() => propertyGroups.GetElementValue("Authors").SplitOnSemiColon());
            _packageDescription = new Lazy<string>(() => propertyGroups.GetElementValue("PackageDescription"));
            _description = new Lazy<string>(() => propertyGroups.GetElementValue("Description"));

            _includeSymbols = new Lazy<bool>(() => propertyGroups.GetElementBoolValue("IncludeSymbols"));
            _includeSource = new Lazy<bool>(() => propertyGroups.GetElementBoolValue("IncludeSource"));
            _isTool = new Lazy<bool>(() => propertyGroups.GetElementBoolValue("IsTool"));
            _noPackageAnalysis = new Lazy<bool>(() => propertyGroups.GetElementBoolValue("NoPackageAnalysis"));
            _includeBuildOutput = new Lazy<bool>(() => propertyGroups.GetElementBoolValue("IncludeBuildOutput"));
            _includeContentInPack = new Lazy<bool>(() => propertyGroups.GetElementBoolValue("IncludeContentInPack", true));
            
            _packageTags = new Lazy<IEnumerable<string>>(() => propertyGroups.GetElementValue("PackageTags").SplitOnCommaOrSpace());
            _contentTargetFolders = new Lazy<IEnumerable<string>>(() =>
            {
                var value = propertyGroups.GetElementValue("ContentTargetFolders");

                return string.IsNullOrEmpty(value) ? ContentTargetFoldersDefaultValue.SplitOnSemiColon() : value.SplitOnSemiColon();
            });

            _nuspecFile = new Lazy<string>(() => propertyGroups.GetElementValue("NuspecFile"));
            _nuspecBasePath = new Lazy<string>(() => propertyGroups.GetElementValue("NuspecBasePath"));
            _nuspecProperties = new Lazy<Dictionary<string, string>>(() =>
            {
                var nameValues = propertyGroups.GetElementValue("NuspecProperties").SplitOnSemiColon();

                return nameValues
                    .Select(nameValue => nameValue.Split('='))
                    .ToDictionary(pair => pair[0], pair => pair[1]);
            });

            _minClientVersion = new Lazy<string>(() => propertyGroups.GetElementValue("MinClientVersion"));
            _buildOutputTargetFolder = new Lazy<string>(() => propertyGroups.GetElementValue("BuildOutputTargetFolder"));
            
            _packageRequireLicenseAcceptance = new Lazy<bool>(() => Convert.ToBoolean(propertyGroups.GetElementValue("PackageRequireLicenseAcceptance")));
            _packageLicenseExpression = new Lazy<string>(() => propertyGroups.GetElementValue("PackageLicenseExpression"));
            _packageOutputPath = new Lazy<string>(() => propertyGroups.GetElementValue("PackageOutputPath"));
            _symbolPackageFormat = new Lazy<string>(() => propertyGroups.GetElementValue("SymbolPackageFormat"));

            _packageLicenseFile = new Lazy<string>(() => propertyGroups.GetElementValue("PackageLicenseFile"));
            _packageLicenseUrl = new Lazy<string>(() => propertyGroups.GetElementValue("PackageLicenseUrl"));
            _packageIconUrl = new Lazy<string>(() => propertyGroups.GetElementValue("PackageIconUrl"));
            _repositoryUrl = new Lazy<string>(() => propertyGroups.GetElementValue("RepositoryUrl"));
            _repositoryType = new Lazy<string>(() => propertyGroups.GetElementValue("RepositoryType"));
            _packageReleaseNotes = new Lazy<string>(() => propertyGroups.GetElementValue("PackageReleaseNotes"));
            _copyright = new Lazy<string>(() => propertyGroups.GetElementValue("Copyright"));

        }

        /// <summary>
        /// Specifies whether the project can be packed. The default value is true.
        /// </summary>
        public bool IsPackable => _isPackable.Value;

        /// <summary>
        /// Specifies the name for the resulting package. If not specified, the pack operation will default to using the AssemblyName or directory name as the name of the package.
        /// </summary>
        public string PackageId => _packageId.Value;

        /// <summary>
        /// Specifies the version that the resulting package will have. Accepts all forms of NuGet version string.
        /// Default is the value of $(Version), that is, of the property Version in the project.
        /// </summary>
        public string PackageVersion => _packageVersion.Value;

        /// <summary>
        /// A human-friendly title of the package, typically used in UI displays as on nuget.org and the Package Manager in Visual Studio. If not specified, the package ID is used instead.
        /// </summary>
        public string Title => _title.Value;

        /// <summary>
        /// List of packages authors, matching the profile names on nuget.org.
        /// These are displayed in the NuGet Gallery on nuget.org and are used to cross-reference packages by the same authors.
        /// </summary>
        public IEnumerable<string> Authors => _authors.Value;

        /// <summary>
        /// A long description of the package for UI display.
        /// </summary>
        public string PackageDescription => _packageDescription.Value;

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
        /// Specifies whether the client must prompt the consumer to accept the package license before installing the package. The default is false.
        /// </summary>
        public bool PackageRequireLicenseAcceptance => _packageRequireLicenseAcceptance.Value;

        /// <summary>
        /// An SPDX license identifier or expression. For example, Apache-2.0.
        /// </summary>
        public string PackageLicenseExpression => _packageLicenseExpression.Value;
        
        /// <summary>
        /// Path to a license file within the package if you are using a license that hasn’t been assigned an SPDX identifier,
        /// or it is a custom license (Otherwise PackageLicenseExpression is preferred).
        /// Replaces PackageLicenseUrl, can't be combined with PackageLicenseExpression and
        /// requires Visual Studio 15.9.4, .NET SDK 2.1.502 or 2.2.101, or newer.
        /// </summary>
        public string PackageLicenseFile => _packageLicenseFile.Value;
        
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
        public IEnumerable<string> PackageTags => _packageTags.Value;
        
        /// <summary>
        /// Determines the output path in which the packed package will be dropped. Default is $(OutputPath).
        /// </summary>
        public string PackageOutputPath => _packageOutputPath.Value;
        
        /// <summary>
        /// Indicates whether the package should create an additional symbols package when the project is packed. The symbols package's format is controlled by the SymbolPackageFormat property.
        /// </summary>
        public bool IncludeSymbols => _includeSymbols.Value;

        /// <summary>
        /// Specifies the format of the symbols package. If "symbols.nupkg", a legacy symbols package will be created with a .symbols.nupkg extension containing PDBs, DLLs, and other output files. If "snupkg", a snupkg symbol package will be created containing the portable PDBs. Default is "symbols.nupkg".
        /// </summary>
        public string SymbolPackageFormat => _symbolPackageFormat.Value;

        /// <summary>
        /// Indicates whether the pack process should create a source package.
        /// The source package contains the library's source code as well as PDB files.
        /// Source files are put under the src/ProjectName directory in the resulting package file.
        /// </summary>
        public bool IncludeSource => _includeSource.Value;

        /// <summary>
        /// Specifies whether all output files are copied to the tools folder instead of the lib folder.
        /// Note that this is different from a DotNetCliTool which is specified by setting the PackageType in the .csproj file.
        /// </summary>
        public bool IsTool => _isTool.Value;

        /// <summary>
        /// Specifies the URL for the repository where the source code for the package resides and/or from which it's being built.
        /// </summary>
        public string RepositoryUrl => _repositoryUrl.Value;

        /// <summary>
        /// Specifies the type of the repository. For example: "git".
        /// </summary>
        public string RepositoryType => _repositoryType.Value;

        /// <summary>
        /// Specifies that pack should not run package analysis after building the package.
        /// </summary>
        public bool NoPackageAnalysis => _noPackageAnalysis.Value;

        /// <summary>
        /// Specifies the minimum version of the NuGet client that can install this package, enforced by nuget.exe and the Visual Studio Package Manager.
        /// </summary>
        public string MinClientVersion => _minClientVersion.Value;

        /// <summary>
        /// Specifies whether the build output assemblies should be packed into the .nupkg file or not.
        /// </summary>
        public bool IncludeBuildOutput => _includeBuildOutput.Value;

        /// <summary>
        /// Specifies whether any items that have a type of Content will be included in the resulting package automatically.
        /// The default is true.
        /// </summary>
        public bool IncludeContentInPack => _includeContentInPack.Value;

        /// <summary>
        /// Specifies the folder where to place the output assemblies.
        /// The output assemblies (and other output files) are copied into their respective framework folders.
        /// </summary>
        public string BuildOutputTargetFolder => _buildOutputTargetFolder.Value;

        /// <summary>
        /// Specifies the default location of where all the content files should go if PackagePath is not specified for them.
        /// The default value is "content;contentFiles".
        /// </summary>
        public IEnumerable<string> ContentTargetFolders => _contentTargetFolders.Value;

        /// <summary>
        /// Relative or absolute path to the .nuspec file being used for packing.
        /// If the .nuspec file is specified, it's used exclusively for packaging information and any information in the
        /// projects is not used.
        /// </summary>
        public string NuspecFile => _nuspecFile.Value;

        /// <summary>
        /// Base path for the .nuspec file.
        /// </summary>
        public string NuspecBasePath => _nuspecBasePath.Value;

        /// <summary>
        /// Dictionary of nuspec properties.
        /// </summary>
        public Dictionary<string, string> NuspecProperties => _nuspecProperties.Value;
    }
}