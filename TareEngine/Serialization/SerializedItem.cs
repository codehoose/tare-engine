namespace TareEngine.Serialization
{
    [Serializable]
    public class SerializedItem
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string slug;
        public string name;
        public string description;
        public string examine;
        public string initial;
        public string[] flags;
        public List<string> words;
#pragma warning restore CS8618
    }
}
