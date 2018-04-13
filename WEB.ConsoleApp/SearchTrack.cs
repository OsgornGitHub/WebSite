using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using WEB.Models;

namespace WEB.ConsoleApp
{
    public class SearchTrack
    {

        public AppDbContext db;

        public SearchTrack(AppDbContext context)
        {
            db = context;
        }


        public List<string> Search()
        {


            List<string> tracks = new List<string>();
            string[] filenames = Directory.GetFiles("D:\\WebSite\\WebSite\\tracks");
            string searchParameter = "mp3";
            foreach (var file in filenames)
            {
                var splited = file.Split(".");
                if (splited[1] == searchParameter)
                {
                    tracks.Add(splited[0]);
                }
            }
            return tracks;
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

        public void Result(List<string> track)
        {
            List<string> tracks = new List<string>(); 
            var nameTrack = "";
            var nameArtist = "";
            tracks = track;
            foreach(var tr in tracks)
            {
                var splited = tr.Split("_");
                nameTrack = splited[0];
                nameArtist = splited[1];
                AddLinkToDb(nameTrack, nameArtist);
            }
        }


        public void AddLinkToDb(string nameTrack, string nameArtist)
        {
            if (db.Tracks.FirstOrDefault(a => a.Name == nameTrack) != null)
            {
                var track = db.Tracks.FirstOrDefault(a => a.Name == nameTrack);
                track.Link = "D:\\WebSite\\WEB\\Tracks" + nameTrack + "_" + nameArtist + ".mp3";
                db.Update(track);
                db.SaveChanges();
            }
            else
            {
                AddTrackToDb(nameTrack, nameArtist, "D:\\WebSite\\WEB\\Tracks" + nameTrack + "_" + nameArtist + ".mp3");
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
            db.SaveChanges();
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
