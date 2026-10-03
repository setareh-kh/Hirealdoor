using Hirealdoor.Dtos.Requests;
using Hirealdoor.DTos.Response;
using Hirealdoor.Models;

namespace Hirealdoor.Services;

public interface IPersonService
{
    Task<PaginateResponseDto<PersonResponseDto>> Filter(PersonIndexDto filter);

    Task<bool> CreatePersonAsync(CreatePersonDto createPersonDto);
}