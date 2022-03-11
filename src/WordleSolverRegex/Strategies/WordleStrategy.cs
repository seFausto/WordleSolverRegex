using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WordleSolverRegex.Strategies
{
    public class WordleStrategy : IBlankdleStrategy
    {
        private readonly Regex InputValidationRegex = new("^[012]{5}$");
        private const int MaxNumberOfAttempts = 5;
        private const string WordListEmbeddedResourcename = "WordleSolverRegex.WordList.txt";
        private const string StartingPattern = "[ABCDEFGHIJKLMNOPQRSTUVWXYZ]";
        private string Suggestion;
        private string WordList;

        private List<string> LetterPattern = Enumerable.Repeat(StartingPattern, 5).ToList();
        private string MustHaveValues = string.Empty;

        public WordleStrategy()
        {
            Suggestion = "AROSE";
            WordList = ReadWordList();
        }

        public string GetNextSuggestion()
        { 
            List<string> possibleAnswers = GetPossibleAnswers();
            Suggestion = GetRandomWord(possibleAnswers);

            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine("----------");
            stringBuilder.AppendLine($"Listing matches: Count {possibleAnswers.Count}");
            stringBuilder.AppendLine($"Must Include: {MustHaveValues}");
            stringBuilder.AppendLine($"Try word: {Suggestion} ");

            return stringBuilder.ToString();
        }

        public void CalculateSuggestions(string input)
        {
            LetterPattern = GeneratePatternsFromInput(LetterPattern, input);
        }

        public int MaxNumberOfAttemps()
        {
            return MaxNumberOfAttempts;
        }

        public string InitialPrompt()
        {
            return $"Start with {Suggestion}";
        }

        public string InputPrompt()
        {
            return "(Only valid values: 0 for Gray, 1 for Yellow, 2 for Green (e.g. 00112) for each Letter)";
        }

        public bool IsValidInput(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            return InputValidationRegex.IsMatch(input);
        }

        private static string ReadWordList()
        {
            var assembly = Assembly.GetExecutingAssembly();

            if (assembly == null)
                throw new NullReferenceException(nameof(assembly));

            var resourceName = WordListEmbeddedResourcename;

            using Stream? stream = assembly.GetManifestResourceStream(resourceName);

            if (stream == null)
                throw new NullReferenceException($"{resourceName}");

            using StreamReader reader = new(stream);

            return reader.ReadToEnd();
        }

        private string GetRandomWord(List<string> possibleAnswers)
        {
            var random = new Random();
            var nextIndex = random.Next(possibleAnswers.Count);

            return possibleAnswers[nextIndex].ToUpper();
        }

        private List<string> GetPossibleAnswers()
        {
            List<string> possibleAnswers = new();

            Regex regex = new(String.Join("", LetterPattern), RegexOptions.IgnoreCase);

            foreach (Match match in regex.Matches(WordList.ToString()))
            {
                var value = match.Value;

                if (MustHaveValues.ToUpper().All(x => value.ToUpper().Contains(x)))
                    possibleAnswers.Add(value);
            }

            return possibleAnswers;
        }

        private List<string> GeneratePatternsFromInput(List<string> letterPattern, string input)
        {
            for (int inputIndex = 0; inputIndex < input.Length; inputIndex++)
            {
                char currentLetter = Suggestion[inputIndex];

                switch (input[inputIndex])
                {
                    case '0':
                        //dont' remove if letter is in must have
                        if (MustHaveValues.Contains(currentLetter))
                            continue;

                        //remove from every other list
                        for (int patternCount = 0; patternCount < letterPattern.Count; patternCount++)
                        {
                            if (IsPattern(letterPattern[patternCount])
                                && letterPattern[patternCount].Contains(currentLetter))
                            {
                                letterPattern[patternCount] = letterPattern[patternCount]
                                    .Remove(letterPattern[patternCount].IndexOf(currentLetter), 1);
                            }
                        }
                        break;

                    case '1':
                        if (!MustHaveValues.Contains(currentLetter))
                        {
                            MustHaveValues += currentLetter;
                        }

                        letterPattern[inputIndex] = letterPattern[inputIndex]
                            .Remove(letterPattern[inputIndex].IndexOf(currentLetter), 1);
                        break;

                    case '2':
                        if (!MustHaveValues.Contains(currentLetter))
                        {
                            MustHaveValues += currentLetter;
                        }

                        letterPattern[inputIndex] = currentLetter.ToString();
                        break;
                    default:
                        throw new ArgumentException("Invalid input");
                }
            }
            return letterPattern;
        }

        private bool IsPattern(string letterPattern)
        {
            return letterPattern.Contains('[');
        }
    }
}
