using Newtonsoft.Json;
using PostMortem.LOGGING.APPLICATION.Contracts;
using PostMortem.SHARED.DOMAIN.Models;

namespace PostMortem.LOGGING.APPLICATION.Implementations;
internal class WriteService : IWriteService
{
    public void Write(MethodLog log)
    {
        var jsonSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        };

        string serializedLog = JsonConvert.SerializeObject(log, jsonSettings);

        string filePath = "..\\PostMortem\\logs";

        if (!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);

        File.WriteAllText($"{filePath}\\{log.Entry.Method}-{DateTime.Now:yyyy.MM.dd-HH.mm.ss.fffffff}", serializedLog);
    }
}

