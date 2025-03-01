﻿using Pustok.Business.ViewModels;
using Pustok.Core.Models;
using System.Linq.Expressions;

namespace Pustok.Business.Services.Interfaces;

public interface IBookService
{
    Task CreateAsync(BookCreateVM vm);
    Task UpdateAsync(int id, BookUpdateVM vm);
    Task<Book> GetByIdAsync(int id);
    Task DeleteAsync(int id);
    Task<ICollection<Book>> GetAllAsync(Expression<Func<Book, bool>> expression, params string[] includes);
    Task<Book> GetByExpressionAsync(Expression<Func<Book, bool>> expression, params string[] includes);
}
