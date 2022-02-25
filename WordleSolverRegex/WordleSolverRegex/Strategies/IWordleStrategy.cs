namespace WordleSolverRegex.Strategies
{
    public interface IWordleStrategy
    {
        string GetNextSuggestion(string input);
        string InitialPrompt();
        string InputPrompt();
        bool IsValidInput(string? input);
        bool IsWinningInput(string input);
        int MaxNumberOfAttemps();
    }
}