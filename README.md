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

### 🧪 **Built-in Testing Framework**
- **Automated test execution** - Run implementations against YAML-defined test cases
- **Comprehensive reporting** - Pass/fail metrics with detailed output
- **Input/output validation** - Ensure algorithms meet expected behavior

### 🎨 **Modern .NET 8 Patterns**
- **Dependency injection** - Clean separation of concerns with Microsoft.Extensions.DI
- **Hosted services** - Proper application lifecycle management
- **Keyed services** - Pattern-based algorithm organization
- **Structured logging** - Comprehensive logging with Microsoft.Extensions.Logging

### 🚀 **Performance & Scalability**
- **Compilation caching** - Avoid redundant algorithm compilation
- **Concurrent operations** - Thread-safe compilation service
- **Memory-efficient** - Optimized for resource usage

## 🏛️ **Architecture Overview**

```
┌─────────────────────────────────────────────────────────────┐
│                    AlgoMonsterComplete                     │
├─────────────────────────────────────────────────────────────┤
│  Interactive Console Interface (InteractiveMenu)           │
├─────────────────────────────────────────────────────────────┤
│  Exercise Execution Engine (ExerciseRunner)                │
├─────────────────────────────────────────────────────────────┤
│  Dynamic Compilation Service (AlgorithmCompilationService) │
├─────────────────────────────────────────────────────────────┤
│  YAML Data Layer (Exercise Definitions)                    │
└─────────────────────────────────────────────────────────────┘
```

### Core Components

- **`InteractiveMenu`** - User interface and navigation system
- **`ExerciseRunner`** - Algorithm execution and testing orchestrator  
- **`AlgorithmCompilationService`** - Roslyn-based dynamic C# compilation
- **`Exercise Models`** - Strong-typed data structures for YAML deserialization
- **`YAML Exercise Files`** - Algorithm definitions with complexity analysis and test cases

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
- `quit` or `q` - Exit the application

### Example Session

```
🚀 Welcome to AlgoMonster Complete with Modern .NET DI!
=========================================================

> ex fundamental-sorting
🎓 exercises in Fundamental Sorting:
  1. 📖 Insertion Sort - Sort array by building sorted portion one element at a time

Select exercise to run (1-1) or press Enter to return: 1

📊 Complexity Analysis:
  ⏰ Time: O(n^2)
  💾 Space: O(1)
  🔄 Stable: Yes
  📍 In-place: Yes

🧪 Unit Tests (Running my implementation against test cases):
Found 4 test case(s)

Test 1/4:
  ✅ PASS
  [All tests pass...]

📊 Test Results: 4/4 passed
🎉 All tests passed!
```

## 📁 **Project Structure**

```
AlgoMonsterComplete/
├── Core/
│   ├── ExerciseRunner.cs              # Algorithm execution orchestrator
│   ├── InteractiveMenu.cs             # User interface and navigation
│   ├── AlgorithmCompilationService.cs # Roslyn compilation service
│   ├── ServiceCollectionExtensions.cs # Dependency injection setup
│   └── Interfaces/                    # Service contracts
├── Models/Common/
│   ├── Exercise.cs                    # Main exercise data model
│   ├── Solution.cs                    # Algorithm implementation model
│   ├── TestCase.cs                    # Test case data model
│   └── ComplexityAnalysis.cs          # Algorithm complexity metadata
├── Data/Constants/
│   └── AlgorithmPatterns.cs           # Pattern mappings and display names
└── Patterns/
    └── FundamentalSorting/
        └── InsertionSort.yml          # Example YAML exercise definition
```

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
    public static List<int> YourMethod(int[] input)
    {
        // Your algorithm implementation here
        return result;
    }

TestCases:
  - Input: "[3, 1, 4, 1, 5]"
    Expected: "[1, 1, 3, 4, 5]"
    Notes: "Basic test case"
```

## 🔮 **Future Roadmap**

### Phase 1: Enhanced Visualization
- **Step-by-step algorithm visualization** - Visual debugging of algorithm execution
- **Performance metrics** - Execution time and memory usage tracking
- **Algorithm comparison** - Side-by-side complexity and performance analysis

### Phase 2: AI Integration
- **Automated diagram generation** - AI service to analyze GitHub repositories and generate Structurizr architectural diagrams
- **Pattern recognition** - Identify architectural patterns in codebases
- **Code analysis** - Integration with Repomix for repository processing

### Phase 3: Extended Platform Support
- **Web interface** - Browser-based version with interactive visualizations
- **API endpoints** - RESTful services for algorithm execution
- **Cloud deployment** - Azure hosting for distributed access

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

## 📊 **Technical Highlights**

This project demonstrates proficiency in:

- **Advanced .NET Architecture** - Modern dependency injection, hosted services, and enterprise patterns
- **Compiler Services** - Runtime C# compilation using Microsoft's Roslyn platform
- **Data-Driven Design** - YAML-based configuration and dynamic behavior
- **Performance Optimization** - Compilation caching and efficient resource management
- **Software Engineering** - Clean code, SOLID principles, and maintainable architecture

## 📄 **License**

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🔗 **Connect**

- **LinkedIn**: [Chase Cupp](https://linkedin.com/in/chasecupp)
- **GitHub**: [@chasecuppdev](https://github.com/chasecuppdev)

---

⭐ **If you find this project interesting or educational, please consider giving it a star!** ⭐
