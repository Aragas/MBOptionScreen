using HarmonyLib;

using TaleWorlds.GauntletUI.Data;
using TaleWorlds.GauntletUI.PrefabSystem;

namespace MBOptionScreen.Patches
{
    /// <summary>
    /// Seems that Harmony won't hook into WidgetFactory.IsCustomType or WidgetFactory.GetCustomType
    /// so it need to be intercepted in a higher level
    /// </summary>
    [HarmonyPatch(typeof(GauntletMovie))]
    [HarmonyPatch("LoadMovie")]
    internal class GauntletMoviePatch
    {
        public static bool Prefix(GauntletMovie __instance)
        {
            if (__instance.MovieName == "ModOptionsScreen")
            {
                var customType = PrefabsLoader.LoadModOptionsScreenPrefab();
                var widgetCreationData = new WidgetCreationData(__instance.Context, __instance.WidgetFactory);
                widgetCreationData.AddExtensionData(__instance);
                var widgetInstantiationResult = customType.Instantiate(widgetCreationData);
                /*
                __instance.RootView = widgetInstantiationResult.GetGauntletView();
                var target = __instance.RootView.Target;
                __instance._movieRootNode.AddChild(target);
                __instance.RootView.RefreshBindingWithChildren();
                */

                return false;
            }

            return true;
        }
    }
}