using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSite.Models;

namespace WebSite.Repositories
{
    public class TrackRepository : IRepository<Track>
    {
        private WebSiteDbContext db;

        public TrackRepository(WebSiteDbContext context)
        {
            this.db = context;
        }

        public IEnumerable<Track> GetAll()
        {
            return db.Tracks;
        }

        public Track Get(Guid id)
        {
            return db.Tracks.Find(id);
        }

        public void Create(Track track)
        {
            db.Tracks.Add(track);
        }


        public void Delete(int id)
        {
            Track track = db.Tracks.Find(id);
            if (track != null)
                db.Tracks.Remove(track);
        }
    }
}
