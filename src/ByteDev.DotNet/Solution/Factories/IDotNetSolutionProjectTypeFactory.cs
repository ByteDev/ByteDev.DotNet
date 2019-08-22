using System;

namespace ByteDev.DotNet.Solution.Factories
{
    public interface IDotNetSolutionProjectTypeFactory
    {
        DotNetSolutionProjectType Create(Guid projectTypeId);
    }
}