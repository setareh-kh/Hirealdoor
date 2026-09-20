using Hirealdoor.Models;

namespace Hirealdoor.Repositories.Repository;
public class LanguageRepository(SqlContext Context):BaseRepository<Language>(Context) , ILanguageRepository
{
    
}