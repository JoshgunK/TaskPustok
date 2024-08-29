using Microsoft.AspNetCore.Mvc;
using Pustok.Business.Exceptions.GenreExceptions;
using Pustok.Business.Services.Interfaces;
using Pustok.Business.ViewModels;


namespace Pustok.MVCApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GenreController : Controller
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _genreService.GetAllAsync(null));
        }

        [HttpGet]
        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GenreCreateVM vm)
        {
            if (!ModelState.IsValid) 
            {                
                return View(vm);
            }

            try
            {
                await _genreService.CreateAsync(vm);
            }
            catch(GenreAlreadyExistException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View(vm);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(vm);
            }          
           
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var data = await _genreService.GetByIdAsync(id);
            if (data == null) throw new NullReferenceException();

            var genrevm = new GenreUpdateVM()
            {
                Name = data.Name,
            };

            return View(genrevm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, GenreUpdateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            try
            {
                await _genreService.Update(id, vm);
            }
            catch (GenreAlreadyExistException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View(vm);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(vm);
            }

            

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            var data = await _genreService.GetByIdAsync(id);
            if (data == null) return NotFound(); 

            await _genreService.DeleteAsync(id); 

            return RedirectToAction("Index");
        }



    }
}
