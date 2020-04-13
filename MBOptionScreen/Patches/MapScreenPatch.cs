using HarmonyLib;

using SandBox.View.Map;

using System;
using System.Collections.Generic;
using System.Reflection;

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
        private static MethodInfo OnEscapeMenuToggledMethod { get; } =
            typeof(MapScreen).GetMethod("OnEscapeMenuToggled", BindingFlags.Instance | BindingFlags.NonPublic);

        public static void Postfix(MapScreen __instance, List<EscapeMenuItemVM> __result)
        {
            __result.Insert(1, new EscapeMenuItemVM(
                new TextObject("{=NqarFr4P}Mod Options", null),
                obj =>
                {
                    OnEscapeMenuToggledMethod.Invoke(__instance, new object[] { false });
                    ScreenManager.PushScreen((ScreenBase) Activator.CreateInstance(MBOptionScreenSubModule.SyncObject.ModOptionScreen));
                },
                null, false, false));
        }
    }
}