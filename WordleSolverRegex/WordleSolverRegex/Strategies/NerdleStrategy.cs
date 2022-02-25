using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WordleSolverRegex.Strategies
{
    internal class NerdleStrategy : IBlankdleStrategy
    {
        private readonly Regex InputValidationRegex = new("^[012]{8}$");
        private const int MaxNumberOfAttempts = 6;
        private const int MaxNumberOfCharacters = 8;
        private const string ValidCharacters = "1234567890+-=*\\]";
        private Dictionary<int, List<char>> PatternList;
        private string Suggestion = "9 + 8 - 10 = 7";
        private string WinningInput = "22222222";
        private string MustHaveValues = string.Empty;

        public NerdleStrategy()
        {
            InitializeDictionary();
        }

        private void InitializeDictionary()
        {
            PatternList = new Dictionary<int, List<char>>();

            for (int entryCount = 0; entryCount < MaxNumberOfCharacters; entryCount++)
            {
                var characterList = new List<char>();
                foreach (var item in ValidCharacters)
                {
                    characterList.Add(item);
                }

                PatternList.Add(entryCount, characterList);
            }
        }

        public string GetNextSuggestion(string input)
        {
            //process input


            var suggestions = GenerateAnswers();
            return "12 + 10 = 22";
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

        private bool IsPattern(string v)
        {
            throw new NotImplementedException();
        }

        public string InitialPrompt()
        {
            return $"Start with {Suggestion}";
        }

        public string InputPrompt()
        {
            return "Only valid values: \n0 for Black (Not Used)\n1 for Pink (used but wrong spot)\n2 " +
                "for Green (correct spot)\n(e.g. 00112) for each value";
        }

        private List<string> GenerateAnswers()
        {
            var answer = new List<string>();

            var equations = GenerateEquations();
            // validate equation

            return equations;
        }

        private List<string> GenerateEquations()
        {
            IEnumerable<string> equations = new List<string>();


            equations = from a in PatternList[0]
                        from b in PatternList[1]
                        from c in PatternList[2]
                        from d in PatternList[3]
                        from e in PatternList[4]
                        from f in PatternList[5]
                        from g in PatternList[6]
                        from h in PatternList[7]
                        select $"{a} {b} {c} {d} {e} {f} {g} {h}";
            return equations.ToList();
        }


        public bool IsValidInput(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            return InputValidationRegex.IsMatch(input);
        }

        public bool IsWinningInput(string input)
        {
            return input == WinningInput;
        }

        public int MaxNumberOfAttemps()
        {
            return MaxNumberOfAttempts;
        }
    }
}