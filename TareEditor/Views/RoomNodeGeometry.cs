using System.Windows;
using TareEditor.Models;

namespace TareEditor.Views
{
    // Single source of truth for a room node's on-canvas footprint and port
    // positions, shared between RoomNodeControl (positions the port ellipses
    // from these values in its constructor) and ExitLineConverter/
    // PendingConnectionConverter (compute connector line endpoints from
    // these same values), so rendering and hit-testing can't drift apart.
    public static class RoomNodeGeometry
    {
        public const double NodeWidth = 160;
        public const double NodeHeight = 90;

        // Port center, relative to the node's own X/Y (top-left corner).
        // Up/Down have no natural edge position, so they're stacked as a
        // small pair near the top-right corner instead.
        public static Point PortCenter(ExitDirection direction) => direction switch
        {
            ExitDirection.North => new Point(NodeWidth / 2, 0),
            ExitDirection.South => new Point(NodeWidth / 2, NodeHeight),
            ExitDirection.East => new Point(NodeWidth, NodeHeight / 2),
            ExitDirection.West => new Point(0, NodeHeight / 2),
            ExitDirection.Up => new Point(141, 0),
            ExitDirection.Down => new Point(141, 14),
            _ => throw new ArgumentOutOfRangeException(nameof(direction))
        };

        // Absolute port center, given the node's canvas position.
        public static Point PortCenter(ExitDirection direction, double nodeX, double nodeY)
        {
            var local = PortCenter(direction);
            return new Point(nodeX + local.X, nodeY + local.Y);
        }
    }
}
