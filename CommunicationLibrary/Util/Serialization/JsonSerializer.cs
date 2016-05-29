using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace CommunicationLibrary.Util.Serialization
{
    public class JsonSerializer : ISerializer
    {
        public string Serialize(object obj) 
        {
            //var jsSerializer = new JavaScriptSerializer();
            return JsonConvert.SerializeObject(obj);
        }

        public T Deserialize<T>(string str)
        {
            //var jsSerializer = new JsonSerializer();
            return JsonConvert.DeserializeObject<T>(str);
        }

        public object DeserializeObject(string str)
        {
            //var jsSerializer = new JsonSerializer();
            return JsonConvert.DeserializeObject(str);
        }
    }
}
