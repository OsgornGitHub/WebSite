using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSite.Models;
using WebSite.Repositories;

namespace WebSite.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Artist> Artists { get; }
        IRepository<Album> Albums { get; }
        IRepository<Track> Tracks { get; }
        IRepository<Similar> Similars { get; }
        void Save();
    }
}
