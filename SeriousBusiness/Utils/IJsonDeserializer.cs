namespace SeriousBusiness.Utils
{
    public interface IJsonDeserializer
    {
        T Deserialize<T>(string jsonStr);
    }
}
