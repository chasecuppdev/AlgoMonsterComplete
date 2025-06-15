using Microsoft.Extensions.Logging;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;
using System.Runtime.Loader;
using System.Collections.Concurrent;
using AlgoMonsterComplete.Core.Interfaces;

namespace AlgoMonsterComplete.Core
{
    public class AlgorithmCompilationService : IAlgorithmCompilationService
    {
        private readonly ILogger<AlgorithmCompilationService> _logger;
        private readonly ConcurrentDictionary<string, MethodInfo?> _compilationCache = new();

        public AlgorithmCompilationService(ILogger<AlgorithmCompilationService> logger)
        {
            _logger = logger;
        }

        public MethodInfo? CompileAlgorithm(string exerciseName, string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                _logger.LogWarning("No implementation code found for {ExerciseName}", exerciseName);
                return null;
            }

            // Check cache first
            var cacheKey = $"{exerciseName}_{code.GetHashCode()}";
            if (_compilationCache.TryGetValue(cacheKey, out var cachedMethod))
            {
                _logger.LogDebug("Using cached compilation for {ExerciseName}", exerciseName);
                return cachedMethod;
            }

            _logger.LogInformation("Compiling algorithm: {ExerciseName}", exerciseName);

            try
            {
                var compiledMethod = PerformCompilation(exerciseName, code);

                // Cache the result (even if null)
                _compilationCache.TryAdd(cacheKey, compiledMethod);

                if (compiledMethod != null)
                {
                    _logger.LogInformation("Successfully compiled algorithm: {ExerciseName}", exerciseName);
                }

                return compiledMethod;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error compiling algorithm for {ExerciseName}", exerciseName);
                _compilationCache.TryAdd(cacheKey, null);
                return null;
            }
        }

        private MethodInfo? PerformCompilation(string exerciseName, string code)
        {
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
            var assemblyName = $"Algorithm_{exerciseName}_{Guid.NewGuid():N}";
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

                _logger.LogError("Compilation failed for {ExerciseName}: {Errors}", exerciseName, errors);
                Console.WriteLine($"❌ Compilation failed: {errors}");
                return null;
            }

            // Load the compiled assembly
            ms.Seek(0, SeekOrigin.Begin);
            var context = AssemblyLoadContext.Default;
            var assembly = context.LoadFromStream(ms);

            var algorithmType = assembly.GetType("DynamicAlgorithm.Algorithm");
            if (algorithmType == null)
            {
                _logger.LogError("Could not find Algorithm class in compiled assembly");
                return null;
            }

            // Enhanced method discovery - look for common algorithm method names and return types
            var method = algorithmType.GetMethod("Sort")              // Sorting algorithms
                       ?? algorithmType.GetMethod("Execute")          // Generic execution
                       ?? algorithmType.GetMethod("Run")              // Generic execution
                       ?? algorithmType.GetMethod("BinarySearch")     // Search algorithms
                       ?? algorithmType.GetMethod("Search")           // Search algorithms
                       ?? algorithmType.GetMethod("Find")             // Search algorithms
                       ?? algorithmType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                           .FirstOrDefault(m => IsValidAlgorithmMethod(m));

            if (method == null)
            {
                var availableMethods = algorithmType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .Where(m => m.DeclaringType == algorithmType)
                    .Select(m => $"{m.Name}({string.Join(", ", m.GetParameters().Select(p => p.ParameterType.Name))})")
                    .ToList();

                _logger.LogError("Could not find suitable method in compiled algorithm. Available methods: {Methods}",
                    string.Join(", ", availableMethods));
                return null;
            }

            return method;
        }

        private static bool IsValidAlgorithmMethod(MethodInfo method)
        {
            // Skip methods that aren't user-defined (like ToString, GetHashCode, etc.)
            if (method.DeclaringType != method.ReflectedType)
                return false;

            var returnType = method.ReturnType;

            // Valid return types for algorithms
            return returnType == typeof(List<int>)           // Sorting algorithms
                || returnType == typeof(int[])               // Sorting algorithms  
                || returnType == typeof(IEnumerable<int>)    // Sorting algorithms
                || returnType == typeof(int)                 // Search algorithms (index)
                || returnType == typeof(bool)                // Boolean search algorithms
                || returnType == typeof(string)              // String algorithms
                || returnType == typeof(List<string>)        // String list algorithms
                || returnType == typeof(double)              // Numeric algorithms
                || returnType == typeof(void);               // In-place algorithms
        }
    }
}