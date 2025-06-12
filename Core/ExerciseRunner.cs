using Microsoft.Extensions.Logging;
using AlgoMonsterComplete.Models.Common;
using AlgoMonsterComplete.Core.Interfaces;
using YamlDotNet.Serialization;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System.Reflection;
using System.Runtime.Loader;

namespace AlgoMonsterComplete.Core;

public class ExerciseRunner : IExerciseRunner
{
    private readonly Exercise _exercise;
    private readonly string _yamlPath;
    private readonly ILogger<ExerciseRunner> _logger;
    private MethodInfo? _compiledMethod;

    public ExerciseRunner(string yamlPath, ILogger<ExerciseRunner> logger)
    {
        _yamlPath = yamlPath;
        _logger = logger;
        var yamlContent = File.ReadAllText(yamlPath);
        var deserializer = new DeserializerBuilder().Build();
        _exercise = deserializer.Deserialize<Exercise>(yamlContent);

        // Compile the algorithm on initialization
        CompileAlgorithm();
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
        if (_compiledMethod == null)
        {
            Console.WriteLine("❌ Algorithm not compiled - cannot run tests");
            return;
        }

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
        try
        {
            var code = _exercise.MySolution?.Implementation;
            if (string.IsNullOrEmpty(code))
            {
                _logger.LogWarning("No implementation code found in YAML for {ExerciseName}", _exercise.Title);
                return;
            }

            _logger.LogInformation("Compiling algorithm: {ExerciseName}", _exercise.Title);

            // Create the full class with the user's method
            var fullCode = $@"
using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicAlgorithm
{{
    public static class Algorithm 
    {{
        {code}
    }}
}}";

            // Parse into syntax tree
            var syntaxTree = CSharpSyntaxTree.ParseText(fullCode);

            // Add required assembly references
            var references = new[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(List<>).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
                MetadataReference.CreateFromFile(Assembly.Load("System.Collections").Location)
            };

            // Create compilation
            var assemblyName = $"Algorithm_{Guid.NewGuid():N}";
            var compilation = CSharpCompilation.Create(
                assemblyName,
                new[] { syntaxTree },
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            // Compile to memory
            using var ms = new MemoryStream();
            var emitResult = compilation.Emit(ms);

            if (!emitResult.Success)
            {
                var errors = string.Join("\n", emitResult.Diagnostics
                    .Where(d => d.Severity == DiagnosticSeverity.Error)
                    .Select(d => d.GetMessage()));

                _logger.LogError("Compilation failed for {ExerciseName}: {Errors}", _exercise.Title, errors);
                Console.WriteLine($"❌ Compilation failed: {errors}");
                return;
            }

            // Load the compiled assembly
            ms.Seek(0, SeekOrigin.Begin);
            var context = AssemblyLoadContext.Default;
            var assembly = context.LoadFromStream(ms);

            var algorithmType = assembly.GetType("DynamicAlgorithm.Algorithm");
            if (algorithmType == null)
            {
                _logger.LogError("Could not find Algorithm class in compiled assembly");
                return;
            }

            // Find the method - look for common method names
            _compiledMethod = algorithmType.GetMethod("Sort")
                           ?? algorithmType.GetMethod("Execute")
                           ?? algorithmType.GetMethod("Run")
                           ?? algorithmType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                               .FirstOrDefault(m => m.ReturnType == typeof(List<int>) || m.ReturnType == typeof(int[]));

            if (_compiledMethod == null)
            {
                _logger.LogError("Could not find suitable method in compiled algorithm");
                return;
            }

            _logger.LogInformation("Successfully compiled algorithm: {ExerciseName}", _exercise.Title);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error compiling algorithm for {ExerciseName}", _exercise.Title);
        }
    }

    private List<int> ExecuteAlgorithm(int[] input)
    {
        if (_compiledMethod == null)
            throw new InvalidOperationException("Algorithm not compiled");

        try
        {
            var result = _compiledMethod.Invoke(null, new object[] { input });

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