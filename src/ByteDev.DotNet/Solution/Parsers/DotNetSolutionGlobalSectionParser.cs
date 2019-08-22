using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ByteDev.DotNet.Solution.Converters;

namespace ByteDev.DotNet.Solution.Parsers
{
    internal class DotNetSolutionGlobalSectionParser : ISolutionTextParser<IList<DotNetSolutionGlobalSection>>
    {
        private const string GlobalSectionPattern = @"GlobalSection(?<Record>(.|\n|\r)*?)EndGlobalSection";
        
        public IList<DotNetSolutionGlobalSection> Parse(string slnText)
        {
            var matches = Regex.Matches(slnText, GlobalSectionPattern, RegexOptions.Multiline);

            var list = new List<DotNetSolutionGlobalSection>();

            foreach (Match match in matches)
            {
                var record = match.Groups["Record"].Value;

                var gc = CreateDotNetSolutionGlobalSection(record);

                list.Add(gc);
            }

            return list;
        }

        private static DotNetSolutionGlobalSection CreateDotNetSolutionGlobalSection(string record)
        {
            var header = record.Substring(0, record.IndexOf('\n')).Trim();

            var gc = new DotNetSolutionGlobalSection
            {
                Name = ExtractName(header),
                Type = ExtractType(header)
            };

            var settingLines = record.Split(new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries)
                .Skip(1)
                .Select(l => l.Trim())
                .Where(l => l != string.Empty)
                .ToArray();

            foreach (var settingLine in settingLines)
            {
                var nameValue = settingLine.Split(new[] {" = "}, StringSplitOptions.None).ToArray();

                gc.Settings.Add(nameValue[0], nameValue[1]);
            }

            return gc;
        }

        private static string ExtractName(string header)
        {
            return Regex.Match(header, @"\((?<Name>.*?)\)").Groups[1].Value;
        }

        private static GlobalSectionType ExtractType(string header)
        {
            var type = Regex.Match(header, @" = (?<Type>.*)$").Groups[1].Value;

            return GlobalSectionTypeConverter.ConvertToType(type);
        }
    }
}