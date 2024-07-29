using Newtonsoft.Json;
using System.IO;

namespace TARE.Engine.Serialization
{
    internal static class ItemsSerializer
    {
        private static readonly string Content = nameof(Content);

        public static SerializedItemsCollection ReadItems(string jsonFile)
        {
            var path = Path.Combine(Content, jsonFile);
            var json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<SerializedItemsCollection>(json);
        }
    }
}
