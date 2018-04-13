using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using WEB.Models;

namespace WEB.Controllers
{
    public class HomeController : Controller
    {

        public AppDbContext db;
        public IConfiguration Configuration { get; set; }

        public HomeController(AppDbContext context, IConfiguration config)
        {
            db = context;
            Configuration = config;
        }

        public IActionResult Index(int page = 1)
        {
            List<ArtistView> list = new List<ArtistView>();
            var pageNum = page;
            var count = 24;
            list = GetNextPage(pageNum, count);
            return View(list);
        }


        public IActionResult GetJson(int page, int count)
        {
            var list = GetNextPage(page, count);
            return Json(list);
        }

        [HttpGet]
        public List<ArtistView> GetNextPage(int page, int count)
        {
            List<ArtistView> list = new List<ArtistView>();
            dynamic ResultJson = GetResponse("http://ws.audioscrobbler.com/2.0/?method=chart.gettopartists&api_key=", page, count);
            foreach (var person in ResultJson.artists.artist)
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
                if (list.Count >= count)
                {
                    break;
                }
                list.Add(artist);
            }
            return list;
        }


        [HttpGet]
        public IActionResult GetArtist(string name)
        {

            var nameForRequest = IsValidName(name);
            var nnn = db.Artists.ToList() ;
            if (db.Artists.FirstOrDefault(a => a.Name == name) != null)
            {
                Artist art = db.Artists.First(a => a.Name == name);
                return View(art);
            }
            dynamic ResultJson = GetResponse("http://ws.audioscrobbler.com/2.0/?method=artist.getinfo&artist=", nameForRequest);
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
                ArtistId = Guid.NewGuid(),
                Name = name,
                Photo = photo,
                Biography = bio
            };
            db.Artists.Add(artist);
            db.SaveChanges();
            return View(artist);
        }

        [HttpGet]
        public IActionResult GetAlbum(string nameArtist, string nameAlbum)
        {
            var nameArtistForRequest = IsValidName(nameArtist);
            var nameAlbumForRequest = IsValidName(nameAlbum);
            //if (db.Albums.Any(a => a.NameAlbum == nameAlbum || a.NameArtist == nameArtist))
            //{
            //    Album alb = db.Albums.FirstOrDefault(a => a.NameAlbum == nameAlbum);
            //    return View(alb);
            //}
            dynamic ResultJson = GetResponse("http://ws.audioscrobbler.com/2.0/?method=album.getinfo&artist=", nameArtistForRequest, nameAlbumForRequest);
            List<Track> tracks = new List<Track>();
            var albumId = Guid.NewGuid();
            foreach (var tr in ResultJson.album.tracks.track)
            {
                Track track = new Track()
                {
                    TrackId = Guid.NewGuid(),
                    Name = tr.name
                };
                tracks.Add(track);
                db.SaveChanges();
            }
            var albumName = ResultJson.album.name;
            var artistName = ResultJson.album.artist;
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
                AlbumId = Guid.NewGuid(),
                NameAlbum = albumName,
                NameArtist = artistName,
                Cover = image,
                Music = tracks
            };
            db.Albums.Add(album);
            db.SaveChanges();
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

        [HttpGet]
        public JsonResult GetTopAlbum(string name, int page, int count)
        {
            var nameForRequest = IsValidName(name);
            List<Album> topAlbums = new List<Album>();
            var nameAlbum = "";
            var cover = "";
            dynamic ResultJson = GetResponse("http://ws.audioscrobbler.com/2.0/?method=artist.gettopalbums&artist=", nameForRequest, page, count);
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
                if (IsValidAlbum(cover, nameAlbum))
                {
                    Album album = new Album()
                    {
                        AlbumId = Guid.NewGuid(),
                        NameAlbum = nameAlbum,
                        NameArtist = name,
                        Cover = cover
                    };
                    //db.Albums.Add(album);
                    topAlbums.Add(album);
                    //db.SaveChanges();
                }
                else topAlbums.Add(GetOneTopAlbum(name, page, count));
            }
            return Json(topAlbums);
        }


        public bool IsValidAlbum(string cover, string name)
        {
            if (name == "null" || cover == "null" || cover == "" || name == "")
            {
                return false;
            }
            return true;
        }

        [HttpGet]
        public Album GetOneTopAlbum(string name, int page, int count)
        {
            var nameForRequest = IsValidName(name);
            dynamic ResultJson = GetResponse("http://ws.audioscrobbler.com/2.0/?method=artist.gettopalbums&artist=", nameForRequest, page, count);
            var nameAlbum = "";
            var cover = "";
            nameAlbum = ResultJson.topalbums.album[0].name;
            foreach (dynamic dyn in ResultJson.topalbums.album[0].image)
            {
                if (dyn.size == "extralarge")
                {
                    cover = dyn.text;
                    break;
                }
            }

            Album album = new Album()
            {
                AlbumId = Guid.NewGuid(),
                NameAlbum = nameAlbum,
                NameArtist = name,
                Cover = cover
            };
            db.Albums.Add(album);
            //db.SaveChanges();
            return album;
        }


        public List<Track> GetTopTracks(string name, int count = 24, int page = 1)
        {
            var nameForRequest = IsValidName(name);
            List<Track> topTracks = new List<Track>();
            dynamic ResultJson = GetResponse("http://ws.audioscrobbler.com/2.0/?method=artist.gettoptracks&artist=", nameForRequest, page, count);
            var nameTrack = "";
            var cover = "";
            foreach (var music in ResultJson.toptracks.track)
            {
                nameTrack = music.name;
                foreach (dynamic dyn in music.image)
                {
                    if (dyn.size == "extralarge")
                    {
                        cover = dyn.text;
                        break;
                    }
                }
                Track track = new Track()
                {
                    TrackId = Guid.NewGuid(),
                    Name = nameTrack,
                    Cover = cover
                };
                topTracks.Add(track);
            }
            return topTracks;
        }

        [HttpGet]
        public List<Similar> GetListSimilar(string name)
        {
            var nameForRequest = IsValidName(name);
            List<Similar> listSimilar = new List<Similar>();
            if (db.Artists.FirstOrDefault(a => a.Name == nameForRequest) != null)
            {
                foreach (var sim in db.Artists.FirstOrDefault(a => a.Name == name).Sim)
                {
                    listSimilar.Add(sim);
                    return listSimilar;
                }
            }
            dynamic ResultJson = GetResponse("http://ws.audioscrobbler.com/2.0/?method=artist.getsimilar&artist=", nameForRequest, 1, 12);
            string nameSimilar = "";
            string photoSimilar = "";
            foreach (var artist in ResultJson.similarartists.artist)
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
                    SimilarId = Guid.NewGuid(),
                    Name = nameSimilar,
                    Photo = photoSimilar
                };
                listSimilar.Add(similar);
                db.Similars.Add(similar);
                db.SaveChanges();
            }
            return listSimilar;
        }


        public JObject GetResponse(string url, string name, int page, int count)
        {
            HttpWebRequest tokenRequest = (HttpWebRequest)WebRequest.Create(url + name + "&api_key=" + Configuration["apikey"] + "&limit=" + count + "&page=" + page + "&format=json");
            HttpWebResponse tokenResponse = (HttpWebResponse)tokenRequest.GetResponse();
            string Result = new StreamReader(tokenResponse.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            Result = Result.Replace("#", "");
            dynamic ResultJson = JObject.Parse(Result);
            return ResultJson;
        }

        public JObject GetResponse(string url, string nameArtist, string nameAlbum)
        {
            HttpWebRequest tokenRequest = (HttpWebRequest)WebRequest.Create(url + nameArtist + "&album=" + nameAlbum + "&api_key=" + Configuration["apikey"] +  "&format=json");
            HttpWebResponse tokenResponse = (HttpWebResponse)tokenRequest.GetResponse();
            string Result = new StreamReader(tokenResponse.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            Result = Result.Replace("#", "");
            dynamic ResultJson = JObject.Parse(Result);
            return ResultJson;
        }

        public JObject GetResponse(string url, string name)
        {
            HttpWebRequest tokenRequest = (HttpWebRequest)WebRequest.Create(url + name + "&api_key=" + Configuration["apikey"] + "&format=json");
            HttpWebResponse tokenResponse = (HttpWebResponse)tokenRequest.GetResponse();
            string Result = new StreamReader(tokenResponse.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            Result = Result.Replace("#", "");
            dynamic ResultJson = JObject.Parse(Result);
            return ResultJson;
        }

        public JObject GetResponse(string url, int page, int count)
        {
            HttpWebRequest tokenRequest = (HttpWebRequest)WebRequest.Create(url + "&limit=" + count + "&page=" + page + "&api_key=" + Configuration["apikey"]  + "&format=json");
            HttpWebResponse tokenResponse = (HttpWebResponse)tokenRequest.GetResponse();
            string Result = new StreamReader(tokenResponse.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            Result = Result.Replace("#", "");
            dynamic ResultJson = JObject.Parse(Result);
            return ResultJson;
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        }
    }

}
