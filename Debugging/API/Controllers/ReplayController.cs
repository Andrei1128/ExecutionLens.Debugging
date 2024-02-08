using PostMortem.Debugging.APPLICATION.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace PostMortem.Debugging.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReplayController(IReplayService _replayService) : ControllerBase
    {
        [HttpPost]
        public ActionResult Replay(string logId)
        {
            if (Debugger.IsAttached)
            {

                _replayService.Replay(logId);

                return Ok("Success!");
            }
            return BadRequest("Debugger is not attached!");
        }
    }
}