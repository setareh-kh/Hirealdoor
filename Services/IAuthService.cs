using Hirealdoor.Dtos.Requests;
using Hirealdoor.DTos.Response;
using Hirealdoor.Models;

namespace Hirealdoor.Services;

public interface IAuthService
{
    Task<RegisterResponseDto> RegisterEmailAsync(EmailRequestDto emailDto);
    Task<bool> VerifyEmailAsync(VerifyEmailDto verifyEmailDto);
    Task<StandardResponseDto> Login(EmailRequestDto emailDto);
    Task<RegisterResponseDto> RegLogin(EmailRequestDto emailDto);

}