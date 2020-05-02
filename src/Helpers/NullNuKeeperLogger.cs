using System;

namespace NuKeeper.PackageReader.Helpers
{
    public class NullNuKeeperLogger : INuKeeperLogger
    {
        public void Detailed(string message)
        {
        }

        public void Error(string message, Exception? ex = null)
        {
        }

        public void Minimal(string message)
        {
        }

        public void Normal(string message)
        {
        }
    }
}
