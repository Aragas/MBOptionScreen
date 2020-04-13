using System;

namespace MBOptionScreen.Attributes
{
    public interface IAttributeWithVersion
    {
        Version GameVersion { get; }
        int ImplementationVersion { get;} 
    }
}