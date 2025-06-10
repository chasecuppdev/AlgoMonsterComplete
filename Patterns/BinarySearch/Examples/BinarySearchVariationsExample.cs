// ========================
// File: Patterns/BinarySearch/Examples/BinarySearchVariationsExample.cs
// ========================
using Microsoft.Extensions.Logging;
using AlgoMonsterComplete.Core;

namespace AlgoMonsterComplete.Patterns.BinarySearch.Examples;

public class BinarySearchVariationsExample : AlgorithmBase
{
    public BinarySearchVariationsExample(ILogger<BinarySearchVariationsExample> logger) : base(logger) { }

    public override string Name => "Binary Search Variations";
    public override string Description => "Find first/last occurrence and insertion points";
    public override string Pattern => "Binary Search";

    protected override async Task RunExampleAsync()
    {
        Console.WriteLine("🎓 Binary Search Variations solve different search problems:");
        Console.WriteLine("1. Find First Occurrence");
        Console.WriteLine("2. Find Last Occurrence");
        Console.WriteLine("3. Find Insertion Point");
        Console.WriteLine();

        var arrayWithDuplicates = new[] { 1, 2, 2, 2, 3, 4, 5 };
        var target = 2;

        // Find first occurrence
        var firstIndex = FindFirst(arrayWithDuplicates, target);
        WriteExample("Find First Occurrence of 2",
            $"[{string.Join(", ", arrayWithDuplicates)}]",
            $"Index {firstIndex}");

        // Find last occurrence
        var lastIndex = FindLast(arrayWithDuplicates, target);
        WriteExample("Find Last Occurrence of 2",
            $"[{string.Join(", ", arrayWithDuplicates)}]",
            $"Index {lastIndex}");

        // Find insertion point
        var insertIndex = FindInsertionPoint(arrayWithDuplicates, 2);
        WriteExample("Find Insertion Point for 2",
            $"[{string.Join(", ", arrayWithDuplicates)}]",
            $"Index {insertIndex}");

        Console.WriteLine("\n📝 Key Insight: Modify the condition to find different positions!");
        Console.WriteLine("• First occurrence: continue searching left when found");
        Console.WriteLine("• Last occurrence: continue searching right when found");
        Console.WriteLine("• Insertion point: find leftmost position where target can be inserted");

        await Task.CompletedTask;
    }

    private int FindFirst(int[] arr, int target)
    {
        int left = 0, right = arr.Length - 1;
        int result = -1;

        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            if (arr[mid] == target)
            {
                result = mid;
                right = mid - 1; // Continue searching left
            }
            else if (arr[mid] < target)
            {
                left = mid + 1;
            }
            else
            {
                right = mid - 1;
            }
        }

        return result;
    }

    private int FindLast(int[] arr, int target)
    {
        int left = 0, right = arr.Length - 1;
        int result = -1;

        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            if (arr[mid] == target)
            {
                result = mid;
                left = mid + 1; // Continue searching right
            }
            else if (arr[mid] < target)
            {
                left = mid + 1;
            }
            else
            {
                right = mid - 1;
            }
        }

        return result;
    }

    private int FindInsertionPoint(int[] arr, int target)
    {
        int left = 0, right = arr.Length - 1;

        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            if (arr[mid] < target)
            {
                left = mid + 1;
            }
            else
            {
                right = mid - 1;
            }
        }

        return left;
    }
}
