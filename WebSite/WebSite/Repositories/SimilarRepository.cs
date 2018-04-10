using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSite.Models;

namespace WebSite.Repositories
{
    public class SimilarRepository : IRepository<Similar>
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

        public Similar Get(Guid id)
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
