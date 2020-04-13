using System;

using TaleWorlds.Library;

namespace MBOptionScreen.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class FileStorageVersionAttribute : Attribute, IAttributeWithVersion
    {
        public ApplicationVersion GameVersion { get; private set; }
        public int ImplementationVersion { get; private set; } = 0;

        public FileStorageVersionAttribute(string gameVersion, int implementationVersion)
        {
            GameVersion = ApplicationVersionParser.TryParse(gameVersion, out var v) ? v : default;
            ImplementationVersion = implementationVersion;
        }
    }
}