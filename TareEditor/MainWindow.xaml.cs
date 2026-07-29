using System.Windows;
using Microsoft.Win32;
using TareEditor.ViewModels;

namespace TareEditor
{
    public partial class MainWindow : Window
    {
        public static readonly System.Windows.Input.RoutedCommand NewCommand = new("NewCommand", typeof(MainWindow));
        public static readonly System.Windows.Input.RoutedCommand OpenCommand = new("OpenCommand", typeof(MainWindow));
        public static readonly System.Windows.Input.RoutedCommand SaveCommand = new("SaveCommand", typeof(MainWindow));
        public static readonly System.Windows.Input.RoutedCommand SaveAsCommand = new("SaveAsCommand", typeof(MainWindow));
        public static readonly System.Windows.Input.RoutedCommand ImportCommand = new("ImportCommand", typeof(MainWindow));
        public static readonly System.Windows.Input.RoutedCommand ExportCommand = new("ExportCommand", typeof(MainWindow));
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private MainViewModel? VM => DataContext as MainViewModel;

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            // Wire command bindings for the routed commands used by keyboard shortcuts
            CommandBindings.Add(new System.Windows.Input.CommandBinding(NewCommand, (s, a) => NewMenuItem_Click(s, null)));
            CommandBindings.Add(new System.Windows.Input.CommandBinding(OpenCommand, (s, a) => OpenMenuItem_Click(s, null)));
            CommandBindings.Add(new System.Windows.Input.CommandBinding(SaveCommand, (s, a) => SaveMenuItem_Click(s, null)));
            CommandBindings.Add(new System.Windows.Input.CommandBinding(SaveAsCommand, (s, a) => SaveAsMenuItem_Click(s, null)));
            CommandBindings.Add(new System.Windows.Input.CommandBinding(ImportCommand, (s, a) => ImportMenuItem_Click(s, null)));
            CommandBindings.Add(new System.Windows.Input.CommandBinding(ExportCommand, (s, a) => ExportMenuItem_Click(s, null)));
        }

        private void NewMenuItem_Click(object sender, RoutedEventArgs e)
            => VM?.NewProjectCommand.Execute(null);

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog { Filter = "Tare Editor Project (*.tare-project.json)|*.tare-project.json|All files (*.*)|*.*" };
            if (dlg.ShowDialog(this) == true)
                VM?.OpenProjectCommand.Execute(dlg.FileName);
        }

        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (VM == null) return;
            if (string.IsNullOrEmpty(VM.CurrentProjectPath))
            {
                SaveAsMenuItem_Click(sender, e);
                return;
            }

            VM.SaveProjectCommand.Execute(null);
        }

        private void SaveAsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog { Filter = "Tare Editor Project (*.tare-project.json)|*.tare-project.json", DefaultExt = ".tare-project.json" };
            if (dlg.ShowDialog(this) == true)
                VM?.SaveProjectAsCommand.Execute(dlg.FileName);
        }

        private void ImportMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog { Filter = "Engine JSON (*.json)|*.json|All files (*.*)|*.*" };
            if (dlg.ShowDialog(this) == true)
                VM?.ImportEngineJsonCommand.Execute(dlg.FileName);
        }

        private void ExportMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog { Filter = "Engine JSON (*.json)|*.json", DefaultExt = ".json" };
            if (dlg.ShowDialog(this) == true)
                VM?.ExportEngineJsonCommand.Execute(dlg.FileName);
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
            => Close();

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);

            if (VM == null) return;
            if (!VM.IsDirty) return;

            var result = MessageBox.Show(this,
                "You have unsaved changes. Do you want to save them before exiting?",
                "Unsaved Changes",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
                return;
            }

            if (result == MessageBoxResult.Yes)
            {
                // If no path, prompt Save As
                if (string.IsNullOrEmpty(VM.CurrentProjectPath))
                {
                    SaveAsMenuItem_Click(this, new RoutedEventArgs());
                    // If still no path, user cancelled Save As -> cancel close
                    if (string.IsNullOrEmpty(VM.CurrentProjectPath))
                    {
                        e.Cancel = true;
                        return;
                    }
                }

                VM.SaveProjectCommand.Execute(null);
            }
        }
    }
}
