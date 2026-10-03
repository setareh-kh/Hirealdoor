using Hirealdoor.Models;

namespace Hirealdoor.Repositories.Repository;
public class LanguageRepository(SqlContext context):BaseRepository<Language>(context) , ILanguageRepository
{
    
}