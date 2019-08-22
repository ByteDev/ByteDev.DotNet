using System.Collections.Generic;

namespace ByteDev.DotNet.Solution
{
    public class DotNetSolutionGlobalSection
    {
        private Dictionary<string, string> _settings;

        /// <summary>
        /// Global section name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Global section load type.
        /// </summary>
        public GlobalSectionType Type { get; set; }

        /// <summary>
        /// Global section settings.
        /// </summary>
        public Dictionary<string, string> Settings
        {
            get => _settings ?? (_settings = new Dictionary<string, string>());
            set => _settings = value;
        }
    }

    public enum GlobalSectionType
    {
        Unknown,
        PreSolution,
        PostSolution
    }
}