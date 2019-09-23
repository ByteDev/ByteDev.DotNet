using System.Collections.Generic;
using System.Linq;

namespace ByteDev.DotNet.Project
{
    public class PackageReference
    {
        private IEnumerable<string> _inclueAssets;
        private IEnumerable<string> _excludeAssets ;
        private IEnumerable<string> _privateAssets;

        /// <summary>
        /// Package name.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Package version number. Typically in: (Major).(Minor).(Patch) format. 
        /// </summary>
        public string Version { get; internal set; }

        /// <summary>
        /// Specifies which assets belonging to the package reference should be consumed.
        /// By default, all package assets are included.
        /// Possible values: Compile;Runtime;ContentFiles;Build;Native;Analyzers or All or None.
        /// </summary>
        public IEnumerable<string> InclueAssets
        {
            get => _inclueAssets ?? (_inclueAssets = Enumerable.Empty<string>());
            internal set => _inclueAssets = value;
        }

        /// <summary>
        /// Specifies which assets belonging to the package reference should not be consumed.
        /// Possible values: Compile;Runtime;ContentFiles;Build;Native;Analyzers or All or None.
        /// </summary>
        public IEnumerable<string> ExcludeAssets
        {
            get => _excludeAssets ?? (_excludeAssets = Enumerable.Empty<string>());
            internal set => _excludeAssets  = value;
        }

        /// <summary>
        /// Specifies which assets belonging to the package reference should be consumed but not flow to the next project.
        /// The Analyzers, Build and ContentFiles assets are private by default when null.
        /// Possible values: Compile;Runtime;ContentFiles;Build;Native;Analyzers or All or None.
        /// </summary>
        public IEnumerable<string> PrivateAssets
        {
            get => _privateAssets ?? (_privateAssets = Enumerable.Empty<string>());
            internal set => _privateAssets = value;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}