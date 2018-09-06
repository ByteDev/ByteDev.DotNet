using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ByteDev.DotNet.Solution.Parsers
{
    internal class SolutionTextParser
    {
        public static string ParseFormatVersion(string slnText)
        {
            const string message = "A valid Format Version could not be found in the sln text.";

            try
            {
                var value = Regex.Match(slnText, "[\r\n]?Microsoft Visual Studio Solution File, Format Version ([0-9.]+)", RegexOptions.Multiline).Groups[1].Value;

                if (string.IsNullOrEmpty(value))
                    throw new InvalidDotNetSolutionException(message);

                return value;
            }
            catch (Exception ex)
            {
                throw new InvalidDotNetSolutionException(message, ex);
            }
        }

        public static string ParseVisualStudioVersion(string slnText)
        {
            const string marker = "VisualStudioVersion = ";

            try
            {
                return Regex.Match(slnText, "^" + marker + "([0-9.]+)", RegexOptions.Multiline).Value.Substring(marker.Length);
            }
            catch (Exception ex)
            {
                throw new InvalidDotNetSolutionException("A valid Visual Studio Version could not be found in the sln text.", ex);
            }
        }

        public static string ParseMinimumVisualStudioVersion(string slnText)
        {
            const string marker = "MinimumVisualStudioVersion = ";

            try
            {
                return Regex.Match(slnText, "^" + marker + "([0-9.]+)", RegexOptions.Multiline).Value.Substring(marker.Length);
            }
            catch (Exception ex)
            {
                throw new InvalidDotNetSolutionException("A valid Minimum Visual Studio Version could not be found in the sln text.", ex);
            }
        }

        public static IList<DotNetSolutionProject> ParseProjects(string slnText)
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

        private static DotNetSolutionProject CreateDotNetSolutionProject(Match match)
        {
            return new DotNetSolutionProject
            {
                TypeId = new Guid(match.Groups[1].Value),
                Name = match.Groups[2].Value,
                Path = match.Groups[3].Value,
                Id = new Guid(match.Groups[4].Value)
            };
        }
    }
}