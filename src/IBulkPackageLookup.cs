using System.Collections.Generic;
using System.Threading.Tasks;

namespace NuKeeper.PackageReader
{
    public interface IBulkPackageLookup
    {
        Task<IDictionary<string, IReadOnlyCollection<PackageSearchMetadata>>> FindVersionUpdates(
            IEnumerable<string> packageIds,
            NuGetSources sources);
    }
}
