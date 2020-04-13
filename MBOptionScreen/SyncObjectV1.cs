using MBOptionScreen.FileDatabase;
using MBOptionScreen.SettingDatabase;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace MBOptionScreen
{
    /// <summary>
    /// A shareable object between multiple mods that will use this library
    /// </summary>
    internal class SyncObjectV1 : InitialStateOption
    {
        public static string SyncId = "modlib_syncronization_object";

        public bool HasInitializedVM { get; internal set; }
        public ScreenBase ModOptionScreen { get; internal set; }
        public IFileStorage FileStorage { get; internal set; }
        public ISettingsStorage SettingsStorage { get; internal set; }

        public SyncObjectV1() : base(SyncId, new TextObject(""), 1, null, true) { }
    }
}