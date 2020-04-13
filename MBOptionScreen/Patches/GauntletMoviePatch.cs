using HarmonyLib;

using System.Reflection;

using TaleWorlds.GauntletUI;
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
        private static PropertyInfo RootViewProperty { get; } =
            typeof(GauntletMovie).GetProperty("RootView", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        private static FieldInfo MovieRootNodeField { get; } =
            typeof(GauntletMovie).GetField("_movieRootNode", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        /// <summary>
        /// Intercept LoadMovie("ModOptionsScreen_v1")
        /// </summary>
        public static bool Prefix(GauntletMovie __instance)
        {
            if (__instance.MovieName == "ModOptionsScreen_v1")
            {
                var customType = PrefabsLoader.LoadModOptionsScreen_v1Prefab();
                var widgetCreationData = new WidgetCreationData(__instance.Context, __instance.WidgetFactory);
                widgetCreationData.AddExtensionData(__instance);
                var widgetInstantiationResult = customType.Instantiate(widgetCreationData);
                RootViewProperty.SetValue(__instance, widgetInstantiationResult.GetGauntletView());
                var target = __instance.RootView.Target;
                var movieRootNode = (Widget) MovieRootNodeField.GetValue(__instance);
                movieRootNode.AddChild(target);
                __instance.RootView.RefreshBindingWithChildren();

                return false;
            }

            return true;
        }
    }
}