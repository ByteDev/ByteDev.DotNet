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

        public override string ToString()
        {
            return FilePath;
        }
    }
}