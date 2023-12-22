using Microsoft.EntityFrameworkCore;

namespace PacMan.Server.DbSchema
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Grid> Grids { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}