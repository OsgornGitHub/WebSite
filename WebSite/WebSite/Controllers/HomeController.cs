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
            HttpWebRequest tokenRequest = (HttpWebRequest)WebRequest.Create("http://ws.audioscrobbler.com/2.0/?method=geo.gettopartists&country=spain&api_key=1068375741deac644574d04838a37810&page=" + page + "&limit=" + count);
            HttpWebResponse tokenResponse = (HttpWebResponse)tokenRequest.GetResponse();
            string Result = new StreamReader(tokenResponse.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            ViewBag.respond = Result;
            string[] ListArtist = Result.Split("<artist>");
            foreach (var person in ListArtist)
            {
                if (Result.IndexOf(person) == 0)
                {
                    continue;
                }
                var name = "";
                var imageLink = "";
                for (var i = person.IndexOf("<name>") + 6; i < person.IndexOf("</name>"); i++)
                {
                    name += person[i];
                }
                for (var i = person.IndexOf("<image size=\"mega\">") + 19; i < person.IndexOf("</image>\n</artist>"); i++)
                {
                    imageLink += person[i];
                }
                OnePerson artist = new OnePerson
                {
                    Name = name,
                    Photo = imageLink
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
