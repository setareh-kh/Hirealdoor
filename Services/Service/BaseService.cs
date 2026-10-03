using System.Linq.Expressions;
using Hirealdoor.DTos.Response;
using Hirealdoor.Models;
using Hirealdoor.Repositories;

namespace Hirealdoor.Services.Service;

public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class, ISqlEntity
{
    private readonly IBaseRepository<TEntity> _repository;

    protected BaseService(IBaseRepository<TEntity> repository)
    {
        _repository = repository;
    }

    protected static StandardResponseDto NotFound() =>
        new() { Success = false, Object = false, Message = "NotFound" };

    protected static StandardResponseDto Success(object obj) =>
        new() { Success = true, Object = obj };

    public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate) =>
        await _repository.FindAsync(predicate);

    public async Task<List<TEntity>> WhereAsync(
        Expression<Func<TEntity, bool>> predicate) =>
        await _repository.WhereAsync(predicate);

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate) =>
        await _repository.CountAsync(predicate);

    public async Task<bool> SaveChanges() =>
        await _repository.SaveChangesAsync();

    public async Task<IEnumerable<TEntity>?> GetAll() =>
        await _repository.GetAllAsync();

    public async Task<TEntity?> GetAsync(int id) =>
        await _repository.GetByIdAsync(id);
}