using System.Collections.Generic;

namespace ByteDev.DotNet.Solution
{
    public class DotNetSolutionProjectSection
    {
        private Dictionary<string, string> _dependencies;

        /// <summary>
        /// Project section name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Project section load type.
        /// </summary>
        public ProjectSectionType Type { get; set; }

        /// <summary>
        /// Declared project dependencies.
        /// </summary>
        public Dictionary<string, string> Dependencies
        {
            get => _dependencies ?? (_dependencies = new Dictionary<string, string>());
            set => _dependencies = value;
        }
    }

    public enum ProjectSectionType
    {
        Unknown,
        PreProject,
        PostProject
    }
}