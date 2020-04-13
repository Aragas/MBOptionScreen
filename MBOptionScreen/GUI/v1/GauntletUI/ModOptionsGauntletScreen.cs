using MBOptionScreen.Attributes;
using MBOptionScreen.GUI.v1.ViewModels;

using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View.Missions;

namespace MBOptionScreen.GUI.v1.GauntletUI
{
    [ModuleOptionVersion("e1.0.0",  1)]
    [ModuleOptionVersion("e1.0.1",  1)]
    [ModuleOptionVersion("e1.0.2",  1)]
    [ModuleOptionVersion("e1.0.3",  1)]
    [ModuleOptionVersion("e1.0.4",  1)]
    [ModuleOptionVersion("e1.0.5",  1)]
    [ModuleOptionVersion("e1.0.6",  1)]
    [ModuleOptionVersion("e1.0.7",  1)]
    [ModuleOptionVersion("e1.0.8",  1)]
    [ModuleOptionVersion("e1.0.9",  1)]
    [ModuleOptionVersion("e1.0.10", 1)]
    [ModuleOptionVersion("e1.1.0",  1)]
    [OverrideView(typeof(ModSettingsScreenVM))]
    public class ModOptionsGauntletScreen : ScreenBase
    {
        private GauntletLayer _gauntletLayer;
        private GauntletMovie _gauntletMovie;
        private ModSettingsScreenVM _dataSource;

        protected override void OnInitialize()
        {
            base.OnInitialize();
            _dataSource = new ModSettingsScreenVM();
            _gauntletLayer = new GauntletLayer(4000, "GauntletLayer");
            _gauntletMovie = _gauntletLayer.LoadMovie("ModOptionsScreen_v1", _dataSource);
            _gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
            _gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
            _gauntletLayer.IsFocusLayer = true;
            AddLayer(_gauntletLayer);
            ScreenManager.TrySetFocus(_gauntletLayer);
        }

        protected override void OnDeactivate()
        {
            LoadingWindow.EnableGlobalLoadingWindow(false);
        }

        protected override void OnFrameTick(float dt)
        {
            base.OnFrameTick(dt);
            if (_gauntletLayer.Input.IsHotKeyReleased("Exit") || _gauntletLayer.Input.IsGameKeyReleased(34))
            {
                _dataSource.ExecuteClose();
                ScreenManager.TrySetFocus(_gauntletLayer);
                ScreenManager.PopScreen();
            }
        }

        protected override void OnFinalize()
        {
            base.OnFinalize();
            RemoveLayer(_gauntletLayer);
            _gauntletLayer.ReleaseMovie(_gauntletMovie);
            _gauntletLayer = null;
            _gauntletMovie = null;
            _dataSource.ExecuteSelect(null);
            //_dataSource.AssignParent(true);
            _dataSource = null;
        }
    }
}
