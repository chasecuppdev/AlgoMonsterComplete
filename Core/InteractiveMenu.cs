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
            Console.WriteLine("  patterns (p)             - Show all available patterns");
            Console.WriteLine("  shortcuts (s)            - Show pattern shortcuts");
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
        // Resolve pattern first
        var resolvedPattern = AlgorithmPatterns.ResolvePattern(patternKey);
        if (resolvedPattern == null)
        {
            Console.WriteLine($"❌ Pattern '{patternKey}' not found");
            await ListPatternsAsync();
            return;
        }

        try
        {
            var exercises = _serviceProvider.GetKeyedServices<IExerciseRunner>(resolvedPattern);
            var exercise = exercises.FirstOrDefault(e =>
                e.Name.Equals(exerciseName, StringComparison.OrdinalIgnoreCase));

            if (exercise == null)
            {
                Console.WriteLine($"❌ Exercise '{exerciseName}' not found in pattern '{resolvedPattern}'");
                await ListExercisesAsync(resolvedPattern);
                return;
            }

            _logger.LogInformation("Running exercise: {exerciseName} in pattern: {Pattern}",
                exerciseName, resolvedPattern);

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
            Console.WriteLine($"  {pattern.Key,-25} - {pattern.Value} ({exerciseCount} exercises)");
        }

        Console.WriteLine("\n💡 Pro tip: Use shortcuts like 'fs', 'bs', 'dp' or partial names like 'fund', 'sort'");
        Console.WriteLine("   Type 'shortcuts' to see all available shortcuts");

        await Task.CompletedTask;
    }

    public async Task ListExercisesAsync(string patternKey)
    {
        // Resolve pattern first
        var resolvedPattern = AlgorithmPatterns.ResolvePattern(patternKey);
        if (resolvedPattern == null || !_patternDisplayNames.ContainsKey(resolvedPattern))
        {
            Console.WriteLine($"❌ Pattern '{patternKey}' not found");
            await ListPatternsAsync();
            return;
        }

        var exercises = _serviceProvider.GetKeyedServices<IExerciseRunner>(resolvedPattern);

        Console.WriteLine($"\n🎓 Exercises in {_patternDisplayNames[resolvedPattern]}:");
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

    private async Task ShowShortcutsAsync()
    {
        Console.WriteLine("\n⚡ Pattern Shortcuts:");
        Console.WriteLine("================");

        foreach (var shortcut in AlgorithmPatterns.PatternShortcuts)
        {
            var displayName = AlgorithmPatterns.PatternDisplayNames[shortcut.Value];
            Console.WriteLine($"  {shortcut.Key,-4} → {shortcut.Value,-25} ({displayName})");
        }

        Console.WriteLine("\n💡 Fuzzy matching also works:");
        Console.WriteLine("  'fund' or 'sort' → fundamental-sorting");
        Console.WriteLine("  'bin' or 'search' → binary-search");
        Console.WriteLine("  'two' or 'pointer' → two-pointers");
        Console.WriteLine("\n📝 Examples:");
        Console.WriteLine("  ex fs        # fundamental-sorting");
        Console.WriteLine("  ex fund      # fundamental-sorting");
        Console.WriteLine("  ex bs        # binary-search");
        Console.WriteLine("  ex dp        # dynamic-programming");

        await Task.CompletedTask;
    }

    private static bool IsQuitCommand(string input)
    {
        var quitCommands = new[] { "quit", "q", "exit" };
        return quitCommands.Contains(input.ToLower());
    }

    private async Task ShowExerciseMenuAsync(string patternInput)
    {
        // 🚀 New: Resolve pattern with smart matching
        var resolvedPattern = AlgorithmPatterns.ResolvePattern(patternInput);
        if (resolvedPattern == null || !_patternDisplayNames.ContainsKey(resolvedPattern))
        {
            Console.WriteLine($"❌ Pattern '{patternInput}' not found");

            // Show suggestion if it was close
            var suggestions = AlgorithmPatterns.PatternDisplayNames.Keys
                .Where(key => key.Contains(patternInput.ToLowerInvariant()))
                .Take(3)
                .ToList();

            if (suggestions.Any())
            {
                Console.WriteLine($"💡 Did you mean: {string.Join(", ", suggestions)}?");
            }

            await ListPatternsAsync();
            return;
        }

        // Show what pattern was resolved to (helpful for fuzzy matches)
        if (!string.Equals(patternInput, resolvedPattern, StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine($"🎯 Resolved '{patternInput}' → '{resolvedPattern}'");
        }

        var exercises = _serviceProvider.GetKeyedServices<IExerciseRunner>(resolvedPattern).OrderBy(e => e.Name).ToList();

        if (!exercises.Any())
        {
            Console.WriteLine($"\n🎓 No exercises available for {_patternDisplayNames[resolvedPattern]} yet.");
            return;
        }

        Console.WriteLine($"\n🎓 Exercises in {_patternDisplayNames[resolvedPattern]}:");
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
                selectedexercise.Name, resolvedPattern);

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

            case "shortcuts" or "s":
                await ShowShortcutsAsync();
                break;

            case "exercises" or "ex" when parts.Length >= 2:
                await ShowExerciseMenuAsync(parts[1]);
                break;

            case "exercises" or "ex":
                Console.WriteLine("❌ Please specify a pattern. Usage: exercises <pattern>");
                Console.WriteLine("💡 Examples: ex fs, ex fund, ex fundamental-sorting");
                await ListPatternsAsync();
                break;

            default:
                Console.WriteLine("❌ Invalid command. Available commands: exercises, patterns, shortcuts, quit");
                break;
        }
    }
}