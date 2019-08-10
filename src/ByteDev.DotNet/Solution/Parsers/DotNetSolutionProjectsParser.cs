using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ByteDev.DotNet.Solution.Parsers
{
    internal class DotNetSolutionProjectsParser : ISolutionTextParser<IList<DotNetSolutionProject>>
    {
        private const string ProjectPattern = 
            "^Project\\(\"{(?<TypeId>[A-F0-9-]+)}\"\\) = " +
            "\"(?<Name>.*?)\", " +
            "\"(?<Path>.*?)\", " +
            "\"{(?<Id>[A-F0-9-]+)}\"";

        private readonly IDotNetSolutionProjectTypeFactory _typeFactory;

        public DotNetSolutionProjectsParser(IDotNetSolutionProjectTypeFactory typeFactory)
        {
            _typeFactory = typeFactory;
        }

        public IList<DotNetSolutionProject> Parse(string slnText)
        {
            var matches = Regex.Matches(slnText, ProjectPattern, RegexOptions.Multiline);

            var list = new List<DotNetSolutionProject>();

            foreach (Match match in matches)
            {
                list.Add(CreateDotNetSolutionProject(match));
            }

            return list;
        }

        private DotNetSolutionProject CreateDotNetSolutionProject(Match match)
        {
            return new DotNetSolutionProject
            {
                Type = _typeFactory.Create(new Guid(match.Groups["TypeId"].Value)),
                Name = match.Groups["Name"].Value,
                Path = match.Groups["Path"].Value,
                Id = new Guid(match.Groups["Id"].Value)
            };
        }
    }
}