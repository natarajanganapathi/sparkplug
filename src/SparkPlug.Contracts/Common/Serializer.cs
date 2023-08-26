namespace SparkPlug.Contracts;

public class NewtonsoftSerializer : ISerializer
{
    public string Serialize<T>(T obj) => JsonConvert.SerializeObject(obj);
    public T? Deserialize<T>(string json) => JsonConvert.DeserializeObject<T>(json);
}
