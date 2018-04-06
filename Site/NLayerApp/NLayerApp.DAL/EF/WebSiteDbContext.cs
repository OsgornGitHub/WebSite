using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using NLayerApp.DAL.Entities;

namespace NLayerApp.DAL.EF
{
    public class WebSiteDbContext : DbContext
    {
        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Similar> Similars { get; set; }

        public WebSiteDbContext(string connectionString)
            : base(connectionString)
        {
        }
    }
}
