using System.Reflection;
using System.Text.RegularExpressions;

namespace WordleSolverRegex
{
    internal class Program
    {
        private const string WinningInput = "22222";
        private const string StartingPattern = "[ABCDEFGHIJKLMNOPQRSTUVWXYZ]";
        private const string WordListEmbeddedResourcename = "WordleSolverRegex.WordList.txt";
        private static Regex inputRegex = new Regex("^[012]{5}$");
        static void Main(string[] args)
        {
            string wordList = ReadWordList();

            List<string> letterPattern = Enumerable.Repeat(StartingPattern, 5).ToList();

            string mustHaveValues = string.Empty;

            string suggestion = "ADIEU";
            Console.WriteLine($"Start with {suggestion}");
            

            int loopCount = 0;

            do
            {
                loopCount++;
                string? input;
                do
                {
                    Console.WriteLine("(Only valid values: 0 for Gray, 1 for Yellow, 2 for Green (e.g. 00112) for each Letter)");
                    input = Console.ReadLine();

                } while (!inputRegex.IsMatch(input ?? string.Empty));

                if (input == null)
                    throw new NullReferenceException(nameof(input));

                if (input == WinningInput)
                {
                    Console.WriteLine("Contratulations!");
                    break;
                }

                letterPattern = GeneratePatternsFromInput(letterPattern, input, suggestion, ref mustHaveValues);

                List<string> possibleAnswers = GetPossibleAnswers(wordList, letterPattern, mustHaveValues);
                suggestion = GetRandomWord(possibleAnswers);

                Console.WriteLine("----------");
                Console.WriteLine($"Listing matches: Count {possibleAnswers.Count}");
                Console.WriteLine($"Must Include: {mustHaveValues}");
                Console.WriteLine($"Try word: {suggestion} ");

            } while (loopCount < 5);

            Console.WriteLine("Hope you got the answer!");
            Console.ReadLine();
        }

        private static string GetRandomWord(List<string> possibleAnswers)
        {
            var random = new Random();
            var nextIndex = random.Next(possibleAnswers.Count);

            return possibleAnswers[nextIndex].ToUpper();
        }

        private static List<string> GetPossibleAnswers(string wordList, List<string> letterPattern,
            string mustHaveLetters)
        {
            List<string> possibleAnswers = new();

            Regex regex = new(String.Join("", letterPattern), RegexOptions.IgnoreCase);

            foreach (Match match in regex.Matches(wordList.ToString()))
            {
                var value = match.Value;

                if (mustHaveLetters.ToUpper().All(x => value.ToUpper().Contains(x)))
                    possibleAnswers.Add(value);
            }

            return possibleAnswers;
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

        private static List<string> GeneratePatternsFromInput(List<string> letterPattern, string input,
            string word, ref string mustHaveLetters)
        {
            for (int inputIndex = 0; inputIndex < input.Length; inputIndex++)
            {
                char currentLetter = word[inputIndex];
         
                switch (input[inputIndex])
                {
                    case '0':
                        //dont' remove if letter is in must have
                        if (mustHaveLetters.Contains(currentLetter))
                            continue;

                        //remove from every other list
                        for (int patternCount = 0; patternCount < letterPattern.Count; patternCount++)
                        {
                            if (IsPattern(letterPattern, patternCount)
                                && letterPattern[patternCount].Contains(currentLetter))
                            {
                                letterPattern[patternCount] = letterPattern[patternCount]
                                    .Remove(letterPattern[patternCount].IndexOf(currentLetter), 1);
                            }
                        }
                        break;

                    case '1':
                        if (!mustHaveLetters.Contains(currentLetter))
                        {
                            mustHaveLetters += currentLetter;
                        }

                        letterPattern[inputIndex] = letterPattern[inputIndex]
                            .Remove(letterPattern[inputIndex].IndexOf(currentLetter), 1);
                        break;

                    case '2':
                        if (!mustHaveLetters.Contains(currentLetter))
                        {
                            mustHaveLetters += currentLetter;
                        }

                        letterPattern[inputIndex] = currentLetter.ToString();
                        break;
                    default:
                        throw new ArgumentException("Invalid input");
                }
            }
            return letterPattern;
        }

        private static bool IsPattern(List<string> letterPattern, int patternCount)
        {
            return letterPattern[patternCount].Contains('[');
        }
    }
}

