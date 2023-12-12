using Microsoft.AspNetCore.Mvc;
using PacMan.Shared;
using PacMan.Shared.Models;

namespace PacMan.Server.Controllers
{
    [ApiController]
    public class EditorController : ControllerBase
    {
        public EditorController()
        {
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Create(TileGrid grid) // ?
        {        
            return Ok();
        }
    }
}