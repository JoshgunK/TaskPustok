using Microsoft.AspNetCore.Mvc;
using Pustok.Business.Services.Interfaces;
using System.Diagnostics;

namespace Pustok.MVCApp.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly IGenreService _genreService;

        public HomeController(IGenreService genreService)
        {
            _genreService = genreService;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
