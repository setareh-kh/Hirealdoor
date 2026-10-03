using Hirealdoor.Dtos.Requests;
using Hirealdoor.Models;

namespace Hirealdoor.Services;

public interface IUserService: IBaseService<User>
{
    Task<User> RegisterEmailAsync(EmailRequestDto emailRequest);
    Task<bool> VerifyEmailAsync(VerifyEmailDto verifyEmailDto);
    Task<User> SelectUserTypeAsync(UserTypeDto userTypeDto);
}