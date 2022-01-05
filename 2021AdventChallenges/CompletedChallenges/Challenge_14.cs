using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventChallenges
{
    public class Challenge_14
    {
        private string _polymer = "";
        private Dictionary<string, char> _rules = new Dictionary<string, char>();

        private Dictionary<string, string> _results = new Dictionary<string, string>();
        private Dictionary<string, Dictionary<char,decimal>> _resultCounts = new Dictionary<string, Dictionary<char, decimal>>();

        public void Challenge_A()
        {
            // Get lines
            string filePath = Path.Combine(@"..\..\Inputs\", "Challenge_14a_input.txt");
            List<string> lines = System.IO.File.ReadAllLines(filePath).ToList<string>();

            // Get starting polymer
            _polymer = lines[0].Trim();

            // Get list of rules
            foreach(string line in lines)
            {
                if(line != _polymer && line != "")
                {
                    // Save that rule
                    string key = line.Split(new[] { " -> " }, StringSplitOptions.None)[0];
                    char value = line.Split(new[] { " -> " }, StringSplitOptions.None)[1][0];

                    _rules.Add(key, value);
                }
            }

            // Go through steps
            int target = 10;
            for(int step = 1; step <= target; step++)
            {
                PerformInsertion();
                Console.WriteLine("Step " + step + ": " + _polymer + "\n");
            }

            // Output
            Console.WriteLine();
            Console.WriteLine("Score: " + ScorePolymer());
            Console.ReadLine();
        }

        public void Challenge_B()
        {
            // Get lines
            string filePath = Path.Combine(@"..\..\Inputs\", "Challenge_14a_input.txt");
            List<string> lines = System.IO.File.ReadAllLines(filePath).ToList<string>();

            // Get starting polymer
            _polymer = lines[0].Trim();

            // Get list of rules
            foreach (string line in lines)
            {
                if (line != _polymer && line != "")
                {
                    // Save that rule
                    string key = line.Split(new[] { " -> " }, StringSplitOptions.None)[0];
                    char value = line.Split(new[] { " -> " }, StringSplitOptions.None)[1][0];

                    _rules.Add(key, value);
                }
            }

            // Go through steps
            int target = 40;
            PerformInsertion_Recursive(target, _polymer);

            Dictionary<char, decimal> output = _resultCounts[_polymer+"-"+target];
            
            // Add in first letter, since all other recursions do not count it
            if (!output.Keys.Contains(_polymer[0]))
            {
                output.Add(_polymer[0], 0);
            }
            output[_polymer[0]]++;

            // Output
            Console.WriteLine();
            Console.WriteLine("Score: " + ScoreLargePolymer(output));
            Console.ReadLine();
        }

        public void PerformInsertion()
        {
            string oldPolymer = _polymer;
            _polymer = "";
            _polymer += oldPolymer[0];

            for(int i = 1; i < oldPolymer.Length; i++)
            {
                char insertMe = GetInsertionForPair(oldPolymer.Substring(i-1, 2));

                if (insertMe != '_')
                {
                    // If that character is valid, insert it
                    _polymer += insertMe;
                }

                _polymer += oldPolymer[i];
            }
        }

        public string PerformInsertionOnString(string input)
        {
            string output = input[0].ToString();

            for (int i = 1; i < input.Length; i++)
            {
                char insertMe = GetInsertionForPair(input.Substring(i - 1, 2));

                if (insertMe != '_')
                {
                    // If that character is valid, insert it
                    output += insertMe;
                }

                output += input[i];
            }

            return output;
        }

        public Dictionary<char, decimal> PerformInsertion(string seed)
        {
            Dictionary<char, decimal> resultingCount = new Dictionary<char, decimal>();

            // We don't want to add the left char, just the right + any insertions
            //if (!resultingCount.Keys.Contains(seed[0]))
            //{
            //    resultingCount.Add(seed[0], 0);
            //}
            //resultingCount[seed[0]]++;

            for (int i = 1; i < seed.Length; i++)
            {
                char insertMe = GetInsertionForPair(seed.Substring(i - 1, 2));

                if (insertMe != '_')
                {
                    // If that character is valid, insert it
                    if(!resultingCount.Keys.Contains(insertMe))
                    {
                        resultingCount.Add(insertMe, 0);
                    }
                    resultingCount[insertMe]++;
                }

                if (!resultingCount.Keys.Contains(seed[i]))
                {
                    resultingCount.Add(seed[i], 0);
                }
                resultingCount[seed[i]]++;
            }

            return resultingCount;
        }

        public Dictionary<char, decimal> PerformInsertion_Recursive(int stepsRemaining, string seed)
        {
            // Check if this solution already exists from a previous iterative solution
            if (_resultCounts.ContainsKey(seed + "-" + stepsRemaining))
                return _resultCounts[seed + "-" + stepsRemaining];

            // If solution doesn't already exist, begin recursion as needed
            string bustedString = (stepsRemaining > 1) ? PerformInsertionOnString(seed) : seed;

            // Create a dictionary of characters to keep track of their counts
            Dictionary<char, decimal> runningCount = new Dictionary<char, decimal>();            

            // After recursively getting the count for lower-level recursions (fewer stepsRemaining),
            // Perform insertions on this level.
            for(int i = 1; i < bustedString.Length; i++)
            {
                // Check if this solution already exists from a previous iterative solution
                if (_resultCounts.ContainsKey(bustedString.Substring(i-1, 2) + "-" + (stepsRemaining-1)))
                {
                    runningCount = AddPolymerCounts(runningCount, _resultCounts[bustedString.Substring(i - 1, 2) + "-" + (stepsRemaining - 1)]);
                }
                else if(stepsRemaining==1)
                {
                    runningCount = AddPolymerCounts(runningCount, PerformInsertion(bustedString.Substring(i - 1, 2)));
                }
                else
                {
                    runningCount = AddPolymerCounts(runningCount, PerformInsertion_Recursive(stepsRemaining-1, bustedString.Substring(i-1,2)));
                }
            }

            // Save solution and return it
            _resultCounts.Add(seed + "-" + stepsRemaining, runningCount);

            Console.Write("Solved " + seed + "-" + stepsRemaining+"\t");
            foreach (char c in runningCount.Keys)
            {
                Console.Write(c + "=" + runningCount[c] + " ");
            }
            Console.WriteLine();

            return runningCount;
        }

        public char GetInsertionForPair(string pair)
        {
            if(_rules.ContainsKey(pair))
            {
                return _rules[pair];
            }

            return '_';
        }

        public int ScorePolymer()
        {
            Dictionary<char, int> count = new Dictionary<char, int>();

            foreach (char c in _polymer)
            {
                if (!count.ContainsKey(c))
                    count.Add(c, 0);

                count[c] = count[c] + 1;
            }

            foreach(char c in count.Keys)
            {
                if(count[c] == count.Values.Max())
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else if(count[c] == count.Values.Min())
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.WriteLine(c + ": " + count[c]);
            }
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.White;

            return count.Values.Max() - count.Values.Min();
        }

        public Dictionary<char, decimal> AddPolymerCounts(Dictionary<char, decimal> countOne, Dictionary<char, decimal> countTwo)
        {
            Dictionary<char, decimal> count = new Dictionary<char, decimal>();

            foreach (char c in countOne.Keys)
            {
                if (!count.ContainsKey(c))
                    count.Add(c, 0);

                count[c] += countOne[c];
            }

            foreach (char c in countTwo.Keys)
            {
                if (!count.ContainsKey(c))
                    count.Add(c, 0);

                count[c] += countTwo[c];
            }

            return count;
        }

        public decimal ScoreLargePolymer(Dictionary<char, decimal> count)
        {
            foreach (char c in count.Keys)
            {
                if (count[c] == count.Values.Max())
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else if (count[c] == count.Values.Min())
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.WriteLine(c + ": " + count[c]);
            }
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.White;

            return count.Values.Max() - count.Values.Min();
        }
    }
}
