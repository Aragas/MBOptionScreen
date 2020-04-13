using MBOptionScreen.Attributes;
using MBOptionScreen.FileDatabase;
using MBOptionScreen.GUI.v1.ViewModels;
using MBOptionScreen.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;

namespace MBOptionScreen.SettingDatabase
{
    [SettingsStorageVersion("e1.0.0",  1)]
    [SettingsStorageVersion("e1.0.1",  1)]
    [SettingsStorageVersion("e1.0.2",  1)]
    [SettingsStorageVersion("e1.0.3",  1)]
    [SettingsStorageVersion("e1.0.4",  1)]
    [SettingsStorageVersion("e1.0.5",  1)]
    [SettingsStorageVersion("e1.0.6",  1)]
    [SettingsStorageVersion("e1.0.7",  1)]
    [SettingsStorageVersion("e1.0.8",  1)]
    [SettingsStorageVersion("e1.0.9",  1)]
    [SettingsStorageVersion("e1.0.10", 1)]
    [SettingsStorageVersion("e1.0.11", 1)]
    [SettingsStorageVersion("e1.1.0",  1)]
    internal class DefaultSettingsStorage : ISettingsStorage
    {
        private Dictionary<string, SettingsBase> AllSettingsDict { get; } = new Dictionary<string, SettingsBase>();

        public List<SettingsBase> AllSettings => AllSettingsDict.Values.ToList();
        public int SettingsCount => AllSettingsDict.Values.Count;
        public List<ModSettingsVM> ModSettingsVMs => GetModSettingsVMs().ToList();

        public DefaultSettingsStorage()
        {
            var settings = new Settings();
            AllSettingsDict.Add(settings.Id, settings);
        }

        public bool RegisterSettings(SettingsBase settingsClass)
        {
            if (!AllSettingsDict.ContainsKey(settingsClass.Id))
            {
                AllSettingsDict.Add(settingsClass.Id, settingsClass);
                return true;
            }
            else
            {
                //TODO:: When debugging log is finished, show a message saying that the key already exists
                return false;
            }
        }

        public ISerializeableFile? GetSettings(string uniqueId) => AllSettingsDict.TryGetValue(uniqueId, out var val) ? val : null;

        public void SaveSettings(SettingsBase settingsInstance)
        {
            FileDatabase.FileDatabase.SaveToFile(settingsInstance.ModuleFolderName, settingsInstance, Location.Configs);
        }

        public IEnumerable<ModSettingsVM> GetModSettingsVMs()
        {
            try
            {
                return AllSettings
                    .Select(settings => new ModSettingsVM(settings))
                    .OrderByDescending(a => a.ModName != Settings.Instance!.Id)
                    .ThenBy(a => a.ModName);
            }
            catch (Exception ex)
            {
                return new List<ModSettingsVM>();
                // TODO
                //ModDebug.ShowError("An error occurred while creating the ViewModels for all mod settings", "Error Occurred", ex);
            }
        }
    }
}