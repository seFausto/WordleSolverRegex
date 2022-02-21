
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
            do
            {
                Console.WriteLine("Type 0 for Gray, 1 for Yellow, 2 for Green (e.g. 00112)");
                var input = Console.ReadLine();

                //process results

                Regex regex = new Regex(String.Join("", letterPattern), RegexOptions.IgnoreCase);

                //var matches = regex.Matches(wordList.ToString());
                //var possibleAnswers = new List<string>();

                //Console.WriteLine($"Listing matches: Count {matches.Count}");
                //Console.WriteLine($"Must Include: {letters}");
                //foreach (Match match in matches)
                //{
                //    var m = match.Value;

                //    if (letters.ToUpper().All(value => m.ToUpper().Contains(value)))
                //        possibleAnswers.Add(m);
                //}

                //var random = new Random();
                //var nextIndex = random.Next(possibleAnswers.Count);

                //Console.WriteLine($"Try word: {possibleAnswers[nextIndex]} ");

                //Console.ReadLine();

            } while (true);





         
        }
    }
}

