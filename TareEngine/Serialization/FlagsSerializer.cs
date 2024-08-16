using Newtonsoft.Json;

namespace TareEngine.Serialization
{
    internal static class FlagsSerializer
    {
        private static readonly string Content = nameof(Content);

        public static SerializedFlagCollection? ReadFlags(string jsonFile)
        {
            var path = Path.Combine(Content, jsonFile);
            var json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<SerializedFlagCollection>(json);
        }
    }
}
