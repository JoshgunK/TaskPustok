using Microsoft.EntityFrameworkCore;
using Pustok.Business.Exceptions.GenreExceptions;
using Pustok.Business.Services.Interfaces;
using Pustok.Business.ViewModels;
using Pustok.Core.Models;
using Pustok.Core.Repositories;
using System.Linq.Expressions;

namespace Pustok.Business.Services.Implementations;

public class GenreService : IGenreService
{
    private readonly IGenreRepo _genreRepo;

    public GenreService(IGenreRepo genreRepo)
    {
        _genreRepo = genreRepo;
    }
    public async Task CreateAsync(GenreCreateVM vm)
    {
        if(await _genreRepo.Table.AnyAsync(g=>g.Name.ToLower()==vm.Name.ToLower()))
        {
            throw new GenreAlreadyExistException("Name","Genre already exist");
        }

        var entity = new Genre() {
            Name = vm.Name,
            IsDeleted = false,
            CreatedDate = DateTime.Now,
            UpdatedDate = DateTime.Now,
        };
        await _genreRepo.CreateAsync(entity);
        await _genreRepo.CommitAsync();
    }

    public async Task DeleteAsync(int id)
    {
       var entity = await _genreRepo.GetByIdAsync(id);
        if (entity == null) { throw new NullReferenceException(); }
        
        _genreRepo.Delete(entity);
        await _genreRepo.CommitAsync();
    }

    public async Task<ICollection<Genre>> GetAllAsync(Expression<Func<Genre, bool>> expression)
    {
        return await _genreRepo.GetAll(expression).ToListAsync();
    }

    public async Task<Genre> GetByIdAsync(int id)
    {
        var entity = await _genreRepo.GetByIdAsync(id);
        if (entity == null) { throw new NullReferenceException(); }
        return entity;
    }

    public async Task Update(int id, GenreUpdateVM vm)
    {
        if (await _genreRepo.Table.AnyAsync(g => g.Name.ToLower() == vm.Name.ToLower() && g.Id !=id ))
        {
            throw new GenreAlreadyExistException("Name", "Genre already exist");
        }

        var entity = await _genreRepo.GetByIdAsync(id);
        if (entity == null) { throw new NullReferenceException(); }
        
        entity.Name = vm.Name;
        entity.UpdatedDate= DateTime.Now;
        
        await _genreRepo.CommitAsync();
    }
}
