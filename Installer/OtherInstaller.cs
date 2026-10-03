namespace Hirealdoor.Installer;

public class OtherInstaller : IInstaller
{
    public void InstallServices(IConfiguration configuration, IServiceCollection services)
    {
        services.AddOpenApi();
        services.AddSwaggerGen();

        services.AddAutoMapper(cfg => { }, typeof(Program));
    }
}