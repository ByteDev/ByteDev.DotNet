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

        [TestCase("net11", TargetType.Framework, "1.1", ".NET Framework 1.1")]
        [TestCase("net20", TargetType.Framework, "2.0", ".NET Framework 2.0")]
        [TestCase("net35", TargetType.Framework, "3.5", ".NET Framework 3.5")]
        [TestCase("net40", TargetType.Framework, "4.0", ".NET Framework 4.0")]
        [TestCase("net403", TargetType.Framework, "4.0.3", ".NET Framework 4.0.3")]
        [TestCase("net45", TargetType.Framework, "4.5", ".NET Framework 4.5")]
        [TestCase("net451", TargetType.Framework, "4.5.1", ".NET Framework 4.5.1")]
        [TestCase("net452", TargetType.Framework, "4.5.2", ".NET Framework 4.5.2")]
        [TestCase("net46", TargetType.Framework, "4.6", ".NET Framework 4.6")]
        [TestCase("net461", TargetType.Framework, "4.6.1", ".NET Framework 4.6.1")]
        [TestCase("net462", TargetType.Framework, "4.6.2", ".NET Framework 4.6.2")]
        [TestCase("net47", TargetType.Framework, "4.7", ".NET Framework 4.7")]
        [TestCase("net471", TargetType.Framework, "4.7.1", ".NET Framework 4.7.1")]
        [TestCase("net472", TargetType.Framework, "4.7.2", ".NET Framework 4.7.2")]
        [TestCase("netcoreapp1.0", TargetType.Core, "1.0", ".NET Core 1.0")]
        [TestCase("netcoreapp1.1", TargetType.Core, "1.1", ".NET Core 1.1")]
        [TestCase("netcoreapp2.0", TargetType.Core, "2.0", ".NET Core 2.0")]
        [TestCase("netcoreapp2.1", TargetType.Core, "2.1", ".NET Core 2.1")]
        [TestCase("netcoreapp2.2", TargetType.Core, "2.2", ".NET Core 2.2")]
        [TestCase("netcoreapp3.0", TargetType.Core, "3.0", ".NET Core 3.0")]
        [TestCase("netcoreapp3.1", TargetType.Core, "3.1", ".NET Core 3.1")]
        [TestCase("netcoreapp5.0", TargetType.Core, "5.0", ".NET 5.0")]
        [TestCase("netstandard1.0", TargetType.Standard, "1.0", ".NET Standard 1.0")]
        [TestCase("netstandard1.1", TargetType.Standard, "1.1", ".NET Standard 1.1")]
        [TestCase("netstandard1.2", TargetType.Standard, "1.2", ".NET Standard 1.2")]
        [TestCase("netstandard1.3", TargetType.Standard, "1.3", ".NET Standard 1.3")]
        [TestCase("netstandard1.4", TargetType.Standard, "1.4", ".NET Standard 1.4")]
        [TestCase("netstandard1.5", TargetType.Standard, "1.5", ".NET Standard 1.5")]
        [TestCase("netstandard1.6", TargetType.Standard, "1.6", ".NET Standard 1.6")]
        [TestCase("netstandard2.0", TargetType.Standard, "2.0", ".NET Standard 2.0")]
        [TestCase("netstandard2.1", TargetType.Standard, "2.1", ".NET Standard 2.1")]
        public void WhenTargetIsNewFormat_ThenSetProperties(string targetValue, TargetType type, string version, string description)
        {
            var sut = new DotNetProjectTarget(targetValue);

            Assert.That(sut.TargetValue, Is.EqualTo(targetValue));
            Assert.That(sut.Type, Is.EqualTo(type));
            Assert.That(sut.Version, Is.EqualTo(version));
            Assert.That(sut.Description, Is.EqualTo(description));
        }

        [TestCase("v4.5", TargetType.Framework, "4.5", ".NET Framework 4.5")]
        [TestCase("v4.7.2", TargetType.Framework, "4.7.2", ".NET Framework 4.7.2")]
        public void WhenTargetIsOldFormat_ThenSetProperties(string targetValue, TargetType type, string version, string description)
        {
            var sut = new DotNetProjectTarget(targetValue);

            Assert.That(sut.TargetValue, Is.EqualTo(targetValue));
            Assert.That(sut.Type, Is.EqualTo(type));
            Assert.That(sut.Version, Is.EqualTo(version));
            Assert.That(sut.Description, Is.EqualTo(description));
        }
    }
}