namespace Hirealdoor.Services;

public interface IEmailService
{
    Task SendVerificationEmailAsync(string email,string code);
}