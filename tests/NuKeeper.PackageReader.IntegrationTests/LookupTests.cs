using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NuKeeper.PackageReader.Helpers;
using NUnit.Framework;

namespace NuKeeper.PackageReader.IntegrationTests
{
    public class LookupTests
    {
        [Test]
        public async Task TestEmptyCase()
        {
            var packages = Enumerable.Empty<string>();
            var versionsMap = await LookupVersions(packages);

            Assert.That(versionsMap, Is.Not.Null);
            Assert.That(versionsMap, Is.Empty);
        }

        [Test]
        public async Task TestHappyCase()
        {
            var packages = new string[] { "Newtonsoft.Json" };
            var versionsMap = await LookupVersions(packages);

            Assert.That(versionsMap, Is.Not.Null);
            var versions = versionsMap["Newtonsoft.Json"];

            PackageSearchMetadataAssert.AssertPopulated(versions);
        }

        [Test]
        public async Task TestFail()
        {
            string badPackageName = BadPackageName();
            var packages = new string[] { badPackageName };
            var versionsMap = await LookupVersions(packages);

            Assert.That(versionsMap, Is.Not.Null);
            Assert.That(versionsMap, Is.Not.Empty);
            Assert.That(versionsMap[badPackageName], Is.Empty);
        }

        [Test]
        public async Task TestSuccessAndFailure()
        {
            string badPackageName = BadPackageName();
            var packages = new string[] { "Newtonsoft.Json", badPackageName };
            var versionsMap = await LookupVersions(packages);

            Assert.That(versionsMap, Is.Not.Null);
            Assert.That(versionsMap, Is.Not.Empty);
            PackageSearchMetadataAssert.AssertPopulated(versionsMap["Newtonsoft.Json"]);


            Assert.That(versionsMap[badPackageName], Is.Not.Null);
            Assert.That(versionsMap[badPackageName], Is.Empty);
        }

        [Test]
        public async Task TestEmptyCaseWithDependencies()
        {
            var packages = Enumerable.Empty<string>();
            var versionsMap = await LookupVersionsWithDependencies(packages);

            Assert.That(versionsMap, Is.Not.Null);
            Assert.That(versionsMap, Is.Empty);
        }

        [Test]
        public async Task TestPackageGetWithDependencies()
        {
            var packages = new string[] { "Microsoft.Extensions.Logging" };
            var versionsMap = await LookupVersionsWithDependencies(packages);

            Assert.That(versionsMap, Is.Not.Null);
            var versions = versionsMap["Microsoft.Extensions.Logging"];

            PackageSearchMetadataAssert.AssertPopulated(versions);

            var currentVersion = versions.First();

            PackageSearchMetadataAssert.AssertPopulated(currentVersion);
            Assert.That(currentVersion.Dependencies, Is.Not.Null);
            Assert.That(currentVersion.Dependencies, Is.Not.Empty);
            foreach(var dependantPackage in currentVersion.Dependencies)
            {
                Assert.That(versionsMap.ContainsKey(dependantPackage.Id), Is.True,
                    $"No data for dependant package '{dependantPackage.Id}'");
            }
        }

        [Test]
        public async Task TestTwoPackagesGetWithDependencies()
        {
            var packages = new string[] { "Microsoft.Extensions.Logging", "AutoMapper" };
            var versionsMap = await LookupVersionsWithDependencies(packages);

            Assert.That(versionsMap, Is.Not.Null);
            var loggingVersions = versionsMap["Microsoft.Extensions.Logging"];
            var mapperVersions = versionsMap["AutoMapper"];

            PackageSearchMetadataAssert.AssertPopulated(loggingVersions);
            PackageSearchMetadataAssert.AssertPopulated(mapperVersions);

            foreach (var dependantPackage in loggingVersions.First().Dependencies)
            {
                Assert.That(versionsMap.ContainsKey(dependantPackage.Id), Is.True,
                    $"No data for dependant package '{dependantPackage.Id}'");
            }

            foreach (var dependantPackage in mapperVersions.First().Dependencies)
            {
                Assert.That(versionsMap.ContainsKey(dependantPackage.Id), Is.True,
                    $"No data for dependant package '{dependantPackage.Id}'");
            }
        }

        private static string BadPackageName()
        {
            return "NoSuchPackage_" + Guid.NewGuid().ToString();
        }

        private static async Task<IDictionary<string, IReadOnlyCollection<PackageSearchMetadata>>> LookupVersions(
            IEnumerable<string> packageNames)
        {
            var logger = new NullNuKeeperLogger();
            var nugetLogger = new NuGetLogger(logger);

            var lookup = new BulkPackageLookup(new PackageVersionsLookup(nugetLogger, logger));
            return await lookup.FindVersionUpdates(packageNames, NuGetSources.GlobalFeed);
        }
        private static async Task<IDictionary<string, IReadOnlyCollection<PackageSearchMetadata>>> LookupVersionsWithDependencies(
            IEnumerable<string> packageNames)
        {
            var lookup = new Lookup(NuGetSources.GlobalFeed);
            return await lookup.LookupPackageVersions(packageNames);
        }
    }
}
