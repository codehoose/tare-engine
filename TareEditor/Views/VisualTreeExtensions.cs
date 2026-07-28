using System.Windows;
using System.Windows.Media;

namespace TareEditor.Views
{
    internal static class VisualTreeExtensions
    {
        public static FrameworkElement? FindAncestorByName(this DependencyObject start, string name)
        {
            DependencyObject? node = start;
            while (node != null)
            {
                if (node is FrameworkElement element && element.Name == name)
                    return element;

                node = VisualTreeHelper.GetParent(node);
            }

            return null;
        }
    }
}
