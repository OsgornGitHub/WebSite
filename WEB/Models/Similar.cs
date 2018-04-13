using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB.Models
{
    public class Similar
    {
        public Guid SimilarId { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }

        public Guid? ArtistId { get; set; }
        public virtual Artist Artist { get; set; }

    }
}
