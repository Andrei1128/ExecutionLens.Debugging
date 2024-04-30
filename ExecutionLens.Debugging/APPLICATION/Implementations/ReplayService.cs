using ExecutionLens.Debugging.APPLICATION.Contracts;
using ExecutionLens.Debugging.DOMAIN.Enums;
using ExecutionLens.Debugging.DOMAIN.Extensions;
using ExecutionLens.Debugging.DOMAIN.Models;
using Newtonsoft.Json;
using System.Reflection;

namespace ExecutionLens.Debugging.APPLICATION.Implementations;

internal class ReplayService(IReflectionService _reflectionService, ILogRepository _logRepository) : IReplayService
{
    public async Task<ResultStatus> Replay(string logId)
    {
        try
        {
            MethodLog? log = await _logRepository.Get(logId);

            if (log is null)
            {
                return ResultStatus.NotFound;
            }

            object classInstance = _reflectionService.CreateInstance(log.ToMock());

            Type type = classInstance.GetType();

            MethodInfo method = type.GetMethod(log.Method)
                ?? throw new Exception($"Method `{log.Method}` not found on Type `{log.Class}`!");

            if (log.Input is not null)
            {
                object[] normalizedParameters = method.NormalizeParametersType(log.Input);

                try
                {
                    method.Invoke(classInstance, normalizedParameters);
                }
                catch(Exception){}
            }
            else
            {
                try
                {
                    method.Invoke(classInstance, null);
                }
                catch (Exception) { }
            }
        }
        catch (Exception ex)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fffffff}.json");
            File.WriteAllText(path, JsonConvert.SerializeObject(ex));

            return ResultStatus.Exception;
        }

        return ResultStatus.Success;
    }
}
