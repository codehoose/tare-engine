using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TARE.Engine.Serialization
{
    [Serializable]
    internal class SerializedRoom
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
