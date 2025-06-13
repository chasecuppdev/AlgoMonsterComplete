using Microsoft.Extensions.Logging;
using AlgoMonsterComplete.Models.Common;
using AlgoMonsterComplete.Core.Interfaces;
using YamlDotNet.Serialization;
using System.Reflection;

namespace AlgoMonsterComplete.Core;

public class ExerciseRunner : IExerciseRunner
{
    private readonly Exercise _exercise;
    private readonly string _yamlPath;
    private readonly ILogger<ExerciseRunner> _logger;
    private readonly IAlgorithmCompilationService _compilationService;
    private MethodInfo? _compiledMethod;

    public ExerciseRunner(string yamlPath, ILogger<ExerciseRunner> logger, IAlgorithmCompilationService compilationService)
    {
        _yamlPath = yamlPath;
        _logger = logger;
        _compilationService = compilationService;

        var yamlContent = File.ReadAllText(yamlPath);
        var deserializer = new DeserializerBuilder().Build();
        _exercise = deserializer.Deserialize<Exercise>(yamlContent);

        // Compile the algorithm on initialization
        CompileAlgorithm();
    }

    public string Name => _exercise.Title;
    public string Description => _exercise.Description;
    public string Pattern => _exercise.Pattern;

    public void RunExerciseAsync()
    {
        Console.WriteLine($"📚 Reference: {_exercise.AlgoMonsterReference}");
        Console.WriteLine();

        DisplayComplexityAnalysis();
        DisplayImplementation();
        RunUnitTests();
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

    private void RunUnitTests()
    {
        if (_compiledMethod == null)
        {
            Console.WriteLine("❌ Algorithm not compiled - cannot run tests");
            return;
        }

        if (_exercise.TestCases == null || !_exercise.TestCases.Any())
        {
            Console.WriteLine("⚠️ No test cases found in YAML file");
            return;
        }

        Console.WriteLine("🧪 Unit Tests (Running my implementation against test cases):");
        Console.WriteLine($"Found {_exercise.TestCases.Count} test case(s)");
        Console.WriteLine();

        int passCount = 0;
        int totalCount = _exercise.TestCases.Count;

        for (int i = 0; i < _exercise.TestCases.Count; i++)
        {
            var testCase = _exercise.TestCases[i];
            Console.WriteLine($"Test {i + 1}/{totalCount}:");

            try
            {
                var input = ParseInput(testCase.Input);
                var result = ExecuteAlgorithm(input);
                var resultStr = FormatOutput(result);

                bool passed = resultStr == testCase.Expected;
                var status = passed ? "✅ PASS" : "❌ FAIL";

                if (passed) passCount++;

                Console.WriteLine($"  {status}");
                Console.WriteLine($"     Input: {testCase.Input}");
                Console.WriteLine($"     Expected: {testCase.Expected}");
                Console.WriteLine($"     Actual: {resultStr}");

                if (!string.IsNullOrEmpty(testCase.Notes))
                    Console.WriteLine($"     Notes: {testCase.Notes}");

                if (!passed)
                {
                    Console.WriteLine($"     ❌ Test failed - arrays don't match");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ❌ ERROR - {ex.Message}");
                Console.WriteLine($"     Input: {testCase.Input}");
                _logger.LogError(ex, "Test case {TestIndex} failed with exception", i + 1);
            }

            Console.WriteLine();
        }

        // Summary
        Console.WriteLine($"📊 Test Results: {passCount}/{totalCount} passed");

        if (passCount == totalCount)
        {
            Console.WriteLine("🎉 All tests passed!");
        }
        else
        {
            Console.WriteLine($"⚠️ {totalCount - passCount} test(s) failed");
        }

        Console.WriteLine();
    }

    private void DemonstrateAlgorithm()
    {
        if (_compiledMethod == null)
        {
            Console.WriteLine("❌ Algorithm not compiled - cannot demonstrate");
            return;
        }

        Console.WriteLine("🔧 Algorithm Demonstration:");
        var demo = new[] { 5, 2, 4, 6, 1, 3 };
        Console.WriteLine($"Input: [{string.Join(", ", demo)}]");

        var result = ExecuteAlgorithm(demo);
        Console.WriteLine($"Output: [{string.Join(", ", result)}]");
        Console.WriteLine();

        // For now, just show input/output. Later we can add step-by-step visualization
        Console.WriteLine("💡 Step-by-step visualization coming in future updates!");
    }

    private void CompileAlgorithm()
    {
        var code = _exercise.MySolution?.Implementation;
        _compiledMethod = _compilationService.CompileAlgorithm(_exercise.Title, code ?? string.Empty);
    }

    private List<int> ExecuteAlgorithm(int[] input)
    {
        if (_compiledMethod == null)
            throw new InvalidOperationException("Algorithm not compiled");

        try
        {
            // Get the method's parameter type
            var parameters = _compiledMethod.GetParameters();
            if (parameters.Length != 1)
                throw new InvalidOperationException("Algorithm method must have exactly one parameter");

            var parameterType = parameters[0].ParameterType;

            // Convert input to the expected parameter type
            object methodInput = ConvertToParameterType(input, parameterType);

            // Execute the method
            var result = _compiledMethod.Invoke(null, new[] { methodInput });

            // Handle different return types
            return result switch
            {
                List<int> list => list,
                int[] array => array.ToList(),
                IEnumerable<int> enumerable => enumerable.ToList(),
                _ => throw new InvalidOperationException($"Unexpected return type: {result?.GetType()}")
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing algorithm");
            throw new InvalidOperationException($"Algorithm execution failed: {ex.Message}", ex);
        }
    }

    private static object ConvertToParameterType(int[] input, Type parameterType)
    {
        // Handle the most common parameter types
        if (parameterType == typeof(int[]))
        {
            return input;
        }
        else if (parameterType == typeof(List<int>))
        {
            return input.ToList();
        }
        else if (parameterType == typeof(IList<int>))
        {
            return input.ToList();
        }
        else if (parameterType == typeof(IEnumerable<int>))
        {
            return input.AsEnumerable();
        }
        else if (parameterType == typeof(ICollection<int>))
        {
            return input.ToList();
        }
        else
        {
            throw new InvalidOperationException(
                $"Unsupported parameter type: {parameterType.Name}. " +
                $"Supported types: int[], List<int>, IList<int>, IEnumerable<int>, ICollection<int>");
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