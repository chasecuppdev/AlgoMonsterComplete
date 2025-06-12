using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AlgoMonsterComplete.Core.Interfaces;

namespace AlgoMonsterComplete.Core;

public class ChallengeRunner : IChallengeRunner
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ChallengeRunner> _logger;
    private readonly Dictionary<string, string> _patternDisplayNames;

    public ChallengeRunner(IServiceProvider serviceProvider, ILogger<ChallengeRunner> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _patternDisplayNames = new Dictionary<string, string>
        {
            // Core patterns from AlgoMonster
            ["fundamental-sorting"] = "Fundamental Sorting",
            ["binary-search"] = "Binary Search",
            ["two-pointers"] = "Two Pointers",
            ["depth-first-search"] = "Depth First Search",
            ["backtracking"] = "Backtracking",
            ["breadth-first-search"] = "Breadth First Search",
            ["graph"] = "Graph",
            ["priority-queue-heap"] = "Priority Queue / Heap",
            ["dynamic-programming"] = "Dynamic Programming",
            ["advanced-data-structures"] = "Advanced Data Structures",
            ["miscellaneous"] = "Miscellaneous",
            // Design patterns
            ["oop-design"] = "OOP Design",
            ["system-design"] = "System Design"
        };
    }

    public async Task RunAsync()
    {
        Console.WriteLine("🚀 Welcome to AlgoMonster Complete with Modern .NET DI!");
        Console.WriteLine("=========================================================");
        Console.WriteLine("📚 Master algorithmic patterns through examples and challenges");

        await ListPatternsAsync();

        while (true)
        {
            Console.WriteLine("\nCommands:");
            Console.WriteLine("  examples <pattern>      - Show interactive menu of examples");
            Console.WriteLine("  challenges <pattern>    - Show interactive menu of challenges");
            Console.WriteLine("  patterns               - Show all available patterns");
            Console.WriteLine("  quit                   - Exit application");
            Console.Write("\n> ");

            var input = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(input) || input.Equals("quit", StringComparison.OrdinalIgnoreCase))
                break;

            await ProcessCommand(input);
        }
    }

    public async Task RunChallengeAsync(string patternKey, string challengeName)
    {
        try
        {
            var challenges = _serviceProvider.GetKeyedServices<IAlgorithmChallenge>(patternKey);
            var challenge = challenges.FirstOrDefault(c =>
                c.Name.Equals(challengeName, StringComparison.OrdinalIgnoreCase));

            if (challenge == null)
            {
                Console.WriteLine($"❌ Challenge '{challengeName}' not found in pattern '{patternKey}'");
                await ListChallengesAsync(patternKey);
                return;
            }

            _logger.LogInformation("Running challenge: {ChallengeName} in pattern: {Pattern}",
                challengeName, patternKey);

            await challenge.ExecuteAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running challenge {ChallengeName}", challengeName);
            Console.WriteLine($"❌ Error: {ex.Message}");
        }
    }

    public async Task RunExampleAsync(string patternKey, string exampleName)
    {
        try
        {
            var examples = _serviceProvider.GetKeyedServices<IAlgorithmExample>(patternKey);
            var example = examples.FirstOrDefault(e =>
                e.Name.Equals(exampleName, StringComparison.OrdinalIgnoreCase));

            if (example == null)
            {
                Console.WriteLine($"❌ Example '{exampleName}' not found in pattern '{patternKey}'");
                await ListExamplesAsync(patternKey);
                return;
            }

            _logger.LogInformation("Running example: {ExampleName} in pattern: {Pattern}",
                exampleName, patternKey);

            await example.ExecuteAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running example {ExampleName}", exampleName);
            Console.WriteLine($"❌ Error: {ex.Message}");
        }
    }

    public async Task ListPatternsAsync()
    {
        Console.WriteLine("\n📚 Available Algorithmic Patterns:");
        foreach (var pattern in _patternDisplayNames)
        {
            var exampleCount = _serviceProvider.GetKeyedServices<IAlgorithmExample>(pattern.Key).Count();
            var challengeCount = _serviceProvider.GetKeyedServices<IAlgorithmChallenge>(pattern.Key).Count();
            Console.WriteLine($"  {pattern.Key,-25} - {pattern.Value} ({exampleCount} examples, {challengeCount} challenges)");
        }
        await Task.CompletedTask;
    }

    public async Task ListChallengesAsync(string patternKey)
    {
        if (!_patternDisplayNames.ContainsKey(patternKey))
        {
            Console.WriteLine($"❌ Pattern '{patternKey}' not found");
            await ListPatternsAsync();
            return;
        }

        var challenges = _serviceProvider.GetKeyedServices<IAlgorithmChallenge>(patternKey);

        Console.WriteLine($"\n🎯 Challenges in {_patternDisplayNames[patternKey]}:");
        if (!challenges.Any())
        {
            Console.WriteLine("  No challenges available yet for this pattern.");
            return;
        }

        foreach (var challenge in challenges.OrderBy(c => c.Difficulty).ThenBy(c => c.Name))
        {
            var difficultyIcon = challenge.Difficulty switch
            {
                ChallengeDifficulty.Easy => "🟢",
                ChallengeDifficulty.Medium => "🟡",
                ChallengeDifficulty.Hard => "🔴",
                _ => "⚪"
            };
            Console.WriteLine($"  {difficultyIcon} {challenge.Name,-25} - {challenge.Description}");
        }

        await Task.CompletedTask;
    }

    public async Task ListExamplesAsync(string patternKey)
    {
        if (!_patternDisplayNames.ContainsKey(patternKey))
        {
            Console.WriteLine($"❌ Pattern '{patternKey}' not found");
            await ListPatternsAsync();
            return;
        }

        var examples = _serviceProvider.GetKeyedServices<IAlgorithmExample>(patternKey);

        Console.WriteLine($"\n🎓 Examples in {_patternDisplayNames[patternKey]}:");
        if (!examples.Any())
        {
            Console.WriteLine("  No examples available yet for this pattern.");
            return;
        }

        foreach (var example in examples.OrderBy(e => e.Name))
        {
            Console.WriteLine($"  📖 {example.Name,-25} - {example.Description}");
        }

        await Task.CompletedTask;
    }

    private async Task ShowExamplesMenuAsync(string patternKey)
    {
        if (!_patternDisplayNames.ContainsKey(patternKey))
        {
            Console.WriteLine($"❌ Pattern '{patternKey}' not found");
            await ListPatternsAsync();
            return;
        }

        var examples = _serviceProvider.GetKeyedServices<IAlgorithmExample>(patternKey).OrderBy(e => e.Name).ToList();

        if (!examples.Any())
        {
            Console.WriteLine($"\n🎓 No examples available for {_patternDisplayNames[patternKey]} yet.");
            return;
        }

        Console.WriteLine($"\n🎓 Examples in {_patternDisplayNames[patternKey]}:");
        for (int i = 0; i < examples.Count; i++)
        {
            Console.WriteLine($"  {i + 1}. 📖 {examples[i].Name} - {examples[i].Description}");
        }

        Console.Write($"\nSelect example to run (1-{examples.Count}) or press Enter to return: ");
        var selection = Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(selection))
            return;

        if (int.TryParse(selection, out int choice) && choice >= 1 && choice <= examples.Count)
        {
            var selectedExample = examples[choice - 1];
            Console.WriteLine($"\n🎓 Running: {selectedExample.Name}");
            Console.WriteLine(new string('=', 60));

            _logger.LogInformation("Running example: {ExampleName} in pattern: {Pattern}",
                selectedExample.Name, patternKey);

            try
            {
                await selectedExample.ExecuteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error running example {ExampleName}", selectedExample.Name);
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("❌ Invalid selection.");
        }
    }

    private async Task ShowChallengesMenuAsync(string patternKey)
    {
        if (!_patternDisplayNames.ContainsKey(patternKey))
        {
            Console.WriteLine($"❌ Pattern '{patternKey}' not found");
            await ListPatternsAsync();
            return;
        }

        var challenges = _serviceProvider.GetKeyedServices<IAlgorithmChallenge>(patternKey)
            .OrderBy(c => c.Difficulty).ThenBy(c => c.Name).ToList();

        if (!challenges.Any())
        {
            Console.WriteLine($"\n🎯 No challenges available for {_patternDisplayNames[patternKey]} yet.");
            return;
        }

        Console.WriteLine($"\n🎯 Challenges in {_patternDisplayNames[patternKey]}:");
        for (int i = 0; i < challenges.Count; i++)
        {
            var difficultyIcon = challenges[i].Difficulty switch
            {
                ChallengeDifficulty.Easy => "🟢",
                ChallengeDifficulty.Medium => "🟡",
                ChallengeDifficulty.Hard => "🔴",
                _ => "⚪"
            };
            Console.WriteLine($"  {i + 1}. {difficultyIcon} {challenges[i].Name} - {challenges[i].Description}");
        }

        Console.Write($"\nSelect challenge to run (1-{challenges.Count}) or press Enter to return: ");
        var selection = Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(selection))
            return;

        if (int.TryParse(selection, out int choice) && choice >= 1 && choice <= challenges.Count)
        {
            var selectedChallenge = challenges[choice - 1];
            Console.WriteLine($"\n🎯 Running: {selectedChallenge.Name}");
            Console.WriteLine(new string('=', 60));

            _logger.LogInformation("Running challenge: {ChallengeName} in pattern: {Pattern}",
                selectedChallenge.Name, patternKey);

            try
            {
                await selectedChallenge.ExecuteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error running challenge {ChallengeName}", selectedChallenge.Name);
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
            case "patterns":
                await ListPatternsAsync();
                break;

            case "examples" when parts.Length >= 2:
                await ShowExamplesMenuAsync(parts[1]);
                break;

            case "challenges" when parts.Length >= 2:
                await ShowChallengesMenuAsync(parts[1]);
                break;

            default:
                Console.WriteLine("❌ Invalid command. Type 'quit' to exit.");
                break;
        }
    }
}