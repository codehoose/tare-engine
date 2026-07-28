using CommunityToolkit.Mvvm.ComponentModel;
using TareEditor.Models;

namespace TareEditor.ViewModels
{
    // One of a room's fixed six compass/vertical ports. Always exists (even
    // when unconnected) so the node UI can render all six ports consistently;
    // Target == null means that port has no exit.
    public partial class ExitViewModel : ObservableObject
    {
        public RoomViewModel Source { get; }
        public ExitDirection Direction { get; }

        [ObservableProperty]
        private RoomViewModel? target;

        [ObservableProperty]
        private string? blockedByFlag;

        // UI-only selection state for the connection-line canvas; deliberately
        // excluded from RoomViewModel.Changed (see that class's constructor).
        [ObservableProperty]
        private bool isSelected;

        public ExitViewModel(RoomViewModel source, ExitDirection direction)
        {
            Source = source;
            Direction = direction;
        }
    }
}
