using System;

namespace ByteDev.DotNet.Solution
{
    public class DotNetSolutionProjectType : IEquatable<DotNetSolutionProjectType>
    {
        public Guid Id { get; }

        public string Description { get; }

        public bool IsSolutionFolder => Id == ProjectTypeIds.SolutionFolder;

        public DotNetSolutionProjectType(Guid id, string description)
        {
            if(string.IsNullOrEmpty(description))
                throw new ArgumentException("Description was null or empty.", nameof(description));

            Id = id;
            Description = description;
        }

        public override string ToString()
        {
            return $"{Description} ({Id})";
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;

            if (ReferenceEquals(obj, this))
                return true;

            return Equals(obj as DotNetSolutionProjectType);
        }

        public bool Equals(DotNetSolutionProjectType other)
        {
            if (other == null)
                return false;

            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return (Id + Description).GetHashCode();
        }
    }
}