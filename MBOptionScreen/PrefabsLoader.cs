using System.Collections.Generic;
using System.Reflection;
using System.Xml;

using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI.PrefabSystem;

namespace MBOptionScreen
{
    /// <summary>
    /// Loads the embded .xml files from the library
    /// </summary>
    public class PrefabsLoader
    {
        private static MethodInfo LoadParametersMethod { get; } = typeof(WidgetPrefab)
            .GetMethod("LoadParameters", BindingFlags.Static | BindingFlags.NonPublic);
        private static MethodInfo LoadConstantsMethod { get; } = typeof(WidgetPrefab)
            .GetMethod("LoadConstants", BindingFlags.Static | BindingFlags.NonPublic);
        private static MethodInfo LoadCustomElementsMethod { get; } = typeof(WidgetPrefab)
            .GetMethod("LoadCustomElements", BindingFlags.Static | BindingFlags.NonPublic);
        private static MethodInfo LoadVisualDefinitionsMethod { get; } = typeof(WidgetPrefab)
            .GetMethod("LoadVisualDefinitions", BindingFlags.Static | BindingFlags.NonPublic);
        private static PropertyInfo RootTemplateProperty { get; } = typeof(WidgetPrefab)
            .GetProperty("RootTemplate", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        private static MethodInfo OnLoadingFinishedMethod { get; } = typeof(PrefabExtension)
            .GetMethod("OnLoadingFinished", BindingFlags.Instance | BindingFlags.NonPublic);


        private static string ModOptionsScreenPath = "MBOptionScreen.GUI.Prefabs.ModOptionsScreen.xml";
        private static string ModSettingsItemPath = "MBOptionScreen.GUI.Prefabs.ModSettingsItem.xml";
        private static string SettingPropertyGroupViewPath = "MBOptionScreen.GUI.Prefabs.SettingPropertyGroupView.xml";
        private static string SettingPropertyViewPath = "MBOptionScreen.GUI.Prefabs.SettingPropertyView.xml";
        private static string SettingViewPath = "MBOptionScreen.GUI.Prefabs.SettingsView.xml";
        
        public static WidgetPrefab LoadModOptionsScreenPrefab() =>
            LoadPrefab(ModOptionsScreenPath);
        public static WidgetPrefab LoadModSettingsItemPrefab() =>
            LoadPrefab(ModSettingsItemPath);
        public static WidgetPrefab LoadSettingPropertyGroupVievPrefab() =>
            LoadPrefab(SettingPropertyGroupViewPath);
        public static WidgetPrefab LoadSettingPropertyViewPrefab() =>
            LoadPrefab(SettingPropertyViewPath);
        public static WidgetPrefab LoadSettingViewPrefab() =>
            LoadPrefab(SettingViewPath);

        private static WidgetPrefab LoadPrefab(string embdedPath)
        {
            var doc = Load(embdedPath);
            return LoadFrom(
                UIResourceManager.WidgetFactory.PrefabExtensionContext,
                UIResourceManager.WidgetFactory.WidgetAttributeContext,
                doc);
        }
        private static WidgetPrefab LoadFrom(PrefabExtensionContext prefabExtensionContext, WidgetAttributeContext widgetAttributeContext, XmlDocument xmlDocument)
        {
            var widgetPrefab = new WidgetPrefab();
            var xmlNode = xmlDocument.SelectSingleNode("Prefab");
            WidgetTemplate widgetTemplate;
            if (xmlNode != null)
            {
                var xmlNode2 = xmlNode.SelectSingleNode("Parameters");
                var xmlNode3 = xmlNode.SelectSingleNode("Constants");
                var xmlNode4 = xmlNode.SelectSingleNode("Variables");
                var xmlNode5 = xmlNode.SelectSingleNode("VisualDefinitions");
                var xmlNode6 = xmlNode.SelectSingleNode("CustomElements");
                var firstChild = xmlNode.SelectSingleNode("Window").FirstChild;
                widgetTemplate = WidgetTemplate.LoadFrom(prefabExtensionContext, widgetAttributeContext, firstChild);
                if (xmlNode2 != null)
                {
                    widgetPrefab.Parameters = (Dictionary<string, string>)LoadParametersMethod.Invoke(null, new object[] { xmlNode2 });
                }
                if (xmlNode3 != null)
                {
                    widgetPrefab.Constants = (Dictionary<string, ConstantDefinition>)LoadConstantsMethod.Invoke(null, new object[] { xmlNode3 });
                }
                if (xmlNode6 != null)
                {
                    widgetPrefab.CustomElements = (Dictionary<string, XmlElement>)LoadCustomElementsMethod.Invoke(null, new object[] { xmlNode6 });
                }
                if (xmlNode5 != null)
                {
                    widgetPrefab.VisualDefinitionTemplates = (Dictionary<string, VisualDefinitionTemplate>)LoadVisualDefinitionsMethod.Invoke(null, new object[] { xmlNode5 });
                }
            }
            else
            {
                var firstChild2 = xmlDocument.SelectSingleNode("Window").FirstChild;
                widgetTemplate = WidgetTemplate.LoadFrom(prefabExtensionContext, widgetAttributeContext, firstChild2);
            }
            widgetTemplate.SetRootTemplate(widgetPrefab);
            RootTemplateProperty.SetValue(widgetPrefab, widgetTemplate);
            foreach (var prefabExtension in prefabExtensionContext.PrefabExtensions)
            {
                OnLoadingFinishedMethod.Invoke(prefabExtension, new object[] { widgetPrefab });
            }
            return widgetPrefab;
        }
        private static XmlDocument Load(string embdedPath)
        {
            using var stream = typeof(PrefabsLoader).Assembly.GetManifestResourceStream(embdedPath);
            var doc = new XmlDocument();
            doc.Load(stream);
            return doc;
        }
    }
}