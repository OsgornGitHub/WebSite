using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using WebSite.Models;

namespace WebSite.Con
{
    public class Api
    {

        public AppDbContext db;

        public Api(AppDbContext context)
        {
            db = context;
        }


        public bool Search(string nameTrack, string nameArtist)
        {
            var nameT = IsValidName(nameTrack);
            var nameA = IsValidName(nameArtist);
            string searchParameter = nameT + "_" + nameA + ".mp3";
            string[] filenames = Directory.GetFiles("D:\\WebSite\\WebSite\\tracks");
            foreach (string name in filenames)
            {
                if (name == searchParameter)
                {
                    return true;
                }
            }
            return false;
        }

        public void AddToDb(string nameTrack, string nameArtist)
        {
            if (db.Albums.FirstOrDefault(a => a.NameArtist == nameArtist) != null)
            {
                var album = db.Albums.FirstOrDefault(a => a.NameArtist == nameArtist);
                var tracks = album.Tracks.FirstOrDefault(tr => tr.Name == nameTrack);
                tracks.Link = "D:\\WebSite\\WebSite\\tracks" + nameTrack + "_" + nameArtist + ".mp3";
            }
            else
            {
                AddTrackToDb(nameTrack, nameArtist, "D:\\WebSite\\WebSite\\tracks" + nameTrack + "_" + nameArtist + ".mp3");
            }
        }

        public void AddTrackToDb(string nameTrack, string nameArtist, string link)
        {
            var nameT = IsValidName(nameTrack);
            var nameA = IsValidName(nameArtist);
            HttpWebRequest tokenRequest = (HttpWebRequest)WebRequest.Create("http://ws.audioscrobbler.com/2.0/?method=track.getInfo&artist=" + nameA + "&track=" + nameT + "&api_key=" + "1068375741deac644574d04838a37810" +  "&format=json");
            HttpWebResponse tokenResponse = (HttpWebResponse)tokenRequest.GetResponse();
            string Result = new StreamReader(tokenResponse.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            dynamic ResultJson = JObject.Parse(Result);
            var cover = "";
            foreach (dynamic dyn in ResultJson.track.album.image)
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
                Name = ResultJson.track.name,
                Cover = cover,
                Link = link
            };
            db.Tracks.Add(track);
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
    }
}
