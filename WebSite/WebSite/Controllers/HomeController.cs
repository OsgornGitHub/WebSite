using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WebSite.Models;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
using WebSite.Repositories;
using WebSite.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace WebSite.Controllers
{
    public class HomeController : Controller
    {
        public IConfiguration Configuration { get; set; }
        public HomeController(IConfiguration config)
        {
            Configuration = config;
        }

        public IActionResult Index()
        {
            List<ArtistView> list = new List<ArtistView>();
            var page = 1;
            var count = 24;
            list = GetNextPage(page, count);
            return View(list);
        }


        [HttpPost]
        public IActionResult GetJson(int page, int count)
        {
            var list = GetNextPage(page, count);
            return Json(list);
        }

        [HttpGet]
        public List<ArtistView> GetNextPage(int page, int count)
        {
            List<ArtistView> list = new List<ArtistView>();
            HttpWebRequest tokenRequest = (HttpWebRequest)WebRequest.Create("http://ws.audioscrobbler.com/2.0/?method=geo.gettopartists&country=spain&api_key=" + Configuration["apikey"] + " &page=" + page + "&limit=" + count + "&format=json");
            HttpWebResponse tokenResponse = (HttpWebResponse)tokenRequest.GetResponse();
            string Result = new StreamReader(tokenResponse.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            Result = Result.Replace("#", "");
            dynamic ResultJson = JObject.Parse(Result);
            foreach(var person in ResultJson.topartists.artist)
            {
                string name = person.name;
                string photo = "";
                foreach (dynamic dyn in person.image)
                {
                    if (dyn.size == "mega")
                    {
                        photo = dyn.text;
                        break;
                    }
                }
                ArtistView artist = new ArtistView
                {
                    Name = name,
                    Photo = photo
                };
                list.Add(artist);
            }
            return list;
        }

        public IActionResult GetArtist(string name)
        {
            List<Artist> artists = new List<Artist>();
            var nameForRequest = IsValidName(name);
            HttpWebRequest tokenRequest = (HttpWebRequest)WebRequest.Create("http://ws.audioscrobbler.com/2.0/?method=artist.getinfo&artist=" + nameForRequest + "&api_key=" + Configuration["apikey"] + "&format=json");
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

            Artist artist = new Artist()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Photo = photo,
                Biography = bio
            };
            return View(artist);
        }

        public IActionResult GetAlbum(string nameArtist, string nameAlbum)
        {
            var nameArtistForRequest = IsValidName(nameArtist);
            var nameAlbumForRequest = IsValidName(nameAlbum);
            HttpWebRequest tokenRequest = (HttpWebRequest)WebRequest.Create("http://ws.audioscrobbler.com/2.0/?method=album.getinfo&artist=" + nameArtistForRequest + "&album=" + nameAlbumForRequest + "&api_key=" + Configuration["apikey"] + "&format=json");
            HttpWebResponse tokenResponse = (HttpWebResponse)tokenRequest.GetResponse();
            string Result = new StreamReader(tokenResponse.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            Result = Result.Replace("#", "");
            dynamic ResultJson = JObject.Parse(Result);
            List<Track> tracks = new List<Track>();
            foreach(var tr in ResultJson.album.tracks.track)
            {
                Track track = new Track()
                {
                    Name = tr.name
                };
                tracks.Add(track);
            }
            var albumName = ResultJson.album.name;
            var artistNAme = ResultJson.album.artist;
            var image = "";
            foreach (dynamic dyn in ResultJson.album.image)
            {
                if (dyn.size == "mega")
                {
                    image = dyn.text;
                    break;
                }
            }
            Album album = new Album()
            {

                NameAlbum = albumName,
                NameArtist = artistNAme,
                Cover = image,
                Tracks = tracks
            };

            return View(album);
        }

        public JsonResult GetSimilar(string name)
        {
            List<Similar> list = new List<Similar>();
            list = GetListSimilar(name);
            return Json(list);
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


        public JsonResult GetTopAlbum(string name, int page, int count)
        {
            var nameForRequest = IsValidName(name);
            List<Album> topAlbums = new List<Album>();
            HttpWebRequest tokenRequest = (HttpWebRequest)WebRequest.Create("http://ws.audioscrobbler.com/2.0/?method=artist.gettopalbums&artist=" + nameForRequest + "&api_key=" + Configuration["apikey"] + "&limit=" + count + "&page=" + page + "&format=json");
            HttpWebResponse tokenResponse = (HttpWebResponse)tokenRequest.GetResponse();
            string Result = new StreamReader(tokenResponse.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            Result = Result.Replace("#", "");
            var nameAlbum = "";
            var cover = "";
            dynamic ResultJson = JObject.Parse(Result);
            foreach (var tr in ResultJson.topalbums.album)
            {
                nameAlbum = tr.name;
                foreach (dynamic dyn in tr.image)
                {
                    if (dyn.size == "extralarge")
                    {
                        cover = dyn.text;
                        break;
                    }
                }
                Album album = new Album()
                {
                    AlbumFk = Guid.NewGuid(),
                    NameAlbum = nameAlbum,
                    NameArtist = name,
                    Cover = cover
                };
                topAlbums.Add(album);
            }
            return Json(topAlbums);
        }

        public List<Track> GetTopTracks(string name, int count = 24, int page = 1)
        {
            var nameForRequest = IsValidName(name);
            List<Track> topTracks = new List<Track>();
            HttpWebRequest tokenRequest = (HttpWebRequest)WebRequest.Create("http://ws.audioscrobbler.com/2.0/?method=artist.gettoptracks&artist=" + nameForRequest + "&api_key=" + Configuration["apikey"] + "&limit=" + count + "&page=" + page + "&format=json");
            HttpWebResponse tokenResponse = (HttpWebResponse)tokenRequest.GetResponse();
            string Result = new StreamReader(tokenResponse.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            Result = Result.Replace("#", "");
            dynamic ResultJson = JObject.Parse(Result);
            var nameTrack = "";
            var link = "";
            foreach (var music in ResultJson.toptracks.track)
            {
                nameTrack = music.name;
                foreach (dynamic dyn in music.image)
                {
                    if (dyn.size == "extralarge")
                    {
                        link = dyn.text;
                        break;
                    }
                }
                Track track = new Track()
                {
                    Name = nameTrack,
                };
                topTracks.Add(track);
            }
            return topTracks;
        }


        public List<Similar> GetListSimilar(string name)
        {

            List<Similar> listSimilar = new List<Similar>();
            var nameForRequest = IsValidName(name);
            HttpWebRequest tokenRequest = (HttpWebRequest)WebRequest.Create("http://ws.audioscrobbler.com/2.0/?method=artist.getinfo&artist=" + nameForRequest + "&api_key=" + Configuration["apikey"] + "&format=json");
            HttpWebResponse tokenResponse = (HttpWebResponse)tokenRequest.GetResponse();
            string Result = new StreamReader(tokenResponse.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            Result = Result.Replace("#", "");
            dynamic ResultJson = JObject.Parse(Result);
            string nameSimilar = "";
            string photoSimilar = "";
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

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
