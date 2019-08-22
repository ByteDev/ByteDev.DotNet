using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ByteDev.DotNet.Solution.Converters;
using ByteDev.DotNet.Solution.Factories;

namespace ByteDev.DotNet.Solution.Parsers
{
    internal class DotNetSolutionProjectsParser : ISolutionTextParser<IList<DotNetSolutionProject>>
    {
        private const string ProjectPattern = 
            "^Project\\(\"{(?<TypeId>[A-F0-9-]+)}\"\\) = " +
            "\"(?<Name>.*?)\", " +
            "\"(?<Path>.*?)\", " +
            "\"{(?<Id>[A-F0-9-]+)}\"" +
            @"(?<Sections>(.|\n|\r)*?)" +
            @"EndProject(\n|\r)";

        private const string ProjectSectionPattern = @"ProjectSection(?<Record>(.|\n|\r)*?)EndProjectSection";

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
                Id = new Guid(match.Groups["Id"].Value),
                ProjectSections = CreateProjectSections(match)
            };
        }

        private static IList<DotNetSolutionProjectSection> CreateProjectSections(Match match)
        {
            var sections = match.Groups["Sections"].Value.Trim();

            var projectSections = new List<DotNetSolutionProjectSection>();

            if (string.IsNullOrEmpty(sections))
                return projectSections;
            
            var psMatches = Regex.Matches(sections, ProjectSectionPattern, RegexOptions.Multiline);

            foreach (Match psMatch in psMatches)
            {
                projectSections.Add(CreateProjectSection(psMatch.Groups["Record"].Value));
            }

            return projectSections;
        }

        private static DotNetSolutionProjectSection CreateProjectSection(string record)
        {
            var header = record.Substring(0, record.IndexOf('\n')).Trim();

            var ps = new DotNetSolutionProjectSection
            {
                Name = ExtractProjectSectionName(header),
                Type = ExtractProjectSectionType(header)
            };

            var dependencyLines = record.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Skip(1)
                .Select(l => l.Trim())
                .Where(l => l != string.Empty)
                .ToArray();

            foreach (var line in dependencyLines)
            {
                var nameValue = line.Split(new[] { " = " }, StringSplitOptions.None).ToArray();

                ps.Dependencies.Add(nameValue[0], nameValue[1]);
            }

            return ps;
        }

        private static string ExtractProjectSectionName(string header)
        {
            return Regex.Match(header, @"\((?<Name>.*?)\)").Groups[1].Value;
        }

        private static ProjectSectionType ExtractProjectSectionType(string header)
        {
            var type = Regex.Match(header, @" = (?<Type>.*)$").Groups[1].Value;

            return ProjectSectionTypeConverter.ConvertToType(type);
        }
    }
}