using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PacMan.Server.DbSchema;
using PacMan.Shared.Models;

namespace PacMan.Server.Controllers
{
    [ApiController]
    [Route("grids")]
    public class GridController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GridController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EditorGridModel gridModel)
        {
            var grid = new Grid
            {
                Name = gridModel.Name,
                GridJson = gridModel.GridJson
            };
            _context.Grids.Add(grid);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<List<EditorGridModel>>> GetGrids()
        {
            var grids = await _context.Grids
                .Select(g => new EditorGridModel
                {
                    Id = g.Id,
                    Name = g.Name
                })
                .ToListAsync();
            return Ok(grids);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<EditorGridModel>> GetGrid([FromRoute]int id)
        {
            var grid = await _context.Grids
                .Select(g => new EditorGridModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    GridJson = g.GridJson
                })
                .FirstOrDefaultAsync(g => g.Id == id);
            return grid is not null ? Ok(grid) : NotFound(id);
        }
    }
}