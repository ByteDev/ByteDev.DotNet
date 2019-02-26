using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ByteDev.DotNet.Solution.Parsers
{
    public class DotNetSolutionProjectsParser : ISolutionTextParser<IList<DotNetSolutionProject>>
    {
        private readonly IDotNetSolutionProjectTypeFactory _typeFactory;

        public DotNetSolutionProjectsParser(IDotNetSolutionProjectTypeFactory typeFactory)
        {
            _typeFactory = typeFactory;
        }

        public IList<DotNetSolutionProject> Parse(string slnText)
        {
            const string pattern = "^Project\\(\"{(?<TypeId>[A-F0-9-]+)}\"\\) = " +
                                   "\"(?<Name>.*?)\", " +
                                   "\"(?<Path>.*?)\", " +
                                   "\"{(?<Id>[A-F0-9-]+)}\"";

            var matches = Regex.Matches(slnText, pattern, RegexOptions.Multiline);

            var list = new List<DotNetSolutionProject>();

            foreach (Match match in matches)
            {
                list.Add(CreateDotNetSolutionProject(match));
            }

            return list;
        }

        private DotNetSolutionProject CreateDotNetSolutionProject(Match match)
        {
            var typeId = new Guid(match.Groups[1].Value);

            return new DotNetSolutionProject
            {
                Type = _typeFactory.Create(typeId),
                Name = match.Groups[2].Value,
                Path = match.Groups[3].Value,
                Id = new Guid(match.Groups[4].Value)
            };
        }
    }
}