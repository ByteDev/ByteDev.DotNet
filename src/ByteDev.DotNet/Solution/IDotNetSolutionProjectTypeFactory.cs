using System;

namespace ByteDev.DotNet.Solution
{
    public interface IDotNetSolutionProjectTypeFactory
    {
        DotNetSolutionProjectType Create(Guid projectTypeId);
    }
}