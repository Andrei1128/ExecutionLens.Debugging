using Common.DOMAIN.Utilities;
using Debugging.APPLICATION.Contracts;
using Debugging.APPLICATION.Helpers;
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

        var classInstance = _reflectionService.CreateInstance(log.ToClassMock());

        var type = classInstance.GetType();

        var methodInfo = type.GetMethod(log.Entry.Method)
            ?? throw new Exception("Could not find method info!");

        ReflectionService.NormalizeInputs(methodInfo, log.Entry.Input);
        methodInfo.Invoke(classInstance, log.Entry.Input);
        return new OkResult();
    }
}
