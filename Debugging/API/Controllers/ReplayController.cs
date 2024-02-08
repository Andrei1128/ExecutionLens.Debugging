using PostMortem.Debugging.APPLICATION.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace PostMortem.Debugging.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ReplayController(IReplayService _replayService) : ControllerBase
{
    [HttpPost]
    public void Replay(string logId) => _replayService.Replay(logId);
}
