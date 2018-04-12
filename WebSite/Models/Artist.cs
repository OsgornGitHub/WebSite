using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.Models
{
    public class Artist
    {
        public Guid ArtistId { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public string Biography { get; set; }


        public ICollection<Album> Albums { get; set; }
        public ICollection<Similar> Similars { get; set; }
        public Artist()
        {
            Similars = new List<Similar>();
            Albums = new List<Album>();
        }



    }
}
