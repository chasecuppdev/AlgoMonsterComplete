// ========================
// File: Patterns/BinarySearch/Examples/BasicBinarySearchExample.cs
// ========================
using Microsoft.Extensions.Logging;
using AlgoMonsterComplete.Core;

namespace AlgoMonsterComplete.Patterns.BinarySearch.Examples;

public class BasicBinarySearchExample : AlgorithmBase
{
    public BasicBinarySearchExample(ILogger<BasicBinarySearchExample> logger) : base(logger) { }

    public override string Name => "Basic Binary Search";
    public override string Description => "Learn the fundamental binary search algorithm";
    public override string Pattern => "Binary Search";

    protected override async Task RunExampleAsync()
    {
        Console.WriteLine("🎓 Binary Search is a divide-and-conquer algorithm for finding a target in a sorted array.");
        Console.WriteLine("⏰ Time Complexity: O(log n) | Space Complexity: O(1)");
        Console.WriteLine();

        var sortedArray = new[] { 1, 3, 5, 7, 9, 11, 13, 15, 17, 19 };
        var target = 7;

        WriteExample("Example: Find target 7 in sorted array",
            $"[{string.Join(", ", sortedArray)}], target = {target}",
            "Index 3");

        Console.WriteLine("Step-by-step execution:");
        var result = BinarySearchWithSteps(sortedArray, target);

        Console.WriteLine($"\n🎯 Final Result: Target {target} found at index {result}");

        // Show the template
        Console.WriteLine("\n📝 Binary Search Template:");
        Console.WriteLine("""
        def binary_search(arr, target):
            left, right = 0, len(arr) - 1
            while left <= right:
                mid = (left + right) // 2
                if arr[mid] == target:
                    return mid
                elif arr[mid] < target:
                    left = mid + 1
                else:
                    right = mid - 1
            return -1
        """);

        await Task.CompletedTask;
    }

    private int BinarySearchWithSteps(int[] arr, int target)
    {
        int left = 0, right = arr.Length - 1;
        int step = 1;

        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            Console.WriteLine($"Step {step}: left={left}, right={right}, mid={mid}, arr[mid]={arr[mid]}");

            if (arr[mid] == target)
            {
                WriteStepByStep($"Step {step}", $"Found target! arr[{mid}] = {arr[mid]}");
                return mid;
            }
            else if (arr[mid] < target)
            {
                WriteStepByStep($"Step {step}", $"arr[{mid}] < {target}, search right half");
                left = mid + 1;
            }
            else
            {
                WriteStepByStep($"Step {step}", $"arr[{mid}] > {target}, search left half");
                right = mid - 1;
            }
            step++;
        }

        return -1;
    }
}