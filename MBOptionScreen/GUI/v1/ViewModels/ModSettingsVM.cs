using MBOptionScreen.ExtensionMethods;
using MBOptionScreen.SettingDatabase;

using System;

using TaleWorlds.Library;

namespace MBOptionScreen.GUI.v1.ViewModels
{
    public class ModSettingsVM : ViewModel
    {
        private bool _isSelected;
        private Action<ModSettingsVM> _executeSelect = null;
        private MBBindingList<SettingPropertyGroup> _settingPropertyGroups;
        public ModSettingsScreenVM Parent { get; private set; } = null;

        public SettingsBase SettingsInstance { get; private set; }
        public UndoRedoStack URS { get; } = new UndoRedoStack();

        [DataSourceProperty]
        public string ModName => SettingsInstance.ModName;
        [DataSourceProperty]
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
        [DataSourceProperty]
        public MBBindingList<SettingPropertyGroup> SettingPropertyGroups
        {
            get => _settingPropertyGroups;
            set
            {
                if (_settingPropertyGroups != value)
                {
                    _settingPropertyGroups = value;
                    OnPropertyChanged(nameof(SettingPropertyGroups));
                }
            }
        }

        public ModSettingsVM(SettingsBase settingsInstance)
        {
            SettingsInstance = settingsInstance;

            SettingPropertyGroups = new MBBindingList<SettingPropertyGroup>();
            SettingPropertyGroups.AddRange(settingsInstance.GetSettingPropertyGroups());

            foreach (var settingGroup in SettingPropertyGroups)
                settingGroup.AssignUndoRedoStack(URS);

            RefreshValues();
        }

        public override void RefreshValues()
        {
            base.RefreshValues();
            foreach (var group in SettingPropertyGroups)
                group.RefreshValues();
            OnPropertyChanged(nameof(IsSelected));
            OnPropertyChanged(nameof(ModName));
            OnPropertyChanged(nameof(SettingPropertyGroups));
        }

        public void AddSelectCommand(Action<ModSettingsVM> command)
        {
            _executeSelect = command;
        }

        public void SetParent(ModSettingsScreenVM parent)
        {
            Parent = parent;
            foreach (var group in SettingPropertyGroups)
                group.SetParent(Parent);
        }

        private void ExecuteSelect()
        {
            _executeSelect?.Invoke(this);
        }
    }
}