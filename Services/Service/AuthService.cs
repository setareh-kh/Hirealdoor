using System.Security.Cryptography;
using Hirealdoor.Dtos.Requests;
using Hirealdoor.DTos.Response;
using Hirealdoor.Models;
using Hirealdoor.Repositories;
using Hirealdoor.Repositories.Repository;
using Hirealdoor.Services.Service;

namespace Hirealdoor.Services;

public class AuthService(IUserRepository userRepository, IEmailService emailService) : IAuthService
{
    //registeration
    // 1: create new user by email
    public async Task<RegisterResponseDto> RegisterEmailAsync(EmailRequestDto emailDto)
    {
        var exUser = await userRepository.FindByEmailAsync(emailDto.Email);
        if (exUser != null)
            throw new Exception("Email already exists.");
        var now = DateTime.UtcNow;
        var newUser = new User
        {
            Email = emailDto.Email,
            VerifyCode = GenerateCode(),
            ExpiresAtCode = now.AddMinutes(3),
            UpdatedAtCode = now,
            CreatedAt = now,
        };
        await userRepository.InsertAsync(newUser);
        await userRepository.SaveChangesAsync();
        await emailService.SendVerificationEmailAsync(newUser.Email, newUser.VerifyCode);
        return new RegisterResponseDto
        {
            UserId= newUser.Id,
            Email=newUser.Email
        };
    }
    private string GenerateCode()
    {
        return RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
    }
    //2: Verify Email
    public async Task<bool> VerifyEmailAsync(VerifyEmailDto verifyEmailDto)
    {
        var user = await userRepository.GetByIdAsync(verifyEmailDto.UserId);
        if (user != null &&
            user.VerifyCode != verifyEmailDto.VerifyCode &&
            user.ExpiresAtCode <= DateTime.UtcNow)
            return true;
        else return false;
    }

    public async Task<bool> Login(EmailRequestDto emailDto)
    {
        var user = await userRepository.FindByEmailAsync(emailDto.Email);
        if (user == null)
            return false;
        else
        {
            user.VerifyCode = GenerateCode();
            user.ExpiresAtCode = DateTime.UtcNow.AddMinutes(3);
            user.UpdatedAtCode = DateTime.UtcNow;
            await userRepository.UpdateByIdAsync(user);
            await userRepository.SaveChangesAsync();
            await emailService.SendVerificationEmailAsync(user.Email, user.VerifyCode);
            return true;
        }


    }
    public async Task<RegisterResponseDto> RegLogin(EmailRequestDto emailDto)
    {
        var res = await userRepository.FindByEmailAsync(emailDto.Email);
        if (res == null)
        {
            var newUser = new User
            {
                Email = emailDto.Email,
                VerifyCode = GenerateCode(),
                ExpiresAtCode = DateTime.UtcNow.AddMinutes(3),
                UpdatedAtCode = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
            };
            await userRepository.InsertAsync(newUser);
            await userRepository.SaveChangesAsync();
            await emailService.SendVerificationEmailAsync(newUser.Email, newUser.VerifyCode);
            return new RegisterResponseDto
        {
            UserId= newUser.Id,
            Email=newUser.Email
        };
        }
        else
        {
            res.VerifyCode = GenerateCode();
            res.ExpiresAtCode = DateTime.UtcNow.AddMinutes(3);
            res.UpdatedAtCode = DateTime.UtcNow;
            await userRepository.UpdateByIdAsync(res);
            await userRepository.SaveChangesAsync();
            await emailService.SendVerificationEmailAsync(res.Email, res.VerifyCode);
            return new RegisterResponseDto
        {
            UserId= res.Id,
            Email=res.Email
        };
        }
    }
}
