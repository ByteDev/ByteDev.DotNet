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
            var sut = CreateSut(TestSlnFiles.NoFormatVersion);

            var ex = Assert.Throws<InvalidDotNetSolutionException>(() =>
            {
                var x = sut.FormatVersion;
            });
            Assert.That(ex.Message, Is.EqualTo("A valid Format Version could not be found in the sln text."));
        }

        [Test]
        public void WhenSlnHasNoVisualStudioVersion_ThenThrowException()
        {
            var sut = CreateSut(TestSlnFiles.NoVsVersion);

            var ex = Assert.Throws<InvalidDotNetSolutionException>(() =>
            {
                var x = sut.VisualStudioVersion;
            });
            Assert.That(ex.Message, Is.EqualTo("A valid Visual Studio Version could not be found in the sln text."));
        }

        [Test]
        public void WhenSlnHasNoMinVsVersion_ThenThrowException()
        {
            var sut = CreateSut(TestSlnFiles.NoMinVsVersion);

            var ex = Assert.Throws<InvalidDotNetSolutionException>(() =>
            {
                var x = sut.MinimumVisualStudioVersion;
            });
            Assert.That(ex.Message, Is.EqualTo("A valid Minimum Visual Studio Version could not be found in the sln text."));
        }

        [Test]
        public void WhenSlnHasProjects_ThenSetProjectProperties()
        {
            var sut = CreateSut(TestSlnFiles.V12);

            Assert.That(sut.Projects.Count, Is.EqualTo(5));

            Assert.That(sut.Projects.First().Type.Id, Is.EqualTo(ProjectTypeIds.CSharpNewFormat));
            Assert.That(sut.Projects.First().Name, Is.EqualTo("ByteDev.DotNet.IntTests"));
            Assert.That(sut.Projects.First().Path, Is.EqualTo(@"ByteDev.DotNet.IntTests\ByteDev.DotNet.IntTests.csproj"));
            Assert.That(sut.Projects.First().Id, Is.EqualTo(new Guid("989150B8-63EE-4213-A7E0-71ECB5A781C8")));

            Assert.That(sut.Projects.Second().Type.Id, Is.EqualTo(ProjectTypeIds.CSharpNewFormat));
            Assert.That(sut.Projects.Second().Name, Is.EqualTo("ByteDev.DotNet"));
            Assert.That(sut.Projects.Second().Path, Is.EqualTo(@"ByteDev.DotNet\ByteDev.DotNet.csproj"));
            Assert.That(sut.Projects.Second().Id, Is.EqualTo(new Guid("321736A6-AE48-46F1-89E5-074D6C105E66")));
            
            Assert.That(sut.Projects.Third().Type.Id, Is.EqualTo(ProjectTypeIds.CSharp));
            Assert.That(sut.Projects.Third().Name, Is.EqualTo("ByteDev.DotNet.UnitTests"));
            Assert.That(sut.Projects.Third().Path, Is.EqualTo(@"ByteDev.DotNet.UnitTests\ByteDev.DotNet.UnitTests.csproj"));
            Assert.That(sut.Projects.Third().Id, Is.EqualTo(new Guid("34D79A1C-6035-486F-B885-FEEF1DB6632A")));
        }

        [Test]
        public void WhenSlnHasSolutionFolder_ThenSetProjectProperties()
        {
            var sut = CreateSut(TestSlnFiles.V12);

            Assert.That(sut.Projects.Fourth().Type.Id, Is.EqualTo(ProjectTypeIds.SolutionFolder));
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

        [Test]
        public void WhenSlnHasGlobalSections_ThenSetGlobalSectionsProperty()
        {
            var sut = CreateSut(TestSlnFiles.V12);

            Assert.That(sut.GlobalSections.Count, Is.EqualTo(5));

            Assert.That(sut.GlobalSections.First().Name, Is.EqualTo("SolutionConfigurationPlatforms"));
            Assert.That(sut.GlobalSections.First().Type, Is.EqualTo(GlobalSectionType.PreSolution));
            Assert.That(sut.GlobalSections.First().Settings.Count, Is.EqualTo(2));
            Assert.That(sut.GlobalSections.First().Settings["Debug|Any CPU"], Is.EqualTo("Debug|Any CPU"));
            Assert.That(sut.GlobalSections.First().Settings["Release|Any CPU"], Is.EqualTo("Release|Any CPU"));

            Assert.That(sut.GlobalSections.Fifth().Name, Is.EqualTo("ExtensibilityGlobals"));
            Assert.That(sut.GlobalSections.Fifth().Type, Is.EqualTo(GlobalSectionType.PostSolution));
            Assert.That(sut.GlobalSections.Fifth().Settings.Count, Is.EqualTo(1));
            Assert.That(sut.GlobalSections.Fifth().Settings["SolutionGuid"], Is.EqualTo("{D663B417-FDD2-470E-B2AF-230ED44C07C6}"));
        }

        private static DotNetSolution CreateSut(string slnFilePath)
        {
            return DotNetSolution.Load(slnFilePath);
        }
    }
}