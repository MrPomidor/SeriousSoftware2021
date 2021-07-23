using Newtonsoft.Json;

namespace SeriousBusiness.Utils
{
    public class JsonDeserializer : IJsonDeserializer
    {
        public T Deserialize<T>(string jsonStr)
        {
            return JsonConvert.DeserializeObject<T>(jsonStr);
        }
    }
}
