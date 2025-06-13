using AlgoMonsterComplete.Core;
using AlgoMonsterComplete.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text;

namespace AlgoMonsterComplete;

public class Program
{
    public static async Task Main(string[] args)
    {
        // Configure console encoding for proper Unicode support
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        var host = CreateHostBuilder(args).Build();

        try
        {
            var interactiveMenu = host.Services.GetRequiredService<IInteractiveMenu>();
            await interactiveMenu.RunAsync();
        }
        catch (Exception ex)
        {
            var logger = host.Services.GetService<ILogger<Program>>();
            logger?.LogError(ex, "Application error occurred");
            Console.WriteLine($"❌ Application error: {ex.Message}");
        }
        finally
        {
            await host.StopAsync(TimeSpan.FromSeconds(5));
            host.Dispose();
        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // Register core infrastructure
                services.AddCoreServices(context.Configuration);
            });
}