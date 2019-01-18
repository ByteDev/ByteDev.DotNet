using System;
using System.Linq;
using ByteDev.Common.Collections;
using ByteDev.DotNet.Solution;
using NUnit.Framework;

namespace ByteDev.DotNet.IntTests.Solution
{
    [TestFixture]
    public class DotNetSolutionTests
    {
        [Test]
        public void WhenSlnTextIsValid_ThenSetProperties()
        {
            var sut = CreateSut(TestSlnFiles.V12);

            Assert.That(sut.FormatVersion, Is.EqualTo("12.00"));
            Assert.That(sut.VisualStudioVersion, Is.EqualTo("15.0.27703.2042"));
            Assert.That(sut.MinimumVisualStudioVersion, Is.EqualTo("10.0.40219.1"));
        }

        [Test]
        public void WhenSlnHasNoFormatVersion_ThenThrowException()
        {
            var ex = Assert.Throws<InvalidDotNetSolutionException>(() => CreateSut(TestSlnFiles.NoFormatVersion));
            Assert.That(ex.Message, Is.EqualTo("A valid Format Version could not be found in the sln text."));
        }

        [Test]
        public void WhenSlnHasNoVisualStudioVersion_ThenThrowException()
        {
            var ex = Assert.Throws<InvalidDotNetSolutionException>(() => CreateSut(TestSlnFiles.NoVsVersion));
            Assert.That(ex.Message, Is.EqualTo("A valid Visual Studio Version could not be found in the sln text."));
        }

        [Test]
        public void WhenSlnHasNoMinVsVersion_ThenThrowException()
        {
            var ex = Assert.Throws<InvalidDotNetSolutionException>(() => CreateSut(TestSlnFiles.NoMinVsVersion));
            Assert.That(ex.Message, Is.EqualTo("A valid Minimum Visual Studio Version could not be found in the sln text."));
        }

        [Test]
        public void WhenSlnHasProjects_ThenSetProjectProperties()
        {
            var sut = CreateSut(TestSlnFiles.V12);

            Assert.That(sut.Projects.Count, Is.EqualTo(5));

            Assert.That(sut.Projects.First().TypeId, Is.EqualTo(new Guid("9A19103F-16F7-4668-BE54-9A1E7A4F7556")));
            Assert.That(sut.Projects.First().Name, Is.EqualTo("ByteDev.DotNet.IntTests"));
            Assert.That(sut.Projects.First().Path, Is.EqualTo(@"ByteDev.DotNet.IntTests\ByteDev.DotNet.IntTests.csproj"));
            Assert.That(sut.Projects.First().Id, Is.EqualTo(new Guid("989150B8-63EE-4213-A7E0-71ECB5A781C8")));
        }

        [Test]
        public void WhenSlnHasSolutionFolder_ThenSetProjectProperties()
        {
            var sut = CreateSut(TestSlnFiles.V12);

            Assert.That(sut.Projects.Fourth().IsSolutionFolder, Is.True);
            Assert.That(sut.Projects.Fourth().TypeId, Is.EqualTo(new Guid("2150E333-8FDC-42A3-9474-1A3956D46DE8")));
            Assert.That(sut.Projects.Fourth().Name, Is.EqualTo("Tests"));
            Assert.That(sut.Projects.Fourth().Path, Is.EqualTo("Tests"));
            Assert.That(sut.Projects.Fourth().Id, Is.EqualTo(new Guid("F8FDF7E8-B7FB-4BA1-8107-DB114CB729BB")));
        }

        [Test]
        public void WhenSlnHasNoProject_ThenReturnEmpty()
        {
            var sut = CreateSut(TestSlnFiles.V12NoProjs);

            Assert.That(sut.Projects.Count, Is.EqualTo(0));
        }

        private DotNetSolution CreateSut(string slnFilePath)
        {
            return DotNetSolution.Load(slnFilePath);
        }
    }
}