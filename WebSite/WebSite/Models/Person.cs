using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.Models
{
    public class Person
    {
        [Key]
        public Guid PersonFk { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public string Biography { get; set; }

        private List<string> topMusic;

        public List<string> GetTopMusic()
        {
            return topMusic;
        }

        public void SetTopMusic(List<string> value)
        {
            topMusic = value;
        }

        private List<string> topAlbums;

        public List<string> GetTopAlbums()
        {
            return topAlbums;
        }

        public void SetTopAlbums(List<string> value)
        {
            topAlbums = value;
        }

        private List<string> similarPerson;

        public List<string> GetSimilarPerson()
        {
            return similarPerson;
        }

        public void SetSimilarPerson(List<string> value)
        {
            similarPerson = value;
        }
    }
}
