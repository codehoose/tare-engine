namespace TareEngine.Serialization
{
    [Serializable]
    public class SerializedFlagSet
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string location;
        public string verb;
        public string noun;
        public string text;
        public string blockedText;
        public string carry;
        public string flag;
        public string type;
        public string when;
        public SerializedFlagTask[] tasks;
#pragma warning restore CS8618
    }
}