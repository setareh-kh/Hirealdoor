using Hirealdoor.DTos.Objects;
using Hirealdoor.Repositories;
using Hirealdoor.Repositories.Repository;
using Hirealdoor.Services;
using Hirealdoor.Services.Service;

namespace Hirealdoor.Installer;

public class ServiceInstaller : IInstaller
{
    public void InstallServices(IConfiguration configuration, IServiceCollection services)
    {
        //install IRepository and Repository as services
        services.AddScoped<IOfficeRepository, OfficeRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<ILanguageRepository, LanguageRepository>();
        services.AddScoped<ITokenRepository, TokenRepository>();
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        //install IService and Service as services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IPersonService, PersonService>();
        services.AddScoped<IAuthService, AuthService>();


        // services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
        var appSettings = configuration
            .GetSection("AppSettings")
            .Get<AppSettings>();
        services.AddSingleton(appSettings!);
    }
}