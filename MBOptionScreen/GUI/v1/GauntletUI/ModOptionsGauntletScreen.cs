using MBOptionScreen.Attributes;
using MBOptionScreen.GUI.v1.ViewModels;

using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace MBOptionScreen.GUI.v1.GauntletUI
{
    [ModuleOptionVersion("1.0.0",  1)]
    [ModuleOptionVersion("1.0.1",  1)]
    [ModuleOptionVersion("1.1.2",  1)]
    [ModuleOptionVersion("1.1.3",  1)]
    [ModuleOptionVersion("1.1.4",  1)]
    [ModuleOptionVersion("1.1.5",  1)]
    [ModuleOptionVersion("1.1.6",  1)]
    [ModuleOptionVersion("1.1.7",  1)]
    [ModuleOptionVersion("1.1.8",  1)]
    [ModuleOptionVersion("1.1.9",  1)]
    [ModuleOptionVersion("1.1.10", 1)]
    [ModuleOptionVersion("1.1.0",  1)]
    public class ModOptionsGauntletScreen : ScreenBase
    {
        private GauntletLayer gauntletLayer;
        private GauntletMovie movie;
        private ModSettingsScreenVM vm;

        protected override void OnInitialize()
        {
            base.OnInitialize();
            gauntletLayer = new GauntletLayer(1);
            gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
            gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericCampaignPanelsGameKeyCategory"));
            gauntletLayer.IsFocusLayer = true;
            ScreenManager.TrySetFocus(gauntletLayer);
            AddLayer(gauntletLayer);
            vm = new ModSettingsScreenVM();
            movie = gauntletLayer.LoadMovie("ModOptionsScreen", vm);
        }

        protected override void OnFrameTick(float dt)
        {
            base.OnFrameTick(dt);
            if (gauntletLayer.Input.IsHotKeyReleased("Exit") || gauntletLayer.Input.IsGameKeyReleased(34))
            {
                vm.ExecuteCancel();
            }
        }

        protected override void OnFinalize()
        {
            base.OnFinalize();
            RemoveLayer(gauntletLayer);
            gauntletLayer.ReleaseMovie(movie);
            gauntletLayer = null;
            movie = null;
            vm.ExecuteSelect(null);
            vm.AssignParent(true);
            vm = null;
        }
    }
}
