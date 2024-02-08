using PostMortem.Common.DOMAIN.Utilities;
using PostMortem.Debugging.APPLICATION.Contracts;
using PostMortem.Debugging.DOMAIN.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using PostMortem.Common.DOMAIN.Models;

namespace PostMortem.Debugging.APPLICATION.Implementations;

[ApiController]
[Route("[controller]")]
public class ReplayController(IReflectionService _reflectionService) : ControllerBase
{
    [HttpPost]
    public IActionResult Replay(string logId)
    {
        string serializedLog = LogSerializer.Read("C:\\Users\\Andrei\\source\\repos\\PostMortem\\PostMortem\\logs\\Order-2024.01.25-21.46.19.4845039");

        MethodLog log = LogSerializer.Deserialize(serializedLog);

        object classInstance = _reflectionService.CreateInstance(log.ToMock());

        Type type = classInstance.GetType();

        MethodInfo method = type.GetMethod(log.Entry.Method)
            ?? throw new Exception($"Method '{log.Entry.Method}' not found on Type '{log.Entry.Class}'!");

        if (log.Entry.Input is not null)
        {
            object[] normalizedParameters = method.NormalizeParametersType(method, log.Entry.Input);
            method.Invoke(classInstance, normalizedParameters);
        }
        else
        {
            method.Invoke(classInstance, null);
        }

        return new OkResult();
    }
}
