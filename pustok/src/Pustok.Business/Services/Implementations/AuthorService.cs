using Microsoft.EntityFrameworkCore;
using Pustok.Business.Exceptions.AuthorExceptions;
using Pustok.Business.Exceptions.CommonExceptions;
using Pustok.Business.Services.Interfaces;
using Pustok.Business.ViewModels;
using Pustok.Core.Models;
using Pustok.Core.Repositories;
using System.Linq.Expressions;

namespace Pustok.Business.Services.Implementations;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepo _authorRepo;

    public AuthorService(IAuthorRepo authorRepo)
    {
       _authorRepo = authorRepo;
    }
    public async Task CreateAsync(AuthorCreateVM vm)
    {
        if(string.IsNullOrEmpty(vm.Fullname))
        {
            throw new AuthorFullNameException("Fullname","Author fullname can not be empty");
        }
        var data = new Author() {

            Fullname = vm.Fullname,
            CreatedDate = DateTime.Now,
            UpdatedDate = DateTime.Now,
            IsDeleted = vm.IsDeleted,
                   
        };
        await _authorRepo.CreateAsync(data);
        await _authorRepo.CommitAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var data = await _authorRepo.GetByIdAsync(id);
        if (data is null) 
        {
            throw new EntityNotFoundException("Author not found");
        }
        _authorRepo.Delete(data);
        await _authorRepo.CommitAsync();
    }

    public async Task<ICollection<Author>> GetAllAsync(Expression<Func<Author, bool>> expression)
    {
        return await _authorRepo.GetAll(expression).ToListAsync();
    }

    public async Task<Author> GetByIdAsync(int id)
    {
        if (id <= 0 || id == null)
        {
            throw new IdIsNotValid("Id is not valid");
        }
        return await _authorRepo.GetByIdAsync(id);
    }

    public async Task Update(int id, AuthorUpdateVM vm)
    {
        if(id<=0 || id==null)
        {
            throw new IdIsNotValid("Id is not valid");
        }
        if (string.IsNullOrEmpty(vm.Fullname))
        {
            throw new AuthorFullNameException("Fullname", "Author fullname can not be empty");
        }

        var data = await _authorRepo.GetByIdAsync(id);
        if (data is null)
        {
            throw new EntityNotFoundException("Author not found");
        }
        
        data.Fullname = vm.Fullname;
        data.UpdatedDate = DateTime.Now;

        await _authorRepo.CommitAsync();
    }
}
