using System.Xml.Linq;
using NUnit.Framework;

namespace ByteDev.DotNet.IntTests
{
    [TestFixture]
    public class DotNetProjectTests
    {
        private const string TestProjStd20 = @"TestProjs\std20.xml";
        private const string TestProjCore21 = @"TestProjs\core21.xml";
        private const string TestProjFramework462 = @"TestProjs\framework462.xml";

        [Test]
        public void WhenProjectXmlIsStandard_ThenReturnTargetFramework()
        {
            var xDocument = XDocument.Load(TestProjStd20);

            var sut = new DotNetProject(xDocument);

            Assert.That(sut.TargetFramework, Is.EqualTo("netstandard2.0"));
        }

        [Test]
        public void WhenProjectXmlIsCore_ThenReturnTargetFramework()
        {
            var xDocument = XDocument.Load(TestProjCore21);

            var sut = new DotNetProject(xDocument);

            Assert.That(sut.TargetFramework, Is.EqualTo("netcoreapp2.1"));
        }

        [Test]
        public void WhenProjectXmlIsFramework_ThenReturnTargetFramework()
        {
            var xDocument = XDocument.Load(TestProjFramework462);

            var sut = new DotNetProject(xDocument);

            Assert.That(sut.TargetFramework, Is.EqualTo("v4.6.2"));
        }
    }
}