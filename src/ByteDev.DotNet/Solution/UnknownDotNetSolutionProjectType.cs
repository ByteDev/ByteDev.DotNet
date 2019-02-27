using System;

namespace ByteDev.DotNet.Solution
{
    public sealed class UnknownDotNetSolutionProjectType : DotNetSolutionProjectType
    {
        public UnknownDotNetSolutionProjectType(Guid id) : base(id, "(Unknown)")
        {
        }
    }
}