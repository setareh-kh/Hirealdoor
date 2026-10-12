using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Hirealdoor.DTos.Objects;
using Hirealdoor.Models;
using Hirealdoor.Services;
using Microsoft.IdentityModel.Tokens;

namespace Hirealdoor.Setting;

public class JwtMiddleware(RequestDelegate next, AppSettings appSettings)
{
    public async Task InvokeAsync(
        HttpContext context,
        IRedisService redisService,
        IUserService userService
    )
    {
        var token = context.Request.Headers.Authorization
                        .FirstOrDefault()?
                        .Split(" ")[^1]
                    ?? context.Request.Query["token"].ToString();
        if (string.IsNullOrWhiteSpace(token))
        {
            await next(context);
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(appSettings.Secret!);
        tokenHandler.ValidateToken(
            token,
            new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            },
            out SecurityToken validatedToken);

        var jwtToken = (JwtSecurityToken)validatedToken;

        var userId = int.Parse(
            jwtToken.Claims.First(x => x.Type == "id").Value);

        var user = await redisService.GetValueAsync<User>($"user_id_{userId}");
        if (user == null)
        {
            user = await userService.GetAsync(userId);
            if (user != null)
            {
                await redisService.SetValueAsync($"user_id_{userId}", user, TimeSpan.FromMinutes(5));
            }
        }

        if (user != null)
        {
            context.Items["user"] = user;
            context.Items["token"] = token;
        }

        await next(context);
    }
}