using System;
using ByteDev.DotNet.Project;
using NUnit.Framework;

namespace ByteDev.DotNet.UnitTests.Project
{
    [TestFixture]
    public class DotNetProjectTargetTests
    {
        [Test]
        public void WhenTargetIsNull_ThenThrowException()
        {
            Assert.Throws<ArgumentException>(() => new DotNetProjectTarget(null));
        }

        [Test]
        public void WhenTargetIsEmpty_ThenThrowException()
        {
            Assert.Throws<ArgumentException>(() => new DotNetProjectTarget(string.Empty));
        }

        [Test]
        public void WhenTargetIsInvalid_ThenThrowException()
        {
            Assert.Throws<InvalidDotNetProjectException>(() => new DotNetProjectTarget("4.5"));
        }

        [TestCase("v4.5", TargetType.Framework, "4.5")]
        [TestCase("v4.7.2", TargetType.Framework, "4.7.2")]
        [TestCase("net471", TargetType.Framework, "471")]
        [TestCase("netcoreapp1.0", TargetType.Core, "1.0")]
        [TestCase("netcoreapp1.1", TargetType.Core, "1.1")]
        [TestCase("netcoreapp2.0", TargetType.Core, "2.0")]
        [TestCase("netcoreapp2.1", TargetType.Core, "2.1")]
        [TestCase("netstandard1.0", TargetType.Standard, "1.0")]
        [TestCase("netstandard1.1", TargetType.Standard, "1.1")]
        [TestCase("netstandard1.6", TargetType.Standard, "1.6")]
        [TestCase("netstandard2.0", TargetType.Standard, "2.0")]
        public void WhenTargetIsValid_ThenSetProperties(string targetValue, TargetType type, string version)
        {
            var sut = new DotNetProjectTarget(targetValue);

            Assert.That(sut.TargetValue, Is.EqualTo(targetValue));
            Assert.That(sut.Type, Is.EqualTo(type));
            Assert.That(sut.Version, Is.EqualTo(version));
        }
    }
}