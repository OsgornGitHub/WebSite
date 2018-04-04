using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebSite.Models;
using Newtonsoft.Json.Linq;

namespace WebSite.Controllers
{
    public class ArtistController : Controller
    {
        public IActionResult GetArtist(string name)
        {
            var nameForRequest = IsValidName(name);
            HttpWebRequest tokenRequest = (HttpWebRequest)WebRequest.Create("http://ws.audioscrobbler.com/2.0/?method=artist.getinfo&artist=" + nameForRequest + "&api_key=1068375741deac644574d04838a37810" + "&format=json");
            HttpWebResponse tokenResponse = (HttpWebResponse)tokenRequest.GetResponse();
            string Result = new StreamReader(tokenResponse.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            Result = Result.Replace("#", "");
            dynamic ResultJson = JObject.Parse(Result);
            string bio = ResultJson.artist.bio.content;
            string photo = "";
            foreach (dynamic dyn in ResultJson.artist.image)
            {
                if (dyn.size == "mega")
                {
                    photo = dyn.text;
                    break;
                }
            }

            ArtistViewModel artist = new ArtistViewModel()
            {
                Name = name,
                Photo = photo,
                Biography = bio
            };
            ViewBag.respond = Result;
            return View(artist);
        }

        public IActionResult GetSimilar(string name)
        {
            List<Similar> list = new List<Similar>();
            list = GetListSimilar(name);
            return View(list);
        }

        private string IsValidName(string name)
        {
            var validName = "";
            if (name.IndexOf(" ") != -1)
            {
                var longName = name.Split(" ");
                for (int i = 0; i < longName.Length; i++)
                {
                    if (i != longName.Length - 1)
                    {
                        validName += (longName[i] + "+");
                    }
                    else
                    {
                        validName += longName[i];
                    }

                }
                return validName;
            }
            return name;
        }


        public List<string> GetTopAlbum(string name)
        {
            List<string> topAlbums = new List<string>();
            return topAlbums;
        }

        public List<string> GetTopTracks(string name)
        {
            List<string> topTracks = new List<string>();
            return topTracks;
        }


        public List<Similar> GetListSimilar(string name)
        {

            List<Similar> listSimilar = new List<Similar>();
            var nameForRequest = IsValidName(name);
            HttpWebRequest tokenRequest = (HttpWebRequest)WebRequest.Create("http://ws.audioscrobbler.com/2.0/?method=artist.getinfo&artist=" + nameForRequest + "&api_key=1068375741deac644574d04838a37810" + "&format=json");
            HttpWebResponse tokenResponse = (HttpWebResponse)tokenRequest.GetResponse();
            string Result = new StreamReader(tokenResponse.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            Result = Result.Replace("#", "");
            dynamic ResultJson = JObject.Parse(Result);
            string nameSimilar = "";
            string photoSimilar = "";
            //string adr = ResultJson.artist.similar[0].artist[0].name;
            foreach (var artist in ResultJson.artist.similar.artist)
            {
                nameSimilar = artist.name;
                foreach (dynamic dyn in artist.image)
                {
                    if (dyn.size == "mega")
                    {
                        photoSimilar = dyn.text;
                        break;
                    }
                }
                Similar similar = new Similar
                {
                    Name = nameSimilar,
                    Photo = photoSimilar
                };
                listSimilar.Add(similar);
            }
            return listSimilar;
        }
    }
}