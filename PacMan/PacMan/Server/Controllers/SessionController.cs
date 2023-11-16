using Microsoft.AspNetCore.Mvc;
using PacMan.Shared;
using PacMan.Shared.Models;

namespace PacMan.Server.Controllers
{
    [ApiController]
    public class SessionController : ControllerBase
    {
        public SessionController()
        {
        }

        [HttpPost("sessions")]
        public async Task<IActionResult> Create()
        {
            var storage = Storage.GetInstance();
            var sessionId = storage.CreateSession();
            return Ok(sessionId);
        }

        [HttpGet("sessions")]
        public async Task<ActionResult<List<SessionStateBaseModel>>> GetSessions()
        {
            var storage = Storage.GetInstance();
            var sessions = storage.GetSessionList();
            return Ok(sessions);
        }
    }
}