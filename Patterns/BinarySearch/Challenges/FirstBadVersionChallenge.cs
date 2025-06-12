// ========================
// File: Patterns/BinarySearch/Challenges/FirstBadVersionChallenge.cs
// ========================
using Microsoft.Extensions.Logging;
using AlgoMonsterComplete.Core.Interfaces;
using AlgoMonsterComplete.Core.BaseClasses;

namespace AlgoMonsterComplete.Patterns.BinarySearch.Challenges;

public class FirstBadVersionChallenge : ChallengeBase
{
    public FirstBadVersionChallenge(ILogger<FirstBadVersionChallenge> logger) : base(logger) { }

    public override string Name => "First Bad Version";
    public override string Description => "Find the first bad version using minimal API calls";
    public override ChallengeDifficulty Difficulty => ChallengeDifficulty.Easy;

    protected override async Task RunChallengeAsync()
    {
        Console.WriteLine("🎯 Problem: You are a product manager and currently leading a team to develop a new product.");
        Console.WriteLine("Unfortunately, the latest version of your product fails the quality check.");
        Console.WriteLine("Since each version is developed based on the previous version, all versions after a bad version are also bad.");
        Console.WriteLine("Given n versions [1, 2, ..., n], find the first bad version that causes subsequent ones to be bad.");
        Console.WriteLine();

        // Test with simulated API
        var testCases = new[]
        {
            (n: 5, firstBad: 4),
            (n: 1, firstBad: 1),
            (n: 10, firstBad: 3),
            (n: 100, firstBad: 50)
        };

        foreach (var (n, firstBad) in testCases)
        {
            _currentFirstBad = firstBad; // Set for simulation
            _apiCalls = 0;

            var result = FindFirstBadVersion(n);
            var status = result == firstBad ? "✅" : "❌";

            Console.WriteLine($"{status} n={n}, first bad={firstBad}, found={result}, API calls={_apiCalls}");
        }

        PromptForInteractiveTest(
            "Enter n and first bad version (e.g., '20 15'):",
            input =>
            {
                var parts = input.Split(' ');
                if (parts.Length == 2 && int.TryParse(parts[0], out int n) && int.TryParse(parts[1], out int firstBad))
                {
                    _currentFirstBad = firstBad;
                    _apiCalls = 0;
                    var result = FindFirstBadVersion(n);
                    Console.WriteLine($"Result: Found first bad version {result} in {_apiCalls} API calls");
                    Console.WriteLine($"Optimal calls would be ⌈log₂({n})⌉ = {Math.Ceiling(Math.Log2(n))}");
                }
            });

        await Task.CompletedTask;
    }

    private int _currentFirstBad;
    private int _apiCalls;

    private bool IsBadVersion(int version)
    {
        _apiCalls++;
        return version >= _currentFirstBad;
    }

    private int FindFirstBadVersion(int n)
    {
        int left = 1, right = n;

        while (left < right)
        {
            int mid = left + (right - left) / 2;
            if (IsBadVersion(mid))
            {
                right = mid; // First bad is at mid or before
            }
            else
            {
                left = mid + 1; // First bad is after mid
            }
        }

        return left;
    }
}