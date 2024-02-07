using Common.DOMAIN.Utilities;
using Debugging.APPLICATION.Contracts;
using Debugging.DOMAIN.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Debugging.APPLICATION.Implementations;

[ApiController]
[Route("[controller]")]
public class ReplayController(IReflectionService _reflectionService) : ControllerBase
{
    [HttpPost]
    public IActionResult Replay(string logId)
    {
        string serializedLog = LogSerializer.Read("C:\\Users\\Andrei\\source\\repos\\PostMortem\\PostMortem\\logs\\Order-2024.01.25-21.46.19.4845039");

        var log = LogSerializer.Deserialize(serializedLog);

        var classInstance = _reflectionService.CreateInstance(log.ToMock());

        var type = classInstance.GetType();

        var method = type.GetMethod(log.Entry.Method)
            ?? throw new Exception($"Method '{log.Entry.Method}' not found on Type '{type}'!");

        if (log.Entry.Input is not null)
        {
            var normalizedParameters = method.NormalizeParametersType(method, log.Entry.Input);
            method.Invoke(classInstance, normalizedParameters);
        }
        else
        {
            method.Invoke(classInstance, null);
        }

        return new OkResult();
    }
}
