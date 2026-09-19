using Hirealdoor.Models;

namespace Hirealdoor.Repositories.Repository;
public class PersonRepository(SqlContext context): BaseRepository<Person>(context) , IPersonRepository
{
    
}