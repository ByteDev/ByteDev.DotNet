using System.Linq;
using ByteDev.Common.Collections;
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

                Assert.That(sut.ProjectTargets.Single().TargetValue, Is.EqualTo("netstandard2.0"));
            }

            [Test]
            public void WhenProjXmlIsCore_ThenReturnTarget()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.ProjectTargets.Single().TargetValue, Is.EqualTo("netcoreapp2.1"));
            }

            [Test]
            public void WhenProjXmIsNewFormatFramework_ThenReturnTarget()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Framework471);

                Assert.That(sut.ProjectTargets.Single().TargetValue, Is.EqualTo("net471"));
            }

            [Test]
            public void WhenProjXmlIsOldFormatFramework_ThenReturnTarget()
            {
                var sut = CreateSut(TestProjFiles.OldFormat.Framework462);

                Assert.That(sut.ProjectTargets.Single().TargetValue, Is.EqualTo("v4.6.2"));
            }

            [Test]
            public void WhenIsMultiTarget_ThenReturnsAllTargets()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Std15AndFramework4);

                Assert.That(sut.IsMultiTarget, Is.True);

                Assert.That(sut.ProjectTargets.First().TargetValue, Is.EqualTo("netstandard1.5"));
                Assert.That(sut.ProjectTargets.Second().TargetValue, Is.EqualTo("net40"));
            }

            [Test]
            public void WhenTargetFrameworkIsNotFirstElement_ThenReturnTarget()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.ProjectTargets.Single().TargetValue, Is.EqualTo("netcoreapp2.1"));
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
        public class IsPackable : DotNetProjectTests
        {
            [Test]
            public void WhenIsPackableIsPresent_ThenReturnIsPackable()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.IsPackable, Is.False);
            }

            [Test]
            public void WhenIsPackableIsNotPresent_ThenReturnTrue()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.IsPackable, Is.True);
            }
        }

        [TestFixture]
        public class PackageVersion : DotNetProjectTests
        {
            [Test]
            public void WhenPackageVersionIsPresent_ThenReturnPackageVersion()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.PackageVersion, Is.EqualTo("1.2.3"));
            }

            [Test]
            public void WhenPackageVersionIsNotPresent_ThenReturnNull()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.PackageVersion, Is.Null);
            }
        }

        [TestFixture]
        public class PackageId : DotNetProjectTests
        {
            [Test]
            public void WhenPackageIdIsPresent_ThenReturnPackageId()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.PackageId, Is.EqualTo("MyPackageName"));
            }

            [Test]
            public void WhenPackageIdIsNotPresent_ThenReturnNull()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.PackageId, Is.Null);
            }
        }

        [TestFixture]
        public class Title : DotNetProjectTests
        {
            [Test]
            public void WhenTitleIsPresent_ThenReturnTitle()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.Title, Is.EqualTo("Some Title"));
            }

            [Test]
            public void WhenTitleIsNotPresent_ThenReturnNull()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.Title, Is.Null);
            }
        }

        [TestFixture]
        public class PackageLicenseFile : DotNetProjectTests
        {
            [Test]
            public void WhenPackageDescriptionIsPresent_ThenReturnValue()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.PackageLicenseFile, Is.EqualTo(@"license\mylicense.txt"));
            }

            [Test]
            public void WhenPackageDescriptionIsNotPrsent_ThenReturnNull()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.PackageLicenseFile, Is.Null);
            }
        }

        [TestFixture]
        public class PackageDescription : DotNetProjectTests
        {
            [Test]
            public void WhenPackageDescriptionIsPresent_ThenReturnPackageDescription()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.PackageDescription, Is.EqualTo("Some description"));
            }

            [Test]
            public void WhenPackageDescriptionIsNotPrsent_ThenReturnNull()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.PackageDescription, Is.Null);
            }
        }

        [TestFixture]
        public class PackageRequireLicenseAcceptance : DotNetProjectTests
        {
            [Test]
            public void WhenPackageRequireLicenseAcceptanceIsPresent_ThenReturnPackageRequireLicenseAcceptance()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.PackageRequireLicenseAcceptance, Is.True);
            }

            [Test]
            public void WhenPackageRequireLicenseAcceptanceIsNotPresent_ThenReturnFalse()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.PackageRequireLicenseAcceptance, Is.False);
            }
        }

        [TestFixture]
        public class PackageLicenseExpression : DotNetProjectTests
        {
            [Test]
            public void WhenPackageLicenseExpressionIsPresent_ThenReturnPackageLicenseExpression()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.PackageLicenseExpression, Is.EqualTo("Apache-2.0"));
            }

            [Test]
            public void WhenPackageLicenseExpressionIsNotPresent_ThenReturnNull()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.PackageLicenseExpression, Is.Null);
            }
        }

        [TestFixture]
        public class Description : DotNetProjectTests
        {
            [Test]
            public void WhenDescriptionIsPresent_ThenReturnDescription()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.Description, Is.EqualTo("new-core21 Description"));
            }

            [Test]
            public void WhenDescriptionIsNotPresent_ThenReturnNull()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.Description, Is.Null);
            }
        }

        [TestFixture]
        public class Authors : DotNetProjectTests
        {
            [Test]
            public void WhenTwoAuthorsIsPresent_ThenReturnTwoAuthors()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.Authors.Count(), Is.EqualTo(2));
                Assert.That(sut.Authors.First(), Is.EqualTo("John Smith"));
                Assert.That(sut.Authors.Second(), Is.EqualTo("Jesus Christ"));
            }

            [Test]
            public void WhenAuthorsIsNotPresent_ThenReturnEmpty()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.Authors, Is.Empty);
            }
        }

        [TestFixture]
        public class PackageOutputPath : DotNetProjectTests
        {
            [Test]
            public void WhenPackageOutputPathIsPresent_ThenReturnPackageOutputPath()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.PackageOutputPath, Is.EqualTo(@"C:\temp"));
            }

            [Test]
            public void WhenPackageOutputPathIsNotPresent_ThenReturnNull()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.PackageOutputPath, Is.Null);
            }
        }

        [TestFixture]
        public class IncludeSymbols : DotNetProjectTests
        {
            [Test]
            public void WhenIncludeSymbolsIsPresent_ThenReturnValue()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.IncludeSymbols, Is.True);
            }

            [Test]
            public void WhenIncludeSymbolsIsNotPresent_ThenReturnFalse()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.IncludeSymbols, Is.False);
            }
        }

        [TestFixture]
        public class IncludeSource : DotNetProjectTests
        {
            [Test]
            public void WhenIncludeSourceIsPresent_ThenReturnValue()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.IncludeSource, Is.True);
            }

            [Test]
            public void WhenIncludeSourceIsNotPresent_ThenReturnFalse()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.IncludeSource, Is.False);
            }
        }

        [TestFixture]
        public class IsTool : DotNetProjectTests
        {
            [Test]
            public void WhenIsToolIsPresent_ThenReturnValue()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.IsTool, Is.True);
            }
            
            [Test]
            public void WhenIsToolIsNotPresent_ThenReturnFalse()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.IsTool, Is.False);
            }
        }

        [TestFixture]
        public class NoPackageAnalysis : DotNetProjectTests
        {
            [Test]
            public void WhenNoPackageAnalysisIsPresent_ThenReturnValue()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.NoPackageAnalysis, Is.True);
            }

            [Test]
            public void WhenNoPackageAnalysisIsNotPresent_ThenReturnFalse()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.NoPackageAnalysis, Is.False);
            }
        }

        [TestFixture]
        public class IncludeBuildOutput : DotNetProjectTests
        {
            [Test]
            public void WhenIncludeBuildOutputIsPresent_ThenReturnValue()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.IncludeBuildOutput, Is.True);
            }

            [Test]
            public void WhenIncludeBuildOutputIsNotPresent_ThenReturnFalse()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.IncludeBuildOutput, Is.False);
            }
        }

        [TestFixture]
        public class IncludeContentInPack : DotNetProjectTests
        {
            [Test]
            public void WhenIncludeContentInPackIsPresent_ThenReturnValue()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.IncludeContentInPack, Is.False);
            }

            [Test]
            public void WhenIncludeContentInPackIsNotPresent_ThenReturnTrue()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.IncludeContentInPack, Is.True);
            }
        }

        [TestFixture]
        public class SymbolPackageFormat : DotNetProjectTests
        {
            [Test]
            public void WhenSymbolPackageFormatIsPresent_ThenReturnValue()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.SymbolPackageFormat, Is.EqualTo("snupkg"));
            }
            
            [Test]
            public void WhenSymbolPackageFormatIsNotPresent_ThenReturnNull()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.SymbolPackageFormat, Is.Null);
            }
        }

        [TestFixture]
        public class BuildOutputTargetFolder : DotNetProjectTests
        {
            [Test]
            public void WhenBuildOutputTargetFolderIsPresent_ThenReturnValue()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.BuildOutputTargetFolder, Is.EqualTo(@"C:\output"));
            }

            [Test]
            public void WhenBuildOutputTargetFolderIsNotPresent_ThenReturnNull()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.BuildOutputTargetFolder, Is.Null);
            }
        }

        [TestFixture]
        public class MinClientVersion : DotNetProjectTests
        {
            [Test]
            public void WhenMinClientVersionIsPresent_ThenReturnValue()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.MinClientVersion, Is.EqualTo("3.3"));
            }

            [Test]
            public void WhenMinClientVersionIsNotPresent_ThenReturnNull()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.MinClientVersion, Is.Null);
            }
        }

        [TestFixture]
        public class NuspecFile : DotNetProjectTests
        {
            [Test]
            public void WhenNuspecFileIsPresent_ThenReturnValue()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.NuspecFile, Is.EqualTo(@"C:\temp\myapp.nuspec"));
            }

            [Test]
            public void WhenNuspecFileIsNotPresent_ThenReturnNull()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.NuspecFile, Is.Null);
            }
        }

        [TestFixture]
        public class NuspecBasePath : DotNetProjectTests
        {
            [Test]
            public void WhenNuspecBasePathIsPresent_ThenReturnValue()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.NuspecBasePath, Is.EqualTo(@"C:\temp"));
            }

            [Test]
            public void WhenNuspecBasePathIsNotPresent_ThenReturnNull()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.NuspecBasePath, Is.Null);
            }
        }

        [TestFixture]
        public class NuspecProperties : DotNetProjectTests
        {
            [Test]
            public void WhenNuspecPropertiesIsPresent_ThenReturnValue()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.NuspecProperties.Keys.Count, Is.EqualTo(3));
                Assert.That(sut.NuspecProperties["name1"], Is.EqualTo("value1"));
                Assert.That(sut.NuspecProperties["name2"], Is.EqualTo("value2"));
                Assert.That(sut.NuspecProperties["name3"], Is.EqualTo("value3"));
            }

            [Test]
            public void WhenNuspecPropertiesIsNotPresent_ThenReturnNull()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.NuspecProperties, Is.Empty);
            }
        }

        [TestFixture]
        public class ContentTargetFolders : DotNetProjectTests
        {
            [Test]
            public void WhenContentTargetFoldersIsPresent_ThenReturnValue()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.ContentTargetFolders.Count(), Is.EqualTo(3));
                Assert.That(sut.ContentTargetFolders.First(), Is.EqualTo("content2"));
                Assert.That(sut.ContentTargetFolders.Second(), Is.EqualTo("contentFiles2"));
                Assert.That(sut.ContentTargetFolders.Third(), Is.EqualTo("someWhereElse"));
            }

            [Test]
            public void WhenContentTargetFoldersIsNotPresent_ThenReturnDefault()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.ContentTargetFolders.Count(), Is.EqualTo(2));
                Assert.That(sut.ContentTargetFolders.First(), Is.EqualTo("content"));
                Assert.That(sut.ContentTargetFolders.Second(), Is.EqualTo("contentFiles"));
            }
        }

        [TestFixture]
        public class Company : DotNetProjectTests
        {
            [Test]
            public void WhenAuthorsIsPresent_ThenReturnDescription()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.Company, Is.EqualTo("Something Ltd."));
            }

            [Test]
            public void WhenAuthorsIsNotPresent_ThenReturnNull()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.Company, Is.Null);
            }
        }

        [TestFixture]
        public class PackageLicenseUrl : DotNetProjectTests
        {
            [Test]
            public void WhenPackageLicenseUrlIsPresent_ThenReturnPackageLicenseUrl()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.PackageLicenseUrl, Is.EqualTo("http://licenseurl/"));
            }

            [Test]
            public void WhenPackageLicenseUrlIsNotPresent_ThenReturnNull()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.PackageLicenseUrl, Is.Null);
            }
        }

        [TestFixture]
        public class PackageProjectUrl : DotNetProjectTests
        {
            [Test]
            public void WhenPackageProjectUrlIsPresent_ThenReturnPackageProjectUrl()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.PackageProjectUrl, Is.EqualTo("http://projecturl/"));
            }

            [Test]
            public void WhenPackageProjectUrlIsNotPresent_ThenReturnNull()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.PackageProjectUrl, Is.Null);
            }
        }

        [TestFixture]
        public class PackageIconUrl : DotNetProjectTests
        {
            [Test]
            public void WhenPackageIconUrlIsPresent_ThenReturnPackageIconUrl()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.PackageIconUrl, Is.EqualTo("http://iconurl/"));
            }

            [Test]
            public void WhenPackageIconUrlIsNotPresent_ThenReturnNull()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.PackageIconUrl, Is.Null);
            }
        }

        [TestFixture]
        public class RepositoryUrl : DotNetProjectTests
        {
            [Test]
            public void WhenRepositoryUrlIsPresent_ThenReturnRepositoryUrl()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.RepositoryUrl, Is.EqualTo("http://repourl/"));
            }

            [Test]
            public void WhenRepositoryUrlIsNotPresent_ThenReturnNull()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.RepositoryUrl, Is.Null);
            }
        }

        [TestFixture]
        public class RepositoryType : DotNetProjectTests
        {
            [Test]
            public void WhenRepositoryTypeIsPresent_ThenReturnRepositoryType()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.RepositoryType, Is.EqualTo("git"));
            }

            [Test]
            public void WhenRepositoryTypeIsNotPresent_ThenReturnNull()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.RepositoryType, Is.Null);
            }
        }

        [TestFixture]
        public class PackageReleaseNotes : DotNetProjectTests
        {
            [Test]
            public void WhenPackageReleaseNotesIsPresent_ThenReturnPackageReleaseNotes()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.PackageReleaseNotes, Is.EqualTo("Some release notes"));
            }

            [Test]
            public void WhenPackageReleaseNotesIsNotPresent_ThenReturnNull()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.PackageReleaseNotes, Is.Null);
            }
        }

        [TestFixture]
        public class Copyright : DotNetProjectTests
        {
            [Test]
            public void WhenCopyrightIsPresent_ThenReturnCopyright()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);

                Assert.That(sut.Copyright, Is.EqualTo("Some Copyright"));
            }

            [Test]
            public void WhenCopyrightIsNotPresent_ThenReturnNull()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.Copyright, Is.Null);
            }
        }

        [TestFixture]
        public class PackageTags : DotNetProjectTests
        {
            [Test]
            public void WhenPackageTagsIsNotPresent_ThenReturnEmpty()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21Exe);

                Assert.That(sut.PackageTags, Is.Empty);
            }

            [Test]
            public void WhenPackageTagsAreDelimitedWithSpaces_ThenReturnAllPackageTags()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21);
                
                Assert.That(sut.PackageTags.First(), Is.EqualTo("something"));
                Assert.That(sut.PackageTags.Second(), Is.EqualTo("program"));
                Assert.That(sut.PackageTags.Third(), Is.EqualTo("exe"));
            }

            [Test]
            public void WhenPackageTagsAreDelimitedWithCommas_ThenReturnAllPackageTags()
            {
                var sut = CreateSut(TestProjFiles.NewFormat.Core21CommaTags);

                Assert.That(sut.PackageTags.First(), Is.EqualTo("tag1"));
                Assert.That(sut.PackageTags.Second(), Is.EqualTo("tag2"));
                Assert.That(sut.PackageTags.Third(), Is.EqualTo("tag3"));
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
    }
}