using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSite.Models;

namespace WebSite.Repositories
{
    public class ArtistRepository : IRepository<Artist>
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

        public Artist Get(Guid id)
        {
            return db.Artists.Find(id);
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
