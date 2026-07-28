using TareEngine.Serialization;

namespace TareEditor.Models
{
    // The file the editor actually opens/saves while working: the engine's
    // game data verbatim (flags/items/actions preserved untouched) plus
    // editor-only layout metadata that has no home in the engine's schema.
    public class EditorProject
    {
        public SerializedGameData GameData { get; set; } = new SerializedGameData
        {
            rooms = new SerializedRoomCollection { rooms = Array.Empty<SerializedRoom>() },
            flags = Array.Empty<SerializedFlag>(),
            items = Array.Empty<SerializedItem>(),
            actions = Array.Empty<SerializedAction>()
        };

        public List<EditorRoomLayout> Layout { get; set; } = new();
    }
}
