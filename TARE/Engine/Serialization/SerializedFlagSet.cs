using System;

namespace TARE.Engine.Serialization
{
    [Serializable]
    internal class SerializedFlagSet
    {
        public string location;
        public string verb;
        public string noun;
        public string text;
        public string carry;
        public string flag;
        public string type;
        public string when;
        public SerializedFlagTask[] tasks;
    }
}