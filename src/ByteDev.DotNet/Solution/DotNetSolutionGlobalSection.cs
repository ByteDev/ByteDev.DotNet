using System.Collections.Generic;

namespace ByteDev.DotNet.Solution
{
    /// <summary>
    /// Represents a global section within a .NET solution.
    /// </summary>
    public class DotNetSolutionGlobalSection
    {
        private Dictionary<string, string> _settings;

        /// <summary>
        /// The global section name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The global section load type.
        /// </summary>
        public GlobalSectionType Type { get; set; }

        /// <summary>
        /// The global section settings.
        /// </summary>
        public Dictionary<string, string> Settings
        {
            get => _settings ?? (_settings = new Dictionary<string, string>());
            set => _settings = value;
        }
    }
}