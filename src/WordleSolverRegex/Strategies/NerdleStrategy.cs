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
        private const string Operands = "1234567890";
        private const string Operators = @"+-=*/";
        private Dictionary<int, List<char>> PatternList = new();
        private string Suggestion = "9+8-10=7";
        private string MustHaveValues = string.Empty;
        private int AttemptCount = 1;
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
                characterList.AddRange(Operands);

                if (entryCount != 0 && entryCount != MaxNumberOfCharacters - 1)
                    characterList.AddRange(Operators);

                PatternList.Add(entryCount, characterList);
            }
        }

        public string GetNextSuggestion(string input)
        {

            AttemptCount++;
            ProcessInput(input);

            if (AttemptCount < 3)
            {
                Suggestion = "12+10=22";
                return Suggestion;
            }
            var suggestions = GenerateAnswers();

            Suggestion = GetRandomEquation(suggestions);

            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine("----------");
            stringBuilder.AppendLine($"Listing matches: Count {suggestions.Count}");
            stringBuilder.AppendLine($"Must Include values: {MustHaveValues}");
            stringBuilder.AppendLine($"Try: {Suggestion} ");

            return stringBuilder.ToString();
        }

        private string GetRandomEquation(List<string> suggestions)
        {
            if (suggestions.Count == 0)
                return "No Suggetsions";

            Random random = new();
            return suggestions[random.Next(suggestions.Count)];
        }

        private void ProcessInput(string input)
        {
            for (int inputIndex = 0; inputIndex < input.Length; inputIndex++)
            {
                char currentCharacter = Suggestion[inputIndex];

                switch (input[inputIndex])
                {
                    case '0':
                        //dont' remove if letter is in must have
                        if (MustHaveValues.Contains(currentCharacter))
                            continue;

                        //remove from every other list
                        for (int patternCount = 0; patternCount < PatternList.Count; patternCount++)
                        {
                            if (PatternList[patternCount].Contains(currentCharacter)
                                && PatternList[patternCount].Count > 1)
                            {
                                PatternList[patternCount].Remove(currentCharacter);
                            }
                        }
                        break;

                    case '1':
                        if (!MustHaveValues.Contains(currentCharacter))
                        {
                            MustHaveValues += currentCharacter;
                        }

                        PatternList[inputIndex].Remove(currentCharacter);
                        break;

                    case '2':
                        if (!MustHaveValues.Contains(currentCharacter))
                        {
                            MustHaveValues += currentCharacter;
                        }

                        PatternList[inputIndex].Clear();
                        PatternList[inputIndex].Add(currentCharacter);
                        break;
                    default:
                        throw new ArgumentException("Invalid input");
                }
            }
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

            answer.AddRange(equations.Where(equation => IsValidEquation(equation)));

            return answer;
        }

        private bool IsValidEquation(string equation)
        {
            if (equation.Count(x => x == '=') != 1)
                return false;

            // if (equation contains all values from must have
            if (!MustHaveValues.All(ch => equation.Contains(ch)))
            {
                return false;
            }

            var formulas = equation.Replace(" ", string.Empty).Split('=');
            Regex regex = new Regex(@"^\d+([\+\*\-\/]\d+)*$");
            foreach (var formula in formulas)
            {
                if (!regex.IsMatch(formula))
                    return false;
            }


            try
            {
                StringToFormula stf = new StringToFormula();
                double result = stf.Eval(formulas[0]);
                double result2 = stf.Eval(formulas[1]);

                return result == result2;
            }
            catch (Exception)
            {
                return false;
            }

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
                        select $"{a}{b}{c}{d}{e}{f}{g}{h}";
            return equations.ToList();
        }

        public bool IsValidInput(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            return InputValidationRegex.IsMatch(input);
        }

        public int MaxNumberOfAttemps()
        {
            return MaxNumberOfAttempts;
        }
    }

    public class StringToFormula
    {
        private readonly string[] _operators = { "-", "+", "/", "*", "^" };
        private readonly Func<double, double, double>[] _operations = {
        (a1, a2) => a1 - a2,
        (a1, a2) => a1 + a2,
        (a1, a2) => a1 / a2,
        (a1, a2) => a1 * a2,
        (a1, a2) => Math.Pow(a1, a2)
    };

        public double Eval(string expression)
        {
            List<string> tokens = getTokens(expression);
            Stack<double> operandStack = new Stack<double>();
            Stack<string> operatorStack = new Stack<string>();
            int tokenIndex = 0;

            while (tokenIndex < tokens.Count)
            {
                string token = tokens[tokenIndex];

                //If this is an operator  
                if (Array.IndexOf(_operators, token) >= 0)
                {
                    while (operatorStack.Count > 0 
                        && Array.IndexOf(_operators, token) < Array.IndexOf(_operators, operatorStack.Peek()))
                    {
                        string op = operatorStack.Pop();
                        double arg2 = operandStack.Pop();
                        double arg1 = operandStack.Pop();
                        operandStack.Push(_operations[Array.IndexOf(_operators, op)](arg1, arg2));
                    }
                    operatorStack.Push(token);
                }
                else
                {
                    operandStack.Push(double.Parse(token));
                }
                tokenIndex += 1;
            }

            while (operatorStack.Count > 0)
            {
                string op = operatorStack.Pop();
                double arg2 = operandStack.Pop();
                double arg1 = operandStack.Pop();
                operandStack.Push(_operations[Array.IndexOf(_operators, op)](arg1, arg2));
            }
            return operandStack.Pop();
        }

        private string getSubExpression(List<string> tokens, ref int index)
        {
            StringBuilder subExpr = new StringBuilder();
            int parenlevels = 1;
            index += 1;
            while (index < tokens.Count && parenlevels > 0)
            {
                string token = tokens[index];
                if (tokens[index] == "(")
                {
                    parenlevels += 1;
                }

                if (tokens[index] == ")")
                {
                    parenlevels -= 1;
                }

                if (parenlevels > 0)
                {
                    subExpr.Append(token);
                }

                index += 1;
            }

            if ((parenlevels > 0))
            {
                throw new ArgumentException("Mis-matched parentheses in expression");
            }
            return subExpr.ToString();
        }

        private List<string> getTokens(string expression)
        {
            string operators = "()^*/+-";
            List<string> tokens = new List<string>();
            StringBuilder sb = new StringBuilder();

            foreach (char c in expression.Replace(" ", string.Empty))
            {
                if (operators.IndexOf(c) >= 0)
                {
                    if ((sb.Length > 0))
                    {
                        tokens.Add(sb.ToString());
                        sb.Length = 0;
                    }
                    tokens.Add(c.ToString());
                }
                else
                {
                    sb.Append(c);
                }
            }

            if ((sb.Length > 0))
            {
                tokens.Add(sb.ToString());
            }
            return tokens;
        }
    }
}