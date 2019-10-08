using System;

namespace ByteDev.DotNet.Solution
{
    /// <summary>
    /// Represents a project reference type within a .NET solution.
    /// </summary>
    public class DotNetSolutionProjectType : IEquatable<DotNetSolutionProjectType>
    {
        /// <summary>
        /// Identifier for the project type.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Description for the project type.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Returns whether the project type a solution folder.
        /// </summary>
        public bool IsSolutionFolder => Id == ProjectTypeIds.SolutionFolder;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.DotNet.Solution.DotNetSolutionProjectType" /> class.
        /// </summary>
        /// <param name="id">Identifier for the project type.</param>
        /// <param name="description">Description for the project type.</param>
        /// <exception cref="T:System.ArgumentException"><paramref name="description" /> is null or empty.</exception>
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