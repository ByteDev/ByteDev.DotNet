using System;
using ByteDev.DotNet.Solution;
using ByteDev.DotNet.Solution.Parsers;
using NUnit.Framework;

namespace ByteDev.DotNet.UnitTests.Solution.Parsers
{
    [TestFixture]
    public class VisualStudioVersionParserTests
    {
        private VisualStudioVersionParser _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new VisualStudioVersionParser();
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
        public void WhenDoesNotContainVsVersion_ThenThrowException()
        {
            Assert.Throws<InvalidDotNetSolutionException>(() => _sut.Parse("VisualStudioVersion = "));
        }

        [Test]
        public void WhenLineDoesNotStartWithVsVersion_ThenThrowException()
        {
            Assert.Throws<InvalidDotNetSolutionException>(() => _sut.Parse(" VisualStudioVersion = 16.0.29709.97"));
        }

        [TestCase("VisualStudioVersion = 16.0.29709.97", "16.0.29709.97")]
        [TestCase("VisualStudioVersion = 16.0.29709.97 ", "16.0.29709.97")]
        [TestCase("VisualStudioVersion = 1", "1")]
        public void WhenContainsVsVersion_ThenReturnVersionNumber(string slnText, string expected)
        {
            var result = _sut.Parse(slnText);

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}