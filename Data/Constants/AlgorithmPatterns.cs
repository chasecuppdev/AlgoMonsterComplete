namespace AlgoMonsterComplete.Data.Constants;

public static class AlgorithmPatterns
{
    public static readonly Dictionary<string, string> FolderToPatternMap = new()
    {
        ["FundamentalSorting"] = "fundamental-sorting",
        ["BinarySearch"] = "binary-search",
        ["TwoPointers"] = "two-pointers",
        ["DepthFirstSearch"] = "depth-first-search",
        ["Backtracking"] = "backtracking",
        ["BreadthFirstSearch"] = "breadth-first-search",
        ["Graph"] = "graph",
        ["PriorityQueueHeap"] = "priority-queue-heap",
        ["DynamicProgramming"] = "dynamic-programming",
        ["AdvancedDataStructures"] = "advanced-data-structures",
        ["Miscellaneous"] = "miscellaneous",
        ["OopDesign"] = "oop-design",
        ["SystemDesign"] = "system-design"
    };

    public static readonly Dictionary<string, string> PatternDisplayNames = new()
    {
        ["fundamental-sorting"] = "Fundamental Sorting",
        ["binary-search"] = "Binary Search",
        ["two-pointers"] = "Two Pointers",
        ["depth-first-search"] = "Depth First Search",
        ["backtracking"] = "Backtracking",
        ["breadth-first-search"] = "Breadth First Search",
        ["graph"] = "Graph",
        ["priority-queue-heap"] = "Priority Queue / Heap",
        ["dynamic-programming"] = "Dynamic Programming",
        ["advanced-data-structures"] = "Advanced Data Structures",
        ["miscellaneous"] = "Miscellaneous",
        ["oop-design"] = "OOP Design",
        ["system-design"] = "System Design"
    };
}