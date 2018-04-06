using System;
using NLayerApp.BLL.DTO;
using NLayerApp.DAL.Entities;
using NLayerApp.DAL.Interfaces;
using NLayerApp.BLL.Infrastructure;
using NLayerApp.BLL.Interfaces;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;

namespace NLayerApp.BLL.Services
{
    public class ArtistService : IArtistService
    {

        IUnitOfWork Database { get; set; }

        public ArtistService(IUnitOfWork uow)
        {
            Database = uow;
        }
        public void AddArtistToDb(ArtistDTO artist)
        {
            Artist art = new Artist()
            {
                ArtistFk = artist.ArtistFk,
                Name = artist.Name,
                Photo = artist.Photo,
                Biography = artist.Biography
            };
            Database.Artists.Create(art);
            throw new NotImplementedException();
        }



        public IEnumerable<ArtistDTO> GetTopArtistLFM(int page, int count)
        {
            List<ArtistDTO> list = new List<ArtistDTO>();
            HttpWebRequest tokenRequest = (HttpWebRequest)WebRequest.Create("http://ws.audioscrobbler.com/2.0/?method=geo.gettopartists&country=spain&api_key=" + "1068375741deac644574d04838a37810" + " &page=" + page + "&limit=" + count + "&format=json");
            HttpWebResponse tokenResponse = (HttpWebResponse)tokenRequest.GetResponse();
            string Result = new StreamReader(tokenResponse.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            Result = Result.Replace("#", "");
            dynamic ResultJson = JObject.Parse(Result);
            for (int i = 0; i < count; i++)
            {
                string name = ResultJson.topartists.artist[i].name;
                string bio = ResultJson.artist.bio.content;
                string photo = "";
                foreach (dynamic dyn in ResultJson.topartists.artist[i].image)
                {
                    if (dyn.size == "mega")
                    {
                        photo = dyn.text;
                        break;
                    }
                }
                ArtistDTO artist = new ArtistDTO()
                {
                    ArtistFk = Guid.NewGuid(),
                    Name = name,
                    Photo = photo,
                    Biography = bio
                };
                AddArtistToDb(artist);
                list.Add(artist);
            }
            return list;
        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }

    }
}
