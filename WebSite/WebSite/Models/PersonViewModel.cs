using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.Models
{
    public class PersonViewModel
    {
        public List<OnePerson> OnePersons { get; set; }
    }
    public class OnePerson
    {
        public string Name { get; set; }
        //public string Photo { get; set; }
    }
}
