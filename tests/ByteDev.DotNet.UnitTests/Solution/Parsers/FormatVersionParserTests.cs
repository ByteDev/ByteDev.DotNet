using System;
using ByteDev.DotNet.Solution;
using ByteDev.DotNet.Solution.Parsers;
using NUnit.Framework;

namespace ByteDev.DotNet.UnitTests.Solution.Parsers
{
    [TestFixture]
    public class FormatVersionParserTests
    {
        private FormatVersionParser _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new FormatVersionParser();
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
        public void WhenDoesNotContainMsVsSlnFileFormatVersion_ThenThrowException()
        {
            Assert.Throws<InvalidDotNetSolutionException>(() => _sut.Parse("Format Version 123"));
        }

        [Test]
        public void WhenLineDoesNotStartWithMsVsSlnFileFormatVersion_ThenThrowException()
        {
            Assert.Throws<InvalidDotNetSolutionException>(() => _sut.Parse(" Microsoft Visual Studio Solution File, Format Version 12.00"));
        }

        [TestCase("Microsoft Visual Studio Solution File, Format Version 12.00", "12.00")]
        [TestCase("\nMicrosoft Visual Studio Solution File, Format Version 12.00", "12.00")]
        [TestCase("Microsoft Visual Studio Solution File, Format Version 1", "1")]
        public void WhenContainsMinVsVersion_ThenReturnVersion(string slnText, string expected)
        {
            var result = _sut.Parse(slnText);

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}