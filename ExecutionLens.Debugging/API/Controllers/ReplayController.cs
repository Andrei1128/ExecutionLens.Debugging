using ExecutionLens.Debugging.APPLICATION.Contracts;
using ExecutionLens.Debugging.DOMAIN.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ExecutionLens.Debugging.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ReplayController(IReplayService _replayService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Replay(string logId)
    {
        if (!Debugger.IsAttached)
        {
            return BadRequest("Debugger is not attached!");
        }

        ResultStatus status = await _replayService.Replay(logId);

        return status switch
        {
            ResultStatus.Success => Ok("Successfully replayed!"),
            ResultStatus.NotFound => NotFound($"Log with id `{logId}` not found!"),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}