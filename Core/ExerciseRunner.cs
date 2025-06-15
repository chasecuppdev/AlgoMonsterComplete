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
                var inputs = ParseInputBasedOnFormat(testCase.Input, _exercise.InputFormat);
                var result = ExecuteAlgorithm(inputs);
                var resultStr = FormatOutput(result);

                bool passed = ValidateResult(resultStr, testCase);
                var status = passed ? "✅ PASS" : "❌ FAIL";

                if (passed) passCount++;

                Console.WriteLine($"  {status}");
                Console.WriteLine($"     Input: {testCase.Input}");

                // Display expected result(s)
                if (testCase.ExpectedOptions != null && testCase.ExpectedOptions.Any())
                {
                    Console.WriteLine($"     Expected: Any of [{string.Join(", ", testCase.ExpectedOptions)}]");
                }
                else
                {
                    Console.WriteLine($"     Expected: {testCase.Expected}");
                }

                Console.WriteLine($"     Actual: {resultStr}");

                if (!string.IsNullOrEmpty(testCase.Notes))
                    Console.WriteLine($"     Notes: {testCase.Notes}");

                if (!passed)
                {
                    Console.WriteLine($"     ❌ Test failed - result doesn't match any expected value");
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

    private bool ValidateResult(string actualResult, TestCase testCase)
    {
        // If ExpectedOptions is provided and has values, check against all options
        if (testCase.ExpectedOptions != null && testCase.ExpectedOptions.Any())
        {
            return testCase.ExpectedOptions.Contains(actualResult);
        }

        // Fall back to single Expected value
        return actualResult == testCase.Expected;
    }

    private void CompileAlgorithm()
    {
        var code = _exercise.MySolution?.Implementation;
        _compiledMethod = _compilationService.CompileAlgorithm(_exercise.Title, code ?? string.Empty);
    }

    private object[] ParseInputBasedOnFormat(string input, string inputFormat)
    {
        // Default behavior: single array parameter (maintains backward compatibility)
        if (string.IsNullOrEmpty(inputFormat))
        {
            return new object[] { ParseInput(input) };
        }

        // Handle specific input formats
        switch (inputFormat.ToLowerInvariant())
        {
            case "array, target":
                return ParseArrayAndTarget(input);

            case "array, target, flag":
                return ParseArrayTargetAndFlag(input);

            // Add more formats as needed
            default:
                throw new NotSupportedException($"InputFormat '{inputFormat}' is not supported. " +
                    "Supported formats: 'array, target', 'array, target, flag'");
        }
    }

    private object[] ParseArrayAndTarget(string input)
    {
        // Expected format: "[1, 2, 3], 5"
        var lastCommaIndex = input.LastIndexOf(", ");
        if (lastCommaIndex == -1)
            throw new FormatException($"Expected format: '[array], target' but got: {input}");

        var arrayPart = input.Substring(0, lastCommaIndex).Trim();
        var targetPart = input.Substring(lastCommaIndex + 2).Trim();

        var array = ParseInput(arrayPart);
        var target = int.Parse(targetPart);

        return new object[] { array, target };
    }

    private object[] ParseArrayTargetAndFlag(string input)
    {
        // Expected format: "[1, 2, 3], 5, true"
        var parts = input.Split(", ");
        if (parts.Length != 3)
            throw new FormatException($"Expected format: '[array], target, flag' but got: {input}");

        var array = ParseInput(parts[0].Trim());
        var target = int.Parse(parts[1].Trim());
        var flag = bool.Parse(parts[2].Trim());

        return new object[] { array, target, flag };
    }

    private object ExecuteAlgorithm(object[] inputs)
    {
        if (_compiledMethod == null)
            throw new InvalidOperationException("Algorithm not compiled");

        try
        {
            // Get the method's parameters
            var parameters = _compiledMethod.GetParameters();

            if (parameters.Length != inputs.Length)
                throw new InvalidOperationException($"Algorithm method expects {parameters.Length} parameter(s), but {inputs.Length} input(s) provided");

            // Convert inputs to the expected parameter types
            var methodInputs = new object[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                methodInputs[i] = ConvertToParameterType(inputs[i], parameters[i].ParameterType);
            }

            // Execute the method
            var result = _compiledMethod.Invoke(null, methodInputs);

            // Handle different return types
            return result switch
            {
                List<int> list => list,
                int[] array => array.ToList(),
                IEnumerable<int> enumerable => enumerable.ToList(),
                int intResult => intResult,
                bool boolResult => boolResult,
                _ => result ?? throw new InvalidOperationException("Method returned null")
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing algorithm");
            throw new InvalidOperationException($"Algorithm execution failed: {ex.Message}", ex);
        }
    }

    private static object ConvertToParameterType(object input, Type parameterType)
    {
        // Handle array/list conversions
        if (input is int[] intArray)
        {
            if (parameterType == typeof(int[]))
                return intArray;
            else if (parameterType == typeof(List<int>))
                return intArray.ToList();
            else if (parameterType == typeof(IList<int>))
                return intArray.ToList();
            else if (parameterType == typeof(IEnumerable<int>))
                return intArray.AsEnumerable();
            else if (parameterType == typeof(ICollection<int>))
                return intArray.ToList();
        }

        // Handle direct type matches
        if (parameterType.IsAssignableFrom(input.GetType()))
            return input;

        // Handle primitive conversions
        if (parameterType == typeof(int) && input is int)
            return input;

        if (parameterType == typeof(bool) && input is bool)
            return input;

        throw new InvalidOperationException(
            $"Cannot convert input type {input.GetType().Name} to parameter type {parameterType.Name}");
    }

    private int[] ParseInput(string input)
    {
        var trimmed = input.Trim('[', ']');

        // Handle empty array case
        if (string.IsNullOrWhiteSpace(trimmed))
            return Array.Empty<int>();

        return trimmed.Split(',')
                      .Select(s => int.Parse(s.Trim()))
                      .ToArray();
    }

    private string FormatOutput(object result)
    {
        return result switch
        {
            List<int> list => $"[{string.Join(", ", list)}]",
            int[] array => $"[{string.Join(", ", array)}]",
            int intResult => intResult.ToString(),
            bool boolResult => boolResult.ToString().ToLower(),
            _ => result?.ToString() ?? "null"
        };
    }
}