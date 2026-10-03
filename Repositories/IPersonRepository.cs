using Hirealdoor.Dtos.Requests;
using Hirealdoor.DTos.Response;
using Hirealdoor.Models;

namespace Hirealdoor.Repositories;
public interface IPersonRepository:IBaseRepository<Person>
{
    Task<PaginateResponseDto<Person>> Filter(PersonIndexDto filter);

    Task<Person> SyncLanguages(Person person, string langIds);
}