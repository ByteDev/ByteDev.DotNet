using System;
using ByteDev.DotNet.Project;
using NUnit.Framework;

namespace ByteDev.DotNet.UnitTests.Project
{
    [TestFixture]
    public class TargetFrameworkTests
    {
        [Test]
        public void WhenTargetIsNull_ThenThrowException()
        {
            Assert.Throws<ArgumentException>(() => _ = new TargetFramework(null));
        }

        [Test]
        public void WhenTargetIsEmpty_ThenThrowException()
        {
            Assert.Throws<ArgumentException>(() => _ = new TargetFramework(string.Empty));
        }

        [Test]
        public void WhenTargetIsInvalid_ThenOnlySetMoniker()
        {
            var sut = new TargetFramework("4.5");

            Assert.That(sut.Moniker, Is.EqualTo("4.5"));
            Assert.That(sut.FrameworkType, Is.EqualTo(TargetFrameworkType.Unknown));
            Assert.That(sut.Version, Is.Empty);
            Assert.That(sut.Description, Is.Null);
        }

        // Full moniker list: https://docs.microsoft.com/en-us/dotnet/standard/frameworks
        
        [TestCase("net11", TargetFrameworkType.Framework, "1.1", ".NET Framework 1.1")]
        [TestCase("net20", TargetFrameworkType.Framework, "2.0", ".NET Framework 2.0")]
        [TestCase("net35", TargetFrameworkType.Framework, "3.5", ".NET Framework 3.5")]
        [TestCase("net40", TargetFrameworkType.Framework, "4.0", ".NET Framework 4.0")]
        [TestCase("net403", TargetFrameworkType.Framework, "4.0.3", ".NET Framework 4.0.3")]
        [TestCase("net45", TargetFrameworkType.Framework, "4.5", ".NET Framework 4.5")]
        [TestCase("net451", TargetFrameworkType.Framework, "4.5.1", ".NET Framework 4.5.1")]
        [TestCase("net452", TargetFrameworkType.Framework, "4.5.2", ".NET Framework 4.5.2")]
        [TestCase("net46", TargetFrameworkType.Framework, "4.6", ".NET Framework 4.6")]
        [TestCase("net461", TargetFrameworkType.Framework, "4.6.1", ".NET Framework 4.6.1")]
        [TestCase("net462", TargetFrameworkType.Framework, "4.6.2", ".NET Framework 4.6.2")]
        [TestCase("net47", TargetFrameworkType.Framework, "4.7", ".NET Framework 4.7")]
        [TestCase("net471", TargetFrameworkType.Framework, "4.7.1", ".NET Framework 4.7.1")]
        [TestCase("net472", TargetFrameworkType.Framework, "4.7.2", ".NET Framework 4.7.2")]
        [TestCase("net48", TargetFrameworkType.Framework, "4.8", ".NET Framework 4.8")]
        [TestCase("netcoreapp1.0", TargetFrameworkType.Core, "1.0", ".NET Core 1.0")]
        [TestCase("netcoreapp1.1", TargetFrameworkType.Core, "1.1", ".NET Core 1.1")]
        [TestCase("netcoreapp2.0", TargetFrameworkType.Core, "2.0", ".NET Core 2.0")]
        [TestCase("netcoreapp2.1", TargetFrameworkType.Core, "2.1", ".NET Core 2.1")]
        [TestCase("netcoreapp2.2", TargetFrameworkType.Core, "2.2", ".NET Core 2.2")]
        [TestCase("netcoreapp3.0", TargetFrameworkType.Core, "3.0", ".NET Core 3.0")]
        [TestCase("netcoreapp3.1", TargetFrameworkType.Core, "3.1", ".NET Core 3.1")]
        [TestCase("netcoreapp5.0", TargetFrameworkType.Core, "5.0", ".NET 5.0")]
        [TestCase("net5", TargetFrameworkType.Core, "5", ".NET 5")]
        [TestCase("net5.0", TargetFrameworkType.Core, "5.0", ".NET 5.0")]
        [TestCase("net5.0-windows", TargetFrameworkType.Core, "5.0", ".NET 5.0")]
        [TestCase("net5.0-macos", TargetFrameworkType.Core, "5.0", ".NET 5.0")]
        [TestCase("netstandard1.0", TargetFrameworkType.Standard, "1.0", ".NET Standard 1.0")]
        [TestCase("netstandard1.1", TargetFrameworkType.Standard, "1.1", ".NET Standard 1.1")]
        [TestCase("netstandard1.2", TargetFrameworkType.Standard, "1.2", ".NET Standard 1.2")]
        [TestCase("netstandard1.3", TargetFrameworkType.Standard, "1.3", ".NET Standard 1.3")]
        [TestCase("netstandard1.4", TargetFrameworkType.Standard, "1.4", ".NET Standard 1.4")]
        [TestCase("netstandard1.5", TargetFrameworkType.Standard, "1.5", ".NET Standard 1.5")]
        [TestCase("netstandard1.6", TargetFrameworkType.Standard, "1.6", ".NET Standard 1.6")]
        [TestCase("netstandard2.0", TargetFrameworkType.Standard, "2.0", ".NET Standard 2.0")]
        [TestCase("netstandard2.1", TargetFrameworkType.Standard, "2.1", ".NET Standard 2.1")]
        [TestCase("netmf", TargetFrameworkType.MicroFramework, "", ".NET Micro Framework")]
        [TestCase("sl4", TargetFrameworkType.Silverlight, "4", "Silverlight 4")]
        [TestCase("sl5", TargetFrameworkType.Silverlight, "5", "Silverlight 5")]
        [TestCase("wp", TargetFrameworkType.WindowsPhone, "", "Windows Phone")]
        [TestCase("wp [wp7]", TargetFrameworkType.WindowsPhone, "7", "Windows Phone 7")]
        [TestCase("wp [wp71]", TargetFrameworkType.WindowsPhone, "7.1", "Windows Phone 7.1")]
        [TestCase("wp75", TargetFrameworkType.WindowsPhone, "7.5", "Windows Phone 7.5")]
        [TestCase("wp8", TargetFrameworkType.WindowsPhone, "8", "Windows Phone 8")]
        [TestCase("wp81", TargetFrameworkType.WindowsPhone, "8.1", "Windows Phone 8.1")]
        [TestCase("wpa81", TargetFrameworkType.WindowsPhone, "8.1", "Windows Phone 8.1")]
        [TestCase("uap [uap10.0]", TargetFrameworkType.UniversalWindowsPlatform, "10.0", "Universal Windows Platform 10.0")]
        [TestCase("uap10.0 [win10] [netcore50]", TargetFrameworkType.UniversalWindowsPlatform, "10.0", "Universal Windows Platform 10.0")]
        [TestCase("uap10.0", TargetFrameworkType.UniversalWindowsPlatform, "10.0", "Universal Windows Platform 10.0")]
        [TestCase("netcore [netcore45]", TargetFrameworkType.WindowsStore, "4.5", "Windows Store 4.5")]
        [TestCase("netcore45", TargetFrameworkType.WindowsStore, "4.5", "Windows Store 4.5")]
        [TestCase("netcore45 [win] [win8]", TargetFrameworkType.WindowsStore, "4.5", "Windows Store 4.5")]
        [TestCase("netcore451 [win81]", TargetFrameworkType.WindowsStore, "4.5.1", "Windows Store 4.5.1")]
        public void WhenTargetIsNewFormat_ThenSetProperties(string moniker, TargetFrameworkType frameworkType, string version, string description)
        {
            var sut = new TargetFramework(moniker);

            Assert.That(sut.Moniker, Is.EqualTo(moniker));
            Assert.That(sut.FrameworkType, Is.EqualTo(frameworkType));
            Assert.That(sut.Version, Is.EqualTo(version));
            Assert.That(sut.Description, Is.EqualTo(description));
        }

        [TestCase("v4.5", TargetFrameworkType.Framework, "4.5", ".NET Framework 4.5")]
        [TestCase("v4.7.2", TargetFrameworkType.Framework, "4.7.2", ".NET Framework 4.7.2")]
        public void WhenTargetIsOldFormatFramework_ThenSetProperties(string moniker, TargetFrameworkType frameworkType, string version, string description)
        {
            var sut = new TargetFramework(moniker);

            Assert.That(sut.Moniker, Is.EqualTo(moniker));
            Assert.That(sut.FrameworkType, Is.EqualTo(frameworkType));
            Assert.That(sut.Version, Is.EqualTo(version));
            Assert.That(sut.Description, Is.EqualTo(description));
        }
    }
}