using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.Models.ViewModels
{
    // TODO: Как ты используешь эту модель?
    public class AlbumViewModel
    {
        public string NameAlbum { get; set; }
        public string NameArtist { get; set; }
        public string Image { get; set; }
        public List<Track> Tracks {get; set;}
    }
}
