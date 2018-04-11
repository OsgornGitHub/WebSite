using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WebSite.Models;

namespace WebSite.Data
{
    public class ArtistMap
    {
        public ArtistMap(EntityTypeBuilder<Artist> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.Name).IsRequired();
            entityBuilder.Property(t => t.Photo).IsRequired();
            entityBuilder.Property(t => t.Biography).IsRequired();
        }
    }
}
