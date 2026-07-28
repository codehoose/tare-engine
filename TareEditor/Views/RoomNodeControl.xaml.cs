using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TareEditor.Models;
using TareEditor.ViewModels;

namespace TareEditor.Views
{
    public partial class RoomNodeControl : UserControl
    {
        private Point? _dragStartMouse;
        private double _dragStartX;
        private double _dragStartY;

        // The ancestor Grid (named "CanvasRoot" in MapCanvasView.xaml) that
        // Phase 6 will apply the zoom/pan RenderTransform to. Measuring mouse
        // positions relative to it - rather than to the window - means every
        // position here is already correct canvas-space, both now (identity)
        // and once that transform is real, with no coordinate math to redo.
        private FrameworkElement? _canvasRoot;

        public RoomNodeControl()
        {
            InitializeComponent();

            PositionPort(NorthPort, ExitDirection.North);
            PositionPort(SouthPort, ExitDirection.South);
            PositionPort(EastPort, ExitDirection.East);
            PositionPort(WestPort, ExitDirection.West);
            PositionPort(UpPort, ExitDirection.Up);
            PositionPort(DownPort, ExitDirection.Down);
        }

        private static void PositionPort(FrameworkElement port, ExitDirection direction)
        {
            var center = RoomNodeGeometry.PortCenter(direction);
            Canvas.SetLeft(port, center.X - port.Width / 2);
            Canvas.SetTop(port, center.Y - port.Height / 2);
        }

        private RoomViewModel? Room => DataContext as RoomViewModel;

        private void Body_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Room == null) return;

            FindOwningMap()?.SelectRoom(Room);
            _canvasRoot = this.FindAncestorByName("CanvasRoot");

            _dragStartMouse = e.GetPosition(_canvasRoot);
            _dragStartX = Room.X;
            _dragStartY = Room.Y;

            Mouse.Capture((IInputElement)sender);
            e.Handled = true;
        }

        private void Body_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dragStartMouse == null || Room == null) return;
            if (e.LeftButton != MouseButtonState.Pressed) return;

            var current = e.GetPosition(_canvasRoot);
            Room.X = _dragStartX + (current.X - _dragStartMouse.Value.X);
            Room.Y = _dragStartY + (current.Y - _dragStartMouse.Value.Y);
        }

        private void Body_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _dragStartMouse = null;
            Mouse.Capture(null);
            e.Handled = true;
        }

        private void Body_LostMouseCapture(object sender, MouseEventArgs e) => _dragStartMouse = null;

        private void Port_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Room == null) return;
            if (sender is not FrameworkElement { Tag: string tag } portElement) return;
            if (!Enum.TryParse<ExitDirection>(tag, out var direction)) return;

            var map = FindOwningMap();
            if (map == null) return;

            _canvasRoot = this.FindAncestorByName("CanvasRoot");
            map.BeginConnectionDrag(Room, direction, e.GetPosition(_canvasRoot));

            Mouse.Capture(portElement);
            e.Handled = true;
        }

        private void Port_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;
            if (FindOwningMap() is not { IsConnectingExit: true } map) return;

            map.UpdateConnectionDrag(e.GetPosition(_canvasRoot));
        }

        private void Port_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var map = FindOwningMap();
            if (map is { IsConnectingExit: true })
            {
                var target = HitTestRoom(e.GetPosition(_canvasRoot));
                map.CompleteConnectionDrag(target);
            }

            Mouse.Capture(null);
            e.Handled = true;
        }

        private void Port_LostMouseCapture(object sender, MouseEventArgs e)
        {
            if (FindOwningMap() is { IsConnectingExit: true } map)
                map.CancelConnectionDrag();
        }

        // Finds whatever room node is visually under a canvas-space point,
        // regardless of mouse capture - used to resolve a connection-drag
        // drop target. Any port (or the body) of the target node counts;
        // the data model has no notion of *which* port received the drop.
        private RoomViewModel? HitTestRoom(Point canvasPosition)
        {
            if (_canvasRoot == null) return null;

            DependencyObject? node = VisualTreeHelper.HitTest(_canvasRoot, canvasPosition)?.VisualHit;
            while (node != null)
            {
                if (node is FrameworkElement { DataContext: RoomViewModel room })
                    return room;

                node = VisualTreeHelper.GetParent(node);
            }

            return null;
        }

        // RoomNodeControl's own DataContext is its RoomViewModel, not the
        // owning MapViewModel, so drag gestures need to walk up to find it.
        private MapViewModel? FindOwningMap()
        {
            DependencyObject? node = this;
            while (node != null)
            {
                if (node is FrameworkElement { DataContext: MapViewModel map })
                    return map;

                node = VisualTreeHelper.GetParent(node);
            }

            return null;
        }
    }
}
