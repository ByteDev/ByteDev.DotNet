using System;
using ByteDev.DotNet.Solution;
using ByteDev.DotNet.Solution.Parsers;
using NUnit.Framework;

namespace ByteDev.DotNet.UnitTests.Solution.Parsers
{
    [TestFixture]
    public class MajorVersionParserTests
    {
        private MajorVersionParser _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new MajorVersionParser();
        }

        [Test]
        public void WhenIsNull_ThenThrowException()
        {
            Assert.Throws<ArgumentException>(() => _sut.Parse(null));
        }

        [Test]
        public void WhenIsEmpty_ThenThrowException()
        {
            Assert.Throws<ArgumentException>(() => _sut.Parse(string.Empty));
        }

        [Test]
        public void WhenDoesNotContainMajorVersion_ThenThrowException()
        {
            Assert.Throws<InvalidDotNetSolutionException>(() => _sut.Parse("# Visual Studio"));
        }

        [TestCase("# Visual Studio 1", 1)]
        [TestCase("# Visual Studio 15", 15)]
        [TestCase("# Visual Studio 100", 100)]
        [TestCase("# Visual Studio Version 1", 1)]
        [TestCase("# Visual Studio Version 16", 16)]
        [TestCase("# Visual Studio Version 100", 100)]
        public void WhenContainsMajorVersion_ThenReturnVersionNumber(string slnText, int expected)
        {
            var result = _sut.Parse(slnText);

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}