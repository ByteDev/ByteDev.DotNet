using System.Collections.Generic;

namespace ByteDev.DotNet.Solution
{
    public class DotNetSolutionProjectSection
    {
        private Dictionary<string, string> _dependencies;

        public string Name { get; set; }

        public ProjectSectionType Type { get; set; }

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