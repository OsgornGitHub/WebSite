using NLayerApp.BLL.DTO;
using System;
using System.Collections.Generic;
namespace NLayerApp.BLL.Interfaces
{
    public interface IArtistService
    {
        IEnumerable<ArtistDTO> GetTopArtistLFM(int page, int count);
        void AddArtistToDb(ArtistDTO artist);
        void Dispose();
    }
}