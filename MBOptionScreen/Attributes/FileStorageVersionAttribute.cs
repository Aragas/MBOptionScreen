using System;

namespace MBOptionScreen.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class FileStorageVersionAttribute : Attribute, IAttributeWithVersion
    {
        public Version GameVersion { get; private set; }
        public int ImplementationVersion { get; private set; } = 0;

        public FileStorageVersionAttribute(string gameVersion, int implementationVersion)
        {
            GameVersion = Version.TryParse(gameVersion, out var v) ? v : new Version(0, 0, 0);
            ImplementationVersion = implementationVersion;
        }
    }
}