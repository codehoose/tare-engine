using Newtonsoft.Json;

namespace TareEngine.Serialization
{
    public class GameDataSerializer
    {
        private static readonly string Content = nameof(Content);

        public static SerializedGameData? GetData(string jsonFile)
        {
            var path = Path.Combine(Content, jsonFile);
            return GetDataFullPath(path);
        }

        public static SerializedGameData? GetDataFullPath(string jsonFile)
        {
            var json = File.ReadAllText(jsonFile);
            return JsonConvert.DeserializeObject<SerializedGameData>(json);
        }
    }
}
