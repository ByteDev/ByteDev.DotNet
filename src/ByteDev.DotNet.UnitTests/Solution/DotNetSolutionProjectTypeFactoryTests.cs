using System;
using ByteDev.DotNet.Solution;
using NUnit.Framework;

namespace ByteDev.DotNet.UnitTests.Solution
{
    [TestFixture]
    public class DotNetSolutionProjectTypeFactoryTests
    {
        private static readonly Guid UnknownTypeId = new Guid("55DB836B-99DB-4B21-A67A-215DE017581A");
        private static readonly Guid SolutionFolderId = new Guid("2150E333-8FDC-42A3-9474-1A3956D46DE8");

        private DotNetSolutionProjectTypeFactory _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new DotNetSolutionProjectTypeFactory();
        }

        [Test]
        public void WhenIdIsUnknownType_ThenReturnUnknownProjectType()
        {
            var result = _sut.Create(UnknownTypeId);

            Assert.That(result, Is.TypeOf<UnknownDotNetSolutionProjectType>());
            Assert.That(result.Id, Is.EqualTo(UnknownTypeId));
            Assert.That(result.Description, Is.EqualTo("(Unknown)"));
        }

        [Test]
        public void WhenIdIsKnownType_ThenReturnKnownProjectType()
        {
            var result = _sut.Create(SolutionFolderId);

            Assert.That(result, Is.TypeOf<DotNetSolutionProjectType>());
            Assert.That(result.Id, Is.EqualTo(SolutionFolderId));
            Assert.That(result.Description, Is.EqualTo("Solution Folder"));
        }
    }
}