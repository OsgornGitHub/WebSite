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
    class AlbumRepository : IRepository<Album>
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

        public IEnumerable<Album> Find(Guid id)
        {
            return db.Albums.Where(a => a.AlbumFk == id).ToList() ;
        }

        public Album Get(int id)
        {
            return db.Albums.Find(id);
        }

        public void Create(Album album)
        {
            db.Albums.Add(album);
        }

        public void Delete(int id)
        {
            Artist album = db.Artists.Find(id);
            if (album != null)
                db.Artists.Remove(album);
        }


    }
}
