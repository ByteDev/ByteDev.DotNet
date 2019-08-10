using System.Collections.Generic;

namespace ByteDev.DotNet.Solution
{
    public class DotNetSolutionGlobalSection
    {
        private Dictionary<string, string> _settings;

        public string Name { get; set; }

        public GlobalSectionType Type { get; set; }

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