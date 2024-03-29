﻿using System.Linq;
using ByteDev.Collections;
using ByteDev.DotNet.Project;
using NUnit.Framework;

namespace ByteDev.DotNet.IntTests.Project
{
    [TestFixture]
    public class DotNetProjectTests
    {
        private static DotNetProject CreateSut(string filePath)
        {
            return DotNetProject.Load(filePath);
        }

        [TestFixture]
        public class Constructor : DotNetProjectTests
        {
            [Test]
            public void WhenNoPropertyGroups_ThenThrowException()
            {
                var ex = Assert.Throws<InvalidDotNetProjectException>(() => CreateSut(TestProjFiles.NewFormat.NoPropertyGroups));
                Assert.That(ex.Message, Is.EqualTo("Project document contains no PropertyGroup elements."));
            }
        }
        
        [TestFixture]
        public class ProjectTargets : DotNetProjectTests
        {
            [Test]
            public void WhenProjXmlIsStandard_ThenReturnTarget()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Std20);

                Assert.That(sut.ProjectTargets.Single().Moniker, Is.EqualTo("netstandard2.0"));
            }

            [Test]
            public void WhenProjXmlIsCore_ThenReturnTarget()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.ProjectTargets.Single().Moniker, Is.EqualTo("netcoreapp2.1"));
            }

            [Test]
            public void WhenProjXmIsNewFormatFramework_ThenReturnTarget()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Framework471);

                Assert.That(sut.ProjectTargets.Single().Moniker, Is.EqualTo("net471"));
            }

            [Test]
            public void WhenProjXmlIsOldFormatFramework_ThenReturnTarget()
            {
                var sut = CreateSut(TestProjFiles.OldFormat.Framework462);

                Assert.That(sut.ProjectTargets.Single().Moniker, Is.EqualTo("v4.6.2"));
            }

            [Test]
            public void WhenIsMultiTarget_ThenReturnsAllTargets()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Std15AndFramework4);

                Assert.That(sut.IsMultiTarget, Is.True);

                Assert.That(sut.ProjectTargets.First().Moniker, Is.EqualTo("netstandard1.5"));
                Assert.That(sut.ProjectTargets.Second().Moniker, Is.EqualTo("net40"));
            }

            [Test]
            public void WhenTargetFrameworkIsNotFirstElement_ThenReturnTarget()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.ProjectTargets.Single().Moniker, Is.EqualTo("netcoreapp2.1"));
            }

            [Test]
            public void WhenNoTargetFrameworkSpecified_ThenSetToEmpty()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.NoTargetFramework);

                Assert.That(sut.ProjectTargets, Is.Empty);
            }
        }

        [TestFixture]
        public class Format : DotNetProjectTests
        {
            [Test]
            public void WhenProjectXmlFormatIsOldStyle_ThenReturnOld()
            {
                var sut = CreateSut(TestProjFiles.OldFormat.Framework462);

                Assert.That(sut.Format, Is.EqualTo(ProjectFormat.Old));
            }

            [Test]
            public void WhenProjectXmlFormatIsNewStyle_ThenReturnNew()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.Format, Is.EqualTo(ProjectFormat.New));
            }
        }
        
        [TestFixture]
        public class ProjectReferences : DotNetProjectTests
        {
            [Test]
            public void WhenNewFormat_AndNoProjectReferences_ThenReturnEmpty()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.ProjectReferences, Is.Empty);
            }

            [Test]
            public void WhenNewFormat_AndTwoProjectReferences_ThenReturnProjectReference()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.ProjectReferences.First().FilePath, Is.EqualTo(@"..\ByteDev.SomeProj1\ByteDev.SomeProj1.csproj"));
                Assert.That(sut.ProjectReferences.Second().FilePath, Is.EqualTo(@"..\ByteDev.SomeProj2\ByteDev.SomeProj2.csproj"));
            }

            [Test]
            public void WhenOldFormat_AndOneProjectReference_ThenReturnProjectReference()
            {
                var sut = CreateSut(TestProjFiles.OldFormat.Framework462);

                Assert.That(sut.ProjectReferences.First().FilePath, Is.EqualTo(@"..\ByteDev.AnotherProj\ByteDev.AnotherProj.csproj"));
            }
        }

        [TestFixture]
        public class PackageReferences : DotNetProjectTests
        {
            [Test]
            public void WhenNewFormat_AndNoPackageReferences_ThenReturnEmpty()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.PackageReferences, Is.Empty);
            }

            [Test]
            public void WhenNewFormat_AndNoAssertDetails_ThenReturnPackageReference()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.PackageReferences.First().Name, Is.EqualTo("Microsoft.NET.Test.Sdk"));
                Assert.That(sut.PackageReferences.First().Version, Is.EqualTo("15.8.0"));
                Assert.That(sut.PackageReferences.First().InclueAssets, Is.Empty);
                Assert.That(sut.PackageReferences.First().ExcludeAssets, Is.Empty);
                Assert.That(sut.PackageReferences.First().PrivateAssets, Is.Empty);
            }

            [Test]
            public void WhenNewFormat_AndReferenceHasFullDetails_ThenReturnPackageReferences()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.PackageReferences.Second().Name, Is.EqualTo("NUnit"));
                Assert.That(sut.PackageReferences.Second().Version, Is.EqualTo("3.10.1"));

                Assert.That(sut.PackageReferences.Second().InclueAssets.First(), Is.EqualTo("Compile"));
                Assert.That(sut.PackageReferences.Second().InclueAssets.Second(), Is.EqualTo("Runtime"));

                Assert.That(sut.PackageReferences.Second().ExcludeAssets.First(), Is.EqualTo("ContentFiles"));
                Assert.That(sut.PackageReferences.Second().ExcludeAssets.Second(), Is.EqualTo("Build"));

                Assert.That(sut.PackageReferences.Second().PrivateAssets.First(), Is.EqualTo("Native"));
                Assert.That(sut.PackageReferences.Second().PrivateAssets.Second(), Is.EqualTo("Analyzers"));
            }

            [Test]
            public void WhenNewFormat_AndPackageReferenceHasNoVersion_ThenReturnPackageReferences()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.PackageReferences.Third().Name, Is.EqualTo("Microsoft.AspNetCore.App"));
                Assert.That(sut.PackageReferences.Third().Version, Is.Null);
            }

            [Test]
            public void WhenOldFormat_ThenReturnEmpty()
            {
                var sut = CreateSut(TestProjFiles.OldFormat.Framework462);

                Assert.That(sut.PackageReferences, Is.Empty);
            }
        }

        [TestFixture]
        public class ExcludedItems : DotNetProjectTests
        {
            [Test]
            public void WhenOldFormat_ThenReturnEmpty()
            {
                var sut = CreateSut(TestProjFiles.OldFormat.Framework462);

                Assert.That(sut.ExcludedItems, Is.Empty);
            }

            [Test]
            public void WhenNewFormat_AndHasNoExcludedItems_ThenReturnEmpty()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.ExcludedItems, Is.Empty);
            }

            [Test]
            public void WhenNewFormat_AndHasExcludedItems_ThenReturnExcludedItems()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Items);

                Assert.That(sut.ExcludedItems.Count(), Is.EqualTo(6));

                Assert.That(sut.ExcludedItems.First().BuildAction, Is.EqualTo("None"));
                Assert.That(sut.ExcludedItems.First().Path, Is.EqualTo("mylogo.png"));

                Assert.That(sut.ExcludedItems.Last().BuildAction, Is.EqualTo("EmbeddedResource"));
                Assert.That(sut.ExcludedItems.Last().Path, Is.EqualTo(@"excluded_folder\**"));
            }
        }

        [TestFixture]
        public class IncludedItems : DotNetProjectTests
        {
            [Test]
            public void WhenOldFormat_ThenReturnEmpty()
            {
                var sut = CreateSut(TestProjFiles.OldFormat.Framework462);

                Assert.That(sut.IncludedItems, Is.Empty);
            }

            [Test]
            public void WhenNewFormat_AndHasNoExcludedItems_ThenReturnEmpty()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.IncludedItems, Is.Empty);
            }

            [Test]
            public void WhenNewFormat_AndHasIncludedItems_ThenReturnIncludedItems()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Items);

                Assert.That(sut.IncludedItems.Count(), Is.EqualTo(5));

                Assert.That(sut.IncludedItems.First().BuildAction, Is.EqualTo("None"));
                Assert.That(sut.IncludedItems.First().Path, Is.EqualTo(@"excluded_folder\image1.jpg"));

                Assert.That(sut.IncludedItems.Last().BuildAction, Is.EqualTo("EmbeddedResource"));
                Assert.That(sut.IncludedItems.Last().Path, Is.EqualTo(@"excluded_folder\image3.jpg"));
            }
        }
        
        [TestFixture]
        public class AssemblyInfo : DotNetProjectTests
        {
            [TestFixture]
            public class Company : AssemblyInfo
            {
                [Test]
                public void WhenCompanyIsPresent_ThenReturnValue()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.AssemblyInfo.Company, Is.EqualTo("Something Ltd."));
                }

                [Test]
                public void WhenCompanyIsNotPresent_ThenReturnNull()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.AssemblyInfo.Company, Is.Null);
                }
            }

            [TestFixture]
            public class Configuration : AssemblyInfo
            {
                [Test]
                public void WhenConfigurationIsPresent_ThenReturnValue()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.AssemblyInfo.Configuration, Is.EqualTo("debug"));
                }

                [Test]
                public void WhenConfigurationIsNotPresent_ThenReturnNull()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.AssemblyInfo.Configuration, Is.Null);
                }
            }

            [TestFixture]
            public class CopyrightAip : AssemblyInfo
            {
                [Test]
                public void WheCopyrightIsPresent_ThenReturnValue()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.AssemblyInfo.Copyright, Is.EqualTo("Some Copyright"));
                }

                [Test]
                public void WhenCopyrightIsNotPresent_ThenReturnNull()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.AssemblyInfo.Copyright, Is.Null);
                }
            }

            [TestFixture]
            public class DescriptionAip : AssemblyInfo
            {
                [Test]
                public void WhenDescriptionIsPresent_ThenReturnValue()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.AssemblyInfo.Description, Is.EqualTo("new-core21 Description"));
                }

                [Test]
                public void WhenDescriptionIsNotPresent_ThenReturnNull()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.AssemblyInfo.Description, Is.Null);
                }
            }

            [TestFixture]
            public class FileVersion : AssemblyInfo
            {
                [Test]
                public void WhenFileVersionIsPresent_ThenReturnValue()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.AssemblyInfo.FileVersion, Is.EqualTo("1.2.3"));
                }

                [Test]
                public void WhenFileVersionIsNotPresent_ThenReturnNull()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.AssemblyInfo.FileVersion, Is.Null);
                }
            }

            [TestFixture]
            public class InformationalVersion : AssemblyInfo
            {
                [Test]
                public void WhenInformationalVersionIsPresent_ThenReturnValue()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.AssemblyInfo.InformationalVersion, Is.EqualTo("beta2"));
                }

                [Test]
                public void WhenInformationalVersionIsNotPresent_ThenReturnNull()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.AssemblyInfo.InformationalVersion, Is.Null);
                }
            }

            [TestFixture]
            public class Product : AssemblyInfo
            {
                [Test]
                public void WhenProductIsPresent_ThenReturnValue()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.AssemblyInfo.Product, Is.EqualTo("Some Product"));
                }

                [Test]
                public void WhenProductIsNotPresent_ThenReturnNull()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.AssemblyInfo.Product, Is.Null);
                }
            }

            [TestFixture]
            public class TitleAip : AssemblyInfo
            {
                [Test]
                public void WhenTitleIsPresent_ThenReturnValue()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.AssemblyInfo.Title, Is.EqualTo("Some Title"));
                }

                [Test]
                public void WhenTitleIsNotPresent_ThenReturnNull()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.AssemblyInfo.Title, Is.Null);
                }
            }

            [TestFixture]
            public class Version : AssemblyInfo
            {
                [Test]
                public void WhenVersionIsPresent_ThenReturnValue()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.AssemblyInfo.Version, Is.EqualTo("3.2.1"));
                }

                [Test]
                public void WhenVersionIsNotPresent_ThenReturnNull()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.AssemblyInfo.Version, Is.Null);
                }
            }

            [TestFixture]
            public class NeutralLanguage : AssemblyInfo
            {
                [Test]
                public void WhenNeutralLanguageIsPresent_ThenReturnValue()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.AssemblyInfo.NeutralLanguage, Is.EqualTo("en"));
                }

                [Test]
                public void WhenNeutralLanguageIsNotPresent_ThenReturnNull()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.AssemblyInfo.NeutralLanguage, Is.Null);
                }
            }
        }

        [TestFixture]
        public class NugetMetaData : DotNetProjectTests
        {
            [TestFixture]
            public class IsPackable : DotNetProjectTests
            {
                [Test]
                public void WhenIsPackableIsPresent_ThenReturnIsPackable()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.NugetMetaData.IsPackable, Is.False);
                }

                [Test]
                public void WhenIsPackableIsNotPresent_ThenReturnTrue()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.NugetMetaData.IsPackable, Is.True);
                }
            }

            [TestFixture]
            public class PackageVersion : DotNetProjectTests
            {
                [Test]
                public void WhenPackageVersionIsPresent_ThenReturnPackageVersion()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.NugetMetaData.PackageVersion, Is.EqualTo("1.2.3"));
                }

                [Test]
                public void WhenPackageVersionIsNotPresent_ThenReturnNull()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.NugetMetaData.PackageVersion, Is.Null);
                }
            }

            [TestFixture]
            public class PackageId : DotNetProjectTests
            {
                [Test]
                public void WhenPackageIdIsPresent_ThenReturnPackageId()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.NugetMetaData.PackageId, Is.EqualTo("MyPackageName"));
                }

                [Test]
                public void WhenPackageIdIsNotPresent_ThenReturnNull()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.NugetMetaData.PackageId, Is.Null);
                }
            }

            [TestFixture]
            public class Title : DotNetProjectTests
            {
                [Test]
                public void WhenTitleIsPresent_ThenReturnTitle()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.NugetMetaData.Title, Is.EqualTo("Some Title"));
                }

                [Test]
                public void WhenTitleIsNotPresent_ThenReturnNull()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.NugetMetaData.Title, Is.Null);
                }
            }

            [TestFixture]
            public class PackageLicenseFile : DotNetProjectTests
            {
                [Test]
                public void WhenPackageLicenseFileIsPresent_ThenReturnValue()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.NugetMetaData.PackageLicenseFile, Is.EqualTo(@"license\mylicense.txt"));
                }

                [Test]
                public void WhenPackageLicenseFileIsNotPrsent_ThenReturnNull()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.NugetMetaData.PackageLicenseFile, Is.Null);
                }
            }

            [TestFixture]
            public class PackageDescription : DotNetProjectTests
            {
                [Test]
                public void WhenPackageDescriptionIsPresent_ThenReturnPackageDescription()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.NugetMetaData.PackageDescription, Is.EqualTo("Some description"));
                }

                [Test]
                public void WhenPackageDescriptionIsNotPrsent_ThenReturnNull()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.NugetMetaData.PackageDescription, Is.Null);
                }
            }

            [TestFixture]
            public class PackageRequireLicenseAcceptance : DotNetProjectTests
            {
                [Test]
                public void WhenPackageRequireLicenseAcceptanceIsPresent_ThenReturnPackageRequireLicenseAcceptance()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.NugetMetaData.PackageRequireLicenseAcceptance, Is.True);
                }

                [Test]
                public void WhenPackageRequireLicenseAcceptanceIsNotPresent_ThenReturnFalse()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.NugetMetaData.PackageRequireLicenseAcceptance, Is.False);
                }
            }

            [TestFixture]
            public class PackageLicenseExpression : DotNetProjectTests
            {
                [Test]
                public void WhenPackageLicenseExpressionIsPresent_ThenReturnPackageLicenseExpression()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.NugetMetaData.PackageLicenseExpression, Is.EqualTo("Apache-2.0"));
                }

                [Test]
                public void WhenPackageLicenseExpressionIsNotPresent_ThenReturnNull()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.NugetMetaData.PackageLicenseExpression, Is.Null);
                }
            }

            [TestFixture]
            public class Description : DotNetProjectTests
            {
                [Test]
                public void WhenDescriptionIsPresent_ThenReturnDescription()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.NugetMetaData.Description, Is.EqualTo("new-core21 Description"));
                }

                [Test]
                public void WhenDescriptionIsNotPresent_ThenReturnNull()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.NugetMetaData.Description, Is.Null);
                }
            }

            [TestFixture]
            public class Authors : DotNetProjectTests
            {
                [Test]
                public void WhenTwoAuthorsIsPresent_ThenReturnTwoAuthors()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.NugetMetaData.Authors.Count(), Is.EqualTo(2));
                    Assert.That(sut.NugetMetaData.Authors.First(), Is.EqualTo("John Smith"));
                    Assert.That(sut.NugetMetaData.Authors.Second(), Is.EqualTo("Jesus Christ"));
                }

                [Test]
                public void WhenAuthorsIsNotPresent_ThenReturnEmpty()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.NugetMetaData.Authors, Is.Empty);
                }
            }

            [TestFixture]
            public class PackageOutputPath : DotNetProjectTests
            {
                [Test]
                public void WhenPackageOutputPathIsPresent_ThenReturnPackageOutputPath()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.NugetMetaData.PackageOutputPath, Is.EqualTo(@"C:\temp"));
                }

                [Test]
                public void WhenPackageOutputPathIsNotPresent_ThenReturnNull()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.NugetMetaData.PackageOutputPath, Is.Null);
                }
            }

            [TestFixture]
            public class IncludeSymbols : DotNetProjectTests
            {
                [Test]
                public void WhenIncludeSymbolsIsPresent_ThenReturnValue()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.NugetMetaData.IncludeSymbols, Is.True);
                }

                [Test]
                public void WhenIncludeSymbolsIsNotPresent_ThenReturnFalse()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.NugetMetaData.IncludeSymbols, Is.False);
                }
            }

            [TestFixture]
            public class IncludeSource : DotNetProjectTests
            {
                [Test]
                public void WhenIncludeSourceIsPresent_ThenReturnValue()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.NugetMetaData.IncludeSource, Is.True);
                }

                [Test]
                public void WhenIncludeSourceIsNotPresent_ThenReturnFalse()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.NugetMetaData.IncludeSource, Is.False);
                }
            }

            [TestFixture]
            public class IsTool : DotNetProjectTests
            {
                [Test]
                public void WhenIsToolIsPresent_ThenReturnValue()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.NugetMetaData.IsTool, Is.True);
                }

                [Test]
                public void WhenIsToolIsNotPresent_ThenReturnFalse()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.NugetMetaData.IsTool, Is.False);
                }
            }

            [TestFixture]
            public class NoPackageAnalysis : DotNetProjectTests
            {
                [Test]
                public void WhenNoPackageAnalysisIsPresent_ThenReturnValue()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.NugetMetaData.NoPackageAnalysis, Is.True);
                }

                [Test]
                public void WhenNoPackageAnalysisIsNotPresent_ThenReturnFalse()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.NugetMetaData.NoPackageAnalysis, Is.False);
                }
            }

            [TestFixture]
            public class IncludeBuildOutput : DotNetProjectTests
            {
                [Test]
                public void WhenIncludeBuildOutputIsPresent_ThenReturnValue()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.NugetMetaData.IncludeBuildOutput, Is.True);
                }

                [Test]
                public void WhenIncludeBuildOutputIsNotPresent_ThenReturnFalse()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.NugetMetaData.IncludeBuildOutput, Is.False);
                }
            }

            [TestFixture]
            public class IncludeContentInPack : DotNetProjectTests
            {
                [Test]
                public void WhenIncludeContentInPackIsPresent_ThenReturnValue()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.NugetMetaData.IncludeContentInPack, Is.False);
                }

                [Test]
                public void WhenIncludeContentInPackIsNotPresent_ThenReturnTrue()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.NugetMetaData.IncludeContentInPack, Is.True);
                }
            }

            [TestFixture]
            public class SymbolPackageFormat : DotNetProjectTests
            {
                [Test]
                public void WhenSymbolPackageFormatIsPresent_ThenReturnValue()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.NugetMetaData.SymbolPackageFormat, Is.EqualTo("snupkg"));
                }

                [Test]
                public void WhenSymbolPackageFormatIsNotPresent_ThenReturnNull()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.NugetMetaData.SymbolPackageFormat, Is.Null);
                }
            }

            [TestFixture]
            public class BuildOutputTargetFolder : DotNetProjectTests
            {
                [Test]
                public void WhenBuildOutputTargetFolderIsPresent_ThenReturnValue()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.NugetMetaData.BuildOutputTargetFolder, Is.EqualTo(@"C:\output"));
                }

                [Test]
                public void WhenBuildOutputTargetFolderIsNotPresent_ThenReturnNull()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.NugetMetaData.BuildOutputTargetFolder, Is.Null);
                }
            }

            [TestFixture]
            public class MinClientVersion : DotNetProjectTests
            {
                [Test]
                public void WhenMinClientVersionIsPresent_ThenReturnValue()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.NugetMetaData.MinClientVersion, Is.EqualTo("3.3"));
                }

                [Test]
                public void WhenMinClientVersionIsNotPresent_ThenReturnNull()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.NugetMetaData.MinClientVersion, Is.Null);
                }
            }

            [TestFixture]
            public class NuspecFile : DotNetProjectTests
            {
                [Test]
                public void WhenNuspecFileIsPresent_ThenReturnValue()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.NugetMetaData.NuspecFile, Is.EqualTo(@"C:\temp\myapp.nuspec"));
                }

                [Test]
                public void WhenNuspecFileIsNotPresent_ThenReturnNull()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.NugetMetaData.NuspecFile, Is.Null);
                }
            }

            [TestFixture]
            public class NuspecBasePath : DotNetProjectTests
            {
                [Test]
                public void WhenNuspecBasePathIsPresent_ThenReturnValue()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.NugetMetaData.NuspecBasePath, Is.EqualTo(@"C:\temp"));
                }

                [Test]
                public void WhenNuspecBasePathIsNotPresent_ThenReturnNull()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.NugetMetaData.NuspecBasePath, Is.Null);
                }
            }

            [TestFixture]
            public class NuspecProperties : DotNetProjectTests
            {
                [Test]
                public void WhenNuspecPropertiesIsPresent_ThenReturnValue()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.NugetMetaData.NuspecProperties.Keys.Count, Is.EqualTo(3));
                    Assert.That(sut.NugetMetaData.NuspecProperties["name1"], Is.EqualTo("value1"));
                    Assert.That(sut.NugetMetaData.NuspecProperties["name2"], Is.EqualTo("value2"));
                    Assert.That(sut.NugetMetaData.NuspecProperties["name3"], Is.EqualTo("value3"));
                }

                [Test]
                public void WhenNuspecPropertiesIsNotPresent_ThenReturnNull()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.NugetMetaData.NuspecProperties, Is.Empty);
                }
            }

            [TestFixture]
            public class ContentTargetFolders : DotNetProjectTests
            {
                [Test]
                public void WhenContentTargetFoldersIsPresent_ThenReturnValue()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.NugetMetaData.ContentTargetFolders.Count(), Is.EqualTo(3));
                    Assert.That(sut.NugetMetaData.ContentTargetFolders.First(), Is.EqualTo("content2"));
                    Assert.That(sut.NugetMetaData.ContentTargetFolders.Second(), Is.EqualTo("contentFiles2"));
                    Assert.That(sut.NugetMetaData.ContentTargetFolders.Third(), Is.EqualTo("someWhereElse"));
                }

                [Test]
                public void WhenContentTargetFoldersIsNotPresent_ThenReturnDefault()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.NugetMetaData.ContentTargetFolders.Count(), Is.EqualTo(2));
                    Assert.That(sut.NugetMetaData.ContentTargetFolders.First(), Is.EqualTo("content"));
                    Assert.That(sut.NugetMetaData.ContentTargetFolders.Second(), Is.EqualTo("contentFiles"));
                }
            }

            [TestFixture]
            public class PackageLicenseUrl : DotNetProjectTests
            {
                [Test]
                public void WhenPackageLicenseUrlIsPresent_ThenReturnPackageLicenseUrl()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.NugetMetaData.PackageLicenseUrl, Is.EqualTo("http://licenseurl/"));
                }

                [Test]
                public void WhenPackageLicenseUrlIsNotPresent_ThenReturnNull()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.NugetMetaData.PackageLicenseUrl, Is.Null);
                }
            }

            [TestFixture]
            public class PackageIconUrl : DotNetProjectTests
            {
                [Test]
                public void WhenPackageIconUrlIsPresent_ThenReturnPackageIconUrl()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.NugetMetaData.PackageIconUrl, Is.EqualTo("http://iconurl/"));
                }

                [Test]
                public void WhenPackageIconUrlIsNotPresent_ThenReturnNull()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.NugetMetaData.PackageIconUrl, Is.Null);
                }
            }

            [TestFixture]
            public class RepositoryUrl : DotNetProjectTests
            {
                [Test]
                public void WhenRepositoryUrlIsPresent_ThenReturnRepositoryUrl()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.NugetMetaData.RepositoryUrl, Is.EqualTo("http://repourl/"));
                }

                [Test]
                public void WhenRepositoryUrlIsNotPresent_ThenReturnNull()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.NugetMetaData.RepositoryUrl, Is.Null);
                }
            }

            [TestFixture]
            public class RepositoryType : DotNetProjectTests
            {
                [Test]
                public void WhenRepositoryTypeIsPresent_ThenReturnRepositoryType()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.NugetMetaData.RepositoryType, Is.EqualTo("git"));
                }

                [Test]
                public void WhenRepositoryTypeIsNotPresent_ThenReturnNull()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.NugetMetaData.RepositoryType, Is.Null);
                }
            }

            [TestFixture]
            public class PackageReleaseNotes : DotNetProjectTests
            {
                [Test]
                public void WhenPackageReleaseNotesIsPresent_ThenReturnPackageReleaseNotes()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.NugetMetaData.PackageReleaseNotes, Is.EqualTo("Some release notes"));
                }

                [Test]
                public void WhenPackageReleaseNotesIsNotPresent_ThenReturnNull()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.NugetMetaData.PackageReleaseNotes, Is.Null);
                }
            }

            [TestFixture]
            public class Copyright : DotNetProjectTests
            {
                [Test]
                public void WhenCopyrightIsPresent_ThenReturnCopyright()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.NugetMetaData.Copyright, Is.EqualTo("Some Copyright"));
                }

                [Test]
                public void WhenCopyrightIsNotPresent_ThenReturnNull()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.NugetMetaData.Copyright, Is.Null);
                }
            }

            [TestFixture]
            public class PackageTags : DotNetProjectTests
            {
                [Test]
                public void WhenPackageTagsIsNotPresent_ThenReturnEmpty()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                    Assert.That(sut.NugetMetaData.PackageTags, Is.Empty);
                }

                [Test]
                public void WhenPackageTagsAreDelimitedWithSpaces_ThenReturnAllPackageTags()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                    Assert.That(sut.NugetMetaData.PackageTags.First(), Is.EqualTo("something"));
                    Assert.That(sut.NugetMetaData.PackageTags.Second(), Is.EqualTo("program"));
                    Assert.That(sut.NugetMetaData.PackageTags.Third(), Is.EqualTo("exe"));
                }

                [Test]
                public void WhenPackageTagsAreDelimitedWithCommas_ThenReturnAllPackageTags()
                {
                    var sut = CreateSut(TestProjFiles.NewFormat.Core21CommaTags);

                    Assert.That(sut.NugetMetaData.PackageTags.First(), Is.EqualTo("tag1"));
                    Assert.That(sut.NugetMetaData.PackageTags.Second(), Is.EqualTo("tag2"));
                    Assert.That(sut.NugetMetaData.PackageTags.Third(), Is.EqualTo("tag3"));
                }
            }
        }
    }
}