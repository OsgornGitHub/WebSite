using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB.Models
{
    public class Artist
    {
        public Guid ArtistId { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public string Biography { get; set; }


        public ICollection<Album> Alb { get; set; }
        public ICollection<Similar> Sim { get; set; }
        public Artist()
        {
            Sim = new List<Similar>();
            Alb = new List<Album>();
        }

    }
}
