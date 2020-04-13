using MBOptionScreen.SettingDatabase;

using System.Xml.Serialization;

namespace MBOptionScreen
{
    public class Settings : SettingsBase<Settings>
    {
        public override string ModName => $"OptionScreen v1";
        public override string ModuleFolderName => "OptionScreen";

        [XmlElement]
        public override string Id { get; set; } = "OptionScreen_v1";
    }
}