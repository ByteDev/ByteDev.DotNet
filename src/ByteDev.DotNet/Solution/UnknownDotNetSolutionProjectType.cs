using System;

namespace ByteDev.DotNet.Solution
{
    public class UnknownDotNetSolutionProjectType : DotNetSolutionProjectType
    {
        public UnknownDotNetSolutionProjectType(Guid id) : base(id, "(Unknown)")
        {
        }
    }
}