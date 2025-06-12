namespace AlgoMonsterComplete.Core.Interfaces;

public interface IChallengeRunner
{
    Task RunAsync();
    Task RunChallengeAsync(string patternKey, string challengeName);
    Task RunExampleAsync(string patternKey, string exampleName);
    Task ListPatternsAsync();
    Task ListChallengesAsync(string patternKey);
    Task ListExamplesAsync(string patternKey);
}