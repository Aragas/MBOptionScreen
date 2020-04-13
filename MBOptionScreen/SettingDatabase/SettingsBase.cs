using MBOptionScreen.Attributes;
using MBOptionScreen.GUI.v1.ViewModels;
using MBOptionScreen.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MBOptionScreen.SettingDatabase
{
    public abstract class SettingsBase<T> : SettingsBase where T : SettingsBase, new()
    {
        private static T? _instance = null;
        public static T? Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                var emptySettings = new T();

                _instance = FileDatabase.FileDatabase.Get<T>(emptySettings.ID);
                if (_instance != null)
                    return _instance;

                _instance = emptySettings;
                SettingsDatabase.SaveSettings(_instance);

                return _instance;
            }
        }
    }

    public abstract class SettingsBase : ISerializeableFile, ISubFolder
    {
        public abstract string ID { get; set; }
        public abstract string ModuleFolderName { get; }
        public abstract string ModName { get; }
        public virtual string SubFolder => "";

        internal List<SettingPropertyGroup> GetSettingPropertyGroups()
        {
            var groups = new List<SettingPropertyGroup>();

            var propList = (from p in GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                            let propAttr = p.GetCustomAttribute<SettingPropertyAttribute>(true)
                            let groupAttr = p.GetCustomAttribute<SettingPropertyGroupAttribute>(true)
                            where propAttr != null
                            let groupAttrToAdd = groupAttr ?? SettingPropertyGroupAttribute.Default
                            select new SettingProperty(propAttr, groupAttrToAdd, p, this)).ToList();


            foreach (var settingProp in propList)
            {
                CheckIsValid(settingProp);
                SettingPropertyGroup group = GetGroupFor(settingProp, groups);
                group.Add(settingProp);
            }

            SettingPropertyGroup noneGroup = GetGroupFor(groups, SettingPropertyGroup.DefaultGroupName);
            if (noneGroup != null && groups.Count > 1)
                groups.Remove(noneGroup);
            else
                noneGroup = null;

            groups.Sort((x, y) => x.GroupName.CompareTo(y.GroupName));
            if (noneGroup != null)
                groups.Add(noneGroup);

            return groups;
        }

        private SettingPropertyGroup GetGroupFor(SettingProperty sp, List<SettingPropertyGroup> groupsList)
        {
            if (sp.GroupAttribute == null)
                throw new Exception($"SettingProperty {sp.Name} has null GroupAttribute");

            SettingPropertyGroup group = groupsList.Where((x) => x.GroupName == sp.GroupAttribute.GroupName).FirstOrDefault();
            if (group == null)
            {
                group = new SettingPropertyGroup(sp.GroupAttribute);
                groupsList.Add(group);
            }
            return group;
        }

        private SettingPropertyGroup GetGroupFor(List<SettingPropertyGroup> groupsList, string groupName)
        {
            return groupsList.Where((x) => x.GroupName == groupName).FirstOrDefault();
        }

        private void CheckIsValid(SettingProperty prop)
        {
            if (!prop.Property.CanRead)
                throw new Exception($"Property {prop.Property.Name} in {prop.SettingsInstance.GetType().FullName} must have a getter.");
            if (!prop.Property.CanWrite)
                throw new Exception($"Property {prop.Property.Name} in {prop.SettingsInstance.GetType().FullName} must have a setter.");
            if (prop.SettingType == SettingType.Int || prop.SettingType == SettingType.Float)
            {
                if (prop.MinValue == prop.MaxValue)
                    throw new Exception($"Property {prop.Property.Name} in {prop.SettingsInstance.GetType().FullName} is a numeric type but the MinValue and MaxValue are the same.");
            }
        }
    }
}
