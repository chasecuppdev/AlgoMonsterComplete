// ========================
// File: Patterns/BinarySearch/Challenges/SearchInsertPositionChallenge.cs
// ========================
using Microsoft.Extensions.Logging;
using AlgoMonsterComplete.Core.Interfaces;
using AlgoMonsterComplete.Core.BaseClasses;

namespace AlgoMonsterComplete.Patterns.BinarySearch.Challenges;

public class SearchInsertPositionChallenge : ChallengeBase
{
    public SearchInsertPositionChallenge(ILogger<SearchInsertPositionChallenge> logger) : base(logger) { }

    public override string Name => "Search Insert Position";
    public override string Description => "Find the position where target should be inserted in sorted array";
    public override ChallengeDifficulty Difficulty => ChallengeDifficulty.Easy;

    protected override async Task RunChallengeAsync()
    {
        Console.WriteLine("🎯 Problem: Given a sorted array of distinct integers and a target value,");
        Console.WriteLine("return the index if the target is found. If not, return the index where it would be inserted.");
        Console.WriteLine("You must write an algorithm with O(log n) runtime complexity.");
        Console.WriteLine();

        var testCases = new[]
        {
            (new[] { 1, 3, 5, 6 }, 5, 2),
            (new[] { 1, 3, 5, 6 }, 2, 1),
            (new[] { 1, 3, 5, 6 }, 7, 4),
            (new[] { 1, 3, 5, 6 }, 0, 0),
            (new[] { 1 }, 0, 0)
        };

        RunAutomatedTests("Search Insert Position",
            (input, expected) => {
                var nums = input[0..^1];
                var target = input[^1];
                var result = SearchInsert(nums, target);
                return result == expected[0];
            },
            testCases.Select(tc => (
                input: tc.Item1.Append(tc.Item2).ToArray(),
                expected: new[] { tc.Item3 }
            )).ToArray());

        PromptForInteractiveTest(
            "Enter sorted array and target (e.g., '1,3,5,6 2'):",
            input =>
            {
                var parts = input.Split(' ');
                if (parts.Length == 2)
                {
                    var nums = parts[0].Split(',').Select(int.Parse).ToArray();
                    var target = int.Parse(parts[1]);
                    var result = SearchInsert(nums, target);
                    Console.WriteLine($"Array: [{string.Join(", ", nums)}]");
                    Console.WriteLine($"Target: {target}");
                    Console.WriteLine($"Insert Position: {result}");
                }
            });

        await Task.CompletedTask;
    }

    private int SearchInsert(int[] nums, int target)
    {
        int left = 0, right = nums.Length - 1;

        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            if (nums[mid] == target)
            {
                return mid;
            }
            else if (nums[mid] < target)
            {
                left = mid + 1;
            }
            else
            {
                right = mid - 1;
            }
        }

        return left; // Insert position
    }
}