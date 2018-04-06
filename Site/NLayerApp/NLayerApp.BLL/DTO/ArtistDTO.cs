using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NLayerApp.BLL.DTO
{
    public  class ArtistDTO
    {
        [Key]
        public Guid ArtistFk { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public string Biography { get; set; }
    }
}
