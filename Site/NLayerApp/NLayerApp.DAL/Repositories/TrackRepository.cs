using NLayerApp.DAL.EF;
using NLayerApp.DAL.Entities;
using NLayerApp.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NLayerApp.DAL.Repositories
{
    class TrackRepository : IRepository<Track>
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

        public IEnumerable<Track> Find(Guid id)
        {
            return db.Tracks.Where(a => a.ThackFk == id).ToList();
        }

        public Track Get(int id)
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
