using Common.DOMAIN.Utilities;
using Debugging.APPLICATION.Contracts;

namespace Debugging.APPLICATION.Implementations;

internal class ReplayService(IReflectionService _reflectionService) : IReplayService
{
    public void Replay(string logId)
    {
        string serializedLog = File.ReadAllText("C:\\Users\\Andrei\\Facultate\\C#\\RepeatableExecutionsTests\\Logging\\logs\\GetWeatherEndpoint-202311232026501458082");

        var log = LogSerializer.Deserialize(serializedLog);

        var classInstance = _reflectionService.CreateInstance(log);

        var type = _reflectionService.GetType(log.Entry.Class);

        var methodInfo = type.GetMethod(log.Entry.Method)
            ?? throw new Exception("Could not find method info!");

        _reflectionService.NormalizeInputs(methodInfo, log.Entry.Input);
        methodInfo.Invoke(classInstance, log.Entry.Input);
    }
}
