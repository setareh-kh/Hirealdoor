using Hirealdoor.Dtos.Requests;

namespace Hirealdoor.Services;
public interface IPersonService
{
    Task<bool> CreatePersonAsync(CreatePersonDto createPersonDto);
}