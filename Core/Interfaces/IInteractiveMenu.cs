namespace AlgoMonsterComplete.Core.Interfaces;

public interface IInteractiveMenu
{
    Task RunAsync();
    Task RunExerciseAsync(string patternKey, string excerciseName);
    Task ListPatternsAsync();
    Task ListExercisesAsync(string patternKey);
}