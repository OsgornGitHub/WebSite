using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLayerApp.BLL.Interfaces;
using NLayerApp.BLL.DTO;
using NLayerApp.WEB.Models;
using AutoMapper;
using NLayerApp.BLL.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace NLayerApp.WEB.Controllers
{
    public class HomeController : Controller
    {
        IArtistService artistService;
        public HomeController(IArtistService serv)
        {
            artistService = serv;
        }
        public ActionResult Index()
        {
            IEnumerable<ArtistDTO> artistDtos = artistService.GetTopArtistLFM(1, 24);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ArtistDTO, ArtistViewModel>()).CreateMapper();
            var artists = mapper.Map<IEnumerable<ArtistDTO>, List<ArtistViewModel>>(artistDtos);
            return View(artists);
        }
    }
}
