using Newtonsoft.Json;
using TareEngine.Serialization;
using UnityEngine;

public class GameDataProvider : MonoBehaviour, IGameDataSerializer
{
    [SerializeField] private TextAsset _dataFile;

    public SerializedGameData GetData(string jsonFile) => JsonConvert.DeserializeObject<SerializedGameData>(_dataFile.text);

    public SerializedGameData GetDataFullPath(string jsonFile) => GetData(jsonFile);
}
