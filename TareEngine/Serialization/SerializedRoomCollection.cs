namespace TareEngine.Serialization
{
    using System;

    [Serializable]
    public class SerializedRoomCollection
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string startRoom;
        public SerializedRoom[] rooms;
#pragma warning restore CS8618
    }
}
