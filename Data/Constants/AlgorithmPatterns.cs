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

    // 🚀 New: Pattern shortcuts for faster typing
    public static readonly Dictionary<string, string> PatternShortcuts = new()
    {
        ["fs"] = "fundamental-sorting",
        ["bs"] = "binary-search",
        ["tp"] = "two-pointers",
        ["dfs"] = "depth-first-search",
        ["bt"] = "backtracking",
        ["bfs"] = "breadth-first-search",
        ["g"] = "graph",
        ["pq"] = "priority-queue-heap",
        ["dp"] = "dynamic-programming",
        ["ads"] = "advanced-data-structures",
        ["misc"] = "miscellaneous",
        ["oop"] = "oop-design",
        ["sys"] = "system-design"
    };

    // 🎯 New: Fuzzy matching keywords
    private static readonly Dictionary<string, string[]> PatternKeywords = new()
    {
        ["fundamental-sorting"] = new[] { "fund", "fundamental", "sort", "sorting", "basic" },
        ["binary-search"] = new[] { "bin", "binary", "search", "bs" },
        ["two-pointers"] = new[] { "two", "pointer", "pointers", "twin" },
        ["depth-first-search"] = new[] { "depth", "dfs", "deep" },
        ["backtracking"] = new[] { "back", "backtrack", "tracking", "bt" },
        ["breadth-first-search"] = new[] { "breadth", "bfs", "wide", "level" },
        ["graph"] = new[] { "graph", "node", "vertex", "edge" },
        ["priority-queue-heap"] = new[] { "priority", "queue", "heap", "pq" },
        ["dynamic-programming"] = new[] { "dynamic", "dp", "memo", "optimization" },
        ["advanced-data-structures"] = new[] { "advanced", "data", "structure", "ads" },
        ["miscellaneous"] = new[] { "misc", "other", "various" },
        ["oop-design"] = new[] { "oop", "object", "design", "class" },
        ["system-design"] = new[] { "system", "sys", "architecture", "scale" }
    };

    /// <summary>
    /// Resolves user input to a canonical pattern key using shortcuts and fuzzy matching
    /// </summary>
    /// <param name="input">User input (e.g., "fs", "fund", "sorting")</param>
    /// <returns>Canonical pattern key or null if no match found</returns>
    public static string? ResolvePattern(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        var normalizedInput = input.ToLowerInvariant().Trim();

        // 1. Exact match (canonical pattern key)
        if (PatternDisplayNames.ContainsKey(normalizedInput))
            return normalizedInput;

        // 2. Shortcut match
        if (PatternShortcuts.TryGetValue(normalizedInput, out var shortcutPattern))
            return shortcutPattern;

        // 3. Fuzzy matching - check if input matches any keywords
        foreach (var pattern in PatternKeywords)
        {
            var patternKey = pattern.Key;
            var keywords = pattern.Value;

            // Check for partial matches in keywords
            if (keywords.Any(keyword =>
                keyword.StartsWith(normalizedInput, StringComparison.OrdinalIgnoreCase) ||
                normalizedInput.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
            {
                return patternKey;
            }
        }

        // 4. Partial match on pattern key itself
        var partialMatch = PatternDisplayNames.Keys
            .FirstOrDefault(key => key.Contains(normalizedInput, StringComparison.OrdinalIgnoreCase));

        return partialMatch;
    }

    /// <summary>
    /// Gets all available shortcuts and examples for help display
    /// </summary>
    public static string GetShortcutHelp()
    {
        var shortcuts = PatternShortcuts
            .Take(5) // Show first 5 as examples
            .Select(kvp => $"{kvp.Key} → {PatternDisplayNames[kvp.Value]}")
            .ToList();

        return string.Join(", ", shortcuts) + "...";
    }
}