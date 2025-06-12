using Microsoft.Extensions.Logging;
using AlgoMonsterComplete.Models.Common;
using AlgoMonsterComplete.Core.Interfaces;
using YamlDotNet.Serialization;

namespace AlgoMonsterComplete.Core;

public class ExerciseRunner : IExerciseRunner
{
    private readonly Exercise _exercise;
    private readonly string _yamlPath;

    public ExerciseRunner(string yamlPath, ILogger<ExerciseRunner> logger)
    {
        _yamlPath = yamlPath;
        var yamlContent = File.ReadAllText(yamlPath);
        var deserializer = new DeserializerBuilder().Build();
        _exercise = deserializer.Deserialize<Exercise>(yamlContent);
    }

    public string Name => _exercise.Title;
    public string Description => _exercise.Description;
    public string Pattern => _exercise.Pattern;

    public async Task RunExerciseAsync()
    {
        Console.WriteLine($"📚 Reference: {_exercise.AlgoMonsterReference}");
        Console.WriteLine();

        DisplayComplexityAnalysis();
        DisplayImplementation();
        await RunUnitTests();
        DemonstrateAlgorithm();
    }

    private void DisplayComplexityAnalysis()
    {
        var analysis = _exercise.MySolution.ComplexityAnalysis;
        Console.WriteLine("📊 Complexity Analysis:");
        Console.WriteLine($"  ⏰ Time: {analysis.Time}");
        Console.WriteLine($"  💾 Space: {analysis.Space}");
        Console.WriteLine($"  🔄 Stable: {(analysis.Stable ? "Yes" : "No")}");
        Console.WriteLine($"  📍 In-place: {(analysis.InPlace ? "Yes" : "No")}");
        Console.WriteLine();
    }

    private void DisplayImplementation()
    {
        if (!string.IsNullOrEmpty(_exercise.MySolution?.Implementation))
        {
            Console.WriteLine("💻 My Implementation:");
            Console.WriteLine(_exercise.MySolution.Implementation);
            Console.WriteLine();
        }
    }

    private async Task RunUnitTests()
    {
        Console.WriteLine("🧪 Unit Tests (Running my implementation against test cases):");

        foreach (var testCase in _exercise.TestCases)
        {
            try
            {
                var input = ParseInput(testCase.Input);
                var result = ExecuteAlgorithm(input);
                var resultStr = FormatOutput(result);

                var status = resultStr == testCase.Expected ? "✅ PASS" : "❌ FAIL";
                Console.WriteLine($"  {status}");
                Console.WriteLine($"     Input: {testCase.Input}");
                Console.WriteLine($"     Expected: {testCase.Expected}");
                Console.WriteLine($"     Actual: {resultStr}");

                if (!string.IsNullOrEmpty(testCase.Notes))
                    Console.WriteLine($"     Notes: {testCase.Notes}");
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ❌ ERROR - {ex.Message}");
                Console.WriteLine($"     Input: {testCase.Input}");
                Console.WriteLine();
            }
        }
    }

    private void DemonstrateAlgorithm()
    {
        Console.WriteLine("🔧 Algorithm Demonstration:");
        var demo = new[] { 5, 2, 4, 6, 1, 3 };
        Console.WriteLine($"Step-by-step with: [{string.Join(", ", demo)}]");
        Console.WriteLine();

        ExecuteAlgorithmWithVisualization(demo);
    }

    private void ExecuteAlgorithmWithVisualization(int[] input)
    {
        var list = input.ToList();

        for (int i = 1; i < list.Count; i++)
        {
            int current = i;
            int elementToInsert = list[current];

            Console.WriteLine($"Step {i}: Inserting {elementToInsert} (position {current})");

            while (current > 0 && list[current] < list[current - 1])
            {
                (list[current], list[current - 1]) = (list[current - 1], list[current]);
                current--;
                Console.WriteLine($"  → Swapped: [{string.Join(", ", list)}]");
            }

            Console.WriteLine($"  ✓ Final: [{string.Join(", ", list)}]");
            Console.WriteLine();
        }
    }

    private int[] ParseInput(string input)
    {
        return input.Trim('[', ']')
                   .Split(',')
                   .Select(s => int.Parse(s.Trim()))
                   .ToArray();
    }

    private string FormatOutput(List<int> result)
    {
        return $"[{string.Join(", ", result)}]";
    }
}