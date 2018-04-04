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
namespace WebSite.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //var client = new LastfmClient("1068375741deac644574d04838a37810", "41b9c11c92392649a91830a740cc3e2f");
            //var response = client.Chart.GetTopArtistsAsync();
            //var result = response.Status;
            //ViewBag.status = result;
            List<OnePerson> list = new List<OnePerson>();
            list = GetNextPage(1, 24);
            var model = new PersonViewModel { OnePersons = list };

            return View(model);
        }
        [HttpGet]
        public List<OnePerson> GetNextPage(int page, int count)
        {
            List<OnePerson> list = new List<OnePerson>();
            HttpWebRequest tokenRequest = (HttpWebRequest)WebRequest.Create("http://ws.audioscrobbler.com/2.0/?method=geo.gettopartists&country=spain&api_key=1068375741deac644574d04838a37810&page=" + page + "&limit=" + count + "&format=json");
            HttpWebResponse tokenResponse = (HttpWebResponse)tokenRequest.GetResponse();
            string Result = new StreamReader(tokenResponse.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            Result = Result.Replace("#", "");
            ViewBag.respond = Result;
            dynamic ResultJson = JObject.Parse(Result);
            for (int i = 0; i < count; i++)
            {
                string name = ResultJson.topartists.artist[i].name;
                string photo = "";
                foreach (dynamic dyn in ResultJson.topartists.artist[i].image)
                {
                    if (dyn.size == "mega")
                    {
                        photo = dyn.text;
                        break;
                    }
                }
                OnePerson artist = new OnePerson
                {
                    Name = name,
                    Photo = photo
                };
                list.Add(artist);
            }
            return list;
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
