using MBOptionScreen.Attributes;
using MBOptionScreen.FileDatabase;
using MBOptionScreen.GUI.v1.ViewModels;
using MBOptionScreen.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;

namespace MBOptionScreen.SettingDatabase
{
    [SettingsStorageVersion("1.0.0",  1)]
    [SettingsStorageVersion("1.0.1",  1)]
    [SettingsStorageVersion("1.1.2",  1)]
    [SettingsStorageVersion("1.1.3",  1)]
    [SettingsStorageVersion("1.1.4",  1)]
    [SettingsStorageVersion("1.1.5",  1)]
    [SettingsStorageVersion("1.1.6",  1)]
    [SettingsStorageVersion("1.1.7",  1)]
    [SettingsStorageVersion("1.1.8",  1)]
    [SettingsStorageVersion("1.1.9",  1)]
    [SettingsStorageVersion("1.1.10", 1)]
    [SettingsStorageVersion("1.1.0",  1)]
    internal class DefaultSettingsStorage : ISettingsStorage
    {
        private List<ModSettingsVM> _modSettingsVMs = null;
        private Dictionary<string, SettingsBase> AllSettingsDict { get; } = new Dictionary<string, SettingsBase>();

        public List<SettingsBase> AllSettings => AllSettingsDict.Values.ToList();
        public int SettingsCount => AllSettingsDict.Values.Count;
        public List<ModSettingsVM> ModSettingsVMs
        {
            get
            {
                if (_modSettingsVMs == null)
                {
                    BuildModSettingsVMs();
                }
                return _modSettingsVMs;
            }
        }

        public DefaultSettingsStorage()
        {
            var settings = new Settings();
            AllSettingsDict.Add(settings.ID, settings);
        }

        public bool RegisterSettings(SettingsBase settingsClass)
        {
            if (!AllSettingsDict.ContainsKey(settingsClass.ID))
            {
                AllSettingsDict.Add(settingsClass.ID, settingsClass);
                _modSettingsVMs = null;
                return true;
            }
            else
            {
                //TODO:: When debugging log is finished, show a message saying that the key already exists
                return false;
            }
        }

        public ISerializeableFile GetSettings(string uniqueID)
        {
            if (AllSettingsDict.ContainsKey(uniqueID))
            {
                return AllSettingsDict[uniqueID];
            }
            else
                return null;
        }

        public void SaveSettings(SettingsBase settingsInstance)
        {
            FileDatabase.FileDatabase.SaveToFile(settingsInstance.ModuleFolderName, settingsInstance, Location.Configs);
        }

        public void BuildModSettingsVMs()
        {
            try
            {
                _modSettingsVMs = new List<ModSettingsVM>();
                foreach (var settings in AllSettings)
                {
                    ModSettingsVM msvm = new ModSettingsVM(settings);
                    _modSettingsVMs.Add(msvm);
                }
                _modSettingsVMs.Sort((x, y) => y.ModName.CompareTo(x.ModName));
            }
            catch (Exception ex)
            {
                // TODO
                //ModDebug.ShowError("An error occurred while creating the ViewModels for all mod settings", "Error Occurred", ex);
            }
        }
    }
}