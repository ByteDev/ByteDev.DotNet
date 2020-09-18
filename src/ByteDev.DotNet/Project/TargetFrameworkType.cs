namespace ByteDev.DotNet.Project
{
    /// <summary>
    /// Represents the project target type.
    /// </summary>
    public enum TargetFrameworkType
    {
        /// <summary>
        /// Unknown framework.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// .NET Framework.
        /// </summary>
        Framework = 1,

        /// <summary>
        /// .NET Core.
        /// </summary>
        Core = 2,

        /// <summary>
        /// .NET Standard.
        /// </summary>
        Standard = 3
    }
}