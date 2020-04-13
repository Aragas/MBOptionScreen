using System;
using TaleWorlds.Library;

namespace MBOptionScreen.Attributes
{
    public interface IAttributeWithVersion
    {
        ApplicationVersion GameVersion { get; }
        int ImplementationVersion { get;}
    }
}