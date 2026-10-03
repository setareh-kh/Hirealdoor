using Hirealdoor.Dtos.Requests;
using Hirealdoor.Services;
using Hirealdoor.Setting;
using Microsoft.AspNetCore.Mvc;

namespace Hirealdoor.Controllers
{
    [ApiController]
    [Route(ApiRoutes.Website.Auth)]
    public class AuthenticationController(IAuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(EmailRequestDto emailRequestDto)
        {
            var user = await authService.RegisterEmailAsync(emailRequestDto);
            return Ok(user);

        }
        

        [HttpPost("verifyemail")]
        public async Task<IActionResult> VerifyEmail(VerifyEmailDto dto)
        {
            var result = await authService.VerifyEmailAsync(dto);

            if (!result)
                return BadRequest("Invalid or expired verification code.");
            return Ok("Email verified successfully.");
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(EmailRequestDto emailRequestDto)
        {
            var user = await authService.RegisterEmailAsync(emailRequestDto);
            return Ok(user);
        }
        [HttpPost("register&login")]
        public async Task<IActionResult> RegLogin(EmailRequestDto emailRequestDto)
        {
            var user = await authService.RegLogin(emailRequestDto);
            return Ok(user);
        }
        

    }
}