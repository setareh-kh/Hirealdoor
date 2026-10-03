using Hirealdoor.Models;
using Microsoft.EntityFrameworkCore;

namespace Hirealdoor.Repositories.Repository;

public class UserRepository(SqlContext context): BaseRepository<User>(context), IUserRepository
{
    public async Task<User?> FindByEmailAsync(string email)
    {
        return await DbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
    }
}
