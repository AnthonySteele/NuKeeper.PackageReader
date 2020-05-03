using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Packaging;
using NuKeeper.PackageReader.Helpers;

namespace NuKeeper.PackageReader
{
    public class Lookup
    {
        private readonly NuGetSources _sources;
        private readonly IBulkPackageLookup _lookup;

        public Lookup(NuGetSources sources)
        {
            _sources = sources;
            var logger = new NullNuKeeperLogger();
            var nugetLogger = new NuGetLogger(logger);

            _lookup = new BulkPackageLookup(new PackageVersionsLookup(nugetLogger, logger));
        }

        public async Task<IDictionary<string, IReadOnlyCollection<PackageSearchMetadata>>> LookupPackageVersions(
            IEnumerable<string> packageNames)
        {
            var results = new Dictionary<string, IReadOnlyCollection<PackageSearchMetadata>>();
            await LookupPackageVersionsInternal(results, packageNames);
            return results;
        }

        private async Task LookupPackageVersionsInternal(
            IDictionary<string, IReadOnlyCollection<PackageSearchMetadata>> allResults,
            IEnumerable<string> packageNames)
        {
            if (! packageNames.Any())
            {
                return;
            }

            var moreResults = await _lookup.FindVersionUpdates(packageNames, _sources);
            allResults.AddRange(moreResults);

            // recurse, get the data on unretrieved dependencies
            var dependantPackages = NamesOfUnretrievedPackageDependencies(allResults, moreResults);
            await LookupPackageVersionsInternal(allResults, dependantPackages);
        }


        private static IEnumerable<string> NamesOfUnretrievedPackageDependencies(
            IDictionary<string, IReadOnlyCollection<PackageSearchMetadata>> allPackages,
            IDictionary<string, IReadOnlyCollection<PackageSearchMetadata>> currentPackages)
        {
            var packageLatestVersions = currentPackages
                .Select(p => p.Value.FirstOrDefault())
                .Where(v => v != null);

            var dependencyIds = packageLatestVersions
                .SelectMany(item => item.Dependencies)
                .Select(dep => dep.Id);

            return dependencyIds
                .Distinct()
                .Where(id => !allPackages.Keys.Contains(id));
        }
    }
}
