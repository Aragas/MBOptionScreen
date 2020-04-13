using HarmonyLib;

using SandBox.View.Map;

using System.Collections.Generic;

using TaleWorlds.Engine.Screens;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection;

namespace MBOptionScreen.Patches
{
    /// <summary>
    /// Adds the Option menu to the In-Game menu
    /// </summary>
    [HarmonyPatch(typeof(MapScreen))]
    [HarmonyPatch("GetEscapeMenuItems")]
    internal class MapScreenPatch
    {
        public static void Postfix(MapScreen __instance, List<EscapeMenuItemVM> __result)
        {
            __result.Insert(1, new EscapeMenuItemVM(
                new TextObject("{=NqarFr4P}Mod Options", null),
                obj => ScreenManager.PushScreen(OptionsScreen.SyncObject.ModOptionScreen),
                null, false, false));
        }
    }
}