using AlgoMonsterComplete.Core;
using AlgoMonsterComplete.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AlgoMonsterComplete;

public class Program
{
    public static async Task Main(string[] args)
    {
        // Enable UTF-8 encoding for emoji support
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var host = CreateHostBuilder(args).Build();

        var interactiveMenu = host.Services.GetRequiredService<IInteractiveMenu>();

        await interactiveMenu.RunAsync();

        await host.RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // Register core infrastructure
                services.AddCoreServices(context.Configuration);
            });
}