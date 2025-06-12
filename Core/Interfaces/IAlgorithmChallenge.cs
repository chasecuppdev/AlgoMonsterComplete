namespace AlgoMonsterComplete.Core.Interfaces;

public interface IAlgorithmChallenge
{
    string Name { get; }
    string Description { get; }
    ChallengeDifficulty Difficulty { get; }
    Task ExecuteAsync();
}

public interface IAlgorithmExample
{
    string Name { get; }
    string Description { get; }
    string Pattern { get; }
    Task ExecuteAsync();
}

public enum ChallengeDifficulty
{
    Easy,
    Medium,
    Hard
}