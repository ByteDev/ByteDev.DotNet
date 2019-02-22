using System.Linq;
using ByteDev.Common.Collections;
using ByteDev.DotNet.Project;
using NUnit.Framework;

namespace ByteDev.DotNet.IntTests.Project
{
    [TestFixture]
    public class DotNetProjectTests
    {
        private static DotNetProject CreateSut(string filePath)
        {
            return DotNetProject.Load(filePath);
        }

        [TestFixture]
        public class Constructor : DotNetProjectTests
        {
            [Test]
            public void WhenNoPropertyGroups_ThenThrowException()
            {
                var ex = Assert.Throws<InvalidDotNetProjectException>(() => CreateSut(TestProjFiles.NewFormat.NoPropertyGroups));
                Assert.That(ex.Message, Is.EqualTo("Project document contains no PropertyGroup elements."));
            }
        }

        [TestFixture]
        public class ProjectTargets : DotNetProjectTests
        {
            [Test]
            public void WhenProjXmlIsStandard_ThenReturnTarget()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Std20);

                Assert.That(sut.ProjectTargets.Single().TargetValue, Is.EqualTo("netstandard2.0"));
            }

            [Test]
            public void WhenProjXmlIsCore_ThenReturnTarget()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.ProjectTargets.Single().TargetValue, Is.EqualTo("netcoreapp2.1"));
            }

            [Test]
            public void WhenProjXmIsNewFormatFramework_ThenReturnTarget()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Framework471);

                Assert.That(sut.ProjectTargets.Single().TargetValue, Is.EqualTo("net471"));
            }

            [Test]
            public void WhenProjXmlIsOldFormatFramework_ThenReturnTarget()
            {
                var sut = CreateSut(TestProjFiles.OldFormat.Framework462);

                Assert.That(sut.ProjectTargets.Single().TargetValue, Is.EqualTo("v4.6.2"));
            }

            [Test]
            public void WhenIsMultiTarget_ThenReturnsAllTargets()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Std15AndFramework4);

                Assert.That(sut.IsMultiTarget, Is.True);

                Assert.That(sut.ProjectTargets.First().TargetValue, Is.EqualTo("netstandard1.5"));
                Assert.That(sut.ProjectTargets.Second().TargetValue, Is.EqualTo("net40"));
            }

            [Test]
            public void WhenTargetFrameworkIsNotFirstElement_ThenReturnTarget()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.ProjectTargets.Single().TargetValue, Is.EqualTo("netcoreapp2.1"));
            }
        }

        [TestFixture]
        public class Format : DotNetProjectTests
        {
            [Test]
            public void WhenProjectXmlFormatIsOldStyle_ThenReturnOld()
            {
                var sut = CreateSut(TestProjFiles.OldFormat.Framework462);

                Assert.That(sut.Format, Is.EqualTo(ProjectFormat.Old));
            }

            [Test]
            public void WhenProjectXmlFormatIsNewStyle_ThenReturnNew()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.Format, Is.EqualTo(ProjectFormat.New));
            }
        }

        [TestFixture]
        public class Description : DotNetProjectTests
        {
            [Test]
            public void WhenDescriptionIsPresent_ThenReturnDescription()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.Description, Is.EqualTo("new-core21 Description"));
            }

            [Test]
            public void WhenDescriptionIsNotPresent_ThenReturnNull()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.Description, Is.Null);
            }
        }

        [TestFixture]
        public class Authors : DotNetProjectTests
        {
            [Test]
            public void WhenAuthorsIsPresent_ThenReturnDescription()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.Authors, Is.EqualTo("John Smith, Jesus Christ"));
            }

            [Test]
            public void WhenAuthorsIsNotPresent_ThenReturnNull()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.Authors, Is.Null);
            }
        }

        [TestFixture]
        public class Company : DotNetProjectTests
        {
            [Test]
            public void WhenAuthorsIsPresent_ThenReturnDescription()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.Company, Is.EqualTo("Something Ltd."));
            }

            [Test]
            public void WhenAuthorsIsNotPresent_ThenReturnNull()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.Company, Is.Null);
            }
        }

        [TestFixture]
        public class PackageTags : DotNetProjectTests
        {
            [Test]
            public void WhenPackageTagsIsNotPresent_ThenReturnEmpty()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.PackageTags, Is.Empty);
            }

            [Test]
            public void WhenPackageTagsIsPresent_ThenReturnAllPackageTags()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);
                
                Assert.That(sut.PackageTags.First(), Is.EqualTo("something"));
                Assert.That(sut.PackageTags.Second(), Is.EqualTo("program"));
                Assert.That(sut.PackageTags.Third(), Is.EqualTo("exe"));
            }
        }

        [TestFixture]
        public class ProjectReferences : DotNetProjectTests
        {
            [Test]
            public void WhenNewFormat_AndNoProjectReferences_ThenReturnEmpty()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.ProjectReferences, Is.Empty);
            }

            [Test]
            public void WhenNewFormat_AndTwoProjectReferences_ThenReturnProjectReference()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.ProjectReferences.First().FilePath, Is.EqualTo(@"..\ByteDev.SomeProj1\ByteDev.SomeProj1.csproj"));
                Assert.That(sut.ProjectReferences.Second().FilePath, Is.EqualTo(@"..\ByteDev.SomeProj2\ByteDev.SomeProj2.csproj"));
            }

            [Test]
            public void WhenOldFormat_AndOneProjectReference_ThenReturnProjectReference()
            {
                var sut = CreateSut(TestProjFiles.OldFormat.Framework462);

                Assert.That(sut.ProjectReferences.First().FilePath, Is.EqualTo(@"..\ByteDev.AnotherProj\ByteDev.AnotherProj.csproj"));
            }
        }

        [TestFixture]
        public class PackageReferences : DotNetProjectTests
        {
            [Test]
            public void WhenNewFormat_AndNoPackageReferences_ThenReturnEmpty()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.PackageReferences, Is.Empty);
            }

            [Test]
            public void WhenNewFormat_AndTwoPackageReferences_ThenReturnPackageReferences()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.PackageReferences.First().Name, Is.EqualTo("Microsoft.NET.Test.Sdk"));
                Assert.That(sut.PackageReferences.First().Version, Is.EqualTo("15.8.0"));

                Assert.That(sut.PackageReferences.Second().Name, Is.EqualTo("NUnit"));
                Assert.That(sut.PackageReferences.Second().Version, Is.EqualTo("3.10.1"));
            }

            [Test]
            public void WhenOldFormat_ThenReturnEmpty()
            {
                var sut = CreateSut(TestProjFiles.OldFormat.Framework462);

                Assert.That(sut.PackageReferences, Is.Empty);
            }
        }
    }
}