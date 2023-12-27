using Common.DOMAIN.Utilities;
using Debugging.APPLICATION.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Debugging.APPLICATION.Implementations;

[ApiController]
[Route("[controller]")]
public class ReplayController(IReflectionService _reflectionService) : ControllerBase
{
    [HttpPost]
    public IActionResult Replay(string logId)
    {
        string serializedLog = LogSerializer.Read("C:\\Users\\Andrei\\Facultate\\C#\\PostMortemTests\\PostMortem\\logs\\Order-2023.12.27-15.30.44.8500139");

        var log = LogSerializer.Deserialize(serializedLog);

        var classInstance = _reflectionService.CreateInstance(log);

        var type = _reflectionService.GetType(log.Entry.Class);

        var methodInfo = type.GetMethod(log.Entry.Method)
            ?? throw new Exception("Could not find method info!");

        _reflectionService.NormalizeInputs(methodInfo, log.Entry.Input);
        methodInfo.Invoke(classInstance, log.Entry.Input);
        return new OkResult();
    }
}
