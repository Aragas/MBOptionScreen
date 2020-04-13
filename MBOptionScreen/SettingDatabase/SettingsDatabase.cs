﻿using MBOptionScreen.GUI.v1.ViewModels;
using MBOptionScreen.Interfaces;

using System.Collections.Generic;

namespace MBOptionScreen.SettingDatabase
{
    internal static class SettingsDatabase
    {
        private static ISettingsStorage SettingsStorage => OptionsScreen.SyncObject.SettingsStorage;

        public static List<SettingsBase> AllSettings => SettingsStorage.AllSettings;
        public static int SettingsCount => SettingsStorage.SettingsCount;
        public static List<ModSettingsVM> ModSettingsVMs => SettingsStorage.ModSettingsVMs;

        public static bool RegisterSettings(SettingsBase settingsClass) =>
            SettingsStorage.RegisterSettings(settingsClass);

        public static ISerializeableFile GetSettings(string uniqueID) =>
            SettingsStorage.GetSettings(uniqueID);

        public static void SaveSettings(SettingsBase settingsInstance) =>
            SettingsStorage.SaveSettings(settingsInstance);

        public static void BuildModSettingsVMs() =>
            SettingsStorage.BuildModSettingsVMs();
    }
}