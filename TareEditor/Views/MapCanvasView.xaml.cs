using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
    }
}
