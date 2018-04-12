using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.Models
{
    public class Album
    {
        public Guid AlbumId { get; set; }
        public string Cover { get; set; }
        public string NameAlbum { get; set; }
        public string NameArtist { get; set; }

        public ICollection<Track> Tracks { get; set; }
        public Album()
        {
            Tracks = new List<Track>();
        }
    }
}
