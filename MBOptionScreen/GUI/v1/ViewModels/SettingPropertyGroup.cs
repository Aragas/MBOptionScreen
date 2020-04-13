using MBOptionScreen.Attributes;
using TaleWorlds.Library;

namespace MBOptionScreen.GUI.v1.ViewModels
{
    public class SettingPropertyGroup : ViewModel
    {
        public const string DefaultGroupName = "Misc";

        public ModSettingsScreenVM MainView { get; private set; }
        public SettingProperty GroupToggleSettingProperty { get; private set; } = null;
        public SettingPropertyGroupAttribute Attribute { get; private set; }
        public UndoRedoStack URS { get; private set; }
        public string HintText
        {
            get
            {
                if (GroupToggleSettingProperty != null && !string.IsNullOrWhiteSpace(GroupToggleSettingProperty.HintText))
                {
                    return $"{GroupToggleSettingProperty.HintText}";
                }
                return "";
            }
        }

        public string GroupName => Attribute.GroupName;

        [DataSourceProperty]
        public string GroupNameDisplay
        {
            get
            {
                string addition = GroupToggle ? "" : "(Disabled)";
                return $"{Attribute.GroupName} {addition}";
            }
        }
        [DataSourceProperty]
        public MBBindingList<SettingProperty> SettingProperties { get; } = new MBBindingList<SettingProperty>();
        [DataSourceProperty]
        public bool GroupToggle
        {
            get
            {
                if (GroupToggleSettingProperty != null)
                    return GroupToggleSettingProperty.BoolValue;
                else
                    return true;
            }
            set
            {
                if (GroupToggleSettingProperty != null && GroupToggleSettingProperty.BoolValue != value)
                {
                    GroupToggleSettingProperty.BoolValue = value;
                    OnPropertyChanged();
                    foreach (var propSetting in SettingProperties)
                    {
                        propSetting.OnPropertyChanged(nameof(SettingProperty.IsEnabled));
                        propSetting.OnPropertyChanged(nameof(SettingProperty.IsSettingVisible));
                        OnPropertyChanged(nameof(GroupNameDisplay));
                    }
                }
            }
        }
        [DataSourceProperty]
        public bool HasGroupToggle => GroupToggleSettingProperty != null;

        public SettingPropertyGroup(SettingPropertyGroupAttribute attribute)
        {
            Attribute = attribute;
        }

        public override void RefreshValues()
        {
            base.RefreshValues();
            foreach (var setting in SettingProperties)
                setting.RefreshValues();

            OnPropertyChanged(nameof(GroupNameDisplay));
        }

        public void Add(SettingProperty sp)
        {
            SettingProperties.Add(sp);
            sp.SetMainView(MainView);
            sp.Group = this;

            if (sp.GroupAttribute.IsMainToggle)
            {
                Attribute = sp.GroupAttribute;
                GroupToggleSettingProperty = sp;
            }
        }

        internal void AssignUndoRedoStack(UndoRedoStack urs)
        {
            URS = urs;
            foreach (var settingProp in SettingProperties)
                settingProp.AssignUndoRedoStack(urs);
        }

        public void SetParent(ModSettingsScreenVM mainView)
        {
            MainView = mainView;
            //foreach (var settingProperty in SettingProperties)
            //    settingProperty.SetMainView(MainView);
        }

        private void OnHover()
        {
            if (MainView != null && !string.IsNullOrWhiteSpace(HintText))
                MainView.HintText = HintText;
        }

        private void OnHoverEnd()
        {
            if (MainView != null)
                MainView.HintText = "";
        }
    }
}
