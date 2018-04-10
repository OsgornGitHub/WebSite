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
        public Guid ThackFk { get; set; }
        public string Name { get; set; }
    }
}
