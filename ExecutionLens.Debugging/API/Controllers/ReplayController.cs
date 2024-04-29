using ExecutionLens.Debugging.APPLICATION.Contracts;
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
        if (Debugger.IsAttached)
        {
            await _replayService.Replay(logId);

            return Ok("Success!");
        }
        return BadRequest("Debugger is not attached!");
    }
}