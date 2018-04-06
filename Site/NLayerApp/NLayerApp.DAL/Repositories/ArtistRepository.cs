using NLayerApp.DAL.EF;
using NLayerApp.DAL.Entities;
using NLayerApp.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace NLayerApp.DAL.Repositories
{
    class ArtistRepository : IRepository<Artist>
    {
        private WebSiteDbContext db;

        public ArtistRepository(WebSiteDbContext context)
        {
            this.db = context;
        }

        public IEnumerable<Artist> GetAll()
        {
            return db.Artists;
        }

        public Artist Get(int id)
        {
            return db.Artists.Find(id);
        }

        public IEnumerable<Artist> Find(Guid id)
        {
            return db.Artists.Where(a => a.ArtistFk == id).ToList();
        }

        public void Create(Artist artist)
        {
            db.Artists.Add(artist);
        }

        public void Delete(int id)
        {
            Artist artist = db.Artists.Find(id);
            if (artist != null)
                db.Artists.Remove(artist);
        }
    }
}

