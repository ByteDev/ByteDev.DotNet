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
            public void WhenProjectXmlIsStandard_ThenReturnTargetFramework()
            {
                var xDocument = XDocument.Load(TestProjFiles.NewFormat.Std20);

                var sut = new DotNetProject(xDocument);

                Assert.That(sut.ProjectTarget.TargetValue, Is.EqualTo("netstandard2.0"));
            }

            [Test]
            public void WhenProjectXmlIsCore_ThenReturnTargetFramework()
            {
                var xDocument = XDocument.Load(TestProjFiles.NewFormat.Core21);

                var sut = new DotNetProject(xDocument);

                Assert.That(sut.ProjectTarget.TargetValue, Is.EqualTo("netcoreapp2.1"));
            }

            [Test]
            public void WhenProjectXmlIsFramework_ThenReturnTargetFramework()
            {
                var xDocument = XDocument.Load(TestProjFiles.OldFormat.Framework462);

                var sut = new DotNetProject(xDocument);

                Assert.That(sut.ProjectTarget.TargetValue, Is.EqualTo("v4.6.2"));
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