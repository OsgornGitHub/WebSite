using System;
using System.ComponentModel.DataAnnotations;

namespace NLayerApp.BLL.DTO
{
    public class AlbumDTO
    {
        [Key]
        public Guid AlbumFk { get; set; }
        public string Cover { get; set; }
        public string NameAlbum { get; set; }
        public string NamePerson { get; set; }
        public string LinkToPerson { get; set; }
    }
}
