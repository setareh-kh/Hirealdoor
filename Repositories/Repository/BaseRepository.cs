using System.Linq.Expressions;
using Hirealdoor.Models;
using Microsoft.EntityFrameworkCore;

namespace Hirealdoor.Repositories.Repository;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, ISqlEntity 
{
    protected readonly SqlContext dbContext;
    protected BaseRepository(SqlContext sqlContext)
    {
        dbContext = sqlContext;
    }
    //Create
    public async Task InsertAsync(TEntity entity)
    {
        await dbContext.Set<TEntity>().AddAsync(entity);
    }
    //Read
    public async Task<List<TEntity>> GetAllAsync()
    {
        return await dbContext.Set<TEntity>().ToListAsync();
    }
    public async Task<TEntity?> GetByIdAsync(int id)
    {
        return await dbContext.Set<TEntity>().FindAsync(id);
    }
     public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
    }
    public async Task<List<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await dbContext.Set<TEntity>().Where(predicate).ToListAsync();
    }
    //Update 
     public async Task UpdateAsync(TEntity entity, TEntity newEntity)
    {
        dbContext.Entry(entity).CurrentValues.SetValues(newEntity);
    }
    public async Task UpdateByIdAsync(TEntity newEntity)
    {
        var entity = await dbContext.Set<TEntity>().FindAsync(newEntity.Id);
       if (entity != null) await UpdateAsync(entity,newEntity);

    }
    //Delete
    public async Task DeleteAsync(TEntity entity)
    {
        dbContext.Set<TEntity>().Remove(entity);
    }
    public async Task DeleteByIdAsync(int id)
    {
        var entity = await dbContext.Set<TEntity>().FindAsync(id);
        if (entity != null) await DeleteAsync(entity);
    }
    //other common operations
    public async Task<bool> SaveChangesAsync() => await dbContext.SaveChangesAsync() > 0;

    

}
