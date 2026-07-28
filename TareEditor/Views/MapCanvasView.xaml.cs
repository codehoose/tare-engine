using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TareEditor.ViewModels;

namespace TareEditor.Views
{
    public partial class MapCanvasView : UserControl
    {
        public MapCanvasView()
        {
            InitializeComponent();
        }

        private void ExitHitArea_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is not MapViewModel map) return;
            if (((FrameworkElement)sender).DataContext is not ExitViewModel exit) return;

            map.SelectExit(exit);
            e.Handled = true;
        }

        private void RemoveExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not MapViewModel map) return;
            if (((FrameworkElement)sender).DataContext is not ExitViewModel exit) return;

            map.DisconnectExitCommand.Execute(exit);
        }

        private void CanvasRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            (DataContext as MapViewModel)?.ClearSelection();
        }

        // Zoom around cursor and pan handling
        private bool _isPanning;
        private Point _panStartMouse;
        private double _panStartX;
        private double _panStartY;

        private void CanvasRoot_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (DataContext is not MapViewModel map) return;

            const double minZoom = 0.25;
            const double maxZoom = 3.0;
            var oldZoom = map.Zoom;
            var factor = e.Delta > 0 ? 1.1 : 1.0 / 1.1;
            var newZoom = Math.Max(minZoom, Math.Min(maxZoom, oldZoom * factor));

            // Cursor position relative to the canvas root (in screen/transformed coordinates)
            var cursor = e.GetPosition(CanvasRoot);

            // Compute the content-space point under the cursor before the zoom
            var tg = Content.RenderTransform as TransformGroup;
            var matrix = tg?.Value ?? Matrix.Identity;

            if (matrix.HasInverse)
            {
                var inv = matrix;
                inv.Invert();
                var contentPoint = inv.Transform(cursor);

                // Apply new zoom
                map.Zoom = newZoom;

                // Recompute pan so the contentPoint maps back to the same cursor location
                map.PanX = cursor.X - newZoom * contentPoint.X;
                map.PanY = cursor.Y - newZoom * contentPoint.Y;
            }
            else
            {
                map.Zoom = newZoom;
            }

            e.Handled = true;
        }

        private void CanvasRoot_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is not MapViewModel map) return;

            // Start panning on middle-button or space+left-button
            if (e.MiddleButton == MouseButtonState.Pressed || (e.LeftButton == MouseButtonState.Pressed && Keyboard.IsKeyDown(Key.Space)))
            {
                _isPanning = true;
                _panStartMouse = e.GetPosition(CanvasRoot);
                _panStartX = map.PanX;
                _panStartY = map.PanY;
                CanvasRoot.CaptureMouse();
                e.Handled = true;
            }
        }

        private void CanvasRoot_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isPanning) return;
            if (DataContext is not MapViewModel map) return;

            var current = e.GetPosition(CanvasRoot);
            var delta = current - _panStartMouse;
            map.PanX = _panStartX + delta.X;
            map.PanY = _panStartY + delta.Y;
            e.Handled = true;
        }

        private void CanvasRoot_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!_isPanning) return;
            _isPanning = false;
            CanvasRoot.ReleaseMouseCapture();
            e.Handled = true;
        }
    }
}
