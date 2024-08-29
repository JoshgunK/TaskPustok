using Microsoft.AspNetCore.Mvc;
using Pustok.Business.Exceptions.CommonExceptions;
using Pustok.Business.Services.Interfaces;
using Pustok.Business.Utilities.Extentions;
using Pustok.Business.ViewModels;
using Pustok.Core.Models;
using Pustok.Core.Repositories;


namespace Pustok.MVCApp.Areas.Admin.Controllers;

[Area("Admin")]
public class BookController : Controller
{
    private readonly IGenreService _genreService;
    private readonly IAuthorService _authorService;
    private readonly IBookService _bookService;
    private readonly IBookImagesRepo _bookImagesRepo;
    private readonly IWebHostEnvironment _env;
    private readonly IBookRepo _bookRepo;

    public BookController(IGenreService genreService, IAuthorService authorService, IBookService bookService, IBookImagesRepo bookImagesRepo, IWebHostEnvironment env, IBookRepo bookRepo)
    {
        _genreService = genreService;
        _authorService = authorService;
        _bookService = bookService;
        _bookImagesRepo = bookImagesRepo;
        _env = env;
        _bookRepo = bookRepo;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _bookService.GetAllAsync(x => x.IsDeleted == false, "BookImages", "Genre", "Author"));
    }

    [HttpGet]
    public async Task<IActionResult> CreateAsync()
    {
        ViewBag.Genres = await _genreService.GetAllAsync(g => g.IsDeleted == false);
        ViewBag.Authors = await _authorService.GetAllAsync(a => a.IsDeleted == false);
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAsync(BookCreateVM vm)
    {
        ViewBag.Genres = await _genreService.GetAllAsync(g => g.IsDeleted == false);
        ViewBag.Authors = await _authorService.GetAllAsync(a => a.IsDeleted == false);

        if (!ModelState.IsValid) return View();

        try
        {
            await _bookService.CreateAsync(vm);
        }
        catch (EntityNotFoundException ex)
        {
            ModelState.AddModelError(ex.PropertyName, ex.Message);
            return View();
        }
        catch (FilevalidationException ex)
        {
            ModelState.AddModelError(ex.PropertyName, ex.Message);
            return View();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> UpdateAsync(int id)
    {
        ViewBag.Genres = await _genreService.GetAllAsync(g => g.IsDeleted == false);
        ViewBag.Authors = await _authorService.GetAllAsync(a => a.IsDeleted == false);
        Book existedBook = null;
        try
        {
            existedBook = await _bookService.GetByExpressionAsync(b => b.Id == id, "Genre", "BookImages", "Author");
        }
        catch (EntityNotFoundException)
        {
            return View("Error");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();
        }
        BookUpdateVM vm = new BookUpdateVM()
        {
            Name = existedBook.Name,
            Description = existedBook.Description,
            StockCount = existedBook.StockCount,
            SalePrice = existedBook.SalePrice,
            CostPrice = existedBook.CostPrice,
            DiscountPercent = existedBook.DiscountPercent,
            IsAvailable = existedBook.IsAvailable,
            ProductCode = existedBook.ProductCode,
            AuthorId = existedBook.AuthorId,
            GenreId = existedBook.GenreId,
            BookImages = existedBook.BookImages,
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateAsync(BookUpdateVM vm, int? id)
    {
        ViewBag.Genres = await _genreService.GetAllAsync(g => g.IsDeleted == false);
        ViewBag.Authors = await _authorService.GetAllAsync(a => a.IsDeleted == false);

        if (id < 1 || id is null) return View("Error");
        if (!ModelState.IsValid) return View();

        try
        {
            await _bookService.UpdateAsync(id.Value, vm);
        }
        catch (EntityNotFoundException ex)
        {
            ModelState.AddModelError(ex.PropertyName, ex.Message);
            return View();
        }
        catch (FilevalidationException ex)
        {
            ModelState.AddModelError(ex.PropertyName, ex.Message);
            return View();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();
        }

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete (int? id)
    {
        ViewBag.Genres = await _genreService.GetAllAsync(g => g.IsDeleted == false);
        ViewBag.Authors = await _authorService.GetAllAsync(a => a.IsDeleted == false);

        if (id < 1 || id is null) return View("Error");
        if (!ModelState.IsValid) return View();

        try
        {
            await _bookService.DeleteAsync(id.Value);
        }
        catch (EntityNotFoundException ex)
        {
            ModelState.AddModelError(ex.PropertyName, ex.Message);
            return View();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();
        }

        return RedirectToAction("Index");

    }



}
