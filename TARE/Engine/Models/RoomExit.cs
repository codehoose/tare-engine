using TARE.Engine.Parser;

namespace TARE.Engine.Models
{
    internal class RoomExit
    {
        public Word Exit { get; }

        public string Slug { get; }
        public string Blocked { get; internal set; }

        public RoomExit(Word exit, string slug)
        {
            Exit = exit;
            Slug = slug;
        }
    }
}
