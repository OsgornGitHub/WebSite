using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WebSite.Models;
using HtmlAgilityPack;
using System.Text;

namespace WebSite.Controllers
{
    public class HomeController : Controller
    {
        private List<Person> SearchList = new List<Person>();
        public IActionResult Index()
        {
            List<string> ListNames = new List<string>();
            List<string> ListPhoto = new List<string>();
            var website = new HtmlWeb();
            website.OverrideEncoding = Encoding.GetEncoding("utf-8");
            var doc = website.Load("https://www.last.fm/ru/tag/top+100/artists");

            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
            {
                HtmlAttribute att = link.Attributes["href"];
                ListNames.Add(att.Value);
            }
            var model = new PersonViewModel();
            foreach (var name in ListNames)
            {
                OnePerson newPerson = new OnePerson()
                {
                    Name = name
                };
                model.OnePersons.Add(newPerson);
            }
            return View(model);
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
