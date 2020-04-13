using MBOptionScreen.GUI.v1.ViewModels;
using MBOptionScreen.Interfaces;

using System.Collections.Generic;

namespace MBOptionScreen.SettingDatabase
{
    public interface ISettingsStorage
    {
        List<SettingsBase> AllSettings { get; }
        int SettingsCount { get; }

        List<ModSettingsVM> ModSettingsVMs { get; }

        bool RegisterSettings(SettingsBase settingsClass);
        ISerializeableFile GetSettings(string uniqueID);
        void SaveSettings(SettingsBase settingsInstance);
    }
}