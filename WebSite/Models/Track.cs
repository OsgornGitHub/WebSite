using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.Models
{
    public class Track
    {
        [Key]
        public Guid TrackId { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Cover { get; set; }
    }
}
