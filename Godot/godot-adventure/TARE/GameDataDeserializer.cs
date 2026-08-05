using System.IO;
using Newtonsoft.Json;
using TareEngine.Serialization;

namespace GodotAdventure.TARE;

public class GameDataDeserializer : IGameDataSerializer
{
    public SerializedGameData GetData(string jsonFile)
    {
        var path = Path.Combine("./thedata", jsonFile);
        if (File.Exists(path))
        {
            return JsonConvert.DeserializeObject<SerializedGameData>(File.ReadAllText(path));
        }

        return null;
    }

    public SerializedGameData GetDataFullPath(string jsonFile) => GetData(jsonFile);
}