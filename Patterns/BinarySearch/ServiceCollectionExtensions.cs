// ========================
// File: Patterns/BinarySearch/ServiceCollectionExtensions.cs
// ========================
using Microsoft.Extensions.DependencyInjection;
using AlgoMonsterComplete.Core.Interfaces;
using AlgoMonsterComplete.Patterns.BinarySearch.Examples;
using AlgoMonsterComplete.Patterns.BinarySearch.Challenges;

namespace AlgoMonsterComplete.Patterns.BinarySearch;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBinarySearchPattern(this IServiceCollection services)
    {
        // Register examples (instructional)
        services.AddKeyedTransient<IAlgorithmExample, BasicBinarySearchExample>("binary-search");
        services.AddKeyedTransient<IAlgorithmExample, BinarySearchVariationsExample>("binary-search");

        // Register challenges (practice problems)
        services.AddKeyedTransient<IAlgorithmChallenge, FirstBadVersionChallenge>("binary-search");
        services.AddKeyedTransient<IAlgorithmChallenge, SearchInsertPositionChallenge>("binary-search");

        return services;
    }
}