using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NLayerApp.DAL.Entities
{
    public class Artist
    {
        [Key]
        public Guid ArtistFk { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public string Biography { get; set; }
    }
}

