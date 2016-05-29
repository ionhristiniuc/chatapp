namespace DataServiceClient.Services.Utils
{
    public interface ISerializer
    {
        string Serialize(object obj);
        T Deserialize<T>(string str);
    }
}