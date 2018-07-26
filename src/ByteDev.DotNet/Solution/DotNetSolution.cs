using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ByteDev.DotNet.Solution
{
    public class DotNetSolution
    {
        public DotNetSolution(string slnText)
        {
            FormatVersion = GetFormatVersion(slnText);
            VisualStudioVersion = GetVisualStudioVersion(slnText);
            MinimumVisualStudioVersion = GetMinimumVisualStudioVersion(slnText);
            Projects = GetProjects(slnText);
        }

        public double FormatVersion { get; }

        public string VisualStudioVersion { get; }

        public string MinimumVisualStudioVersion { get; }

        public IEnumerable<DotNetSolutionProject> Projects { get; }

        private static double GetFormatVersion(string slnText)
        {
            return double.Parse(Regex.Match(slnText, "[\r\n]?Microsoft Visual Studio Solution File, Format Version ([0-9.]+)", RegexOptions.Multiline).Groups[1].Value, CultureInfo.InvariantCulture);
        }

        private static string GetVisualStudioVersion(string slnText)
        {
            const string marker = "VisualStudioVersion = ";

            return Regex.Match(slnText, "^" + marker + "([0-9.]+)", RegexOptions.Multiline).Value.Substring(marker.Length);
        }

        private string GetMinimumVisualStudioVersion(string slnText)
        {
            const string marker = "MinimumVisualStudioVersion = ";

            return Regex.Match(slnText, "^" + marker + "([0-9.]+)", RegexOptions.Multiline).Value.Substring(marker.Length);
        }

        private IEnumerable<DotNetSolutionProject> GetProjects(string slnText)
        {
            const string pattern = "^Project\\(\"(?<HostId>{[A-F0-9-]+})\"\\) = " +
                "\"(?<Name>.*?)\", " +
                "\"(?<Path>.*?)\", " +
                "\"(?<TypeId>{[A-F0-9-]+})\"";

            var matches = Regex.Matches(slnText, pattern, RegexOptions.Multiline);

            var list = new List<DotNetSolutionProject>();

            foreach (Match match in matches)
            {
                var proj = new DotNetSolutionProject
                {
                    HostId = match.Groups[1].Value,
                    Name = match.Groups[2].Value,
                    Path = match.Groups[3].Value,
                    TypeId = match.Groups[4].Value
                };

                list.Add(proj);
            }

            return list;
        }
    }

    // TODO: check proper names for properties
    // TODO: cast to Guid
    // TODO: check better way to get values (by name?)

    public class DotNetSolutionProject
    {
        public string HostId { get; set; }           // Guid
        public string Name { get; set; }
        public string Path { get; set; }
        public string TypeId { get; set; }       // Guid
    }
}