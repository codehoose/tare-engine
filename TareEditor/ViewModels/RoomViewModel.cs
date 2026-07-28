using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using TareEditor.Models;
using TareEngine.Serialization;

namespace TareEditor.ViewModels
{
    public partial class RoomViewModel : ObservableObject
    {
        // The underlying serialization DTOs this view model edits in place.
        // Room/Layout are what actually gets written out on save/export.
        public SerializedRoom Room { get; }
        internal EditorRoomLayout Layout { get; }

        public ObservableCollection<string> Graphics { get; }

        // Always exactly six entries, one per ExitDirection, created once and
        // never added to/removed from - see ExitViewModel.
        public ObservableCollection<ExitViewModel> Exits { get; } = new();

        [ObservableProperty]
        private bool isSelected;

        // Raised on any change that should mark the containing project dirty.
        // Deliberately excludes IsSelected, which is UI-only and never persisted.
        public event EventHandler? Changed;

        public RoomViewModel(SerializedRoom room, EditorRoomLayout layout)
        {
            Room = room;
            Layout = layout;

            Graphics = new ObservableCollection<string>(room.graphic ?? Array.Empty<string>());
            Graphics.CollectionChanged += (_, _) =>
            {
                Room.graphic = Graphics.ToArray();
                Changed?.Invoke(this, EventArgs.Empty);
            };

            foreach (var direction in ExitDirectionKeys.AllDirections)
            {
                var exit = new ExitViewModel(this, direction);
                exit.PropertyChanged += (_, e) =>
                {
                    if (e.PropertyName != nameof(ExitViewModel.IsSelected))
                        Changed?.Invoke(this, EventArgs.Empty);
                };
                Exits.Add(exit);
            }

            PropertyChanged += (_, e) =>
            {
                if (e.PropertyName != nameof(IsSelected))
                    Changed?.Invoke(this, EventArgs.Empty);
            };
        }

        public ExitViewModel GetExit(ExitDirection direction) => Exits.First(e => e.Direction == direction);

        public string Slug
        {
            get => Room.slug;
            set => SetProperty(Room.slug, value, v => Room.slug = v);
        }

        public string Short
        {
            get => Room.shortname;
            set => SetProperty(Room.shortname, value, v => Room.shortname = v);
        }

        public string Description
        {
            get => Room.description;
            set => SetProperty(Room.description, value, v => Room.description = v);
        }

        public string? GraphicFlag
        {
            get => Room.graphicFlag;
            set => SetProperty(Room.graphicFlag, value, v => Room.graphicFlag = v);
        }

        public double X
        {
            get => Layout.X;
            set => SetProperty(Layout.X, value, v => Layout.X = v);
        }

        public double Y
        {
            get => Layout.Y;
            set => SetProperty(Layout.Y, value, v => Layout.Y = v);
        }

        // Rebuilds Room.exits/Room.blockers from the current Exits state.
        // Called before the room is serialized (see MapViewModel.ToProject) -
        // the Exits collection, not the dictionaries, is authoritative while editing.
        public void SyncExitsToModel()
        {
            var exits = new Dictionary<string, string>();
            var blockers = new Dictionary<string, string>();

            foreach (var exit in Exits)
            {
                if (exit.Target == null) continue;

                var key = ExitDirectionKeys.ToKey(exit.Direction);
                exits[key] = exit.Target.Slug;

                if (!string.IsNullOrWhiteSpace(exit.BlockedByFlag))
                    blockers[key] = exit.BlockedByFlag!;
            }

            Room.exits = exits;
            Room.blockers = blockers.Count > 0 ? blockers : null;
        }

        public override string ToString() => Slug;
    }
}
