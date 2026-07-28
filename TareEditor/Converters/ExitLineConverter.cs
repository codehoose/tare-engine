using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using TareEditor.Models;
using TareEditor.ViewModels;
using TareEditor.Views;

namespace TareEditor.Converters
{
    // Computes an exit's connector-line geometry, bound (via MultiBinding) to
    // the source/target rooms' X/Y so it recalculates automatically whenever
    // either endpoint moves. Values, in order: Source.X, Source.Y, Target.X,
    // Target.Y, Source, Target, Direction.
    //
    // The target endpoint is anchored to the *opposite* direction's port on
    // the target room (see ExitDirectionKeys.Opposite) - a rendering
    // convention, since the data model itself has no "entry port" concept.
    public class ExitLineConverter : IMultiValueConverter
    {
        private const double ArrowLength = 10;
        private const double ArrowSpreadDegrees = 28;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 7) return Geometry.Empty;
            if (values.Any(v => v == DependencyProperty.UnsetValue)) return Geometry.Empty;
            if (values[4] is not RoomViewModel source) return Geometry.Empty;
            if (values[5] is not RoomViewModel target) return Geometry.Empty;
            if (values[6] is not ExitDirection direction) return Geometry.Empty;

            var sourcePoint = RoomNodeGeometry.PortCenter(direction, source.X, source.Y);

            return ReferenceEquals(source, target)
                ? BuildSelfLoop(sourcePoint, direction, source.X, source.Y)
                : BuildLine(sourcePoint, RoomNodeGeometry.PortCenter(ExitDirectionKeys.Opposite(direction), target.X, target.Y));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotSupportedException();

        private static Geometry BuildLine(Point from, Point to)
        {
            var geometry = new PathGeometry();
            var figure = new PathFigure { StartPoint = from, IsClosed = false };
            figure.Segments.Add(new LineSegment(to, true));
            geometry.Figures.Add(figure);

            AddArrowhead(geometry, to, to - from);
            return geometry;
        }

        private static Geometry BuildSelfLoop(Point exitPoint, ExitDirection direction, double nodeX, double nodeY)
        {
            var entryPoint = RoomNodeGeometry.PortCenter(ExitDirectionKeys.Opposite(direction), nodeX, nodeY);

            // Bulge the loop out away from the node's edge so it reads as a
            // loop rather than a line cutting back through the room rectangle.
            var bulge = (exitPoint - entryPoint).Length / 2 + 30;
            var away = AwayFromNode(direction);
            var control1 = exitPoint + away * bulge;
            var control2 = entryPoint + away * bulge;

            var geometry = new PathGeometry();
            var figure = new PathFigure { StartPoint = exitPoint, IsClosed = false };
            figure.Segments.Add(new BezierSegment(control1, control2, entryPoint, true));
            geometry.Figures.Add(figure);

            AddArrowhead(geometry, entryPoint, entryPoint - control2);
            return geometry;
        }

        private static Vector AwayFromNode(ExitDirection direction) => direction switch
        {
            ExitDirection.North => new Vector(0, -1),
            ExitDirection.South => new Vector(0, 1),
            ExitDirection.East => new Vector(1, 0),
            ExitDirection.West => new Vector(-1, 0),
            ExitDirection.Up => new Vector(0, -1),
            ExitDirection.Down => new Vector(0, 1),
            _ => new Vector(0, -1)
        };

        private static void AddArrowhead(PathGeometry geometry, Point tip, Vector incomingDirection)
        {
            if (incomingDirection.Length < 0.0001) return;
            incomingDirection.Normalize();

            var back = -incomingDirection * ArrowLength;
            var figure = new PathFigure { StartPoint = tip + Rotate(back, ArrowSpreadDegrees), IsClosed = false };
            figure.Segments.Add(new LineSegment(tip, true));
            figure.Segments.Add(new LineSegment(tip + Rotate(back, -ArrowSpreadDegrees), true));
            geometry.Figures.Add(figure);
        }

        private static Vector Rotate(Vector v, double degrees)
        {
            var radians = degrees * Math.PI / 180.0;
            var cos = Math.Cos(radians);
            var sin = Math.Sin(radians);
            return new Vector(v.X * cos - v.Y * sin, v.X * sin + v.Y * cos);
        }
    }
}
