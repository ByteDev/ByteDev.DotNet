using System;

namespace ByteDev.DotNet.Solution
{
    /// <summary>
    /// Represents an unknown .NET solution project type.
    /// </summary>
    public sealed class UnknownDotNetSolutionProjectType : DotNetSolutionProjectType
    {
        internal UnknownDotNetSolutionProjectType(Guid id) : base(id, "(Unknown)")
        {
        }
    }
}