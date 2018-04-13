using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB.Models
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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Album>(e =>
            {
                e.HasOne(r => r.Artist).WithMany(t => t.Alb);
            });
            modelBuilder.Entity<Similar>(e =>
            {
                e.HasOne(r => r.Artist).WithMany(t => t.Sim);
            });
            modelBuilder.Entity<Track>(e =>
            {
                e.HasOne(r => r.Album).WithMany(t => t.Music);
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
