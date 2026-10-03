using Hirealdoor.Dtos.Requests;
using Hirealdoor.DTos.Response;
using Hirealdoor.Models;

namespace Hirealdoor.Services;

public interface IUserService: IBaseService<User>
{
    Task<RegisterResponseDto> RegisterEmailAsync(EmailRequestDto emailRequest);
    Task<User> SelectUserTypeAsync(UserTypeDto userTypeDto);
}