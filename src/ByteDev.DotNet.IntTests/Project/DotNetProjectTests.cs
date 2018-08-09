using System.Xml.Linq;
using ByteDev.DotNet.Project;
using NUnit.Framework;

namespace ByteDev.DotNet.IntTests.Project
{
    internal static class TestProjects
    {
        public const string TestProjStd20 = @"Project\TestProjs\std20.xml";
        public const string TestProjCore21 = @"Project\TestProjs\core21.xml";
        public const string TestProjFramework462 = @"Project\TestProjs\framework462.xml";
    }

    [TestFixture]
    public class DotNetProjectTests
    {
        [TestFixture]
        public class ProjectTarget : DotNetProjectTests
        {
            [Test]
            public void WhenProjectXmlIsStandard_ThenReturnTargetFramework()
            {
                var xDocument = XDocument.Load(TestProjects.TestProjStd20);

                var sut = new DotNetProject(xDocument);

                Assert.That(sut.ProjectTarget.TargetValue, Is.EqualTo("netstandard2.0"));
            }

            [Test]
            public void WhenProjectXmlIsCore_ThenReturnTargetFramework()
            {
                var xDocument = XDocument.Load(TestProjects.TestProjCore21);

                var sut = new DotNetProject(xDocument);

                Assert.That(sut.ProjectTarget.TargetValue, Is.EqualTo("netcoreapp2.1"));
            }

            [Test]
            public void WhenProjectXmlIsFramework_ThenReturnTargetFramework()
            {
                var xDocument = XDocument.Load(TestProjects.TestProjFramework462);

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
                var xDocument = XDocument.Load(TestProjects.TestProjFramework462);

                var sut = new DotNetProject(xDocument);

                Assert.That(sut.Format, Is.EqualTo(ProjectFormat.Old));
            }

            [Test]
            public void WhenProjectXmlFormatIsNewStyle_ThenReturnNew()
            {
                var xDocument = XDocument.Load(TestProjects.TestProjCore21);

                var sut = new DotNetProject(xDocument);

                Assert.That(sut.Format, Is.EqualTo(ProjectFormat.New));
            }
        }
    }
}