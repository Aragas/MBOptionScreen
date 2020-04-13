using HarmonyLib;
using TaleWorlds.MountAndBlade;

namespace MBOptionScreen.Patches
{
    /// <summary>
    /// Instead of making the user manually call EndInitializeV1
    /// it can do it itself
    /// </summary>
    [HarmonyPatch(typeof(Module))]
    [HarmonyPatch("SetInitialModuleScreenAsRootScreen")]
    internal class ModulePatch
    {
        public static void Postfix()
        {
            ModOptions.EndInitializeV1();
        }
    }
}