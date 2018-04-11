using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WebSite.Models;

namespace WebSite.Data
{
    public class SimilarMap
    {
        public SimilarMap(EntityTypeBuilder<Similar> entityBuilder)
        {
            entityBuilder.HasKey(t => t.ArtistId);
            entityBuilder.Property(t => t.Name).IsRequired();
            entityBuilder.Property(t => t.Photo).IsRequired();
        }
    }
}
