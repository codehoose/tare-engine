using System;

namespace TARE.Engine.Serialization
{
    [Serializable]
    internal class SerializedRoomCollection
    {
        public string startRoom;
        public SerializedRoom[] rooms;
    }
}
