namespace CommunicationLibrary.Util.Serialization
{
    public interface ISerializer
    {
        string Serialize(object obj);
        T Deserialize<T>(string str);
        object DeserializeObject(string str);
    }
}
