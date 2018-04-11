using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WebSite.Models;

namespace WebSite.Data
{
    public class TrackMap
    {
        public TrackMap(EntityTypeBuilder<Track> entityBuilder)
        {
            entityBuilder.HasKey(t => t.ThackFk);
            entityBuilder.Property(t => t.Name).IsRequired();
        }
    }
}
