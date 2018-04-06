using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NLayerApp.DAL.Entities
{
    public class Album
    {
        [Key]
        public Guid AlbumFk { get; set; }
        public string Cover { get; set; }
        public string NameAlbum { get; set; }
        public string NamePerson { get; set; }
        public string LinkToPerson { get; set; }
    }
}

