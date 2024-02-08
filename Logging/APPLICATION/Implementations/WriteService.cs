using PostMortem.Common.DOMAIN.Utilities;
using PostMortem.Logging.APPLICATION.Contracts;
using PostMortem.Common.DOMAIN.Models;

namespace PostMortem.Logging.APPLICATION.Implementations;

internal class WriteService : IWriteService
{
    public void Write(MethodLog log)
    {
        string serializedLog = LogSerializer.Serialize(log);

        string filePath = "..\\PostMortemTests\\logs";

        if (!Directory.Exists(filePath))
            Directory.CreateDirectory(filePath);

        File.WriteAllText($"{filePath}\\{log.Entry.Method}-{DateTime.Now:yyyy.MM.dd-HH.mm.ss.fffffff}", serializedLog);
    }
}

