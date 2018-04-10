using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WebSite.Interface;
using WebSite.Models;
using WebSite.Repository;

namespace WebSite.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private WebSiteDbContext db;
        private ArtistRepository artistRepository;
        private AlbumRepository albumRepository;
        private TrackRepository trackRepository;
        private SimilarRepository similarRepository;

        public EFUnitOfWork(WebSiteDbContext context)
        {
            db = context;
        }
        public IRepository<Artist> Artists
        {
            get
            {
                if (artistRepository == null)
                    artistRepository = new ArtistRepository(db);
                return artistRepository;
            }
        }

        public IRepository<Album> Albums
        {
            get
            {
                if (albumRepository == null)
                    albumRepository = new AlbumRepository(db);
                return albumRepository;
            }
        }

        public IRepository<Track> Tracks
        {
            get
            {
                if (trackRepository == null)
                    trackRepository = new TrackRepository(db);
                return trackRepository;
            }
        }

        public IRepository<Similar> Similars
        {
            get
            {
                if (similarRepository == null)
                    similarRepository = new SimilarRepository(db);
                return similarRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
