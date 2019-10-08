using System;

namespace ByteDev.DotNet.Solution.Factories
{
    internal interface IDotNetSolutionProjectTypeFactory
    {
        DotNetSolutionProjectType Create(Guid projectTypeId);
    }
}