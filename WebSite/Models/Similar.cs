using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.Models
{
    public class Similar
    {
        public Guid SimilarId { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
    }
}
