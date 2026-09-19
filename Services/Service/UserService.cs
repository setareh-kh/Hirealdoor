using System.Security.Cryptography;
using Hirealdoor.Dtos.Requests;
using Hirealdoor.Models;
using Hirealdoor.Repositories;
namespace Hirealdoor.Services.Service;


public class UserService(IUserRepository userRepository, IEmailService emailService): IUserService
{

    //Use Case:registeration
    // 1: create new user by email
    public async Task<User> RegisterEmailAsync(EmailRequestDto emailRequest)
    {
        var existingUser = await userRepository.FindByEmailAsync(emailRequest.Email);

        if (existingUser != null)
            throw new Exception("Email already exists.");
        var now = DateTime.UtcNow;

        var newUser = new User
        {
            Email = emailRequest.Email,
            VerifyCode = GenerateCode(),
            ExpiresAtCode = now.AddMinutes(3),
            UpdatedAtCode = now,
            CreatedAt = now,
            IsEmailVerified = false
        };
        await userRepository.InsertAsync(newUser);
        await userRepository.SaveChangesAsync();
        await emailService.SendVerificationEmailAsync(newUser.Email, newUser.VerifyCode);
        return newUser;
    }
    private string GenerateCode()
    {
        return RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
    }
    //2: Verify Email
    public async Task<bool> VerifyEmailAsync(VerifyEmailDto verifyEmailDto)
    {
        var user = await userRepository.GetByIdAsync(verifyEmailDto.UserId);
        if (user == null)
            return false;
        if (user.IsEmailVerified)
            return true;
        if (user.VerifyCode != verifyEmailDto.VerifyCode)
            return false;
        if (user.ExpiresAtCode <= DateTime.UtcNow)
            return false;
        user.IsEmailVerified = true;
        await userRepository.UpdateByIdAsync(user);
        await userRepository.SaveChangesAsync();
        return true;
    }
    //3:Select User Type
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
