using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB.Models
{
    public class Album
    {
        public Guid AlbumId { get; set; }
        public string Cover { get; set; }
        public string NameAlbum { get; set; }
        public string NameArtist { get; set; }

        public Guid? ArtistId { get; set; }
        public virtual Artist Artist { get; set; }

        public ICollection<Track> Music { get; set; }
        public Album()
        {
            Music = new List<Track>();
        }
    }
}
