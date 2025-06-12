using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AlgoMonsterComplete.Core.Interfaces;
using AlgoMonsterComplete.Data.Constants;

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
        RegisterExercisesFromYaml(services);

        return services;
    }

    private static void RegisterExercisesFromYaml(IServiceCollection services)
    {
        var yamlFiles = Directory.GetFiles("Patterns", "*.yml", SearchOption.AllDirectories);

        foreach (var yamlFile in yamlFiles)
        {
            var pathParts = yamlFile.Split(Path.DirectorySeparatorChar);
            var folderName = pathParts[1];

            if (AlgorithmPatterns.FolderToPatternMap.TryGetValue(folderName, out var pattern))
            {
                services.AddKeyedTransient<IExerciseRunner>(pattern, (provider, key) =>
                    new ExerciseRunner(yamlFile, provider.GetService<ILogger<ExerciseRunner>>()));
            }
        }
    }
}