using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventChallenges
{
    public class Challenge_10
    {
        private List<char> _openingBrackets = new List<char>() { '(', '[', '{', '<' };
        private List<char> _closingBrackets = new List<char>() { ')', ']', '}', '>' };
        public void Challenge_A()
        {
            // Get lines
            string filePath = Path.Combine(@"..\..\Inputs\", "Challenge_10a_input.txt");
            List<string> lines = System.IO.File.ReadAllLines(filePath).ToList<string>();

            // Go through each line
            int score = 0;
            foreach (string line in lines)
            {
                char illegalCharacter = GetIllegalCharacter(line);
                score += GetSyntaxScoreForChar(illegalCharacter);
            }

            // Output
            Console.WriteLine("\nTotal score: " + score);
            Console.ReadLine();
        }

        public void Challenge_B()
        {
            // Get lines
            string filePath = Path.Combine(@"..\..\Inputs\", "Challenge_10a_input.txt");
            List<string> lines = System.IO.File.ReadAllLines(filePath).ToList<string>();

            // Go through each line, ignore syntax errors from Challenge_A
            List<double> scores = new List<double>();
            foreach (string line in lines)
            {
                // Line is incomplete, we need to use autocomplete
                scores.Add(AutoComplete(line));
            }

            // Delete all zero scores
            for(int i = scores.Count -1; i >= 0; i--)
            {
                if (scores[i] == 0)
                    scores.RemoveAt(i);
            }

            // Sort scores
            scores.Sort();
            for (int i = 0; i < scores.Count; i++)
            {
                if (i < scores.Count / 2)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else if (i == scores.Count / 2)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }

                Console.WriteLine($"{i}\t{scores[i]}");
            }

            // Get winning score from middle (assume alway an odd number of scores)
            // Note: dividing an odd integer by 2 should round down (9/2 = 4.5 = 4)
            // Which should accound for 0-index (index 4 = 5th element = middle of 9 elements)
            double score = scores[scores.Count / 2];

            // Output
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nTotal score: " + score);
            Console.ReadLine();
        }

        public char GetIllegalCharacter(string line)
        {
            List<char> pendingBrackets = new List<char>();

            foreach(char c in line)
            {
                if(_openingBrackets.Contains(c))
                {
                    // Opening bracket, add to list
                    pendingBrackets.Add(c);
                }
                else if (_closingBrackets.Contains(c))
                {
                    // Closing bracket, check if illegal
                    if(pendingBrackets.Last<char>() == GetMatchingBracket(c))
                    {
                        // This bracket closes the most recent chunk, consider chunk resolved
                        pendingBrackets.RemoveAt(pendingBrackets.Count - 1);
                    }
                    else
                    {
                        // This bracket does not close the most recent chunk, it's illegal
                        return c;
                    }
                }
            }

            return 'x';
        }

        public int GetSyntaxScoreForChar(char illegalCharacter)
        {
            switch(illegalCharacter)
            {
                case ')':
                    return 3;
                case ']':
                    return 57;
                case '}':
                    return 1197;
                case '>':
                    return 25137;
                default:
                    break;
            }

            return 0;
        }

        public int GetAutocompleteScoreForChar(char illegalCharacter)
        {
            switch (illegalCharacter)
            {
                case ')':
                    return 1;
                case ']':
                    return 2;
                case '}':
                    return 3;
                case '>':
                    return 4;
                default:
                    break;
            }

            return 0;
        }

        public char GetMatchingBracket(char bracket)
        {
            switch (bracket)
            {
                case ')':
                    return '(';
                case '(':
                    return ')';
                case ']':
                    return '[';
                case '[':
                    return ']';
                case '}':
                    return '{';
                case '{':
                    return '}';
                case '>':
                    return '<';
                case '<':
                    return '>';
                default:
                    break;
            }

            return 'x';
        }

        public double AutoComplete(string line)
        {
            List<char> pendingBrackets = new List<char>();

            foreach (char c in line)
            {
                if (_openingBrackets.Contains(c))
                {
                    // Opening bracket, add to list
                    pendingBrackets.Add(c);                
                }
                else if (_closingBrackets.Contains(c))
                {
                    // Closing bracket, check if illegal
                    if (pendingBrackets.Last<char>() == GetMatchingBracket(c))
                    {
                        // This bracket closes the most recent chunk, consider chunk resolved
                        pendingBrackets.RemoveAt(pendingBrackets.Count - 1);                        
                    }
                    else
                    {
                        // This bracket does not close the most recent chunk, its syntax is illegal, no score
                        return 0;
                    }
                }
            }

            // Complete the line
            double score = 0;
            for (int i = pendingBrackets.Count-1; i >= 0; i--)
            {
                char nextClosingBracket = GetMatchingBracket(pendingBrackets[i]);
                int change = GetAutocompleteScoreForChar(nextClosingBracket);
                score = score * 5 + change;
            }

            return score;
        }
    }
}
