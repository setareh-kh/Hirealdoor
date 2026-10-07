using Hirealdoor.Models;

namespace Hirealdoor.Repositories.Repository;
public class TokenRepository(SqlContext context):BaseRepository<Token>(context) , ITokenRepository
{
    
}