using System.Linq.Expressions;
using Hirealdoor.Dtos.Requests;
using Hirealdoor.DTos.Response;
using Hirealdoor.Models;

namespace Hirealdoor.Repositories.Repository;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, ISqlEntity
{
    protected readonly SqlContext DbContext;

    protected BaseRepository(SqlContext sqlContext)
    {
        DbContext = sqlContext;
    }

    public async Task<PaginateResponseDto<TEntity>> Paginate(BaseFilterRequest filter,
        IQueryable<TEntity> queryable)
    {
        var page = filter.Page is > 0 ? filter.Page : 1;
        var pageSize = filter.Pager > 0 ? filter.Pager : 12;

        if (!string.IsNullOrWhiteSpace(filter.OrderBy))
        {
            var items = filter.OrderBy.Split(":");
            var orderBy = items[0];
            var desc = items[1];

            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderFunc =
                desc == "desc"
                    ? data => data.OrderByDescending(x => EF.Property<object>(x!, ToCamelCase(orderBy)))
                    : data => data.OrderBy(x => EF.Property<object>(x!, ToCamelCase(orderBy)));

            queryable = orderFunc(queryable);
        }
        else
        {
            queryable = queryable.OrderByDescending(x => EF.Property<object>(x!, ToCamelCase("Id")));
        }

        var res = await queryable
            .Skip((int)((page - 1) * pageSize))
            .Take((int)pageSize)
            .ToListAsync();

        var counts = 0;
        if (filter.Countable == true)
        {
            counts = await queryable.CountAsync();
        }

        return new PaginateResponseDto<TEntity>()
        {
            Items = res,
            Page = page,
            Pages = (int)Math.Ceiling(counts / (float)pageSize),
            Pager = pageSize,
            Total = counts
        };
    }

    //Create
    public async Task InsertAsync(TEntity entity)
    {
        await DbContext.Set<TEntity>().AddAsync(entity);
    }

    //Read
    public async Task<List<TEntity>> GetAllAsync()
    {
        return await DbContext.Set<TEntity>().ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(int id)
    {
        return await DbContext.Set<TEntity>().FindAsync(id);
    }

    public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await DbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
    }

    public async Task<List<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await DbContext.Set<TEntity>().Where(predicate).ToListAsync();
    }

    //Update 
    public async Task UpdateAsync(TEntity entity, TEntity newEntity)
    {
        DbContext.Entry(entity).CurrentValues.SetValues(newEntity);
    }

    public async Task UpdateByIdAsync(TEntity newEntity)
    {
        var entity = await DbContext.Set<TEntity>().FindAsync(newEntity.Id);
        if (entity != null) await UpdateAsync(entity, newEntity);
    }

    //Delete
    public async Task DeleteAsync(TEntity entity)
    {
        DbContext.Set<TEntity>().Remove(entity);
    }

    public async Task DeleteByIdAsync(int id)
    {
        var entity = await DbContext.Set<TEntity>().FindAsync(id);
        if (entity != null) await DeleteAsync(entity);
    }

    public async Task<int> WhereDeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var entities = await DbContext.Set<TEntity>().Where(predicate).ToListAsync();
        DbContext.Set<TEntity>().RemoveRange(entities);
        return await DbContext.SaveChangesAsync();
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate) =>
        await DbContext.Set<TEntity>().CountAsync(predicate);

    public async Task<long> SumAsync(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, long>> selector) =>
        await DbContext.Set<TEntity>()
            .Where(predicate)
            .SumAsync(selector);

    //other common operations
    public async Task<bool> SaveChangesAsync() => await DbContext.SaveChangesAsync() > 0;

    public string ToCamelCase(string s)
    {
        if (s.Length < 2) return s.ToLower();
        return char.ToUpper(s[0]) + s[1..];
    }
}