using Microsoft.Extensions.Logging;

namespace AlgoMonsterComplete.Core.Interfaces;

public interface IExerciseRunner
{
    string Name { get; }
    string Description { get; }
    string Pattern { get; }
    Task RunExerciseAsync();
}