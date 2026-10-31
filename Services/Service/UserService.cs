using System.Security.Cryptography;
using Hangfire;
using Hirealdoor.Dtos.Requests;
using Hirealdoor.DTos.Response;
using Hirealdoor.Models;
using Hirealdoor.Repositories;

namespace Hirealdoor.Services.Service;

public class UserService(IUserRepository userRepository, IEmailService emailService)
    : BaseService<User>(userRepository), IUserService
{
    //Use Case:registeration
    // 1: create new user by email
    public async Task<StandardResponseDto> RegisterEmailAsync(EmailRequestDto emailRequest)
    {
        var existingUser = await userRepository.FindByEmailAsync(emailRequest.Email);

        if (existingUser != null)
            return new StandardResponseDto
            {
                Success = false,
                Message = "Email already exists.",
                Object = null
            };
        var now = DateTime.UtcNow;

        var newUser = new User
        {
            Email = emailRequest.Email,
            VerifyCode = GenerateCode(),
            ExpiresAtCode = now.AddMinutes(3),
            UpdatedAtCode = now,
            CreatedAt = now,
        };
        await userRepository.InsertAsync(newUser);
        await userRepository.SaveChangesAsync();
        /*BackgroundJob.Enqueue("critical", () =>
            emailService.SendVerificationEmailAsync(newUser.Email, newUser.VerifyCode));
        BackgroundJob.Schedule(
            "critical",
            () => emailService.SendVerificationEmailAsync(newUser.Email, newUser.VerifyCode),
            TimeSpan.FromDays(2)
        );*/
        return new StandardResponseDto
        {
            Success = true,
            Message = "",
            Object = new
            {
                UserId = newUser.Id,
                Email = newUser.Email,
                Code = newUser.VerifyCode
            }
        };
    }

    private string GenerateCode()
    {
        return RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
    }

    public async Task<User> SelectUserTypeAsync(UserTypeDto userTypeDto)
    {
        // Implementation for selecting user type
        var user = await userRepository.GetByIdAsync(userTypeDto.UserId);
        if (user == null)
            throw new Exception("User not found.");
        user.Type = userTypeDto.Type;
        await userRepository.UpdateByIdAsync(user);
        await userRepository.SaveChangesAsync();
        return user;
    }
}