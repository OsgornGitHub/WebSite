using Microsoft.EntityFrameworkCore;

namespace WebSite.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Similar> Similars { get; set; }
        public DbSet<Track> Tracks { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }
}
