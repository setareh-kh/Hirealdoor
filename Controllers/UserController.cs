using Hirealdoor.Dtos.Requests;
using Hirealdoor.Services;
using Microsoft.AspNetCore.Mvc;
namespace Hirealdoor.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController(IUserService userService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(EmailRequestDto emailRequestDto)
        {
            var user = await userService.RegisterEmailAsync(emailRequestDto);
            return Ok(user);
        }
        [HttpPost("verifyemail")]
        public async Task<IActionResult> VerifyEmail(VerifyEmailDto dto)
        {
            var result =await userService.VerifyEmailAsync(dto);

            if (!result)
                return BadRequest("Invalid or expired verification code.");
            return Ok("Email verified successfully.");
        }
        [HttpPost("usertype")]
        public async Task<IActionResult> SelectUserType(UserTypeDto userTypeDto)
        {
            var result = await userService.SelectUserTypeAsync(userTypeDto);
            return Ok(result);
        }   
        
    }
}