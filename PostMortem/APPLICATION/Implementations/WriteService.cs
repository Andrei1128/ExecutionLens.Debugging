using Common.DOMAIN.Utilities;
using Logging.APPLICATION.Contracts;
using PostMortem.SHARED.DOMAIN.Models;

namespace Logging.APPLICATION.Implementations;
internal class WriteService : IWriteService
{
    public void Write(MethodLog log)
    {
        string serializedLog = LogSerializer.Serialize(log);

        string filePath = "..\\PostMortem\\logs";

        if (!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);

        File.WriteAllText($"{filePath}\\{log.Entry.Method}-{DateTime.Now:yyyy.MM.dd-HH.mm.ss.fffffff}", serializedLog);
    }
}

