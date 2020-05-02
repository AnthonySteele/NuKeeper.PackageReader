using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NuKeeper.PackageReader.IntegrationTests
{
    public class LookupTests
    {
        [Test]
        public async Task TestHappyCase()
        {
            var packages = new string[] { "Newtonsoft.Json" };
            var versionsMap = await Lookup.LookupVersions(packages, NuGetSources.GlobalFeed);

            Assert.That(versionsMap, Is.Not.Null);
            var versions = versionsMap["Newtonsoft.Json"];

            Assert.That(versions, Is.Not.Null);
            Assert.That(versions, Is.Not.Empty);
        }

        [Test]
        public async Task TestFail()
        {
            string badPackageName = BadPackageName();
            var packages = new string[] { badPackageName };
            var versionsMap = await Lookup.LookupVersions(packages, NuGetSources.GlobalFeed);

            Assert.That(versionsMap, Is.Not.Null);
            Assert.That(versionsMap, Is.Not.Empty);
            Assert.That(versionsMap[badPackageName], Is.Empty);
        }

        [Test]
        public async Task TestBoth()
        {
            string badPackageName = BadPackageName();
            var packages = new string[] { "Newtonsoft.Json", badPackageName };
            var versionsMap = await Lookup.LookupVersions(packages, NuGetSources.GlobalFeed);

            Assert.That(versionsMap, Is.Not.Null);
            Assert.That(versionsMap, Is.Not.Empty);
            Assert.That(versionsMap["Newtonsoft.Json"], Is.Not.Null);
            Assert.That(versionsMap["Newtonsoft.Json"], Is.Not.Empty);

            Assert.That(versionsMap[badPackageName], Is.Not.Null);
            Assert.That(versionsMap[badPackageName], Is.Empty);
        }

        private static string BadPackageName()
        {
            return "NoSuchPackage_" + Guid.NewGuid().ToString();
        }
    }
}
