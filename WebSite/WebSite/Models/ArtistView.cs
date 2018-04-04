using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.Models
{
    public class ArtistView
    {
        [Key]
        public Guid PersonFk { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public string Biography { get; set; }

        public List<string> TopMusic { get; set; }
        public List<string> TopAlbums { get; set; }
        public List<string> TimilarPerson { get; set; }
    }
}
