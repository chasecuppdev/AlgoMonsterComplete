// ========================
// File: Infrastructure/ServiceCollectionExtensions.cs
// ========================
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AlgoMonsterComplete.Core;
using AlgoMonsterComplete.Core.Interfaces;

namespace AlgoMonsterComplete.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Configure logging
        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Information);
        });

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<IChallengeRunner, ChallengeRunner>();
        return services;
    }
}