using System;
using System.Linq;
using ByteDev.Collections;
using ByteDev.DotNet.Solution;
using NUnit.Framework;

namespace ByteDev.DotNet.IntTests.Solution
{
    [TestFixture]
    public class DotNetSolutionTests
    {
        private static DotNetSolution CreateSut(string slnFilePath)
        {
            return DotNetSolution.Load(slnFilePath);
        }

        [TestFixture]
        public class FormatVersion : DotNetSolutionTests
        {
            [Test]
            public void WhenSlnHasFormatVersion_ThenSetsFormatVersion()
            {
                var sut = CreateSut(TestSlnFiles.V12);

                Assert.That(sut.FormatVersion, Is.EqualTo("12.00"));
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
        }

        [TestFixture]
        public class MajorVersion : DotNetSolutionTests
        {
            [Test]
            public void WhenSlnHasMajorVersion_ThenSetsMajorVersion()
            {
                var sut = CreateSut(TestSlnFiles.V12);

                Assert.That(sut.MajorVersion, Is.EqualTo(15));
            }

            [Test]
            public void WhenSlnHasNoMajorVersion_ThenThrowException()
            {
                var sut = CreateSut(TestSlnFiles.NoMajorVersion);

                var ex = Assert.Throws<InvalidDotNetSolutionException>(() =>
                {
                    var x = sut.MajorVersion;
                });
                Assert.That(ex.Message, Is.EqualTo("A valid Major Version could not be found in the sln text."));
            }
        }

        [TestFixture]
        public class VisualStudioVersion : DotNetSolutionTests
        {
            [Test]
            public void WhenSlnHasVisualStudioVersion_ThenSetsVisualStudioVersion()
            {
                var sut = CreateSut(TestSlnFiles.V12);

                Assert.That(sut.VisualStudioVersion, Is.EqualTo("15.0.27703.2042"));
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
        }

        [TestFixture]
        public class MinimumVisualStudioVersion : DotNetSolutionTests
        {
            [Test]
            public void WhenSlnHasMinimumVisualStudioVersion_ThenSetMinimumVisualStudioVersion()
            {
                var sut = CreateSut(TestSlnFiles.V12);

                Assert.That(sut.MinimumVisualStudioVersion, Is.EqualTo("10.0.40219.1"));
            }
            
            [Test]
            public void WhenSlnHasNoMinVisualStudioVersion_ThenThrowException()
            {
                var sut = CreateSut(TestSlnFiles.NoMinVsVersion);

                var ex = Assert.Throws<InvalidDotNetSolutionException>(() =>
                {
                    var x = sut.MinimumVisualStudioVersion;
                });
                Assert.That(ex.Message, Is.EqualTo("A valid Minimum Visual Studio Version could not be found in the sln text."));
            }
        }

        [TestFixture]
        public class Projects : DotNetSolutionTests
        {
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

                Assert.That(sut.Projects.Fourth().Type.Id, Is.EqualTo(ProjectTypeIds.SolutionFolder));
                Assert.That(sut.Projects.Fourth().Name, Is.EqualTo("Tests"));
                Assert.That(sut.Projects.Fourth().Path, Is.EqualTo("Tests"));
                Assert.That(sut.Projects.Fourth().Id, Is.EqualTo(new Guid("F8FDF7E8-B7FB-4BA1-8107-DB114CB729BB")));

                Assert.That(sut.Projects.Fifth().Type.Id, Is.EqualTo(ProjectTypeIds.SolutionFolder));
                Assert.That(sut.Projects.Fifth().Name, Is.EqualTo("nuget"));
                Assert.That(sut.Projects.Fifth().Path, Is.EqualTo("nuget"));
                Assert.That(sut.Projects.Fifth().Id, Is.EqualTo(new Guid("62D43D35-230B-4452-8F11-BC3FD4E18F95")));
            }

            [Test]
            public void WhenSlnHasNoProjects_ThenReturnEmpty()
            {
                var sut = CreateSut(TestSlnFiles.V12NoProjs);

                Assert.That(sut.Projects.Count, Is.EqualTo(0));
            }

            [Test]
            public void WhenProjectHasSections_ThenSetSectionProperty()
            {
                var sut = CreateSut(TestSlnFiles.V12);

                Assert.That(sut.Projects.Fifth().ProjectSections.First().Name, Is.EqualTo("SolutionItems"));
                Assert.That(sut.Projects.Fifth().ProjectSections.First().Type, Is.EqualTo(ProjectSectionType.PreProject));
                Assert.That(sut.Projects.Fifth().ProjectSections.First().Dependencies.Count, Is.EqualTo(1));
                Assert.That(sut.Projects.Fifth().ProjectSections.First().Dependencies[@".nuget\NuGet.Config"], Is.EqualTo(@".nuget\NuGet.Config"));

                Assert.That(sut.Projects.Fifth().ProjectSections.Second().Name, Is.EqualTo("TestItems"));
                Assert.That(sut.Projects.Fifth().ProjectSections.Second().Type, Is.EqualTo(ProjectSectionType.PostProject));
                Assert.That(sut.Projects.Fifth().ProjectSections.Second().Dependencies.Count, Is.EqualTo(2));
                Assert.That(sut.Projects.Fifth().ProjectSections.Second().Dependencies[@"{B381C2FB-9E2E-4833-822B-1EC5DFF2458F}"], Is.EqualTo(@"{B381C2FB-9E2E-4833-822B-1EC5DFF2458F}"));
                Assert.That(sut.Projects.Fifth().ProjectSections.Second().Dependencies[@"{C391D787-0B39-4122-9712-714A9F9742CD}"], Is.EqualTo(@"{C391D787-0B39-4122-9712-714A9F9742CD}"));
            }

            [Test]
            public void WhenProjectHasNoSections_ThenSetToEmpty()
            {
                var sut = CreateSut(TestSlnFiles.V12);

                Assert.That(sut.Projects.First().ProjectSections, Is.Empty);
            }
        }

        [TestFixture]
        public class GlobalSections : DotNetSolutionTests
        {
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

            [Test]
            public void WhenSlnHasNoGlobalSections_ThenSetGlobalSectionsToEmpty()
            {
                var sut = CreateSut(TestSlnFiles.V12NoGlobalSections);

                Assert.That(sut.GlobalSections, Is.Empty);
            }

            [Test]
            public void WhenSlnHasNoGlobal_ThenSetGlobalSectionsToEmpty()
            {
                var sut = CreateSut(TestSlnFiles.V12NoGlobal);

                Assert.That(sut.GlobalSections, Is.Empty);
            }
        }
    }
}