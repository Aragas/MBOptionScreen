using MBOptionScreen.Attributes;

using System;
using System.Linq;
using System.Reflection;

using TaleWorlds.Core;
using TaleWorlds.Library;

namespace MBOptionScreen
{
    internal static class AttributeHelper
    {
        // Rewrite
        public static (TypeInfo Type, TAttribute Attribute) Get<TAttribute>(ApplicationVersion version) where TAttribute : Attribute, IAttributeWithVersion
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.DefinedTypes);

            var attributes = types
                .Where(t => t.GetCustomAttributes<TAttribute>().Any())
                .ToDictionary(k => k, v => v.GetCustomAttributes<TAttribute>());

            (TypeInfo Type, TAttribute Attribute) maxMatching = default;
            foreach (var pair in attributes)
            {
                TAttribute maxFound = null;
                // TODO
                try { maxFound = pair.Value.Where(a => a.GameVersion.IsSame(version)).MaxBy(a => a.ImplementationVersion); }
                catch { maxFound = null; }

                if (maxFound == null)
                    continue;

                if (maxMatching.Attribute == null)
                {
                    maxMatching.Type = pair.Key;
                    maxMatching.Attribute = maxFound;
                }

                if (maxMatching.Attribute.ImplementationVersion < maxFound.ImplementationVersion)
                {
                    maxMatching.Type = pair.Key;
                    maxMatching.Attribute = maxFound;
                }
            }

            if (maxMatching.Type == null) // no matching game version, using the latest
            {
                foreach (var pair in attributes)
                {
                    var maxFound = pair.Value
                        .OrderByDescending(a => a.ImplementationVersion)
                        .ThenByDescending(a => a.GameVersion)
                        .FirstOrDefault();
                    if (maxFound == null)
                        continue;

                    if (maxMatching.Attribute == null)
                    {
                        maxMatching.Type = pair.Key;
                        maxMatching.Attribute = maxFound;
                    }

                    if (maxMatching.Attribute.ImplementationVersion < maxFound.ImplementationVersion)
                    {
                        maxMatching.Type = pair.Key;
                        maxMatching.Attribute = maxFound;
                    }
                }
            }

            if (maxMatching.Type == null)
                throw new Exception();

            return maxMatching;
        }
    }
}