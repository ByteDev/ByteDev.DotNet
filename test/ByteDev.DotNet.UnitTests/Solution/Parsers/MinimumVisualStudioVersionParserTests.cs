using System;
using ByteDev.DotNet.Solution;
using ByteDev.DotNet.Solution.Parsers;
using NUnit.Framework;

namespace ByteDev.DotNet.UnitTests.Solution.Parsers
{
    [TestFixture]
    public class MinimumVisualStudioVersionParserTests
    {
        private MinimumVisualStudioVersionParser _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new MinimumVisualStudioVersionParser();
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
        public void WhenDoesNotContainMinVsVersion_ThenThrowException()
        {
            Assert.Throws<InvalidDotNetSolutionException>(() => _sut.Parse("MinimumVisualStudioVersion = "));
        }

        [Test]
        public void WhenLineDoesNotStartWithMinimumVisualStudioVersion_ThenThrowException()
        {
            Assert.Throws<InvalidDotNetSolutionException>(() => _sut.Parse(" MinimumVisualStudioVersion = 10.0.40219.1"));
        }

        [TestCase("MinimumVisualStudioVersion = 10.0.40219.1", "10.0.40219.1")]
        [TestCase("MinimumVisualStudioVersion = 1", "1")]
        public void WhenContainsMinVsVersion_ThenReturnVersion(string slnText, string expected)
        {
            var result = _sut.Parse(slnText);

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}