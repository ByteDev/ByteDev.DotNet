namespace ByteDev.DotNet.Solution
{
    /// <summary>
    /// Represents the load type for a project section.
    /// </summary>
    public enum ProjectSectionType
    {
        /// <summary>
        /// Unknown load type (type could not be determined).
        /// </summary>
        Unknown,

        /// <summary>
        /// Load pre-project.
        /// </summary>
        PreProject,

        /// <summary>
        /// Load post-project.
        /// </summary>
        PostProject
    }
}