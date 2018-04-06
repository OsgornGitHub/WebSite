using Newtonsoft.Json.Linq;
using NLayerApp.DAL.EF;
using NLayerApp.DAL.Entities;
using NLayerApp.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace NLayerApp.DAL.Repositories
{
    class SimilarRepository : IRepository<Similar>
    {
        private WebSiteDbContext db;

        public SimilarRepository(WebSiteDbContext context)
        {
            this.db = context;
        }

        public IEnumerable<Similar> GetAll()
        {
            return db.Similars;
        }

        public IEnumerable<Similar> Find(Guid id)
        {
            return db.Similars.Where(a => a.ArtistId == id).ToList();
        }

        public Similar Get(int id)
        {
            return db.Similars.Find(id);
        }

        public void Create(Similar similar)
        {
            db.Similars.Add(similar);
        }

        public void Delete(int id)
        {
            Similar similar = db.Similars.Find(id);
            if (similar != null)
                db.Similars.Remove(similar);
        }
    }
}
