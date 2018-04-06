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
            //var client = new LastfmClient("1068375741deac644574d04838a37810", "41b9c11c92392649a91830a740cc3e2f");
            //var response = client.Chart.GetTopArtistsAsync();
            //var result = response.Status;
            //ViewBag.status = result;
            List<OnePerson> list = new List<OnePerson>();
            var page = 1;
            var count = 24;
            list = GetNextPage(page, count);
            var model = new PersonViewModel { OnePersons = list };
            return View(model);
        }
        [HttpPost]
        public IActionResult GetJson(int page, int count)
        {
            var list = GetNextPage(page, count);
            return  Json(list);
        }

        [HttpGet]
        public List<OnePerson> GetNextPage(int page, int count)
        {
            List<OnePerson> list = new List<OnePerson>();
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
