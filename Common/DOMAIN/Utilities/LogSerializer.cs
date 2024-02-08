using Newtonsoft.Json;
using PostMortem.Common.DOMAIN.Models;

namespace PostMortem.Common.DOMAIN.Utilities;

public class LogSerializer
{
    private static readonly JsonSerializerSettings JsonSettings = new() { TypeNameHandling = TypeNameHandling.All };
    public static string Serialize(MethodLog log) => JsonConvert.SerializeObject(log, JsonSettings);
    public static string Read(string path) => File.ReadAllText(path);
    public static MethodLog Deserialize(string serializedLog) => JsonConvert.DeserializeObject<MethodLog>(serializedLog, JsonSettings)
        ?? throw new Exception("Could not deserialize!");
}
