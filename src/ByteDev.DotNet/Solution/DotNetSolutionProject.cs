using System;
using System.Collections.Generic;

namespace ByteDev.DotNet.Solution
{
    public class DotNetSolutionProject
    {
        private IList<DotNetSolutionProjectSection> _projectSections;

        public DotNetSolutionProjectType Type { get; set; }
        
        public string Name { get; set; }

        public string Path { get; set; }

        public Guid Id { get; set; }

        /// <summary>
        /// Collection of project section settings.
        /// </summary>
        public IList<DotNetSolutionProjectSection> ProjectSections
        {
            get => _projectSections ?? (_projectSections = new List<DotNetSolutionProjectSection>());
            set => _projectSections = value;
        }
    }
}