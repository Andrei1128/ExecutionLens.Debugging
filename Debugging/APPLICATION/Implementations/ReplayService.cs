using Common.PERSISTANCE.Contracts;
using PostMortem.Common.DOMAIN.Models;
using PostMortem.Debugging.APPLICATION.Contracts;
using PostMortem.Debugging.DOMAIN.Extensions;
using System.Reflection;

namespace PostMortem.Debugging.APPLICATION.Implementations;

internal class ReplayService(IReflectionService _reflectionService, ILogRepository _logRepository) : IReplayService
{
    public async Task Replay(string logId)
    {
        MethodLog log = await _logRepository.Get(logId);

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
