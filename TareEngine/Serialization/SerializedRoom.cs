using Newtonsoft.Json;

namespace TareEngine.Serialization
{
    [Serializable]
    public class SerializedRoom
    {
        public string slug;
        [JsonProperty("short")]
        public string shortname;
        public string description;
        public string[] graphic;
        public string graphicFlag;
        public Dictionary<string, string> exits;
        public Dictionary<string, string> blockers;
    }
}
