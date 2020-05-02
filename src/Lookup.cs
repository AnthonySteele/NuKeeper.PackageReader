using System.Collections.Generic;
using System.Threading.Tasks;
using NuKeeper.PackageReader.Helpers;

namespace NuKeeper.PackageReader
{
    public static class Lookup
    {
        public static async Task<IDictionary<string, IReadOnlyCollection<PackageSearchMetadata>>> LookupVersions(
            IEnumerable<string> packageNames, NuGetSources sources)
        {
            var logger = new NullNuKeeperLogger();
            var nugetLogger = new NuGetLogger(logger);

            var lookup = new BulkPackageLookup(new PackageVersionsLookup(nugetLogger, logger));

            return await lookup.FindVersionUpdates(packageNames, sources);
        }
    }
}
