using System;
using System.Collections.Generic;

namespace ByteDev.DotNet.Solution
{
    /// <summary>
    /// Represents a project reference within a .NET solution.
    /// </summary>
    public class DotNetSolutionProject
    {
        private IList<DotNetSolutionProjectSection> _projectSections;

        /// <summary>
        /// Project type.
        /// </summary>
        public DotNetSolutionProjectType Type { get; set; }
        
        /// <summary>
        /// Project name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Project file location.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Unique project ID.
        /// </summary>
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