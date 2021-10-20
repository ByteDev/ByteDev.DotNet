namespace ByteDev.DotNet.Project
{
    /// <summary>
    /// Represents an item (included or excluded) within a project.
    /// </summary>
    public class ProjectItem
    {
        /// <summary>
        /// Item's build action type.
        /// For example: "None", "Content", "EmbeddedResource", "Compile" etc.
        /// </summary>
        public string BuildAction {get; set; }

        /// <summary>
        /// Item path.
        /// </summary>
        public string Path { get; set; }
    }
}