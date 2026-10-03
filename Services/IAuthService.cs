using Hirealdoor.Dtos.Requests;
using Hirealdoor.Models;

namespace Hirealdoor.Services;

public interface IAuthService
{
    Task<User?> RegisterEmailAsync(EmailRequestDto emailDto);
    Task<bool> VerifyEmailAsync(VerifyEmailDto verifyEmailDto);
    Task<bool> Login(EmailRequestDto emailDto);
    Task<User?> RegLogin(EmailRequestDto emailDto);

}