using System;
using System.Collections.Generic;
using System.IO;
using ByteDev.DotNet.Solution.Parsers;

namespace ByteDev.DotNet.Solution
{
    public class DotNetSolution
    {
        public DotNetSolution(string slnText)
        {
            if(string.IsNullOrEmpty(slnText))
                throw new ArgumentException("Solution text was null or empty.", nameof(slnText));

            FormatVersion = SolutionTextParser.ParseFormatVersion(slnText);
            VisualStudioVersion = SolutionTextParser.ParseVisualStudioVersion(slnText);
            MinimumVisualStudioVersion = SolutionTextParser.ParseMinimumVisualStudioVersion(slnText);
            Projects = SolutionTextParser.ParseProjects(slnText);
        }

        public string FormatVersion { get; }

        public string VisualStudioVersion { get; }

        public string MinimumVisualStudioVersion { get; }

        public IList<DotNetSolutionProject> Projects { get; }

        public static DotNetSolution Load(string slnFilePath)
        {
            string slnText = File.ReadAllText(slnFilePath);

            return new DotNetSolution(slnText);
        }
    }
}