using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NuKeeper.PackageReader
{
    public class BulkPackageLookup : IBulkPackageLookup
    {
        private readonly IPackageVersionsLookup _packageLookup;

        public BulkPackageLookup(IPackageVersionsLookup packageLookup)
        {
            _packageLookup = packageLookup;
        }

        public async Task<IDictionary<string, IReadOnlyCollection<PackageSearchMetadata>>> FindVersionUpdates(
            IEnumerable<string> packageIds, NuGetSources sources)
        {
            var lookupTasks = packageIds
                .Select(id => FindVersionUpdates(id, sources))
                .ToList();

            await Task.WhenAll(lookupTasks);

            var allPackageData = new Dictionary<string, IReadOnlyCollection<PackageSearchMetadata>>(StringComparer.OrdinalIgnoreCase);

            foreach (var lookupTask in lookupTasks)
            {
                var (packageId, packageVersions) = lookupTask.Result;
                if (!allPackageData.ContainsKey(packageId))
                {
                    allPackageData.Add(packageId, packageVersions);
                }
            }

            return allPackageData;
        }

        private async Task<(string PackageId, IReadOnlyCollection<PackageSearchMetadata> PackageVersions)> FindVersionUpdates(string packageId, NuGetSources sources)
        {
            var packageVersions = await _packageLookup.FindVersionUpdates(packageId, sources);
            return (packageId, packageVersions);
        }
    }
}
