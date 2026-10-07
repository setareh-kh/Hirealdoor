using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Hirealdoor.DTos.Objects;
using Microsoft.IdentityModel.Tokens;

namespace Hirealdoor.Setting;

public class JwtMiddleware(RequestDelegate next, AppSettings appSettings)
{
    public async Task InvokeAsync(HttpContext context)
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
        var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
        context.Items["user"] = userId;
        context.Items["token"] = token;

        await next(context);
    }
}