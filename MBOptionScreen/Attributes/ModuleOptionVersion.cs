using System;

using TaleWorlds.Library;

namespace MBOptionScreen.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class ModuleOptionVersionAttribute : Attribute, IAttributeWithVersion
    {
        public ApplicationVersion GameVersion { get; }
        public int ImplementationVersion { get; }

        public ModuleOptionVersionAttribute(string gameVersion, int implementationVersion)
        {
            GameVersion = ApplicationVersionParser.TryParse(gameVersion, out var v) ? v : default;
            ImplementationVersion = implementationVersion;
        }
    }
}