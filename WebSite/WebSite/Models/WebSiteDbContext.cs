using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.Models
{
    public class WebSiteDbContext : DbContext
    {
        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Similar> Similars { get; set; }

        public WebSiteDbContext(DbContextOptions<WebSiteDbContext> options)
            : base(options)
        {
        }

    }
}
