using Microsoft.AspNetCore.Mvc;
using PacMan.Shared;

namespace PacMan.Server.Controllers
{
    [ApiController]
    [Route("game")]
    public class GameController : ControllerBase
    {

        private readonly ILogger<GameController> _logger;

        public GameController(ILogger<GameController> logger)
        {
            _logger = logger;
        }
    }
}