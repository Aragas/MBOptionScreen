using System.Xml;

using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI.PrefabSystem;

namespace MBOptionScreen
{
    /// <summary>
    /// Loads the embed .xml files from the library
    /// </summary>
    internal static class PrefabsLoader
    {
        private static readonly string ModOptionsScreen_v1Path = "MBOptionScreen._Data.GUI.v1.Prefabs.ModOptionsScreen.xml";

        private static readonly string ModSettingsItem_v1Path = "MBOptionScreen._Data.GUI.v1.Prefabs.ModSettingsItem.xml";

        private static readonly string SettingPropertyGroupView_v1Path = "MBOptionScreen._Data.GUI.v1.Prefabs.SettingPropertyGroupView.xml";

        private static readonly string SettingPropertyView_v1Path = "MBOptionScreen._Data.GUI.v1.Prefabs.SettingPropertyView.xml";

        private static readonly string SettingView_v1Path = "MBOptionScreen._Data.GUI.v1.Prefabs.SettingsView.xml";

        public static void Initialize()
        {
            LoadSettingView_v1Prefab();
            LoadModSettingsItem_v1Prefab();
            LoadSettingPropertyView_v1Prefab();
            LoadSettingPropertyGroupView_v1Prefab();
        }

        private static void LoadModSettingsItem_v1Prefab()
        {
            if (UIResourceManager.WidgetFactory.GetCustomType("ModSettingsItem") == null)
                PrefabInjector.InjectDocumentAndCreate("ModSettingsItem", Load(ModSettingsItem_v1Path));
        }
        private static void LoadSettingPropertyView_v1Prefab()
        {
            if (UIResourceManager.WidgetFactory.GetCustomType("SettingPropertyView") == null)
                PrefabInjector.InjectDocumentAndCreate("SettingPropertyView", Load(SettingPropertyView_v1Path));
        }
        private static void LoadSettingPropertyGroupView_v1Prefab()
        {
            if (UIResourceManager.WidgetFactory.GetCustomType("SettingPropertyGroupView") == null)
                PrefabInjector.InjectDocumentAndCreate("SettingPropertyGroupView", Load(SettingPropertyGroupView_v1Path));
        }
        private static void LoadSettingView_v1Prefab()
        {
            if (UIResourceManager.WidgetFactory.GetCustomType("SettingsView") == null)
                PrefabInjector.InjectDocumentAndCreate("SettingsView", Load(SettingView_v1Path));
        }
        public static WidgetPrefab LoadModOptionsScreen_v1Prefab()
        {
            return PrefabInjector.InjectDocumentAndCreate("ModOptionsScreen", Load(ModOptionsScreen_v1Path));
        }

        private static XmlDocument Load(string embedPath)
        {
            using var stream = typeof(PrefabsLoader).Assembly.GetManifestResourceStream(embedPath);
            var doc = new XmlDocument();
            doc.Load(stream);
            return doc;
        }
    }
}