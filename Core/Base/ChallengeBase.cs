using Microsoft.Extensions.Logging;
using AlgoMonsterComplete.Core.Interfaces;

namespace AlgoMonsterComplete.Core.BaseClasses;

/// <summary>
/// Base class for practice challenges that test understanding of patterns
/// </summary>
public abstract class ChallengeBase : IAlgorithmChallenge
{
    protected readonly ILogger Logger;

    protected ChallengeBase(ILogger logger)
    {
        Logger = logger;
    }

    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract ChallengeDifficulty Difficulty { get; }

    public virtual async Task ExecuteAsync()
    {
        Logger.LogInformation("Starting challenge: {ChallengeName}", Name);

        var difficultyIcon = Difficulty switch
        {
            ChallengeDifficulty.Easy => "🟢",
            ChallengeDifficulty.Medium => "🟡",
            ChallengeDifficulty.Hard => "🔴",
            _ => "⚪"
        };

        Console.WriteLine($"🎯 Challenge: {Name} {difficultyIcon}");
        Console.WriteLine($"📝 Description: {Description}");
        Console.WriteLine(new string('-', 60));

        try
        {
            await RunChallengeAsync();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Challenge {ChallengeName} failed", Name);
            Console.WriteLine($"❌ Challenge failed: {ex.Message}");
            throw;
        }

        Console.WriteLine(new string('-', 60));
        Console.WriteLine("✅ Challenge completed!");
    }

    protected abstract Task RunChallengeAsync();

    protected List<string> SplitWords(string input)
    {
        return string.IsNullOrEmpty(input)
            ? new List<string>()
            : input.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
    }

    protected void WriteTestCase(string input, string expected, string actual)
    {
        var status = expected == actual ? "✅" : "❌";
        Console.WriteLine($"{status} Input: {input} | Expected: {expected} | Got: {actual}");
    }

    protected void RunAutomatedTests<T>(string testName, Func<T[], T[], bool> testFunction, (T[] input, T[] expected)[] testCases)
    {
        Console.WriteLine($"🧪 Running automated tests for {testName}...\n");

        for (int i = 0; i < testCases.Length; i++)
        {
            var (input, expected) = testCases[i];
            var inputCopy = input.ToArray(); // Protect original input
            var result = testFunction(inputCopy, expected);

            var inputStr = $"[{string.Join(", ", input)}]";
            var expectedStr = $"[{string.Join(", ", expected)}]";
            var resultStr = $"[{string.Join(", ", inputCopy)}]";

            var status = result ? "✅" : "❌";
            Console.WriteLine($"{status} Test {i + 1}: Input: {inputStr} | Expected: {expectedStr} | Got: {resultStr}");
        }
    }

    protected void PromptForInteractiveTest(string prompt, Action<string> testAction)
    {
        Console.WriteLine($"\n{new string('-', 30)}");
        Console.WriteLine("🎮 Interactive Test:");
        Console.WriteLine(prompt);

        var input = Console.ReadLine();
        if (!string.IsNullOrEmpty(input))
        {
            testAction(input);
        }
    }
}