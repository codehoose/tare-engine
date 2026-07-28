using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TareEditor.Models;
using TareEditor.Services;

namespace TareEditor.ViewModels
{
    // Owns the currently open project and its dirty/path state. Deliberately
    // takes explicit file paths rather than opening dialogs itself, so it stays
    // UI-free; Phase 7 wires actual Open/SaveFileDialog calls that feed paths
    // into these commands.
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private MapViewModel currentMap;

        [ObservableProperty]
        private string? currentProjectPath;

        [ObservableProperty]
        private bool isDirty;

        public MainViewModel()
        {
            currentMap = MapViewModel.FromProject(new EditorProject());
            currentMap.Changed += OnMapChanged;
        }

        partial void OnCurrentMapChanging(MapViewModel oldValue, MapViewModel newValue)
            => oldValue.Changed -= OnMapChanged;

        partial void OnCurrentMapChanged(MapViewModel oldValue, MapViewModel newValue)
            => newValue.Changed += OnMapChanged;

        private void OnMapChanged(object? sender, EventArgs e) => IsDirty = true;

        [RelayCommand]
        private void NewProject()
        {
            CurrentMap = MapViewModel.FromProject(new EditorProject());
            CurrentProjectPath = null;
            IsDirty = false;
        }

        [RelayCommand]
        private void OpenProject(string path)
        {
            var project = ProjectIoService.LoadProject(path);
            CurrentMap = MapViewModel.FromProject(project);
            CurrentProjectPath = path;
            IsDirty = false;
        }

        [RelayCommand]
        private void SaveProject()
        {
            if (CurrentProjectPath == null)
                throw new InvalidOperationException("No project path set; use SaveProjectAs.");

            ProjectIoService.SaveProject(CurrentMap.ToProject(), CurrentProjectPath);
            IsDirty = false;
        }

        [RelayCommand]
        private void SaveProjectAs(string path)
        {
            ProjectIoService.SaveProject(CurrentMap.ToProject(), path);
            CurrentProjectPath = path;
            IsDirty = false;
        }

        [RelayCommand]
        private void ImportEngineJson(string path)
        {
            var project = ProjectIoService.ImportEngineJson(path);
            CurrentMap = MapViewModel.FromProject(project);
            CurrentProjectPath = null; // imported file isn't the editor-native project file
            IsDirty = true; // synthesized layout is a pending change worth saving
        }

        [RelayCommand]
        private void ExportEngineJson(string path)
            => ProjectIoService.ExportEngineJson(CurrentMap.ToProject(), path);
    }
}
