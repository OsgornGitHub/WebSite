using System;
using NLayerApp.DAL.EF;
using NLayerApp.DAL.Interfaces;
using NLayerApp.DAL.Entities;

namespace NLayerApp.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private WebSiteDbContext db;
        private AlbumRepository albumRepository;
        private SimilarRepository similarRepository;
        private TrackRepository trackRepository;
        private ArtistRepository artistRepository;
        private string connectionString = "DefaultConnection";

        public EFUnitOfWork(string connectionString)
        {
            db = new WebSiteDbContext(connectionString);
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

        public IRepository<Artist> Artists
        {
            get
            {
                if (artistRepository == null)
                    artistRepository = new ArtistRepository(db);
                return artistRepository;
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

        public IRepository<Track> Tracks
        {
            get
            {
                if (trackRepository == null)
                    trackRepository = new TrackRepository(db);
                return trackRepository;
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