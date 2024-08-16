namespace TareEngine.Serialization
{
    [Serializable]
    public class SerializedItem
    {
        public string slug;
        public string name;
        public string description;
        public string examine;
        public string initial;
        public string[] flags;
        public List<string> words;
    }
}
