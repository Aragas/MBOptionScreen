using System;

namespace MBOptionScreen.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ModuleOptionVersionAttribute : Attribute, IAttributeWithVersion
    {
        public Version GameVersion { get; private set; }
        public int ImplementationVersion { get; private set; } = 0;

        public ModuleOptionVersionAttribute(string gameVersion, int implementationVersion)
        {
            GameVersion = Version.TryParse(gameVersion, out var v) ? v : new Version(0, 0, 0);
            ImplementationVersion = implementationVersion;
        }
    }
}