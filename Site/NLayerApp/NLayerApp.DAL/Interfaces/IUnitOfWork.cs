using NLayerApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace NLayerApp.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Album> Albums { get; }
        IRepository<Artist> Artists { get; }
        IRepository<Similar> Similars { get; }
        IRepository<Track> Tracks { get; }
        void Save();
    }
}
