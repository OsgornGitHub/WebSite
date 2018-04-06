using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NLayerApp.BLL.DTO
{
    public class SimilarDTO
    {
        [Key]
        public Guid ArtistId { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
    }
}
