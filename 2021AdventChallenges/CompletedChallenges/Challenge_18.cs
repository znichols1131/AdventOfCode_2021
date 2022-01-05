using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventChallenges
{
    public class Challenge_18
    {
        private string _currentSnailNumber;

        public void Challenge_A()
        {
            // Get input
            string filePath = Path.Combine(@"..\..\Inputs\", "Challenge_18a_input.txt");
            string[] lines = System.IO.File.ReadAllLines(filePath);

            // Create snail pair inputs
            foreach (string newSnailPair in lines)
            {
                if (_currentSnailNumber is null)
                {
                    _currentSnailNumber = newSnailPair;
                    Console.Write("After start:    ");
                    Console.WriteLine(_currentSnailNumber);
                }
                else
                {
                    _currentSnailNumber = AddSnailNumbers(_currentSnailNumber, newSnailPair);

                    Console.Write("\nAfter addition: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(_currentSnailNumber);
                    Console.ForegroundColor = ConsoleColor.White;

                    _currentSnailNumber = ReduceSnailNumber(_currentSnailNumber);
                }
            }

            // Output
            Console.WriteLine("\n\nAnswer: " + _currentSnailNumber);
            Console.WriteLine("Magnitude: " + GetMagnitude(_currentSnailNumber));
            Console.ReadLine();
        }

        public void Challenge_B()
        {
            //  Answer from 10-minute test session:
            //  First snail number:[[[8,4],[[5,2],[7,0]]],[[[9,7],[8,9]],7]]
            //  Second snail number:[[[8,[9,9]],9],[[3,[2,8]],[[9,5],[2,9]]]]
            //  Result:[[[[8,8],[8,0]],[[8,9],[9,8]]],[[[8,8],[8,8]],[[8,8],[8,8]]]]
            //  Magnitude: 4784

            // Get input
            string filePath = Path.Combine(@"..\..\Inputs\", "Challenge_18a_input.txt");
            string[] lines = System.IO.File.ReadAllLines(filePath);

            // Record highest magnitude
            string firstSnailNumber = "";
            string secondSnailNumber = "";
            string resultingSnailNumber = "";
            int highestMagnitude = 0;

            // For testing purposes
            int testNumber = 1;
            int totalTests = (lines.Count()) * (lines.Count() - 1);

            // Test snail inputs
            for(int i = 0; i < lines.Count(); i++)
            {
                for(int j = 0; j < lines.Count(); j++)
                {
                    // Don't use the same number on itself
                    if (i == j)
                        continue;

                    testNumber++;

                    // Perform addition
                    string result = AddSnailNumbers(lines[i], lines[j]);
                    result = ReduceSnailNumber(result);
                    int magnitude = GetMagnitude(result);

                    // Check if this beats the record
                    if(magnitude > highestMagnitude)
                    {
                        highestMagnitude = magnitude;
                        firstSnailNumber = lines[i];
                        secondSnailNumber = lines[j];
                        resultingSnailNumber = result;
                    }
                }

                Console.WriteLine($"Completed test {testNumber} of {totalTests}");
            }

            // Output
            Console.WriteLine($"First snail number:  " + firstSnailNumber);
            Console.WriteLine($"Second snail number: " + secondSnailNumber);
            Console.WriteLine($"Result:              " + resultingSnailNumber);
            Console.WriteLine("\nMagnitude: " + highestMagnitude);
            Console.ReadLine();
        }

        public string AddSnailNumbers(string firstNum, string secondNum)
        {
            return $"[{firstNum},{secondNum}]";
        }

        public string ReduceSnailNumber(string input)
        {
            bool needsExploded = false;
            bool needsSplit = false;
            do
            {
                // Check if any pairs need exploded
                needsExploded = NeedsExploded(input);                     
                if (needsExploded)
                {
                    // Perform explosion
                    input = PerformExplosion(input);
                    //Console.Write("After explode:  ");
                    //Console.WriteLine(input);

                    // Changes made
                    goto Changes_Made;
                }

                // Check if any pairs need split
                needsSplit = NeedsSplit(input);
                if (needsSplit)
                {
                    // Perform split
                    input = PerformSplit(input);

                    //Console.Write("After split:    ");
                    //Console.WriteLine(input);

                    // Changes made
                    goto Changes_Made;
                }

            // Jump to here if any changes were made
            Changes_Made:;

            } while (needsExploded || needsSplit);

            // Return the reduced snail pair
            return input;
        }

        public int GetMagnitude(string input)
        {
            int commaIndex = FindMiddleCommaIndex(input);

            // Parse left side
            int leftValue = 0;
            string leftInput = input.Substring(1, commaIndex - 1);
            try
            {
                // See if it's a number
                leftValue = int.Parse(leftInput);
            }
            catch
            {
                // Otherwise, it's a nested snail pair
                leftValue = GetMagnitude(leftInput);
            }

            // Parse right side
            int rightValue = 0;
            string rightInput = input.Substring(commaIndex + 1, input.Length - commaIndex - 2);
            try
            {
                // See if it's a number
                rightValue = int.Parse(rightInput);
            }
            catch
            {
                // Otherwise, it's a nested snail pair
                rightValue = GetMagnitude(rightInput);
            }

            return 3 * leftValue + 2 * rightValue;
        }

        public bool NeedsExploded(string input)
        {
            int numberOfBrackets = 0;
            foreach(char c in input.ToLower())
            {
                if (c == '[')
                    numberOfBrackets++;

                if (c == ']')
                    numberOfBrackets--;

                // If a pair is detected with 4 parents (making it the 5th-deep), we need to explode it.
                if (numberOfBrackets >= 5)
                    return true;
            }

            return false;
        }

        public string PerformExplosion(string input)
        {
            int openBrackets = 0;

            int startIndex = -1;
            int commaIndex = -1;
            int endIndex = -1;

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '[')
                {
                    openBrackets++;

                    // Record index if this is the start of the pair to be exploded (4 parents)
                    if (openBrackets == 5)
                        startIndex = i;
                }

                if (input[i] == ']')
                {
                    openBrackets--;

                    // Record index if this is the end of the pair to be exploded (4 parents)
                    if (openBrackets == 4)
                        endIndex = i;
                }

                // Record index of comma in the pair to be exploded
                if (input[i] == ',' && openBrackets == 5)
                    commaIndex = i;

                // If we have all of our indices, jump ahead.
                if (!(startIndex < 0 || commaIndex < 0 || endIndex < 0))
                    goto Indices_Fulfilled;
            }

            // If we weren't able to get all the indices required, return original input
            if (startIndex < 0 || commaIndex < 0 || endIndex < 0)
                return input;

        Indices_Fulfilled:;

            // Otherwise we can proceed with the explosion.
            // Note: assume that there can never be a pair with 5 parents because we will explode immediately per rules.
            // Thus, the pair to explode has two integer values (no children pairs).

            // Find the left value of the exploding pair, throw it to first number to its left
            int leftValue = int.Parse(input.Substring(startIndex + 1, commaIndex - startIndex - 1));
            string leftString = CatchExplodingValue(input.Substring(0, startIndex), leftValue, true);

            // Find the right value of the exploding pair, throw it to the first number to its right
            int rightValue = int.Parse(input.Substring(commaIndex + 1, endIndex - commaIndex - 1));
            string rightString = CatchExplodingValue(input.Substring(endIndex + 1, input.Length - endIndex - 1), rightValue, false);

            // Replace exploded pair with 0
            return leftString + "0" + rightString;
        }

        public string CatchExplodingValue(string input, int value, bool movingLeft)
        {
            int startIndex = -1;
            int endIndex = -1;

            if(movingLeft)
            {
                // We're moving to the left
                for(int i = input.Length - 1; i >= 0; i--)
                {
                    char c = input[i];
                    if (!"[,]".Contains(c))
                    {
                        // Assume number
                        startIndex = i;
                        if (endIndex < 0)
                            endIndex = i;
                    }
                    else if (startIndex >= 0 && endIndex >= 0)
                    {
                        // If we have both our indices, that means we captured the number
                        int exValue = int.Parse(input.Substring(startIndex, endIndex - startIndex + 1));
                        exValue += value;
                        input = input.Remove(startIndex, endIndex - startIndex + 1);
                        input = input.Insert(startIndex, exValue.ToString());
                        return input;
                    }
                }
            }
            else
            {
                // We're moving to the right
                for(int i = 0; i < input.Length; i++)
                {
                    char c = input[i];
                    if (!"[,]".Contains(c))
                    {
                        // Assume number
                        endIndex = i;
                        if (startIndex < 0)
                            startIndex = i;
                    }
                    else if (startIndex >= 0 && endIndex >= 0)
                    {
                        // If we have both our indices, that means we captured the number
                        int exValue = int.Parse(input.Substring(startIndex, endIndex - startIndex + 1));
                        exValue += value;
                        input = input.Remove(startIndex, endIndex - startIndex + 1);
                        input = input.Insert(startIndex, exValue.ToString());
                        return input;
                    }
                }
            }

            // If we weren't able to find a number in this direction, the exploding value is lost.
            // Return the unchanged input string.
            return input;
        }

        public bool NeedsSplit(string input)
        {
            string[] components = input.Split(new char[] { ']', ',', '[' }, StringSplitOptions.RemoveEmptyEntries);
            
            // Check if any snail pair or sub-snail pair has a value greater than 9
            foreach(string s in components)
            {
                if (int.Parse(s) >= 10)
                    return true;
            }

            // No values required splitting
            return false;
        }

        public string PerformSplit(string input)
        {
            int startIndex = -1;
            int endIndex = -1;

            for(int i = 0; i < input.Length; i++)
            {
                if(!"[,]".Contains(input[i]))
                {
                    // Assume number
                    endIndex = i;
                    if (startIndex < 0)
                        startIndex = i;
                }
                else if(",]".Contains(input[i]))
                {
                    // What could have bee a number is definitely over.
                    // If our indices are filled, check that number
                    if(startIndex >= 0 && endIndex >= 0)
                    {
                        int value = int.Parse(input.Substring(startIndex, endIndex - startIndex + 1));
                        
                        if(value >= 10)
                        {
                            // This is the number to split
                            int firstValue = value / 2;
                            int secondValue = value - firstValue;

                            input = input.Remove(startIndex, endIndex - startIndex + 1);
                            input = input.Insert(startIndex, $"[{firstValue},{secondValue}]");
                            return input;
                        }
                        else
                        {
                            // Restore both to default values
                            startIndex = -1;
                            endIndex = -1;
                        }
                    }
                    else
                    {
                        // Restore both to default values
                        startIndex = -1;
                        endIndex = -1;
                    }
                }
            }

            // No changes needed
            return input;
        }

        public int FindMiddleCommaIndex(string input)
        {
            int openBrackets = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '[')
                    openBrackets++;

                if (input[i] == ']')
                    openBrackets--;

                if (input[i] == ',' && openBrackets == 1)
                    return i;
            }

            // Failure
            return -1;
        }

    }
}
