using Newtonsoft.Json;
using TareEditor.Models;
using TareEngine.Serialization;

// System.IO isn't part of this TFM's implicit global usings (unlike plain
// net10.0, which is why TareEngine's serializers don't need it explicitly).
using System.IO;

namespace TareEditor.Services
{
    // Two distinct file formats live behind this service:
    //  - the editor-native project file (EditorProject: game data + layout),
    //    which is what Load/Save operate on;
    //  - the engine's own combined game-data JSON (thedata.json's shape),
    //    which Import reads from and Export projects down to, dropping layout.
    public static class ProjectIoService
    {
        private static readonly JsonSerializerSettings JsonSettings = new()
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        };

        private const int LayoutColumns = 4;
        private const double LayoutSpacingX = 220;
        private const double LayoutSpacingY = 160;

        public static EditorProject LoadProject(string path)
        {
            var json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<EditorProject>(json)
                ?? throw new InvalidDataException($"'{path}' did not contain a valid TareEditor project.");
        }

        public static void SaveProject(EditorProject project, string path)
        {
            var json = JsonConvert.SerializeObject(project, JsonSettings);
            File.WriteAllText(path, json);
        }

        public static EditorProject ImportEngineJson(string path)
        {
            var json = File.ReadAllText(path);
            var gameData = JsonConvert.DeserializeObject<SerializedGameData>(json)
                ?? throw new InvalidDataException($"'{path}' did not contain valid game data.");

            gameData.rooms ??= new SerializedRoomCollection();
            gameData.rooms.rooms ??= Array.Empty<SerializedRoom>();
            gameData.flags ??= Array.Empty<SerializedFlag>();
            gameData.items ??= Array.Empty<SerializedItem>();
            gameData.actions ??= Array.Empty<SerializedAction>();

            var project = new EditorProject { GameData = gameData };
            SynthesizeMissingLayout(project);
            return project;
        }

        public static void ExportEngineJson(EditorProject project, string path)
        {
            var json = JsonConvert.SerializeObject(project.GameData, JsonSettings);
            File.WriteAllText(path, json);
        }

        // Grid-place any room that doesn't already have a layout entry, so
        // importing engine JSON that was never touched by the editor still
        // produces a usable (if untidy) starting canvas.
        private static void SynthesizeMissingLayout(EditorProject project)
        {
            var placed = new HashSet<string>(project.Layout.Select(l => l.Slug));
            var rooms = project.GameData.rooms.rooms;

            for (var i = 0; i < rooms.Length; i++)
            {
                var slug = rooms[i].slug;
                if (!placed.Add(slug)) continue;

                project.Layout.Add(new EditorRoomLayout
                {
                    Slug = slug,
                    X = (i % LayoutColumns) * LayoutSpacingX,
                    Y = (i / LayoutColumns) * LayoutSpacingY
                });
            }
        }
    }
}
