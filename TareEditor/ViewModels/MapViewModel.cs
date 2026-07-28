using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TareEditor.Models;
using TareEngine.Serialization;

namespace TareEditor.ViewModels
{
    public partial class MapViewModel : ObservableObject
    {
        // The originally-loaded game data, mutated in place (rooms only) on
        // ToProject() so flags/items/actions survive untouched - see
        // implementation-plan.md Phase 2/3 notes on preserving the whole file.
        private readonly SerializedGameData _gameData;

        public ObservableCollection<RoomViewModel> Rooms { get; } = new();

        // Flattened view of every room's Exits, kept in sync with Rooms in
        // OnRoomsCollectionChanged - this is what the connection-line canvas
        // layer binds to.
        public ObservableCollection<ExitViewModel> AllExits { get; } = new();

        [ObservableProperty]
        private RoomViewModel? startRoom;

        [ObservableProperty]
        private double zoom = 1.0;

        [ObservableProperty]
        private double panX;

        [ObservableProperty]
        private double panY;

        // Live drag-to-connect state (Phase 5 authoring gesture). UI-only,
        // never persisted, so it deliberately doesn't touch Changed.
        [ObservableProperty]
        private bool isConnectingExit;

        [ObservableProperty]
        private RoomViewModel? pendingSourceRoom;

        [ObservableProperty]
        private ExitDirection? pendingSourceDirection;

        [ObservableProperty]
        private Point pendingConnectionEnd;

        // Raised on any change that should mark the containing project dirty.
        public event EventHandler? Changed;

        private MapViewModel(SerializedGameData gameData)
        {
            _gameData = gameData;
            Rooms.CollectionChanged += OnRoomsCollectionChanged;
        }

        public static MapViewModel FromProject(EditorProject project)
        {
            var map = new MapViewModel(project.GameData);
            var layoutBySlug = project.Layout.ToDictionary(l => l.Slug);
            var roomsBySlug = new Dictionary<string, RoomViewModel>();

            foreach (var room in project.GameData.rooms.rooms)
            {
                var layout = layoutBySlug.TryGetValue(room.slug, out var l)
                    ? l
                    : new EditorRoomLayout { Slug = room.slug };

                var vm = new RoomViewModel(room, layout);
                roomsBySlug[room.slug] = vm;
                map.Rooms.Add(vm);
            }

            // Second pass: wire exit targets now that every room exists, since
            // an exit can point forward to a room later in the JSON array (or
            // to itself, for maze rooms).
            foreach (var room in map.Rooms)
            {
                if (room.Room.exits == null) continue;

                foreach (var (key, targetSlug) in room.Room.exits)
                {
                    if (!ExitDirectionKeys.TryParse(key, out var direction)) continue;
                    if (!roomsBySlug.TryGetValue(targetSlug, out var target)) continue; // dangling; Phase 8 validator flags this

                    var exit = room.GetExit(direction);
                    exit.Target = target;

                    if (room.Room.blockers != null && room.Room.blockers.TryGetValue(key, out var flag))
                        exit.BlockedByFlag = flag;
                }
            }

            if (!string.IsNullOrEmpty(project.GameData.rooms.startRoom)
                && roomsBySlug.TryGetValue(project.GameData.rooms.startRoom, out var start))
            {
                map.StartRoom = start;
            }

            return map;
        }

        public EditorProject ToProject()
        {
            foreach (var room in Rooms)
                room.SyncExitsToModel();

            _gameData.rooms.rooms = Rooms.Select(r => r.Room).ToArray();
            _gameData.rooms.startRoom = StartRoom?.Slug ?? string.Empty;

            return new EditorProject
            {
                GameData = _gameData,
                Layout = Rooms.Select(r => r.Layout).ToList()
            };
        }

        [RelayCommand]
        private void AddRoom()
        {
            var slug = GenerateUniqueSlug();
            var room = new SerializedRoom
            {
                slug = slug,
                shortname = "New room",
                description = string.Empty,
                exits = new Dictionary<string, string>()
            };
            var layout = new EditorRoomLayout { Slug = slug, X = 40 * Rooms.Count, Y = 40 * Rooms.Count };

            Rooms.Add(new RoomViewModel(room, layout));
        }

        [RelayCommand]
        private void RemoveRoom(RoomViewModel room)
        {
            foreach (var other in Rooms)
            {
                if (other == room) continue;
                foreach (var exit in other.Exits.Where(e => e.Target == room))
                {
                    exit.Target = null;
                    exit.BlockedByFlag = null;
                }
            }

            if (StartRoom == room) StartRoom = null;
            Rooms.Remove(room);
        }

        [RelayCommand]
        private void ConnectExit(ExitConnectionRequest request)
            => request.Source.GetExit(request.Direction).Target = request.Target;

        [RelayCommand]
        private void DisconnectExit(ExitViewModel exit)
        {
            exit.Target = null;
            exit.BlockedByFlag = null;
        }

        public void SelectRoom(RoomViewModel? room)
        {
            foreach (var r in Rooms) r.IsSelected = ReferenceEquals(r, room);
            foreach (var e in AllExits) e.IsSelected = false;
        }

        public void SelectExit(ExitViewModel? exit)
        {
            foreach (var r in Rooms) r.IsSelected = false;
            foreach (var e in AllExits) e.IsSelected = ReferenceEquals(e, exit);
        }

        public void ClearSelection()
        {
            foreach (var r in Rooms) r.IsSelected = false;
            foreach (var e in AllExits) e.IsSelected = false;
        }

        // Drag-to-connect gesture: a port's MouseLeftButtonDown begins it, its
        // MouseMove updates the rubber-band endpoint, and its MouseUp either
        // completes (if dropped on a room) or cancels (dropped on empty canvas)
        // - see RoomNodeControl's Port_* handlers.
        public void BeginConnectionDrag(RoomViewModel source, ExitDirection direction, Point canvasPosition)
        {
            PendingSourceRoom = source;
            PendingSourceDirection = direction;
            PendingConnectionEnd = canvasPosition;
            IsConnectingExit = true;
        }

        public void UpdateConnectionDrag(Point canvasPosition) => PendingConnectionEnd = canvasPosition;

        public void CompleteConnectionDrag(RoomViewModel? targetRoom)
        {
            if (IsConnectingExit && PendingSourceRoom != null && PendingSourceDirection.HasValue && targetRoom != null)
                PendingSourceRoom.GetExit(PendingSourceDirection.Value).Target = targetRoom;

            CancelConnectionDrag();
        }

        public void CancelConnectionDrag()
        {
            PendingSourceRoom = null;
            PendingSourceDirection = null;
            IsConnectingExit = false;
        }

        private string GenerateUniqueSlug()
        {
            var existing = new HashSet<string>(Rooms.Select(r => r.Slug));
            var i = existing.Count + 1;
            string candidate;
            do
            {
                candidate = $"new-room-{i}";
                i++;
            } while (existing.Contains(candidate));

            return candidate;
        }

        private void OnRoomsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (RoomViewModel room in e.OldItems)
                {
                    room.Changed -= OnRoomChanged;
                    foreach (var exit in room.Exits)
                        AllExits.Remove(exit);
                }
            }

            if (e.NewItems != null)
            {
                foreach (RoomViewModel room in e.NewItems)
                {
                    room.Changed += OnRoomChanged;
                    foreach (var exit in room.Exits)
                        AllExits.Add(exit);
                }
            }

            Changed?.Invoke(this, EventArgs.Empty);
        }

        private void OnRoomChanged(object? sender, EventArgs e) => Changed?.Invoke(this, EventArgs.Empty);
    }

    public record ExitConnectionRequest(RoomViewModel Source, ExitDirection Direction, RoomViewModel? Target);
}
