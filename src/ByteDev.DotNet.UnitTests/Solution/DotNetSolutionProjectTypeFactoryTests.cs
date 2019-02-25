using System;
using ByteDev.DotNet.Solution;
using NUnit.Framework;

namespace ByteDev.DotNet.UnitTests.Solution
{
    [TestFixture]
    public class DotNetSolutionProjectTypeFactoryTests
    {
        private static readonly Guid SolutionFolder = new Guid("2150E333-8FDC-42A3-9474-1A3956D46DE8");

        private DotNetSolutionProjectTypeFactory _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new DotNetSolutionProjectTypeFactory();
        }

        [Test]
        public void WhenDoesNotExist_ThenThrowException()
        {
            Assert.Throws<ArgumentException>(() => _sut.Create(Guid.Empty));
        }

        [Test]
        public void WhenDoesExist_ThenReturnProjectType()
        {
            var result = _sut.Create(SolutionFolder);

            Assert.That(result.Id, Is.EqualTo(SolutionFolder));
            Assert.That(result.Description, Is.EqualTo("Solution Folder"));
        }
    }
}