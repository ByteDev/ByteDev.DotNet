using System;
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
        private const string TestSlnV12_0Projs = @"Solution\TestSlns\sln-v12-0projs.txt";
        private const string TestSln_NoFormatVersion = @"Solution\TestSlns\sln-no-formatversion.txt";

        [Test]
        public void WhenSlnTextIsValid_ThenSetProperties()
        {
            var slnText = GetSlnText(TestSlnV12);

            var sut = new DotNetSolution(slnText);

            Assert.That(sut.FormatVersion, Is.EqualTo(12));
            Assert.That(sut.VisualStudioVersion, Is.EqualTo("15.0.27703.2042"));
            Assert.That(sut.MinimumVisualStudioVersion, Is.EqualTo("10.0.40219.1"));
        }

        [Test]
        public void WhenSlnHasNoFormatVersion_ThenThrowException()
        {
            var slnText = GetSlnText(TestSln_NoFormatVersion);

            Assert.Throws<InvalidDotNetSolutionException>(() => new DotNetSolution(slnText));
        }

        [Test]
        public void WhenSlnHasProjects_ThenSetProjectProperties()
        {
            var slnText = GetSlnText(TestSlnV12);

            var sut = new DotNetSolution(slnText);

            Assert.That(sut.Projects.Count(), Is.EqualTo(4));

            Assert.That(sut.Projects.First().TypeId, Is.EqualTo(new Guid("9A19103F-16F7-4668-BE54-9A1E7A4F7556")));
            Assert.That(sut.Projects.First().Name, Is.EqualTo("ByteDev.DotNet.IntTests"));
            Assert.That(sut.Projects.First().Path, Is.EqualTo(@"ByteDev.DotNet.IntTests\ByteDev.DotNet.IntTests.csproj"));
            Assert.That(sut.Projects.First().Id, Is.EqualTo(new Guid("989150B8-63EE-4213-A7E0-71ECB5A781C8")));
        }

        [Test]
        public void WhenSlnHasNoProject_ThenReturnEmpty()
        {
            var slnText = GetSlnText(TestSlnV12_0Projs);

            var sut = new DotNetSolution(slnText);

            Assert.That(sut.Projects.Count(), Is.EqualTo(0));
        }

        private string GetSlnText(string slnFilePath)
        {
            return File.ReadAllText(slnFilePath);
        }
    }
}