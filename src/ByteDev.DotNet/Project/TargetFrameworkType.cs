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
        Standard = 3,

        /// <summary>
        /// Windows Store.
        /// </summary>
        WindowsStore = 4,

        /// <summary>
        /// .NET Micro Framework
        /// </summary>
        MicroFramework = 5,

        /// <summary>
        /// Silverlight
        /// </summary>
        Silverlight = 6,

        /// <summary>
        /// Windows Phone.
        /// </summary>
        WindowsPhone = 7,

        /// <summary>
        /// Universal Windows Platform.
        /// </summary>
        UniversalWindowsPlatform = 8
    }
}