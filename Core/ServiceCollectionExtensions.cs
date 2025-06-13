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

        // Register core services
        services.AddSingleton<IAlgorithmCompilationService, AlgorithmCompilationService>();
        services.AddSingleton<IInteractiveMenu, InteractiveMenu>();

        // Register exercises from YAML files
        RegisterExercisesFromYaml(services);

        return services;
    }

    private static void RegisterExercisesFromYaml(IServiceCollection services)
    {
        if (!Directory.Exists("Patterns"))
        {
            Console.WriteLine("⚠️ Patterns directory not found - no exercises loaded");
            return;
        }

        var yamlFiles = Directory.GetFiles("Patterns", "*.yml", SearchOption.AllDirectories);

        if (!yamlFiles.Any())
        {
            Console.WriteLine("⚠️ No YAML files found in Patterns directory");
            return;
        }

        int registeredCount = 0;
        foreach (var yamlFile in yamlFiles)
        {
            var pathParts = yamlFile.Split(Path.DirectorySeparatorChar);
            if (pathParts.Length < 2) continue;

            var folderName = pathParts[1];

            if (AlgorithmPatterns.FolderToPatternMap.TryGetValue(folderName, out var pattern))
            {
                // Register as Singleton to avoid recompilation
                services.AddKeyedSingleton<IExerciseRunner>(pattern, (provider, key) =>
                    new ExerciseRunner(yamlFile,
                        provider.GetRequiredService<ILogger<ExerciseRunner>>(),
                        provider.GetRequiredService<IAlgorithmCompilationService>()));

                registeredCount++;
            }
            else
            {
                Console.WriteLine($"⚠️ Unknown pattern folder: {folderName} (file: {yamlFile})");
            }
        }

        Console.WriteLine($"✅ Registered {registeredCount} exercise(s) from YAML files");
    }
}