using Hirealdoor.Models;

namespace Hirealdoor.Repositories;
public interface IUserRepository: IBaseRepository<User>
{
    Task<User?> FindByEmailAsync(string email);
}
