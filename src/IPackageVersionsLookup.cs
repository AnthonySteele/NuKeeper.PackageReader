using System.Collections.Generic;
using System.Threading.Tasks;

namespace NuKeeper.PackageReader
{
    public interface IPackageVersionsLookup
    {
        Task<IReadOnlyCollection<PackageSearchMetadata>> FindVersionUpdates(
            string packageId,  NuGetSources sources);
    }
}
