using ByteDev.DotNet.Solution;
using NUnit.Framework;

namespace ByteDev.DotNet.Shared.PackageTests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void WhenSlnExists_ThenRead()
        {
            var sln = new DotNetSolution(SlnText);

            Assert.That(sln.MajorVersion, Is.EqualTo(16));
        }

        private const string SlnText = "Microsoft Visual Studio Solution File, Format Version 12.00\r\n# Visual Studio Version 16\r\nVisualStudioVersion = 16.0.29709.97\r\nMinimumVisualStudioVersion = 10.0.40219.1\r\n";
    }
}