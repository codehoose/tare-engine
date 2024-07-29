using System;

namespace TARE.Engine.Serialization
{
    [Serializable]
    internal class SerializedFlag
    {
        public string slug;
        public SerializedFlagSet set;
    }
}
