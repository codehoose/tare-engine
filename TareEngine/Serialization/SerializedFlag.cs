namespace TareEngine.Serialization
{
    [Serializable]
    public class SerializedFlag
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string slug;
        public SerializedFlagSet[] cond; // conditions
#pragma warning restore CS8618 
    }
}
