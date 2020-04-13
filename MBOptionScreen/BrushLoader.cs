using System.Xml;

using TaleWorlds.Engine.GauntletUI;

namespace MBOptionScreen
{
    /// <summary>
    /// Loads the embed .xml files from the library
    /// </summary>
    internal static class BrushLoader
    {
        private static readonly string DividerBrushes_v1Path = "MBOptionScreen._Data.GUI.v1.Brushes.DividerBrushes.xml";
        private static readonly string ModSettingsItemBrush_v1Path = "MBOptionScreen._Data.GUI.v1.Brushes.ModSettingsItemBrush.xml";
        private static readonly string ResetButtonBrush_v1Path = "MBOptionScreen._Data.GUI.v1.Brushes.ResetButtonBrush.xml";
        private static readonly string TextBrushes_v1Path = "MBOptionScreen._Data.GUI.v1.Brushes.TextBrushes.xml";

        public static void Initialize()
        {
            LoadDividerBrushes_v1Prefab();
            LoadModSettingsItemBrush_v1Prefab();
            LoadResetButtonBrush_v1Prefab();
            LoadTextBrushes_v1Prefab();
        }
        private static void LoadDividerBrushes_v1Prefab()
        {
            if (UIResourceManager.BrushFactory.GetBrush("Divider.White_v1") == null)
                BrushInjector.InjectDocument(Load(DividerBrushes_v1Path));
        }
        private static void LoadModSettingsItemBrush_v1Prefab()
        {
            if (UIResourceManager.BrushFactory.GetBrush("Mod.Setting.Item_v1") == null)
                BrushInjector.InjectDocument(Load(ModSettingsItemBrush_v1Path));
        }
        private static void LoadResetButtonBrush_v1Prefab()
        {
            if (UIResourceManager.BrushFactory.GetBrush("MobLib.ResetButton_v1") == null)
                BrushInjector.InjectDocument(Load(ResetButtonBrush_v1Path));
        }
        private static void LoadTextBrushes_v1Prefab()
        {
            if (UIResourceManager.BrushFactory.GetBrush("ModSettings.Hint.Text_v1") == null)
                BrushInjector.InjectDocument(Load(TextBrushes_v1Path));
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