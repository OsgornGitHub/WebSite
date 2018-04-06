using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NLayerApp.BLL.DTO
{
    public class TrackDTO
    {
        [Key]
        public Guid ThackFk { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
    }
}
