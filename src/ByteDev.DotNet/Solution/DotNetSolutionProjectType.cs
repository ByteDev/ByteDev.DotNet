using System;

namespace ByteDev.DotNet.Solution
{
    public class DotNetSolutionProjectType
    {
        public Guid Id { get; }

        public string Description { get; }

        public DotNetSolutionProjectType(Guid id, string description)
        {
            Id = id;
            Description = description;
        }

        public override string ToString()
        {
            return $"{Description} ({Id})";
        }
    }
}