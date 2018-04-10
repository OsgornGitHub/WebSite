using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSite.Models;
using WebSite.Repositories;

namespace WebSite.Repository
{

    public class AlbumRepository : IRepository<Album>
    {
        private WebSiteDbContext db;

        public AlbumRepository(WebSiteDbContext context)
        {
            this.db = context;
        }

        public IEnumerable<Album> GetAll()
        {
            return db.Albums;
        }

        public Album Get(Guid id)
        {
            return db.Albums.Find(id);
        }

        public void Create(Album album)
        {
            db.Albums.Add(album);
        }

        public void Delete(int id)
        {
            Album album = db.Albums.Find(id);
            if (album != null)
                db.Albums.Remove(album);
        }
    }

}
