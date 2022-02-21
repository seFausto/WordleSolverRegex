
using System.Text;
using System.Text.RegularExpressions;

namespace WordleSolverRegex
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var pathToWordListFile = @"C:\github\WordleSolverRegex\WordleSolverRegex\WordleSolverRegex\WordList.txt";
            var wordList = new StringBuilder();
            foreach (var item in File.ReadAllLines(pathToWordListFile))
            {
                wordList.AppendLine(item);
            }

            var letterPattern = new List<string>()
            {
                "[ABCDEFGHIJKLMNOPQRSTUVWXYZ]",
                "[ABCDEFGHIJKLMNOPQRSTUVWXYZ]",
                "[ABCDEFGHIJKLMNOPQRSTUVWXYZ]",
                "[ABCDEFGHIJKLMNOPQRSTUVWXYZ]",
                "[ABCDEFGHIJKLMNOPQRSTUVWXYZ]"
            };

            string letters = "";
            Console.WriteLine("start with 'CRANE'");
            string word = "CRANE";
            do
            {
                Console.WriteLine("Type 0 for Gray, 1 for Yellow, 2 for Green (e.g. 00112)");
                var input = Console.ReadLine();

                //process input
                letterPattern = ProcessInput(letterPattern, input, word, ref letters);

                Regex regex = new Regex(String.Join("", letterPattern), RegexOptions.IgnoreCase);

                var matches = regex.Matches(wordList.ToString());
                var possibleAnswers = new List<string>();

                Console.WriteLine($"Listing matches: Count {matches.Count}");
                Console.WriteLine($"Must Include: {letters}");
                foreach (Match match in matches)
                {
                    var m = match.Value;

                    if (letters.ToUpper().All(value => m.ToUpper().Contains(value)))
                        possibleAnswers.Add(m);
                }

                var random = new Random();
                var nextIndex = random.Next(possibleAnswers.Count);
                word = possibleAnswers[nextIndex].ToUpper();
                Console.WriteLine($"Try word: {word} ");

                Console.ReadLine();

            } while (true);
        }

        private static List<string> ProcessInput(List<string> letterPattern, string input,
            string word, ref string letters)
        {

            for (int charCount = 0; charCount < input.Length; charCount++)
            {
                char currentChar = word[charCount];

                if (input[charCount] == '0')
                {
                    //remove from all lists
                    //remove from every other list
                    for (int patternCount = 0; patternCount < letterPattern.Count; patternCount++)
                    {
                        if (IsPattern(letterPattern, patternCount) && letterPattern[patternCount].Contains(currentChar))
                            letterPattern[patternCount] = letterPattern[patternCount].Remove(letterPattern[patternCount].IndexOf(currentChar), 1);
                    }
                }
                else if (input[charCount] == '1')
                {
                    if (!letters.Contains(currentChar))
                    {
                        letters += currentChar;
                    }

                    //remove from every other list
                    for (int patternCount = 0; patternCount < letterPattern.Count; patternCount++)
                    {
                        if (patternCount != charCount)
                            continue;

                        letterPattern[patternCount] = letterPattern[patternCount].Remove(letterPattern[patternCount].IndexOf(currentChar), 1);
                    }
                }
                else if (input[charCount] == '2')
                {
                    if (!letters.Contains(currentChar))
                    {
                        letters += currentChar;
                    }

                    letterPattern[charCount] = currentChar.ToString();
                }
            }
            return letterPattern;
        }

        private static bool IsPattern(List<string> letterPattern, int patternCount)
        {
            return letterPattern[patternCount].Contains("[");
        }
    }
}

