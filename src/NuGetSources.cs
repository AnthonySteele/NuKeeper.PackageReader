using System;
using System.Collections.Generic;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using NuGet.Configuration;
using NuKeeper.PackageReader.Helpers;

namespace NuKeeper.PackageReader
{
    public class NuGetSources
    {
        public NuGetSources(params string[] sources)
        {
            if (!sources.Any())
            {
                throw new ArgumentException("At least one source must be specified", nameof(sources));
            }

            Items = sources
                .Select(s => new PackageSource(s))
                .ToList();
        }

        public static NuGetSources GlobalFeed => new NuGetSources(NuGetConstants.V3FeedUrl);

        public IReadOnlyCollection<PackageSource> Items { get; }

        public override string ToString()
        {
            return Items.Select(s => s.SourceUri.ToString()).JoinWithCommas();
        }
    }
}
