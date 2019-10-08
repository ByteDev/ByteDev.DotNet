namespace ByteDev.DotNet.Solution
{
    /// <summary>
    /// Represents the load type for a global section.
    /// </summary>
    public enum GlobalSectionType
    {
        /// <summary>
        /// Unknown load type (type could not be determined).
        /// </summary>
        Unknown,

        /// <summary>
        /// Load pre-solution.
        /// </summary>
        PreSolution,

        /// <summary>
        /// Load post-solution.
        /// </summary>
        PostSolution
    }
}