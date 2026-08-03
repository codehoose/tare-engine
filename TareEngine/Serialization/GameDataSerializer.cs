namespace TareEngine.Serialization
{
    using Newtonsoft.Json;
    using System.IO;

    public class GameDataSerializer : IGameDataSerializer
    {
        private static readonly string Content = nameof(Content);

        public SerializedGameData? GetData(string jsonFile)
        {
            var path = Path.Combine(Content, jsonFile);
            return GetDataFullPath(path);
        }

        public SerializedGameData? GetDataFullPath(string jsonFile)
        {
            var json = File.ReadAllText(jsonFile);
            return JsonConvert.DeserializeObject<SerializedGameData>(json);
        }
    }
}
