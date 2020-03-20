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

        /// <summary>
        /// Returns a string representation of <see cref="T:ByteDev.DotNet.Solution.DotNetSolutionProjectType" />.
        /// </summary>
        /// <returns>String representation.</returns>
        public override string ToString()
        {
            return $"{Description} ({Id})";
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>True if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;

            if (ReferenceEquals(obj, this))
                return true;

            return Equals(obj as DotNetSolutionProjectType);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="other">The object to compare with the current object.</param>
        /// <returns>True if the specified object  is equal to the current object; otherwise, false.</returns>
        public bool Equals(DotNetSolutionProjectType other)
        {
            if (other == null)
                return false;

            return Id == other.Id;
        }

        /// <summary>
        /// Returns the hash code for this type.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return (Id + Description).GetHashCode();
        }
    }
}