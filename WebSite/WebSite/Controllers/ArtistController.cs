using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebSite.Models;

namespace WebSite.Controllers
{
    public class ArtistController : Controller
    {
        public IActionResult GetArtist(string name)
        {
            var nameForRequest = IsValidName(name);
            HttpWebRequest tokenRequest = (HttpWebRequest)WebRequest.Create("http://ws.audioscrobbler.com/2.0/?method=artist.getinfo&artist=" + nameForRequest + "&api_key=1068375741deac644574d04838a37810");
            HttpWebResponse tokenResponse = (HttpWebResponse)tokenRequest.GetResponse();
            string Result = new StreamReader(tokenResponse.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            var bio = "";
            var imageLink = "";
            for (var i = Result.IndexOf("<content>") + 9; i < Result.IndexOf("</content>"); i++)
            {
                bio += Result[i];
            }
            for (var i = Result.IndexOf("<image size=\"mega\">") + 19; i < Result.IndexOf("</image>\n<image size=\"\">"); i++)
            {
                imageLink += Result[i];
            }
            ArtistViewModel artist = new ArtistViewModel()
            {
                Name = name,
                Photo = imageLink,
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
                    validName +=  longName[i];
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
            HttpWebRequest tokenRequest = (HttpWebRequest)WebRequest.Create("http://ws.audioscrobbler.com/2.0/?method=artist.getinfo&artist=" + nameForRequest + "&api_key=1068375741deac644574d04838a37810");
            HttpWebResponse tokenResponse = (HttpWebResponse)tokenRequest.GetResponse();
            string Result = new StreamReader(tokenResponse.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            var stringSimilar = "";
            for (var i = Result.IndexOf("<similar>") + 9; i < Result.IndexOf("</similar>"); i++)
            {
                stringSimilar += Result[i];
            }
            string[] splitStringSimilar = stringSimilar.Split("<artist>");

            foreach(var similar in splitStringSimilar)
            {
                var nameSimilar = "";
                var photoSimilar = "";
                if (similar == "") continue;
                for (var i = similar.IndexOf("<name>") + 6; i < similar.IndexOf("</name>"); i++)
                {
                    nameSimilar += similar[i];
                }
                for (var i = similar.IndexOf("<image size=\"mega\">") + 19; i < similar.IndexOf("</image>\n<image size=\"\">"); i++)
                {
                    photoSimilar += similar[i];
                }
                Similar artist = new Similar
                {
                    Name = nameSimilar,
                    Photo = photoSimilar
                };
                listSimilar.Add(artist);
            }

            return listSimilar;
        }
    }
}