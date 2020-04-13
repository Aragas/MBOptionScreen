using MBOptionScreen.Interfaces;

namespace MBOptionScreen.FileDatabase
{
    internal static class FileDatabase
    {
        private static IFileStorage FileStorage => OptionsScreen.SyncObject.FileStorage;

        public static T Get<T>(string id) where T : ISerializeableFile =>
            FileStorage == null ? default : FileStorage.Get<T>(id);

        public static bool Initialize(string moduleName) =>
            FileStorage?.Initialize(moduleName) == true;

        public static bool SaveToFile(string moduleName, ISerializeableFile sf, Location location = Location.Modules) =>
            FileStorage?.SaveToFile(moduleName, sf, location) == true;
    }
}