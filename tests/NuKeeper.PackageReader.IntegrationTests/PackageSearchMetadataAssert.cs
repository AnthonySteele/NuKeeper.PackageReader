using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace NuKeeper.PackageReader.IntegrationTests
{
    public static class PackageSearchMetadataAssert
    {
        public static void AssertPopulated(IEnumerable<PackageSearchMetadata> packageVersions)
        {
            Assert.That(packageVersions, Is.Not.Null);
            Assert.That(packageVersions, Is.Not.Empty);

            if (packageVersions == null)
            {
                throw new ArgumentNullException(nameof(packageVersions));
            }

            foreach (var package in packageVersions)
            {
                AssertPopulated(package);
            }
        }

        public static void AssertPopulated(PackageSearchMetadata packageVersion)
        {
            Assert.That(packageVersion, Is.Not.Null);

            if (packageVersion == null)
            {
                throw new ArgumentNullException(nameof(packageVersion));
            }

            Assert.That(packageVersion.Identity, Is.Not.Null);
            Assert.That(packageVersion.Published, Is.Not.Null);
            Assert.That(packageVersion.Source, Is.Not.Null);
            Assert.That(packageVersion.Dependencies, Is.Not.Null);
            Assert.That(packageVersion.ToString(), Is.Not.Empty);
        }
    }
}
