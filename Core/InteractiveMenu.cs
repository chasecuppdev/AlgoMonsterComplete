using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using AlgoMonsterComplete.Core.Interfaces;
using AlgoMonsterComplete.Data.Constants;

namespace AlgoMonsterComplete.Core;

public class InteractiveMenu : IInteractiveMenu
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<InteractiveMenu> _logger;
    private readonly IHostApplicationLifetime _lifetime;
    private readonly Dictionary<string, string> _patternDisplayNames;

    public InteractiveMenu(IServiceProvider serviceProvider, ILogger<InteractiveMenu> logger, IHostApplicationLifetime lifetime)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _lifetime = lifetime;
        _patternDisplayNames = AlgorithmPatterns.PatternDisplayNames;
    }

    public async Task RunAsync()
    {
        Console.WriteLine("🚀 Welcome to AlgoMonster Complete with Modern .NET DI!");
        Console.WriteLine("=========================================================");
        Console.WriteLine("📚 A Personal Catalog of AlgoMonster Solutions and Practice");

        await ListPatternsAsync();

        while (true)
        {
            Console.WriteLine("\nCommands:");
            Console.WriteLine("  exercises <pattern> (ex) - Show interactive menu of exercises");
            Console.WriteLine("  challenges <pattern>     - Show interactive menu of challenges");
            Console.WriteLine("  patterns (p)             - Show all available patterns");
            Console.WriteLine("  quit (q)                 - Exit application");
            Console.Write("\n> ");

            var input = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(input))
                continue;

            // Handle quit commands
            if (IsQuitCommand(input))
            {
                Console.WriteLine("👋 Goodbye! Happy algorithm practicing!");
                _lifetime.StopApplication();
                break;
            }

            await ProcessCommand(input);
        }
    }

    public async Task RunExerciseAsync(string patternKey, string exerciseName)
    {
        try
        {
            var exercises = _serviceProvider.GetKeyedServices<IExerciseRunner>(patternKey);
            var exercise = exercises.FirstOrDefault(e =>
                e.Name.Equals(exerciseName, StringComparison.OrdinalIgnoreCase));

            if (exercise == null)
            {
                Console.WriteLine($"❌ exercise '{exerciseName}' not found in pattern '{patternKey}'");
                await ListExercisesAsync(patternKey);
                return;
            }

            _logger.LogInformation("Running exercise: {exerciseName} in pattern: {Pattern}",
                exerciseName, patternKey);

            exercise.RunExerciseAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running exercise {exerciseName}", exerciseName);
            Console.WriteLine($"❌ Error: {ex.Message}");
        }
    }

    public async Task ListPatternsAsync()
    {
        Console.WriteLine("\n📚 Available Algorithmic Patterns:");
        foreach (var pattern in _patternDisplayNames)
        {
            var exerciseCount = _serviceProvider.GetKeyedServices<IExerciseRunner>(pattern.Key).Count();
            var challengeCount = _serviceProvider.GetKeyedServices<IExerciseRunner>(pattern.Key).Count();
            Console.WriteLine($"  {pattern.Key,-25} - {pattern.Value} ({exerciseCount} exercises, {challengeCount} challenges)");
        }
        await Task.CompletedTask;
    }

    public async Task ListExercisesAsync(string patternKey)
    {
        if (!_patternDisplayNames.ContainsKey(patternKey))
        {
            Console.WriteLine($"❌ Pattern '{patternKey}' not found");
            await ListPatternsAsync();
            return;
        }

        var exercises = _serviceProvider.GetKeyedServices<IExerciseRunner>(patternKey);

        Console.WriteLine($"\n🎓 exercises in {_patternDisplayNames[patternKey]}:");
        if (!exercises.Any())
        {
            Console.WriteLine("  No exercises available yet for this pattern.");
            return;
        }

        foreach (var exercise in exercises.OrderBy(e => e.Name))
        {
            Console.WriteLine($"  📖 {exercise.Name,-25} - {exercise.Description}");
        }

        await Task.CompletedTask;
    }

    private static bool IsQuitCommand(string input)
    {
        var quitCommands = new[] { "quit", "q", "exit" };
        return quitCommands.Contains(input.ToLower());
    }

    private async Task ShowExerciseMenuAsync(string patternKey)
    {
        if (!_patternDisplayNames.ContainsKey(patternKey))
        {
            Console.WriteLine($"❌ Pattern '{patternKey}' not found");
            await ListPatternsAsync();
            return;
        }

        var exercises = _serviceProvider.GetKeyedServices<IExerciseRunner>(patternKey).OrderBy(e => e.Name).ToList();

        if (!exercises.Any())
        {
            Console.WriteLine($"\n🎓 No exercises available for {_patternDisplayNames[patternKey]} yet.");
            return;
        }

        Console.WriteLine($"\n🎓 exercises in {_patternDisplayNames[patternKey]}:");
        for (int i = 0; i < exercises.Count; i++)
        {
            Console.WriteLine($"  {i + 1}. 📖 {exercises[i].Name} - {exercises[i].Description}");
        }

        Console.Write($"\nSelect exercise to run (1-{exercises.Count}) or press Enter to return: ");
        var selection = Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(selection))
            return;

        if (int.TryParse(selection, out int choice) && choice >= 1 && choice <= exercises.Count)
        {
            var selectedexercise = exercises[choice - 1];
            Console.WriteLine($"\n🎓 Running: {selectedexercise.Name}");
            Console.WriteLine(new string('=', 60));

            _logger.LogInformation("Running exercise: {exerciseName} in pattern: {Pattern}",
                selectedexercise.Name, patternKey);

            try
            {
                selectedexercise.RunExerciseAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error running exercise {exerciseName}", selectedexercise.Name);
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("❌ Invalid selection.");
        }
    }

    private async Task ProcessCommand(string input)
    {
        var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0) return;

        var command = parts[0].ToLower();

        switch (command)
        {
            case "patterns" or "p":
                await ListPatternsAsync();
                break;

            case "exercises" or "ex" when parts.Length >= 2:
                await ShowExerciseMenuAsync(parts[1]);
                break;

            case "exercises" or "ex":
                Console.WriteLine("❌ Please specify a pattern. Usage: exercises <pattern>");
                await ListPatternsAsync();
                break;

            default:
                Console.WriteLine("❌ Invalid command. Available commands: exercises, patterns, quit");
                break;
        }
    }
}