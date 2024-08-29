using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Pustok.Business.Exceptions.CommonExceptions;
using Pustok.Business.Services.Interfaces;
using Pustok.Business.Utilities.Extentions;
using Pustok.Business.ViewModels;
using Pustok.Core.Models;
using Pustok.Core.Repositories;
using System.Drawing;
using System.Linq.Expressions;

namespace Pustok.Business.Services.Implementations;

public class BookService : IBookService
{
    private readonly IBookRepo _bookRepo;
    private readonly IGenreRepo _genreRepo;
    private readonly IAuthorRepo _authorRepo;
    private readonly IBookImagesRepo _bookImagesRepo;
    private readonly IWebHostEnvironment _env;

    public BookService(IBookRepo bookRepo, IGenreRepo genreRepo, IAuthorRepo authorRepo,
            IBookImagesRepo bookImagesRepo, IWebHostEnvironment env)
    {
        _bookRepo = bookRepo;
        _genreRepo = genreRepo;
        _authorRepo = authorRepo;
        _bookImagesRepo = bookImagesRepo;
        _env = env;
    }
    public async Task CreateAsync(BookCreateVM vm)
    {
        if (await _genreRepo.Table.AllAsync(g => g.Id != vm.GenreId))
        {
            throw new EntityNotFoundException("GenreId", "Genre is not found");

        }

        if (await _authorRepo.Table.AllAsync(a => a.Id != vm.AuthorId))
        {
            throw new EntityNotFoundException("AuthorId", "Author is not found");
        }


        Book book = new Book()
        {
            Name = vm.Name,
            Description = vm.Description,
            StockCount = vm.StockCount,
            SalePrice = vm.SalePrice,
            CostPrice = vm.CostPrice,
            DiscountPercent = vm.DiscountPercent,
            IsAvailable = vm.IsAvailable,
            IsDeleted = false,
            ProductCode = vm.ProductCode,
            AuthorId = vm.AuthorId,
            GenreId = vm.GenreId,
        };

        if (vm.PosterImage is not null)
        {
            if (vm.PosterImage.ContentType != "image/jpeg" && vm.PosterImage.ContentType != "image/png")
            {
                throw new FilevalidationException("PosterImage", "Content type must be png or jpeg");
            }
            if (vm.PosterImage.Length > 2 * 1024 * 1024)
            {
                throw new FilevalidationException("PosterImage", "Image size must be less than 2 MB");
            }
            BookImages bookImage = new BookImages()
            {
                //FileManager.SaveFile(_env.WebRootPath, "uploads/books", vm.PosterImage); - bu asagidaki kodla eyni isi gorur,savefile metodunun da iki tipin qeyd etmisem, extentionlu ve adi.
                ImageURL = vm.PosterImage.SaveFile(_env.WebRootPath, "uploads/books"),
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                IsDeleted = false,
                IsPoster = true,
                Book = book,
            };
            await _bookImagesRepo.CreateAsync(bookImage);
        }

        if (vm.HoverImage is not null)
        {
            if (vm.HoverImage.ContentType != "image/jpeg" && vm.HoverImage.ContentType != "image/png")
            {
                throw new FilevalidationException("HoverImage", "Content type must be png or jpeg");
            }
            if (vm.HoverImage.Length > 2 * 1024 * 1024)
            {
                throw new FilevalidationException("HoverImage", "Image size must be less than 2 MB");
            }
            BookImages bookImage = new BookImages()
            {
                //FileManager.SaveFile(_env.WebRootPath, "uploads/books", vm.PosterImage); - bu asagidaki kodla eyni isi gorur,savefile metodunun da iki tipin qeyd etmisem, extentionlu ve adi.
                ImageURL = vm.HoverImage.SaveFile(_env.WebRootPath, "uploads/books"),
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                IsDeleted = false,
                IsPoster = false,
                Book = book
            };
            await _bookImagesRepo.CreateAsync(bookImage);
        }

        if (vm.Images.Count > 0)
        {
            foreach (var image in vm.Images)
            {
                if (image.ContentType != "image/jpeg" && image.ContentType != "image/png")
                {
                    throw new FilevalidationException("Images", "Content type must be png or jpeg");
                }
                if (image.Length > 2 * 1024 * 1024)
                {
                    throw new FilevalidationException("Images", "Image size must be less than 2 MB");
                }
                BookImages bookImage = new BookImages()
                {
                    //FileManager.SaveFile(_env.WebRootPath, "uploads/books", vm.PosterImage); - bu asagidaki kodla eyni isi gorur,savefile metodunun da iki tipin qeyd etmisem, extentionlu ve adi.
                    ImageURL = image.SaveFile(_env.WebRootPath, "uploads/books"),
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    IsDeleted = false,
                    IsPoster = null,
                    Book = book
                };
                await _bookImagesRepo.CreateAsync(bookImage);
            }
        }

        await _bookRepo.CreateAsync(book);
        await _bookRepo.CommitAsync();
    }
      
    public async Task<ICollection<Book>> GetAllAsync(Expression<Func<Book, bool>> expression, params string[] includes)
    {
        return await _bookRepo.GetAll(expression, includes).ToListAsync();
    }

    public async Task<Book> GetByExpressionAsync(Expression<Func<Book, bool>> expression, params string[] includes)
    {
        var data = await _bookRepo.GetByExpressionAsync(expression, includes);
        if (data == null) { throw new EntityNotFoundException("Book not found"); }
        return data;
    }

    public async Task<Book> GetByIdAsync(int id)
    {
        var data = await _bookRepo.GetByIdAsync(id);
        if (data == null) { throw new EntityNotFoundException("Book not found"); }
        return data;
    }

    public async Task UpdateAsync(int id, BookUpdateVM vm)
    {
        if (await _genreRepo.Table.AllAsync(g => g.Id != vm.GenreId))
        {
            throw new EntityNotFoundException("GenreId", "Genre is not found");

        }

        if (await _authorRepo.Table.AllAsync(a => a.Id != vm.AuthorId))
        {
            throw new EntityNotFoundException("AuthorId", "Author is not found");
        }

        var existBook = await _bookRepo.GetByExpressionAsync(b => b.Id == id, "BookImages", "Genre", "Author");
        if (existBook == null)
        {
            throw new EntityNotFoundException("BookId", "Book not found");
        }

        existBook.Name = vm.Name;
        existBook.Description = vm.Description;
        existBook.StockCount = vm.StockCount;
        existBook.SalePrice = vm.SalePrice;
        existBook.CostPrice = vm.CostPrice;
        existBook.DiscountPercent = vm.DiscountPercent;
        existBook.IsAvailable = vm.IsAvailable;
        existBook.IsDeleted = false;
        existBook.ProductCode = vm.ProductCode;
        existBook.AuthorId = vm.AuthorId;
        existBook.GenreId = vm.GenreId;


        if (vm.PosterImage is not null)
        {
            if (vm.PosterImage.ContentType != "image/jpeg" && vm.PosterImage.ContentType != "image/png")
            {
                throw new FilevalidationException("PosterImage", "Content type must be png or jpeg");
            }
            if (vm.PosterImage.Length > 2 * 1024 * 1024)
            {
                throw new FilevalidationException("PosterImage", "Image size must be less than 2 MB");
            }
            BookImages bookImage = new BookImages()
            {
                //FileManager.SaveFile(_env.WebRootPath, "uploads/books", vm.PosterImage); - bu asagidaki kodla eyni isi gorur,savefile metodunun da iki tipin qeyd etmisem, extentionlu ve adi.
                ImageURL = vm.PosterImage.SaveFile(_env.WebRootPath, "uploads/books"),
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                IsDeleted = false,
                IsPoster = true,
                BookId = existBook.Id,
            };
            await _bookImagesRepo.CreateAsync(bookImage);
            _bookImagesRepo.Delete(existBook.BookImages.FirstOrDefault(bi => bi.IsPoster == true));
            existBook.BookImages.FirstOrDefault(bi => bi.IsPoster == true)?.ImageURL.DeleteFile(_env.WebRootPath, "uploads/books");

        }

        if (vm.HoverImage is not null)
        {
            if (vm.HoverImage.ContentType != "image/jpeg" && vm.HoverImage.ContentType != "image/png")
            {
                throw new FilevalidationException("HoverImage", "Content type must be png or jpeg");
            }
            if (vm.HoverImage.Length > 2 * 1024 * 1024)
            {
                throw new FilevalidationException("HoverImage", "Image size must be less than 2 MB");
            }
            BookImages bookImage = new BookImages()
            {
                //FileManager.SaveFile(_env.WebRootPath, "uploads/books", vm.PosterImage); - bu asagidaki kodla eyni isi gorur,savefile metodunun da iki tipin qeyd etmisem, extentionlu ve adi.
                ImageURL = vm.HoverImage.SaveFile(_env.WebRootPath, "uploads/books"),
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                IsDeleted = false,
                IsPoster = false,
                BookId = existBook.Id,
            };
            await _bookImagesRepo.CreateAsync(bookImage);

            _bookImagesRepo.Delete(existBook.BookImages.FirstOrDefault(bi => bi.IsPoster == false));
            existBook.BookImages.FirstOrDefault(bi => bi.IsPoster == false)?.ImageURL.DeleteFile(_env.WebRootPath, "uploads/books");
        }

        if (vm.BookImagesIds != null)
        {
            foreach (var item in existBook.BookImages.Where(bi => !vm.BookImagesIds.Exists(bid => bi.Id == bid) && bi.IsPoster == null))
            {
                item.ImageURL.DeleteFile(_env.WebRootPath, "uploads/books");
            }
            existBook.BookImages.RemoveAll(bi => !vm.BookImagesIds.Exists(bid => bi.Id == bid) && bi.IsPoster == null);
            //existBook.BookImages.RemoveAll(bi=> !vm.BookImagesIds.Contains(bi.Id) && bi.IsPoster==null);- yuxaridaki ile eynidir.
        }
        else
        {
            foreach (var item in existBook.BookImages.Where(bi => bi.IsPoster == null))
            {
                item.ImageURL.DeleteFile(_env.WebRootPath, "uploads/books");
            }
            existBook.BookImages.RemoveAll(bi => bi.IsPoster == null);
        }

        if (vm.Images is not null)
        {
            if (vm.Images.Count > 0)
            {
                foreach (var image in vm.Images)
                {
                    if (image.ContentType != "image/jpeg" && image.ContentType != "image/png")
                    {
                        throw new FilevalidationException("Images", "Content type must be png or jpeg");
                    }
                    if (image.Length > 2 * 1024 * 1024)
                    {
                        throw new FilevalidationException("Images", "Image size must be less than 2 MB");
                    }
                    BookImages bookImage = new BookImages()
                    {
                        //FileManager.SaveFile(_env.WebRootPath, "uploads/books", vm.PosterImage); - bu asagidaki kodla eyni isi gorur,savefile metodunun da iki tipin qeyd etmisem, extentionlu ve adi.
                        ImageURL = image.SaveFile(_env.WebRootPath, "uploads/books"),
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        IsDeleted = false,
                        IsPoster = null,
                        BookId = existBook.Id,
                    };
                    await _bookImagesRepo.CreateAsync(bookImage);
                    existBook.BookImages.Add(bookImage);
                }

            }
        }
        await _bookRepo.CommitAsync();
    } 
    public async Task DeleteAsync(int id)
    {
        
        var existBook = await _bookRepo.GetByExpressionAsync(b => b.Id == id, "BookImages", "Genre", "Author");
               
        if (existBook == null)
        {
            throw new EntityNotFoundException("BookId", "Book not found");
        }
            
        var posterImage = existBook.BookImages.FirstOrDefault(bi => bi.IsPoster == true);
        if (posterImage != null)
        {
            posterImage.ImageURL.DeleteFile(_env.WebRootPath, "uploads/books");
            _bookImagesRepo.Delete(posterImage);
        }

        var hoverImage = existBook.BookImages.FirstOrDefault(bi => bi.IsPoster == false);
        if (hoverImage != null)
        {
            hoverImage.ImageURL.DeleteFile(_env.WebRootPath, "uploads/books");
            _bookImagesRepo.Delete(hoverImage);
        }

        foreach (var image in existBook.BookImages.Where(bi => bi.IsPoster == null).ToList())
        {
            image.ImageURL.DeleteFile(_env.WebRootPath, "uploads/books");
            _bookImagesRepo.Delete(image);
        }

         _bookRepo.Delete(existBook);    
        await _bookRepo.CommitAsync();
    }


}
