namespace Hirealdoor.Services.Service;
public class EmailService : IEmailService
{
    public async Task SendVerificationEmailAsync(string email,string code)
    {
        // SMTP
        // SendGrid
        // Mailgun
        // ...
    }
}