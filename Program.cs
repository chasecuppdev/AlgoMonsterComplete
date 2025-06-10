// ========================
// File: Program.cs
// ========================
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AlgoMonsterComplete.Core;
using AlgoMonsterComplete.Core.Interfaces;
using AlgoMonsterComplete.Patterns.BinarySearch;
using AlgoMonsterComplete.Infrastructure;

namespace AlgoMonsterComplete;

public class Program
{
    public static async Task Main(string[] args)
    {
        // Enable UTF-8 encoding for emoji support
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var host = CreateHostBuilder(args).Build();

        var challengeRunner = host.Services.GetRequiredService<IChallengeRunner>();

        await challengeRunner.RunAsync();

        await host.RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // Register core infrastructure
                services.AddInfrastructureServices(context.Configuration);

                // Register algorithmic patterns
                services.AddBinarySearchPattern();
                // services.AddTwoPointersPattern();
                // services.AddDepthFirstSearchPattern();
                // services.AddBacktrackingPattern();
                // ... other patterns as we build them

                // Register main application services
                services.AddApplicationServices();
            });
}