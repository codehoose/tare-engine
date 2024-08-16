using TareEngine.Serialization;

namespace TareEdit
{
    internal class Rooms
    {

        public string StartRoom { get; set; }

        public List<SerializedRoom> RoomCollection { get; set; } = new();

        public Rooms(SerializedRoomCollection rooms)
        {
            StartRoom = rooms.startRoom;
            RoomCollection.AddRange(rooms.rooms);
        }
    }
}
