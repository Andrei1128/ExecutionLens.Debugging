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
        string serializedLog = LogSerializer.Read("C:\\Users\\Andrei\\source\\repos\\PostMortem\\PostMortem\\logs\\Order-2024.01.24-10.21.43.2130045");

        var log = LogSerializer.Deserialize(serializedLog);

        var classInstance = _reflectionService.CreateInstance(log.ToClassMock());

        var type = _reflectionService.GetType(log.Entry.Class);

        var methodInfo = type.GetMethod(log.Entry.Method)
            ?? throw new Exception("Could not find method info!");

        _reflectionService.NormalizeInputs(methodInfo, log.Entry.Input);
        methodInfo.Invoke(classInstance, log.Entry.Input);
        return new OkResult();
    }
}
