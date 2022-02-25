using System.Reflection;
using System.Text.RegularExpressions;
using WordleSolverRegex.Strategies;

namespace WordleSolverRegex
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IBlankdleStrategy strategy = new NerdleStrategy();


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

