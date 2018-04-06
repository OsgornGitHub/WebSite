using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NLayerApp.DAL.Entities
{
    public class Track
    {
        [Key]
        public Guid ThackFk { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
    }
}
