namespace TareEngine.Serialization
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class SerializedRoom
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string slug;
        [JsonProperty("short")]
        public string shortname;
        public string description;
        public string[] graphic;
        public string graphicFlag;
        public Dictionary<string, string> exits;
        public Dictionary<string, string> blockers;
#pragma warning restore CS8618
    }
}
