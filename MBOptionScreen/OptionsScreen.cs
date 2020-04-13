using HarmonyLib;

using MBOptionScreen.Attributes;
using MBOptionScreen.FileDatabase;
using MBOptionScreen.SettingDatabase;

using System;
using System.Collections;
using System.Linq;
using System.Reflection;

using TaleWorlds.Core;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

using Module = TaleWorlds.MountAndBlade.Module;

namespace MBOptionScreen
{
    public static class OptionsScreen
    {
        private static FieldInfo InitialStateOptionsField { get; } =
            typeof(Module).GetField("_initialStateOptions", BindingFlags.Instance | BindingFlags.NonPublic);

        internal static SyncObjectV1 SyncObject;
        private static Module _module;

        // Rewrite
        public static (TypeInfo Type, TAttribute Attribute) Get<TAttribute>(Version version) where TAttribute : Attribute, IAttributeWithVersion
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
                try { maxFound = pair.Value.Where(a => a.GameVersion == version).MaxBy(a => a.ImplementationVersion); }
                catch { maxFound = null;}

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

        // TODO
        private static Version version = Version.Parse("1.1.0");

        // If there is a need to change the behaviour, instead of overriding any
        // existing version, we should create a new one.
        public static void InitializeV1(Module module, SettingsBase[] settingsArray)
        {
            _module = module;

            SyncObject = (SyncObjectV1) module.GetInitialStateOptionWithId(SyncObjectV1.SyncId);
            if (SyncObject == null) // This is the first mod to initialize MBOptionScreen
            {
                try
                {
                    new Harmony("bannerlord.mboptionscreen").PatchAll(typeof(OptionsScreen).Assembly);
                }
                catch (Exception ex)
                {
                    // TODO
                }
                
                // Find the latest implementation among loaded mods
                // as the game seems to be able to load multiple versions of an
                // assembly if it has a strong name
                var modOptionsGauntletScreenType = Get<ModuleOptionVersionAttribute>(version);
                var fileStorageType = Get<FileStorageVersionAttribute>(version);
                var settingsStorageType = Get<SettingsStorageVersionAttribute>(version);

                SyncObject = new SyncObjectV1();
                SyncObject.FileStorage = (IFileStorage) Activator.CreateInstance(fileStorageType.Type);
                SyncObject.SettingsStorage = (ISettingsStorage) Activator.CreateInstance(settingsStorageType.Type);
                SyncObject.ModOptionScreen = (ScreenBase) Activator.CreateInstance(modOptionsGauntletScreenType.Type);

                module.AddInitialStateOption(SyncObject);
                module.AddInitialStateOption(new InitialStateOption("ModOptionsMenu", new TextObject("{=HiZbHGvYG}Mod Options"), 9990, () =>
                    ScreenManager.PushScreen(SyncObject.ModOptionScreen), false));

                FileDatabase.FileDatabase.Initialize("ModOptions");
            }

            foreach (var settings in settingsArray)
                SettingsDatabase.RegisterSettings(settings);
        }
        internal static void EndInitializeV1()
        {
            if (!SyncObject.HasInitializedVM)
            {
                SettingsDatabase.BuildModSettingsVMs();
                SyncObject.HasInitializedVM = true;
            }

            var list = InitialStateOptionsField.GetValue(_module) as IList;
            list?.Remove(SyncObject);
        }
    }
}