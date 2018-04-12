using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using WebSite.Models;

namespace WebSite.Controllers
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

        public IActionResult Index()
        {
            List<ArtistView> list = new List<ArtistView>();
            var page = 1;
            var count = 24;
            list = GetNextPage(page, count);
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
            HttpWebRequest tokenRequest = (HttpWebRequest)WebRequest.Create("http://ws.audioscrobbler.com/2.0/?method=chart.gettopartists&api_key=" + Configuration["apikey"] + " &page=" + page + "&limit=" + count + "&format=json");
            HttpWebResponse tokenResponse = (HttpWebResponse)tokenRequest.GetResponse();
            string Result = new StreamReader(tokenResponse.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            Result = Result.Replace("#", "");
            dynamic ResultJson = JObject.Parse(Result);
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
            //if (db.Artists.FirstOrDefault(a => a.Name == nameForRequest) != null)
            //{
            //    Artist art = db.Artists.FirstOrDefault(a => a.Name == name);
            //    return View(art);
            //}
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
                ArtistId = Guid.NewGuid(),
                Name = name,
                Photo = photo,
                Biography = bio
            };
            db.Artists.Add(artist);
            return View(artist);
        }

        [HttpGet]
        public IActionResult GetAlbum(string nameArtist, string nameAlbum)
        {
            var nameArtistForRequest = IsValidName(nameArtist);
            var nameAlbumForRequest = IsValidName(nameAlbum);

            //if (db.Albums.FirstOrDefault(a => a.NameAlbum == nameAlbum) != null)
            //{
            //    Album alb = db.Albums.FirstOrDefault(a => a.NameAlbum == nameAlbum);
            //    return View(alb);
            //}

            HttpWebRequest tokenRequest = (HttpWebRequest)WebRequest.Create("http://ws.audioscrobbler.com/2.0/?method=album.getinfo&artist=" + nameArtistForRequest + "&album=" + nameAlbumForRequest + "&api_key=" + Configuration["apikey"] + "&format=json");
            HttpWebResponse tokenResponse = (HttpWebResponse)tokenRequest.GetResponse();
            string Result = new StreamReader(tokenResponse.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            Result = Result.Replace("#", "");
            dynamic ResultJson = JObject.Parse(Result);
            List<Track> tracks = new List<Track>();
            foreach (var tr in ResultJson.album.tracks.track)
            {
                Track track = new Track()
                {
                    Name = tr.name
                };
                tracks.Add(track);
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
                Tracks = tracks
            };
            db.Albums.Add(album);
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
                if (IsValidAlbum(cover, nameAlbum))
                {
                    Album album = new Album()
                    {
                        AlbumId = Guid.NewGuid(),
                        NameAlbum = nameAlbum,
                        NameArtist = name,
                        Cover = cover
                    };
                    db.Albums.Add(album);
                    topAlbums.Add(album);
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
            HttpWebRequest tokenRequest = (HttpWebRequest)WebRequest.Create("http://ws.audioscrobbler.com/2.0/?method=artist.gettopalbums&artist=" + nameForRequest + "&api_key=" + Configuration["apikey"] + "&limit=" + count + "&page=" + page + "&format=json");
            HttpWebResponse tokenResponse = (HttpWebResponse)tokenRequest.GetResponse();
            string Result = new StreamReader(tokenResponse.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            Result = Result.Replace("#", "");
            var nameAlbum = "";
            var cover = "";
            dynamic ResultJson = JObject.Parse(Result);
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
            return album;
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
                db.Tracks.Add(track);
                topTracks.Add(track);
            }
            return topTracks;
        }

        [HttpGet]
        public List<Similar> GetListSimilar(string name)
        {
            var nameForRequest = IsValidName(name);
            List<Similar> listSimilar = new List<Similar>();
            //if (db.Artists.FirstOrDefault(a => a.Name == nameForRequest).Similars != null)
            //{
            //    foreach (var sim in db.Artists.FirstOrDefault(a => a.Name == name).Similars)
            //    {
            //        listSimilar.Add(sim);
            //        return listSimilar;
            //    }
            //}
            HttpWebRequest tokenRequest = (HttpWebRequest)WebRequest.Create("http://ws.audioscrobbler.com/2.0/?method=artist.getsimilar&artist=" + nameForRequest + "&api_key=" + Configuration["apikey"] + "&limit=12" + "&format=json");
            HttpWebResponse tokenResponse = (HttpWebResponse)tokenRequest.GetResponse();
            string Result = new StreamReader(tokenResponse.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            Result = Result.Replace("#", "");
            dynamic ResultJson = JObject.Parse(Result);
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
            }
            return listSimilar;
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        }
    }
}
