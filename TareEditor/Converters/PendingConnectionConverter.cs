using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using TareEditor.Models;
using TareEditor.ViewModels;
using TareEditor.Views;

namespace TareEditor.Converters
{
    // Rubber-band line shown while dragging a new connection from a port
    // (Phase 5 authoring gesture). Values, in order: IsConnectingExit,
    // PendingSourceRoom, PendingSourceDirection, PendingConnectionEnd.
    public class PendingConnectionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 4) return Geometry.Empty;
            if (values[0] is not bool isConnecting || !isConnecting) return Geometry.Empty;
            if (values[1] is not RoomViewModel source) return Geometry.Empty;
            if (values[2] is not ExitDirection direction) return Geometry.Empty;
            if (values[3] is not Point end) return Geometry.Empty;

            var start = RoomNodeGeometry.PortCenter(direction, source.X, source.Y);

            var figure = new PathFigure { StartPoint = start, IsClosed = false };
            figure.Segments.Add(new LineSegment(end, true));
            return new PathGeometry(new[] { figure });
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
