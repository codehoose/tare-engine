namespace TareEditor.Models
{
    // Matches TareEngine.Parser.ParserDictionaryFactory's DirectionWord primaries
    // ("North","South","East","West","Down","Up"), lowercased to match the exit
    // dictionary keys used in the engine's JSON (see thedata.json).
    public enum ExitDirection
    {
        North,
        South,
        East,
        West,
        Up,
        Down
    }

    public static class ExitDirectionKeys
    {
        public static IReadOnlyList<ExitDirection> AllDirections { get; } = new[]
        {
            ExitDirection.North, ExitDirection.South, ExitDirection.East,
            ExitDirection.West, ExitDirection.Up, ExitDirection.Down
        };

        public static string ToKey(ExitDirection direction) => direction switch
        {
            ExitDirection.North => "north",
            ExitDirection.South => "south",
            ExitDirection.East => "east",
            ExitDirection.West => "west",
            ExitDirection.Up => "up",
            ExitDirection.Down => "down",
            _ => throw new ArgumentOutOfRangeException(nameof(direction))
        };

        public static bool TryParse(string key, out ExitDirection direction)
        {
            switch (key.ToLowerInvariant())
            {
                case "north": direction = ExitDirection.North; return true;
                case "south": direction = ExitDirection.South; return true;
                case "east": direction = ExitDirection.East; return true;
                case "west": direction = ExitDirection.West; return true;
                case "up": direction = ExitDirection.Up; return true;
                case "down": direction = ExitDirection.Down; return true;
                default:
                    direction = default;
                    return false;
            }
        }

        // Which port on the target node an exit line is drawn into: a North
        // exit is assumed to arrive at the target's South port, and so on.
        // The data model has no notion of an "entry port" (SerializedRoom.exits
        // just maps direction -> target slug), so this convention is purely a
        // rendering choice - see ExitLineConverter.
        public static ExitDirection Opposite(ExitDirection direction) => direction switch
        {
            ExitDirection.North => ExitDirection.South,
            ExitDirection.South => ExitDirection.North,
            ExitDirection.East => ExitDirection.West,
            ExitDirection.West => ExitDirection.East,
            ExitDirection.Up => ExitDirection.Down,
            ExitDirection.Down => ExitDirection.Up,
            _ => throw new ArgumentOutOfRangeException(nameof(direction))
        };
    }
}
