namespace TareEngine.Serialization
{
    public interface IGameDataSerializer
    {
        SerializedGameData? GetData(string jsonFile);
        SerializedGameData? GetDataFullPath(string jsonFile);
    }
}