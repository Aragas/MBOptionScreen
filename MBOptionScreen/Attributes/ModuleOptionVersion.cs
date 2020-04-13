using System;

using TaleWorlds.Library;

namespace MBOptionScreen.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ModuleOptionVersionAttribute : Attribute, IAttributeWithVersion
    {
        public ApplicationVersion GameVersion { get; private set; }
        public int ImplementationVersion { get; private set; } = 0;

        public ModuleOptionVersionAttribute(string gameVersion, int implementationVersion)
        {
            GameVersion = ApplicationVersionParser.TryParse(gameVersion, out var v) ? v : default;
            ImplementationVersion = implementationVersion;
        }
    }
}