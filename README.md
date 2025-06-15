# AlgoMonsterComplete

[![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-blue.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

> A sophisticated .NET 8 console application that transforms algorithm practice into an enterprise-grade learning experience. Built at the intersection of coding interview preparation, systems design, and modern .NET architecture patterns.

## 🎯 **Project Vision**

While using [AlgoMonster](https://algo.monster) for interview preparation, I wanted to go beyond solving isolated functions in browser windows. AlgoMonsterComplete merges algorithm practice with hands-on experience in production-quality software architecture, creating a personal catalog that showcases both fundamental computer science skills and modern enterprise development patterns.

## ⚡ **Key Features**

### 🏗️ **Data-Driven Architecture**
- **YAML-based exercises** - Algorithms stored as configuration, not hardcoded classes
- **Dynamic compilation** - Runtime C# compilation using Roslyn compiler services
- **Hot-swappable implementations** - Update algorithms without rebuilding the application
- **Pattern shortcuts** - Smart fuzzy matching (`fs` → `fundamental-sorting`, `dp` → `dynamic-programming`)

### 🧪 **Built-in Testing Framework**
- **Automated test execution** - Run implementations against YAML-defined test cases
- **Comprehensive reporting** - Pass/fail metrics with detailed output
- **Edge case validation** - Extensive test coverage including boundary conditions

### 🎨 **Modern .NET 8 Patterns**
- **Dependency injection** - Clean separation of concerns with Microsoft.Extensions.DI
- **Hosted services** - Proper application lifecycle management
- **Keyed services** - Pattern-based algorithm organization
- **Structured logging** - Comprehensive logging with Microsoft.Extensions.Logging

### 🚀 **Performance & Scalability**
- **Compilation caching** - Avoid redundant algorithm compilation
- **Concurrent operations** - Thread-safe compilation service
- **Polymorphic input handling** - Supports int[], List<int>, IEnumerable<int> automatically

## 🏛️ **Architecture Overview**

```
┌─────────────────────────────────────────────────────────────┐
│                    AlgoMonsterComplete                     │
├─────────────────────────────────────────────────────────────┤
│  Interactive Console Interface (InteractiveMenu)           │
│    ├─ Pattern Shortcuts & Fuzzy Matching                   │
│    └─ Smart Command Resolution                             │
├─────────────────────────────────────────────────────────────┤
│  Exercise Execution Engine (ExerciseRunner)                │
│    ├─ Polymorphic Input Conversion                         │
│    └─ Comprehensive Test Validation                        │
├─────────────────────────────────────────────────────────────┤
│  Dynamic Compilation Service (AlgorithmCompilationService) │
│    ├─ Roslyn-based Runtime Compilation                     │
│    └─ Intelligent Method Discovery                         │
├─────────────────────────────────────────────────────────────┤
│  YAML Data Layer (Exercise Definitions)                    │
│    ├─ Algorithm Implementations                            │
│    └─ Comprehensive Test Suites                            │
└─────────────────────────────────────────────────────────────┘
```

## 🛠️ **Technology Stack**

- **.NET 8.0** - Latest LTS framework with modern C# features
- **Microsoft.CodeAnalysis.CSharp** - Roslyn compiler services for runtime compilation
- **YamlDotNet** - YAML parsing and deserialization
- **Microsoft.Extensions.*** - Dependency injection, hosting, logging, and configuration
- **C# 12** - Modern language features including primary constructors and collection expressions

## 🚀 **Getting Started**

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Windows Terminal (recommended for best Unicode support)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/chasecuppdev/AlgoMonsterComplete.git
   cd AlgoMonsterComplete
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the application**
   ```bash
   dotnet build
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

## 📋 **Usage**

### Interactive Commands

- `exercises <pattern>` or `ex <pattern>` - Browse exercises by algorithmic pattern
- `patterns` or `p` - List all available algorithmic patterns  
- `shortcuts` or `s` - Show all pattern shortcuts and fuzzy matching examples
- `quit` or `q` - Exit the application

### Pattern Shortcuts

The system supports smart pattern resolution:

```bash
> ex fs           # fundamental-sorting
> ex fund         # fundamental-sorting (fuzzy match)
> ex bs           # binary-search  
> ex dp           # dynamic-programming
> ex sort         # fundamental-sorting (keyword match)
```

### Example Session

```
🚀 Welcome to AlgoMonster Complete with Modern .NET DI!
=========================================================

> ex fs
🎯 Resolved 'fs' → 'fundamental-sorting'

🎓 Exercises in Fundamental Sorting:
  1. 📖 Bubble Sort - Sort array by repeatedly swapping adjacent elements that are out of order
  2. 📖 Insertion Sort - Sort array by building sorted portion one element at a time
  3. 📖 Merge Sort - Divide & conquer sorting using recursive splitting and two-pointer merging
  4. 📖 Quick Sort - Divide & conquer sorting using in-place Hoare partitioning with dual pointers
  5. 📖 Selection Sort - Sort array by repeatedly finding minimum element and placing it at the beginning

Select exercise to run (1-5) or press Enter to return: 4

🎓 Running: Quick Sort
============================================================
📚 Reference: https://algo.monster/problems/advanced_sorting

📊 Complexity Analysis:
  ⏰ Time: O(n log n) avg, O(n^2) worst
  💾 Space: O(log n)
  🔄 Stable: No
  📍 In-place: Yes

💻 My Implementation:
public static List<int> SortList(List<int> list)
{
    // QuickSort (In-Place)
    SortListInterval(list, 0, list.Count - 1);
    return list;
}

public static void SortListInterval(List<int> list, int startIndex, int endIndex)
{
    if (startIndex >= endIndex) 
        return;
    int pivotValue = list[startIndex];
    int leftPointer = startIndex - 1, rightPointer = endIndex + 1;
    
    while (true)
    {
        do {leftPointer++;} while (list[leftPointer] < pivotValue);
        do {rightPointer--;} while (list[rightPointer] > pivotValue);
        if (leftPointer >= rightPointer)
        {
            SortListInterval(list, startIndex, rightPointer);
            SortListInterval(list, rightPointer + 1, endIndex);
            return;
        }
        int temp = list[leftPointer];
        list[leftPointer] = list[rightPointer];
        list[rightPointer] = temp;
    }
}

🧪 Unit Tests (Running my implementation against test cases):
Found 10 test case(s)

Test 1/10:
  ✅ PASS
     Input: []
     Expected: []
     Actual: []
     Notes: Empty array - base case test

Test 10/10:
  ✅ PASS
     Input: [64, 34, 25, 12, 22, 11, 90, 88, 76, 50, 42]
     Expected: [11, 12, 22, 25, 34, 42, 50, 64, 76, 88, 90]
     Actual: [11, 12, 22, 25, 34, 42, 50, 64, 76, 88, 90]
     Notes: Comprehensive partitioning test - multiple recursive levels

📊 Test Results: 10/10 passed
🎉 All tests passed!

🔧 Algorithm Demonstration:
Input: [5, 2, 4, 6, 1, 3]
Output: [1, 2, 3, 4, 5, 6]
```

## 📁 **Project Structure**

```
AlgoMonsterComplete/
├── Core/
│   ├── ExerciseRunner.cs              # Algorithm execution orchestrator
│   ├── InteractiveMenu.cs             # User interface with smart pattern resolution
│   ├── AlgorithmCompilationService.cs # Roslyn compilation with caching
│   ├── ServiceCollectionExtensions.cs # Dependency injection setup
│   └── Interfaces/                    # Service contracts
├── Models/Common/
│   ├── Exercise.cs                    # Main exercise data model
│   ├── Solution.cs                    # Algorithm implementation model
│   ├── TestCase.cs                    # Test case data model
│   └── ComplexityAnalysis.cs          # Algorithm complexity metadata
├── Data/Constants/
│   └── AlgorithmPatterns.cs           # Pattern mappings, shortcuts, and fuzzy matching
└── Patterns/
    ├── FundamentalSorting/
    │   ├── BubbleSort.yml             # O(n²) with early termination optimization
    │   ├── InsertionSort.yml          # O(n²) iterative building
    │   ├── SelectionSort.yml          # O(n²) minimum finding
    │   ├── MergeSort.yml              # O(n log n) functional divide & conquer
    │   └── QuickSort.yml              # O(n log n) Hoare partitioning
    └── [Future patterns...]
```

## 🎯 **Current Algorithm Collection**

### Fundamental Sorting (5 algorithms)
- **Insertion Sort** - Building sorted portion iteratively
- **Selection Sort** - Repeatedly finding minimum elements  
- **Bubble Sort** - Adjacent swapping with early termination optimization
- **Merge Sort** - Functional divide & conquer with GetRange() sublists
- **Quick Sort** - In-place Hoare partitioning with dual pointers

### Advanced Features Demonstrated
- **Recursive algorithms** (Merge Sort, Quick Sort)
- **In-place operations** (Quick Sort, Bubble Sort)
- **Functional approaches** (Merge Sort)
- **Performance optimizations** (Bubble Sort early termination)
- **Complex pointer logic** (Hoare partitioning)

## 📝 **Adding New Algorithms**

Create a new YAML file in the appropriate pattern directory:

```yaml
Title: "Your Algorithm"
Description: "Brief description of what it does"
Pattern: "pattern-key"
AlgoMonsterReference: "https://algo.monster/problems/your-problem"

MySolution:
  ComplexityAnalysis:
    Time: "O(n log n)"
    Space: "O(1)"
    Stable: true
    InPlace: false
  
  Implementation: |
    public static List<int> YourMethod(List<int> input)
    {
        // Your algorithm implementation here
        return result;
    }

TestCases:
  - Input: "[3, 1, 4, 1, 5]"
    Expected: "[1, 1, 3, 4, 5]"
    Notes: "Basic test case"
  - Input: "[]"
    Expected: "[]"
    Notes: "Empty array edge case"
  - Input: "[42]"
    Expected: "[42]"
    Notes: "Single element"
```

The system automatically:
- **Discovers and registers** your YAML file
- **Compiles your algorithm** using Roslyn
- **Handles different parameter types** (int[], List<int>, IEnumerable<int>)
- **Caches compilation** for performance
- **Runs comprehensive tests** with detailed reporting

## 🔮 **Future Roadmap**

### Phase 1: Pattern Expansion
- **Binary Search** - Logarithmic search algorithms
- **Two Pointers** - Linear scanning techniques
- **Depth-First Search** - Graph traversal algorithms
- **Dynamic Programming** - Optimization problem solving

### Phase 2: Enhanced Visualization
- **Step-by-step algorithm visualization** - Visual debugging of algorithm execution
- **Performance metrics** - Execution time and memory usage tracking
- **Algorithm comparison** - Side-by-side complexity and performance analysis

### Phase 3: AI Integration
- **Automated diagram generation** - AI service to analyze GitHub repositories and generate Structurizr architectural diagrams
- **Pattern recognition** - Identify architectural patterns in codebases
- **Code analysis** - Integration with Repomix for repository processing

### Phase 4: Extended Platform Support
- **Web interface** - Browser-based version with interactive visualizations
- **API endpoints** - RESTful services for algorithm execution
- **Cloud deployment** - Azure hosting for distributed access

## 📊 **Technical Highlights**

This project demonstrates proficiency in:

- **Advanced .NET Architecture** - Modern dependency injection, hosted services, and enterprise patterns
- **Compiler Services** - Runtime C# compilation using Microsoft's Roslyn platform
- **Data-Driven Design** - YAML-based configuration and dynamic behavior
- **Performance Optimization** - Compilation caching and efficient resource management
- **Algorithm Implementation** - From O(n²) fundamentals to sophisticated O(n log n) divide & conquer
- **Software Engineering** - Clean code, SOLID principles, and maintainable architecture

## 🤝 **Contributing**

Contributions are welcome! This project serves as both a personal learning tool and a demonstration of modern .NET architecture patterns.

### Development Setup

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-algorithm`)
3. Follow the existing code style and patterns
4. Add appropriate tests and documentation
5. Submit a pull request

### Code Style

- Follow Microsoft's [C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions)
- Use meaningful variable names and clear documentation
- Maintain the data-driven architecture patterns
- Ensure all new algorithms include comprehensive test cases

## 📄 **License**

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🔗 **Connect**

- **LinkedIn**: [Chase Cupp](https://linkedin.com/in/chasecupp)
- **GitHub**: [@chasecuppdev](https://github.com/chasecuppdev)

---

⭐ **If you find this project interesting or educational, please consider giving it a star!** ⭐
