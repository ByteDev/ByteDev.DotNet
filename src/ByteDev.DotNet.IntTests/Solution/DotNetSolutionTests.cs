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
        private const string TestSlnV12ZeroProjs = @"Solution\TestSlns\sln-v12-0projs.txt";

        private const string TestSlnNoFormatVersion = @"Solution\TestSlns\sln-no-formatversion.txt";
        private const string TestSlnNoVsVersion = @"Solution\TestSlns\sln-no-vsversion.txt";
        private const string TestSlnNoMinVsVersion = @"Solution\TestSlns\sln-no-minvsversion.txt";

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
            var slnText = GetSlnText(TestSlnNoFormatVersion);

            var ex = Assert.Throws<InvalidDotNetSolutionException>(() => new DotNetSolution(slnText));
            Assert.That(ex.Message, Is.EqualTo("A valid Format Version could not be found in the sln text."));
        }

        [Test]
        public void WhenSlnHasNoVisualStudioVersion_ThenThrowException()
        {
            var slnText = GetSlnText(TestSlnNoVsVersion);

            var ex = Assert.Throws<InvalidDotNetSolutionException>(() => new DotNetSolution(slnText));
            Assert.That(ex.Message, Is.EqualTo("A valid Visual Studio Version could not be found in the sln text."));
        }

        [Test]
        public void WhenSlnHasNoMinVsVersion_ThenThrowException()
        {
            var slnText = GetSlnText(TestSlnNoMinVsVersion);

            var ex = Assert.Throws<InvalidDotNetSolutionException>(() => new DotNetSolution(slnText));
            Assert.That(ex.Message, Is.EqualTo("A valid Minimum Visual Studio Version could not be found in the sln text."));
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
            var slnText = GetSlnText(TestSlnV12ZeroProjs);

            var sut = new DotNetSolution(slnText);

            Assert.That(sut.Projects.Count(), Is.EqualTo(0));
        }

        private string GetSlnText(string slnFilePath)
        {
            return File.ReadAllText(slnFilePath);
        }
    }
}