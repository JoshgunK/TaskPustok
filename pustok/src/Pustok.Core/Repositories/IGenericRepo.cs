using Microsoft.EntityFrameworkCore;
using Pustok.Core.Models;
using System.Linq.Expressions;

namespace Pustok.Core.Repositories;

public interface IGenericRepo<TEntity> where TEntity : BaseEntity, new()
{
    public DbSet<TEntity> Table {  get; }
    Task CreateAsync(TEntity entity);
    Task<TEntity> GetByIdAsync(int? id, params string[] includes);
    Task<TEntity> GetByExpressionAsync(Expression<Func<TEntity, bool>> expression, params string[] includes);
    IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression, params string[] includes);
    void Delete(TEntity entity);
    Task<int> CommitAsync();
}
