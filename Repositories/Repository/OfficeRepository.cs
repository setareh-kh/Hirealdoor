using Hirealdoor.Models;

namespace Hirealdoor.Repositories.Repository;
public class OfficeRepository(SqlContext context): BaseRepository<Person>(context) , IPersonRepository
{
    
}