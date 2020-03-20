namespace ByteDev.DotNet.Project
{
    /// <summary>
    /// Represents a reference to a project from a .NET project file.
    /// </summary>
    public class ProjectReference
    {
        /// <summary>
        /// Path to the project file.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Returns a string representation of <see cref="T:ByteDev.DotNet.Project.ProjectReference" />.
        /// </summary>
        /// <returns>String representation.</returns>
        public override string ToString()
        {
            return FilePath;
        }
    }
}