using System;
using System.Collections.Generic;
using System.IO;
using ByteDev.DotNet.Solution.Parsers;

namespace ByteDev.DotNet.Solution
{
    public class DotNetSolution
    {
        private readonly Lazy<string> _formatVersion;
        private readonly Lazy<string> _visualStudioVersion;
        private readonly Lazy<string> _minimumVisualStudioVersion;
        private readonly Lazy<IList<DotNetSolutionProject>> _projects;

        public DotNetSolution(string slnText)
        {
            if(string.IsNullOrEmpty(slnText))
                throw new ArgumentException("Solution text was null or empty.", nameof(slnText));

            _formatVersion = new Lazy<string>(() => new FormatVersionParser().Parse(slnText));
            _visualStudioVersion = new Lazy<string>(() => new VisualStudioVersionParser().Parse(slnText));
            _minimumVisualStudioVersion = new Lazy<string>(() => new MinimumVisualStudioVersionParser().Parse(slnText));

            _projects = new Lazy<IList<DotNetSolutionProject>>(() => new DotNetSolutionProjectsParser(new DotNetSolutionProjectTypeFactory()).Parse(slnText));
        }

        public string FormatVersion => _formatVersion.Value;

        public string VisualStudioVersion => _visualStudioVersion.Value;

        public string MinimumVisualStudioVersion => _minimumVisualStudioVersion.Value;

        public IList<DotNetSolutionProject> Projects => _projects.Value;

        public static DotNetSolution Load(string slnFilePath)
        {
            string slnText = File.ReadAllText(slnFilePath);

            return new DotNetSolution(slnText);
        }
    }
}