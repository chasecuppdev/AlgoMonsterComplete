using Microsoft.Extensions.Logging;
using AlgoMonsterComplete.Core.BaseClasses;
using AlgoMonsterComplete.Models.Common;
using YamlDotNet.Serialization;

namespace AlgoMonsterComplete.Patterns.FundamentalSorting.Examples;

public class InsertionSortSolution : AlgorithmBase
{
    private readonly SolutionContent _content;

    public InsertionSortSolution(ILogger<InsertionSortSolution> logger) : base(logger)
    {
        var yamlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            "Patterns", "FundamentalSorting", "Examples", "InsertionSortSolution.yml");
        var yamlContent = File.ReadAllText(yamlPath);
        var deserializer = new DeserializerBuilder().Build();
        _content = deserializer.Deserialize<SolutionContent>(yamlContent);
    }

    public override string Name => _content.Title;
    public override string Description => _content.Description;
    public override string Pattern => _content.Pattern;

    protected override async Task RunExampleAsync()
    {
        Console.WriteLine($"📚 Reference: {_content.AlgoMonsterReference}");
        Console.WriteLine();

        DisplayComplexityAnalysis();
        await RunTestCases();
        DemonstrateImplementation();
    }

    private void DisplayComplexityAnalysis()
    {
        var analysis = _content.MySolution.ComplexityAnalysis;
        Console.WriteLine("📊 Complexity Analysis:");
        Console.WriteLine($"  ⏰ Time: {analysis.Time}");
        Console.WriteLine($"  💾 Space: {analysis.Space}");
        Console.WriteLine($"  🔄 Stable: {(analysis.Stable ? "Yes" : "No")}");
        Console.WriteLine($"  📍 In-place: {(analysis.InPlace ? "Yes" : "No")}");
        Console.WriteLine();
    }

    private async Task RunTestCases()
    {
        Console.WriteLine("🧪 Test Cases:");
        foreach (var testCase in _content.TestCases)
        {
            var input = ParseInput(testCase.Input);
            var result = InsertionSort(input.ToList());
            var resultStr = $"[{string.Join(", ", result)}]";

            var status = resultStr == testCase.Expected ? "✅" : "❌";
            Console.WriteLine($"  {status} Input: {testCase.Input}");
            Console.WriteLine($"     Expected: {testCase.Expected}");
            Console.WriteLine($"     Got: {resultStr}");
            if (!string.IsNullOrEmpty(testCase.Notes))
                Console.WriteLine($"     Notes: {testCase.Notes}");
            Console.WriteLine();
        }
    }

    private void DemonstrateImplementation()
    {
        Console.WriteLine("🔧 Implementation Walkthrough:");
        var demo = new[] { 5, 2, 4, 6, 1, 3 };
        Console.WriteLine($"Starting with: [{string.Join(", ", demo)}]");
        Console.WriteLine();

        InsertionSortWithVisualization(demo.ToList());
    }

    private List<int> InsertionSort(List<int> list)
    {
        for (int i = 1; i < list.Count; i++)
        {
            int current = i;
            while (current > 0 && list[current] < list[current - 1])
            {
                (list[current], list[current - 1]) = (list[current - 1], list[current]);
                current--;
            }
        }
        return list;
    }

    private List<int> InsertionSortWithVisualization(List<int> list)
    {
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
        return list;
    }

    private int[] ParseInput(string input)
    {
        return input.Trim('[', ']')
                   .Split(',')
                   .Select(s => int.Parse(s.Trim()))
                   .ToArray();
    }
}