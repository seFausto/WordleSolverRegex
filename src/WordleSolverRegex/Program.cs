using System.Reflection;
using System.Text.RegularExpressions;
using WordleSolverRegex.Strategies;

namespace WordleSolverRegex
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("(W)ordle or (N)erdle?");
            var strategyToUse = Console.ReadLine();

            IBlankdleStrategy strategy;
             
            strategy = strategyToUse?.ToUpper() switch
            {
                "W" => new WordleStrategy(),
                "N" => new NerdleStrategy(),
                _ => throw new ArgumentException("Strategy Not Defined"),
            };

            Console.WriteLine(strategy.InitialPrompt());

            int loopCount = 0;

            do
            {
                loopCount++;
                string? input;
                do
                {
                    Console.WriteLine(strategy.InputPrompt());
                    input = Console.ReadLine();

                } while (!strategy.IsValidInput(input));

                if (input == null)
                    throw new NullReferenceException(nameof(input));

                if (strategy.IsWinningInput(input))
                {
                    Console.WriteLine("Contratulations!");
                    break;
                }

                Console.WriteLine(strategy.GetNextSuggestion(input));

            } while (loopCount < strategy.MaxNumberOfAttemps());

            Console.WriteLine("Hope you got the answer!");
            Console.ReadLine();
        }
    }
}

