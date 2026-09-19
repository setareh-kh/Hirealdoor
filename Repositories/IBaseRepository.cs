using System.Linq.Expressions;
using Hirealdoor.Models;


namespace Hirealdoor.Repositories;

public interface IBaseRepository<TEntity> where TEntity : class, ISqlEntity
{
    Task InsertAsync(TEntity entity);
    Task<List<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(int id);
    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate);
    Task<List<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate);
    Task UpdateAsync(TEntity entity, TEntity newEntity);
    Task UpdateByIdAsync(TEntity newEntity);
    Task DeleteAsync(TEntity entity);
    Task DeleteByIdAsync(int id);
    Task<bool> SaveChangesAsync();
}