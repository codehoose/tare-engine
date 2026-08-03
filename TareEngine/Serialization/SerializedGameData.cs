namespace TareEngine.Serialization
{
    using System;

    [Serializable]
    public class SerializedGameData
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public SerializedRoomCollection rooms;
        public SerializedFlag[] flags;
        public SerializedItem[] items;
        public SerializedAction[] actions;
        public CondActions condActions;
#pragma warning restore CS8618
    }
}
