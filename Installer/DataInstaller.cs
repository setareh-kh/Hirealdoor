using Hangfire;
using Hangfire.Redis.StackExchange;
using Hirealdoor.Models;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace Hirealdoor.Installer;

public class DataInstaller : IInstaller
{
    public void InstallServices(IConfiguration configuration, IServiceCollection services)
    {
        var myConnection = configuration.GetConnectionString("MySqlConnection") ?? throw new InvalidOperationException(
            "Connection string 'MySqlConnection' was not found.");
        services.AddDbContext<SqlContext>(opts => opts.UseMySQL(myConnection));

        /*
        var connectionString = configuration.GetValue<string>("MongoDbDatabase:ConnectionString");
        var databaseName = configuration.GetValue<string>("MongoDbDatabase:DatabaseName");

        services.AddHangfire(x => x
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseMongoStorage(client, databaseName, new MongoStorageOptions
            {
                MigrationOptions = new MongoMigrationOptions
                {
                    MigrationStrategy = new MigrateMongoMigrationStrategy(),
                    BackupStrategy = new CollectionMongoBackupStrategy()
                },
                Prefix = "hangfire.mongo",
                CheckConnection = true,
                CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.TailNotificationsCollection
            })
        );
        */
        var redisString = configuration.GetValue<string>("RedisConnectionStrings:Redis");
        var redis = ConnectionMultiplexer.Connect(redisString ?? "");
        services.AddHangfire(configuration =>
        {
            configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseRedisStorage(redis,new RedisStorageOptions{Prefix ="{Hirealdoor-hangfire}:", Db = 0});
        });


        services.AddHangfireServer(serverOptions =>
        {
            serverOptions.ServerName = "Hangfire.Redis server 1";
            serverOptions.WorkerCount = 2;
            serverOptions.Queues = ["default"]; // Define your queues
        });

        services.AddHangfireServer(serverOptions =>
        {
            serverOptions.ServerName = "Hangfire.Redis server 2";
            serverOptions.WorkerCount = 10;
            serverOptions.Queues = ["critical"]; // Define your queues
        });
    }
}