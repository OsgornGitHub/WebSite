using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.Models
{
    public class Artist
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public string Biography { get; set; }
    }
}
