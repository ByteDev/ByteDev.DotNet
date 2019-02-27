using System;

namespace ByteDev.DotNet.Solution
{
    public sealed class UnknownDotNetSolutionProjectType : DotNetSolutionProjectType
    {
        internal UnknownDotNetSolutionProjectType(Guid id) : base(id, "(Unknown)")
        {
        }
    }
}