namespace TareEngine.Serialization
{
    [Serializable]
    public class SerializedGameData
    {
        public SerializedRoomCollection rooms;
        public SerializedFlag[] flags;
        public SerializedItem[] items;
        public SerializedAction[] actions;
    }
}
