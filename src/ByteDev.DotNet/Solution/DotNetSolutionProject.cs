using System;

namespace ByteDev.DotNet.Solution
{
    public class DotNetSolutionProject
    {
        public DotNetSolutionProjectType Type { get; set; }
        
        public string Name { get; set; }

        public string Path { get; set; }

        public Guid Id { get; set; }
    }
}