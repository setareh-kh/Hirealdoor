using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DeviceDetectorNET;
using DeviceDetectorNET.Cache;
using Hirealdoor.DTos.Objects;
using Hirealdoor.Dtos.Requests;
using Hirealdoor.DTos.Response;
using Hirealdoor.Extentions;
using Hirealdoor.Models;
using Hirealdoor.Repositories;
using Hirealdoor.Repositories.Repository;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Hirealdoor.Services.Service;

public class AuthService(
    AppSettings appSettings,
    IUserRepository userRepository,
    ITokenRepository tokenRepository,
    IHttpContextAccessor httpContextAccessor,
    IEmailService emailService
) : IAuthService
{
    private const int LimitTokenDays = 15;

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
            UserId = newUser.Id,
            Email = newUser.Email
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

    public async Task<StandardResponseDto> Login(EmailRequestDto emailDto)
    {
        var user = await userRepository.FindByEmailAsync(emailDto.Email);
        if (user == null)
            return new StandardResponseDto { Success = false, Message = "User not found" };
        else
        {
            user.VerifyCode = GenerateCode();
            user.ExpiresAtCode = DateTime.UtcNow.AddMinutes(3);
            user.UpdatedAtCode = DateTime.UtcNow;
            await userRepository.UpdateByIdAsync(user);
            await userRepository.SaveChangesAsync();
            await emailService.SendVerificationEmailAsync(user.Email, user.VerifyCode);
            var token = await GenerateJwtToken(user);
            return new StandardResponseDto
            {
                Success = true, Message = "", Object = new
                {
                    Id = user.Id,
                    Email = user.Email,
                    Token = token
                }
            };
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
                UserId = newUser.Id,
                Email = newUser.Email
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
                UserId = res.Id,
                Email = res.Email
            };
        }
    }

    private async Task<string> GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(appSettings.Secret!);
        var expire = DateTime.Now.AddDays(LimitTokenDays);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim("id", user.Id.ToString()),
                new Claim("mobile", user.Mobile ?? ""),
                new Claim("type", user.Type?.ToString() ?? "")
            ]),
            Expires = expire,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };


        string userAgent = (httpContextAccessor.HttpContext?.Request.Headers.UserAgent ?? "")!;
        var dd = new DeviceDetector(userAgent);
        dd.SetCache(new DictionaryCache());
        dd.Parse();
        var os = dd.GetOs()?.Match;
        var clientMatch = dd.GetBrowserClient()?.Match;

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);

        var t = new Token
        {
            UserId = user.Id,
            Hash = jwt,
            CreatedAt = DateTime.Now,
            IpAddress = httpContextAccessor.HttpContext?.GetRealClientIpAddress(),
            ExpiredAt = expire,
            Os = $"{os?.Name ?? "os"} {os?.Version ?? ""}",
            Browser = clientMatch?.Name ?? "browser",
            Active = true
        };

        await tokenRepository.InsertAsync(t);
        await tokenRepository.SaveChangesAsync();

        return jwt;
    }
}