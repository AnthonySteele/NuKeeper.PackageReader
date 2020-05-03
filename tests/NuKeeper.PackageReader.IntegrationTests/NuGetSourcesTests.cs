using System;
using NUnit.Framework;

namespace NuKeeper.PackageReader.IntegrationTests
{
    public class NuGetSourcesTests
    {
        [Test]
        public void ShouldThrowWithNoSources()
        {
            Assert.Throws<ArgumentException>(() => new NuGetSources());
        }

        [Test]
        public void ShouldReadOneSource()
        {
            var subject = new NuGetSources("one");

            Assert.That(subject.Items.Count, Is.EqualTo(1));
            Assert.That(subject.ToString(), Is.EqualTo("one"));
        }

        [Test]
        public void ShouldReadMultipleSources()
        {
            var subject = new NuGetSources("http://one.com", "http://two.com");


            Assert.That(subject.Items.Count, Is.EqualTo(2));
            Assert.That(subject.ToString(), Is.EqualTo("http://one.com, http://two.com"));
        }
    }
}
