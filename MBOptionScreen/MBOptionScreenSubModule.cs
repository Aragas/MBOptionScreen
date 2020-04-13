using HarmonyLib;

using MBOptionScreen.Attributes;
using MBOptionScreen.FileDatabase;
using MBOptionScreen.SettingDatabase;

using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using TaleWorlds.DotNet;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

using Module = TaleWorlds.MountAndBlade.Module;

namespace MBOptionScreen
{
    public class MBOptionScreenSubModule : MBSubModuleBase
    {
        private static FieldInfo InitialStateOptionsField { get; } =
            typeof(Module).GetField("_initialStateOptions", BindingFlags.Instance | BindingFlags.NonPublic);

        internal static SyncObjectV1 SyncObject;

        protected override void OnSubModuleLoad()
        {
            SyncObject = (SyncObjectV1) Module.CurrentModule.GetInitialStateOptionWithId(SyncObjectV1.SyncId);
            if (SyncObject == null) // This is the first mod to initialize MBOptionScreen
            {
                try
                {
                    new Harmony("bannerlord.mboptionscreen").PatchAll(typeof(MBOptionScreenSubModule).Assembly);
                }
                catch (Exception ex)
                {
                    // TODO
                }

                // Find the latest implementation among loaded mods
                // as the game seems to be able to load multiple versions of an
                // assembly if it has a strong name
                var version = ApplicationVersionParser.TryParse(Managed.GetVersionStr(), out var v) ? v : default;
                var modOptionsGauntletScreenType = AttributeHelper.Get<ModuleOptionVersionAttribute>(version);
                var fileStorageType = AttributeHelper.Get<FileStorageVersionAttribute>(version);
                var settingsStorageType = AttributeHelper.Get<SettingsStorageVersionAttribute>(version);

                SyncObject = new SyncObjectV1
                {
                    FileStorage = (IFileStorage) Activator.CreateInstance(fileStorageType.Type),
                    SettingsStorage = (ISettingsStorage) Activator.CreateInstance(settingsStorageType.Type),
                    ModOptionScreen = modOptionsGauntletScreenType.Type
                };

                Module.CurrentModule.AddInitialStateOption(SyncObject); // Workaround
                Module.CurrentModule.AddInitialStateOption(new InitialStateOption("ModOptionsMenu", new TextObject("{=HiZbHGvYG}Mod Options"), 9990, () =>
                {
                    var screen = (ScreenBase) Activator.CreateInstance(SyncObject.ModOptionScreen);
                    ScreenManager.PushScreen(screen);
                }, false));

                FileDatabase.FileDatabase.Initialize("ModOptions");
            }


            var settingsEnumerable = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.DefinedTypes)
                .Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(SettingsBase)) && t != typeof(Settings));
            foreach (var settings in settingsEnumerable)
                SettingsDatabase.RegisterSettings((SettingsBase) Activator.CreateInstance(settings));
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            BrushLoader.Initialize();
            PrefabsLoader.Initialize();

            if (!SyncObject.HasInitializedVM)
            {
                //SettingsDatabase.BuildModSettingsVMs();
                SyncObject.HasInitializedVM = true;
            }

            var list = InitialStateOptionsField.GetValue(Module.CurrentModule) as IList;
            list?.Remove(SyncObject);
        }
    }
}
