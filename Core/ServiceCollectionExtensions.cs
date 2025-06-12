using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AlgoMonsterComplete.Core.Interfaces;

namespace AlgoMonsterComplete.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCoreServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Configure logging
        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Information);
        });

        services.AddSingleton<IInteractiveMenu, InteractiveMenu>();
        services.AddTransient<IExerciseRunner, ExerciseRunner>();

        return services;
    }
}