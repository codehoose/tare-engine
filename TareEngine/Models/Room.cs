namespace TareEngine.Models
{
    public class Room
    {
        private string[] _graphics;

        public string Short { get; }
        public string Description { get; }
        public string Slug { get; }
        public string GraphicFlag { get; }
        public string Graphic => _graphics!= null && _graphics.Length > 0 ? _graphics[0] : string.Empty;
        public bool HasGraphic => !string.IsNullOrEmpty(Graphic);
        public List<RoomExit> Exits { get; } = new List<RoomExit>();
        public List<Item> Items { get; } = new List<Item>();

        public Room(string shortDesc, string description, string slug, string[] graphics, string graphicFlag, IEnumerable<RoomExit> exits)
        {
            Short = shortDesc;
            Slug = slug;
            Description = description;
            _graphics = graphics;
            GraphicFlag = graphicFlag;
            Exits.AddRange(exits);
        }

        public string GetGraphic(int index)
        {
            if (_graphics == null || _graphics.Length == 0) return string.Empty;
            var i = index % _graphics.Length;
            return _graphics[i];
        }
    }
}
