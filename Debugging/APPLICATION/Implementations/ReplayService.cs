using PostMortem.Common.DOMAIN.Models;
using PostMortem.Common.DOMAIN.Utilities;
using PostMortem.Debugging.APPLICATION.Contracts;
using PostMortem.Debugging.DOMAIN.Extensions;
using System.Reflection;

namespace PostMortem.Debugging.APPLICATION.Implementations;

internal class ReplayService(IReflectionService _reflectionService) : IReplayService
{
    public void Replay(string logId)
    {
        string serializedLog = LogSerializer.Read("C:\\Users\\Andrei\\source\\repos\\PostMortemTests\\PostMortemTests\\logs\\Order-2024.02.10-15.44.31.5085831");

        MethodLog log = LogSerializer.Deserialize(serializedLog);

        object classInstance = _reflectionService.CreateInstance(log.ToMock());

        Type type = classInstance.GetType();

        MethodInfo method = type.GetMethod(log.Entry.Method)
            ?? throw new Exception($"Method '{log.Entry.Method}' not found on Type '{log.Entry.Class}'!");

            if (log.Entry.Input is not null)
            {
                object[] normalizedParameters = method.NormalizeParametersType(log.Entry.Input);
                method.Invoke(classInstance, normalizedParameters);
            }
            else
            {
                method.Invoke(classInstance, null);
            }
        }
}
