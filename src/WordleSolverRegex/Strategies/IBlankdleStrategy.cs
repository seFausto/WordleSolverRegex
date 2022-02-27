namespace WordleSolverRegex.Strategies
{
    public interface IBlankdleStrategy
    {
        string GetNextSuggestion(string input);
        string InitialPrompt();
        string InputPrompt();
        bool IsValidInput(string? input);
        bool IsWinningInput(string input)
        {
            return input.Distinct().Count() == 1
                    && input.Contains("2");
        }
        int MaxNumberOfAttemps();
    }
}