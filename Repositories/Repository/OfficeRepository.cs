using Hirealdoor.Models;

namespace Hirealdoor.Repositories.Repository;
public class OfficeRepository(SqlContext context): BaseRepository<Office>(context), IOfficeRepository
{
    
}