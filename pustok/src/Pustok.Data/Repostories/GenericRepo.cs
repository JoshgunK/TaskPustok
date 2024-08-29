using Microsoft.EntityFrameworkCore;
using Pustok.Core.Models;
using Pustok.Core.Repositories;
using Pustok.Data.DAL;
using System.Linq.Expressions;

namespace Pustok.Data.Repostories;

public class GenericRepo<TEntity> : IGenericRepo<TEntity> where TEntity : BaseEntity, new()
{
    private readonly AppDBContext _context;

    public GenericRepo(AppDBContext context)
    {
        _context = context;
    }

    public DbSet<TEntity> Table => _context.Set<TEntity>();
    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Delete(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
    }

    public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression, params string[] includes)
    {
        var query = _context.Set<TEntity>().AsQueryable();
        if (includes.Length > 0)
        {
            foreach (string include in includes)
            {
                query = query.Include(include);
            }
        }

        return expression is not null
            ?  query.Where(expression)
            :  query;
    }

    public async Task<TEntity> GetByExpressionAsync(Expression<Func<TEntity, bool>> expression, params string[] includes)
    {
        var query = _context.Set<TEntity>().AsQueryable();
        if (includes.Length > 0)
        {
            foreach (string include in includes)
            {
                query = query.Include(include);
            }
        }

        return expression is not null
            ? await query.Where(expression).FirstOrDefaultAsync()
            : await query.FirstOrDefaultAsync();     

    }

    public async Task<TEntity> GetByIdAsync(int? id, params string[] includes)
    {
        var query = _context.Set<TEntity>().AsQueryable();
        if (includes.Length > 0)
        {
            foreach (string include in includes) 
            {
                query = query.Include(include);
            }
        }
        return await query.FirstOrDefaultAsync(q=>q.Id==id);
    }

    public async Task CreateAsync(TEntity entity)
    {
        await _context.AddAsync(entity);
    }
}
