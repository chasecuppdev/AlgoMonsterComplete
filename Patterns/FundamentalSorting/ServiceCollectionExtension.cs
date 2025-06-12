using Microsoft.Extensions.DependencyInjection;
using AlgoMonsterComplete.Core.Interfaces;
using AlgoMonsterComplete.Patterns.FundamentalSorting.Examples;

namespace AlgoMonsterComplete.Patterns.FundamentalSorting;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFundamentalSortingPattern(this IServiceCollection services)
    {
        // Register sorting algorithm solutions
        services.AddKeyedTransient<IAlgorithmExample, InsertionSortSolution>("fundamental-sorting");

        return services;
    }
}