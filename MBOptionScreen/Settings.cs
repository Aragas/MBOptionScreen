using MBOptionScreen.SettingDatabase;

using System.Xml.Serialization;

namespace MBOptionScreen
{
    public class Settings : SettingsBase<Settings>
    {
        public override string ModName => $"ModLib v1";
        public override string ModuleFolderName => "ModLib";

        [XmlElement]
        public override string ID { get; set; } = "ModLib_v1";
    }
}