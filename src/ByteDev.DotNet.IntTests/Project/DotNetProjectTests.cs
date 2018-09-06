using System.Xml.Linq;
using ByteDev.DotNet.Project;
using NUnit.Framework;

namespace ByteDev.DotNet.IntTests.Project
{
    [TestFixture]
    public class DotNetProjectTests
    {
        [TestFixture]
        public class Constructor : DotNetProjectTests
        {
            [Test]
            public void WhenNoPropertyGroups_ThenThrowException()
            {
                var xDocument = XDocument.Load(TestProjFiles.NewFormat.NoPropertyGroups);

                var ex = Assert.Throws<InvalidDotNetProjectException>(() => new DotNetProject(xDocument));
                Assert.That(ex.Message, Is.EqualTo("Project document contains no PropertyGroup elements."));
            }
        }

        [TestFixture]
        public class ProjectTarget : DotNetProjectTests
        {
            [Test]
            public void WhenProjXmlIsStandard_ThenReturnTargetFramework()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Std20);

                Assert.That(sut.ProjectTarget.TargetValue, Is.EqualTo("netstandard2.0"));
            }

            [Test]
            public void WhenProjXmlIsCore_ThenReturnTargetFramework()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.ProjectTarget.TargetValue, Is.EqualTo("netcoreapp2.1"));
            }

            [Test]
            public void WhenProjXmIsNewFormatFramework_ThenReturnTargetFramework()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Framework471);

                Assert.That(sut.ProjectTarget.TargetValue, Is.EqualTo("net471"));
            }

            [Test]
            public void WhenProjXmlIsOldFormatFramework_ThenReturnTargetFramework()
            {
                var sut = CreateSut(TestProjFiles.OldFormat.Framework462);

                Assert.That(sut.ProjectTarget.TargetValue, Is.EqualTo("v4.6.2"));
            }

            private static DotNetProject CreateSut(string filePath)
            {
                var xDocument = XDocument.Load(filePath);

                return new DotNetProject(xDocument);
            }
        }

        [TestFixture]
        public class Format : DotNetProjectTests
        {
            [Test]
            public void WhenProjectXmlFormatIsOldStyle_ThenReturnOld()
            {
                var xDocument = XDocument.Load(TestProjFiles.OldFormat.Framework462);

                var sut = new DotNetProject(xDocument);

                Assert.That(sut.Format, Is.EqualTo(ProjectFormat.Old));
            }

            [Test]
            public void WhenProjectXmlFormatIsNewStyle_ThenReturnNew()
            {
                var xDocument = XDocument.Load(TestProjFiles.NewFormat.Core21);

                var sut = new DotNetProject(xDocument);

                Assert.That(sut.Format, Is.EqualTo(ProjectFormat.New));
            }
        }
    }
}