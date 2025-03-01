﻿using Pustok.Business.ViewModels;
using Pustok.Core.Models;
using System.Linq.Expressions;

namespace Pustok.Business.Services.Interfaces;

public interface IGenreService
{
    Task CreateAsync (GenreCreateVM vm);
    Task Update(int id, GenreUpdateVM vm);
    Task<Genre> GetByIdAsync(int id);
    Task DeleteAsync(int id);
    Task<ICollection<Genre>> GetAllAsync(Expression<Func<Genre,bool>> expression);
}
