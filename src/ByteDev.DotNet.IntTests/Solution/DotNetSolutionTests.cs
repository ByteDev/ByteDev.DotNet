using System.IO;
using System.Linq;
using ByteDev.DotNet.Solution;
using NUnit.Framework;

namespace ByteDev.DotNet.IntTests.Solution
{
    [TestFixture]
    public class DotNetSolutionTests
    {
        private const string TestSlnV12 = @"Solution\TestSlns\sln-v12.txt";

        [Test]
        public void WhenSlnTextIsValid_ThenSetProperties()
        {
            var slnText = File.ReadAllText(TestSlnV12);

            var sut = new DotNetSolution(slnText);

            Assert.That(sut.FormatVersion, Is.EqualTo(12));
            Assert.That(sut.VisualStudioVersion, Is.EqualTo("15.0.27703.2042"));
            Assert.That(sut.MinimumVisualStudioVersion, Is.EqualTo("10.0.40219.1"));
            Assert.That(sut.Projects.Count(), Is.EqualTo(4));
        }
    }
}