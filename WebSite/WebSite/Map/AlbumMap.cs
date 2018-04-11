using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WebSite.Models;

namespace WebSite.Data
{
    public class AlbumMap
    {
        public AlbumMap(EntityTypeBuilder<Album> entityBuilder)
        {
            entityBuilder.HasKey(t => t.AlbumFk);
            entityBuilder.Property(t => t.NameAlbum).IsRequired();
            entityBuilder.Property(t => t.NameArtist).IsRequired();
            entityBuilder.Property(t => t.Cover).IsRequired();
            entityBuilder.Property(t => t.Tracks).IsRequired();
        }
    }
}
