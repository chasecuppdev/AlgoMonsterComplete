// ========================
// File: Core/AlgorithmBase.cs
// ========================
using Microsoft.Extensions.Logging;
using AlgoMonsterComplete.Core.Interfaces;

namespace AlgoMonsterComplete.Core;

/// <summary>
/// Base class for instructional algorithm examples that demonstrate patterns and techniques
/// </summary>
public abstract class AlgorithmBase : IAlgorithmExample
{
    protected readonly ILogger Logger;

    protected AlgorithmBase(ILogger logger)
    {
        Logger = logger;
    }

    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract string Pattern { get; }

    public virtual async Task ExecuteAsync()
    {
        Logger.LogInformation("Running algorithm example: {ExampleName} (Pattern: {Pattern})", Name, Pattern);

        Console.WriteLine($"🎓 Algorithm Example: {Name}");
        Console.WriteLine($"📚 Pattern: {Pattern}");
        Console.WriteLine($"📝 Description: {Description}");
        Console.WriteLine(new string('-', 60));

        try
        {
            await RunExampleAsync();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Algorithm example {ExampleName} failed", Name);
            Console.WriteLine($"❌ Example failed: {ex.Message}");
            throw;
        }

        Console.WriteLine(new string('-', 60));
        Console.WriteLine("✅ Example completed!");
    }

    protected abstract Task RunExampleAsync();

    protected List<string> SplitWords(string input)
    {
        return string.IsNullOrEmpty(input)
            ? new List<string>()
            : input.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
    }

    protected void WriteExample(string description, string input, string output)
    {
        Console.WriteLine($"📖 {description}");
        Console.WriteLine($"   Input:  {input}");
        Console.WriteLine($"   Output: {output}");
        Console.WriteLine();
    }

    protected void WriteStepByStep(string step, string explanation)
    {
        Console.WriteLine($"👉 {step}: {explanation}");
    }
}