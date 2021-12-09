using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventChallenges
{
    public class Challenge_08
    {
        private List<bool[]> _answerKey = new List<bool[]>();
        private bool[] _defaultKey = new bool[] { true, true, true, true, true, true, true };
        private char[] _letters = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g' };

        public Challenge_08()
        {
            // Populate answer key
            // 0 = top
            // 1 = top-right
            // 2 = bottom-right
            // 3 = bottom
            // 4 = bottom-left
            // 5 = top-left
            // 6 = middle
            _answerKey.Add(new bool[] { true, true, true, true, true, true, false });       // Zero
            _answerKey.Add(new bool[] { false, true, true, false, false, false, false });   // One
            _answerKey.Add(new bool[] { true, true, false, true, true, false, true });      // Two
            _answerKey.Add(new bool[] { true, true, true, true, false, false, true });      // Three
            _answerKey.Add(new bool[] { false, true, true, false, false, true, true });     // Four
            _answerKey.Add(new bool[] { true, false, true, true, false, true, true });      // Five
            _answerKey.Add(new bool[] { true, false, true, true, true, true, true });       // Six
            _answerKey.Add(new bool[] { true, true, true, false, false, false, false });    // Seven
            _answerKey.Add(new bool[] { true, true, true, true, true, true, true });        // Eight
            _answerKey.Add(new bool[] { true, true, true, true, false, true, true });      // Nine
        }
        public void Challenge_A()
        {
            // Get lines
            string path = @"C:\Users\Zach Nichols\ElevenFiftyProjects\Practice\2021AdventChallenges\2021AdventChallenges\Inputs\Challenge_08a_input.txt";
            List<string> lines = System.IO.File.ReadAllLines(path).ToList<string>();

            // Get a list of lines
            // Each line will have a list of 10 signal patterns and 4 outputs
            List<List<string>> entries = new List<List<string>>();
            foreach(string line in lines)
            {
                string[] splitLine = line.Split('|');
                List<string> outputStrings = new List<string>();

                // Parse 10 unique signal patterns
                foreach (string sigPat in splitLine[0].Trim().Split(' '))
                {
                    outputStrings.Add(sigPat.Trim());
                }

                // Parse 4 outputs
                foreach (string output in splitLine[1].Trim().Split(' '))
                {
                    outputStrings.Add(output.Trim());
                }

                entries.Add(outputStrings);
            }

            // Keep track of easy numbers
            int count = 0;

            // Try to convert those outputs to numbers
            List<List<int>> outputs = new List<List<int>>();
            foreach(List<string> entryStr in entries)
            {
                List<int> entry = new List<int>();
                for(int i = 10; i < entryStr.Count; i++)
                {
                    string signalStr = entryStr[i]; 

                    int e = ConvertSignal_ChallengeA(signalStr);
                    entry.Add(e);

                    if (e > 0)
                        count++;
                }

                outputs.Add(entry);
            }

            Console.WriteLine("Easy numbers (1, 4, 7, 8): " + count);
            Console.ReadLine();
        }

        public int ConvertSignal_ChallengeA(string signal)
        {
            switch (signal.Length)
            {
                case 2:
                    // Must be number 1                    
                    return 1;

                case 3:
                    // Must be number 7
                    return 2;

                case 4:
                    // Must be number 4
                    return 3;

                case 7:
                    // Must be number 8
                    return 4;

                default:
                    break;
            }

            return -1;
        }

        public void Challenge_B()
        {
            // Get lines
            string path = @"C:\Users\Zach Nichols\ElevenFiftyProjects\Practice\2021AdventChallenges\2021AdventChallenges\Inputs\Challenge_08a_input.txt";
            List<string> lines = System.IO.File.ReadAllLines(path).ToList<string>();

            // Get a list of lines
            // Each line will have a list of 10 signal patterns and 4 outputs
            List<List<string>> entries = new List<List<string>>();
            foreach (string line in lines)
            {
                string[] splitLine = line.Split('|');
                List<string> outputStrings = new List<string>();

                // Parse 10 unique signal patterns
                foreach (string sigPat in splitLine[0].Trim().Split(' '))
                {
                    outputStrings.Add(sigPat.Trim());
                }

                // Parse 4 outputs
                foreach (string output in splitLine[1].Trim().Split(' '))
                {
                    outputStrings.Add(output.Trim());
                }

                entries.Add(outputStrings);
            }

            // Try to convert those outputs to numbers
            int sum = 0;
            foreach (List<string> line in entries)
            {
                sum += ConvertLineOfSignals(line);
            }

            Console.WriteLine("Sum of readings: " + sum);
            Console.ReadLine();
        }

        public int ConvertLineOfSignals(List<string> signals)
        {
            // Set up a key
            // 0 = top
            // 1 = top-right
            // 2 = bottom-right
            // 3 = bottom
            // 4 = bottom-left
            // 5 = top-left
            // 6 = middle
            List<List<bool>> key = new List<List<bool>>();
            for(int i = 0; i<7; i++)
            {
                key.Add(_defaultKey.ToList<bool>());
            }

            // Solve the key for this line
            bool success = false;
            while(!success)
            {
                foreach (string signal in signals)
                {
                    //Console.Clear();
                    //PrintKey(key);
                    //Console.WriteLine("Input: " + signal + "\n");

                    RefineKey(key, signal);

                    //PrintKey(key);
                    //Console.ReadLine();
                }

                // If any line on the digit has a single confirmed letter, remove that letter from the possibilities on all other lines
                for(int line = 0; line < key.Count; line++)
                {
                    int trueCount = 0;
                    int lastTrueIndex = 0;
                    for (int letter = 0; letter < key[line].Count; letter++)
                    {
                        if (key[line][letter])
                        {
                            trueCount++;
                            lastTrueIndex = letter;
                        }
                    }

                    if(trueCount == 1)
                    {
                        // This row only has a one true value
                        for(int lineAgain = 0; lineAgain < key.Count; lineAgain++)
                        {
                            // Remove that character from possibilities on all other rows
                            if (lineAgain != line)
                                key[lineAgain][lastTrueIndex] = false;
                        }
                    }
                }

                // If any column has a single confirmed letter, remove that letter from the possibilities on all other columns
                for (int letter = 0; letter < key[0].Count; letter++)
                {
                    int trueCount = 0;
                    int lastTrueIndex = 0;
                    for (int line = 0; line < key.Count; line++)
                    {
                        if (key[line][letter])
                        {
                            trueCount++;
                            lastTrueIndex = line;
                        }
                    }

                    if (trueCount == 1)
                    {
                        // This column only has one true value
                        for(int letterAgain = 0; letterAgain < key[lastTrueIndex].Count; letterAgain++)
                        {
                            // Remove that character from possibilities on all other rows
                            if (letterAgain != letter)
                                key[lastTrueIndex][letterAgain] = false;
                        }
                    }
                }

                // Check if key is solved
                bool complete = true;
                foreach (List<bool> line in key)
                {
                    int trueCount = 0;
                    foreach (bool b in line)
                    {
                        if (b)
                            trueCount++;
                    }
                    complete = complete && (trueCount == 1);
                }

                success = complete;
            }

            // Now interpret the line
            // Read the last 4 digits (after the first 10)
            int answer = 0;
            for (int i = 10; i < signals.Count; i++)
            {

                int e = GetNumberForCompletedKey(signals[i], key);
                answer = 10 * answer + e;
            }

            return answer;
        }

        public void RefineKey(List<List<bool>> key, string signal)
        {
            // Check easy numbers
            switch (signal.Length)
            {
                case 2:
                    // Must be number 1
                    UpdateKeyForAnswer(key, 1, signal);
                    return;

                case 3:
                    // Must be number 7
                    UpdateKeyForAnswer(key, 7, signal);
                    return;

                case 4:
                    // Must be number 4
                    UpdateKeyForAnswer(key, 4, signal);
                    return;

                case 7:
                    // Must be number 8, not helpful in refining
                    //Console.WriteLine("Answer : 8");
                    return;

                default:
                    break;
            }

            // Find what lines contain those characters
            List<bool> lineContainsChar = new List<bool> { false, false, false, false, false, false, false };
            List<int> letters = new List<int>();
            
            foreach (char c in signal)
            {
                int letter = GetIndexForLetter(c);
                if (!letters.Contains(letter))
                    letters.Add(letter);

                // For each line position, see if this letter is still valid
                for (int line = 0; line < key.Count; line++)
                {
                    bool beforeBool = lineContainsChar[line];
                    bool keyBool = key[line][letter];                    
                    lineContainsChar[line] = keyBool || beforeBool;
                }

                //Console.WriteLine("Character: " + c + "\n0 1 2 3 4 5 6");
                //foreach(bool b in lineContainsChar)
                //{
                //    if(b)
                //    {
                //        Console.ForegroundColor = ConsoleColor.Green;
                //        Console.Write("T ");
                //    }else
                //    {
                //        Console.Write("- ");
                //    }
                //    Console.ForegroundColor = ConsoleColor.White;
                //}
                //Console.WriteLine("\n");
            }

            // Check if right side is solved
            // First check if the right side is down to only two possible letters per row
            bool rightSideComplete = false;
            bool capableOfFillingRightSide = false;
            if (TrueCount(key[1]) <= 2 && TrueCount(key[2]) <= 2)
            {
                // At this point, both rows (positions) have <=2 answers that could work
                List<List<int>> workingIndices = new List<List<int>>();

                // Check only the top right and top left lines
                for(int line = 1; line <= 2; line++)
                {
                    workingIndices.Add(new List<int>());

                    // Find all letters that meet the requirements
                    for(int letter = 0; letter < key[line].Count; letter++)
                    {
                        if(key[line][letter])
                        {
                            workingIndices[line-1].Add(letter);
                        }
                    }
                }

                // Remove duplicates
                workingIndices = RemoveDuplicatesFromList(workingIndices);

                // If only two characters met the requirements for both rows, the right side is complete
                rightSideComplete = (workingIndices[0].Count == 2 && workingIndices[1].Count == 2);

                // Check if there are enough characters in the signal to fill both positions simultaneously
                int fillCount = 0;
                foreach (int letter in letters)
                {
                    if(workingIndices[0].Contains(letter))
                    {
                        fillCount++;
                    }
                }

                // If both are capable of being filled by the letter, success
                // Note: if only one letter is present in letters, count will only be 1
                capableOfFillingRightSide = (fillCount == 2);                
            }

            // Now check 5-line digits (2, 3, 5)
            if (signal.Length == 5)
            {
                if(!lineContainsChar[2])
                {
                    // Bottom right doesn't contain it, must be 2
                    UpdateKeyForAnswer(key, 2, signal);
                    return;
                }
                else if (!lineContainsChar[1])
                {
                    // Top right doesn't contain it, must be 5
                    UpdateKeyForAnswer(key, 5, signal);
                    return;
                }
                else if (!lineContainsChar[4] && !lineContainsChar[5])
                {
                    // Bottom left and top left don't contain it, must be 3
                    UpdateKeyForAnswer(key, 3, signal);
                    return;
                }
                else if (rightSideComplete && lineContainsChar[1] && lineContainsChar[2] && capableOfFillingRightSide)
                {
                    // The top right and bottom right are collectively solved (ex: ab or ba)
                    // and the current signal contains both
                    // so it must be 3
                    UpdateKeyForAnswer(key, 3, signal);
                }
            }

            // Now check 6-line digits (6, 9, 0)
            if(signal.Length == 6)
            {
                if(!lineContainsChar[1])
                {
                    // Top right doesn't contain it, must be a 6
                    UpdateKeyForAnswer(key, 6, signal);
                    return;
                }
                else if(!lineContainsChar[4])
                {
                    // Bottom left doesn't contain it, must be a 6
                    UpdateKeyForAnswer(key, 9, signal);
                    return;
                }
                else if (!lineContainsChar[6])
                {
                    // Middle doesn't contain it, must be a 0
                    UpdateKeyForAnswer(key, 0, signal);
                    return;
                }
                else if (rightSideComplete && (lineContainsChar[1] || lineContainsChar[2]) && !capableOfFillingRightSide)
                {
                    // The top right and bottom right are collectively solved (ex: ab or ba)
                    // but the current signal only contains one or the other
                    // so it must be 6
                    UpdateKeyForAnswer(key, 6, signal);
                }
            }
        }

        public int GetIndexForLetter(char letter)
        {
            for (int i = 0; i < _letters.Length; i++)
            {
                if (_letters[i] == letter)
                    return i;
            }

            // Not found
            return -1;
        }

        public int GetNumberForCompletedKey(string signal, List<List<bool>> key)
        {
            // Figure out which lines in the digit are populated by the signal data
            List<bool> lineContainsChar = new List<bool> { false, false, false, false, false, false, false };
            for (int i = 0; i < key.Count; i++)
            {
                foreach (char c in signal)
                {
                    int j = GetIndexForLetter(c);

                    // If this letter matches the position in the key
                    if (key[i][j])
                    {
                        lineContainsChar[i] = true;
                    }
                }
            }

            // Match the populated lines to the answer key to figure out which number it is
            for (int i = 0; i < _answerKey.Count; i++)
            {
                if (_answerKey[i].SequenceEqual<bool>(lineContainsChar))
                    return i;
            }

            // Couldn't figure it out this try
            return -1;
        }
        public void UpdateKeyForAnswer(List<List<bool>> key, int answer, string signal)
        {
            bool[] positionKey = _answerKey[answer];

            //Console.WriteLine("Answer: " + answer);

            // For each character in signal
            List<int> indicesOfSignalChars = new List<int>();
            foreach (char c in signal)
            {
                indicesOfSignalChars.Add(GetIndexForLetter(c));
            }

            // For each line in answer key
            for (int i = 0; i < key.Count; i++)
            {
                // For each possible character
                for(int j = 0; j < key[i].Count; j++)
                {
                    // If this line should be populated for the answer
                    // and that character is valid giventhe signal input
                    if(positionKey[i] && indicesOfSignalChars.Contains(j))
                    {
                        // It's supposed to be occupied and it could be correct

                    } else if (!positionKey[i] && !indicesOfSignalChars.Contains(j))
                    {
                        // It's not supposed to be occupied but it wasn't correct for this number anyways
                    }
                    else
                    {
                        // Remove this letter from the possibilities for this line
                        key[i][j] = false;
                    }
                }
            }
        }

        public int TrueCount(List<bool> line)
        {
            int trueCount = 0;
            foreach (bool b in line)
            {
                if (b)
                    trueCount++;
            }

            return trueCount;
        }

        public List<List<int>> RemoveDuplicatesFromList(List<List<int>> list)
        {
            List<List<int>> result = new List<List<int>>();
            foreach(List<int> row in list)
            {
                List<int> rowResult = new List<int>();
                foreach (int i in row)
                {
                    if (!rowResult.Contains(i))
                        rowResult.Add(i);
                }

                result.Add(rowResult);
            }            

            return result;
        }

        public void PrintKey(List<List<bool>> key)
        {
            Console.WriteLine();
            Console.WriteLine("  A B C D E F G");
            int lineCount = 0;
            foreach(List<bool> line in key)
            {
                Console.Write(lineCount + " ");

                foreach (bool b in line)
                {
                    Console.Write((b ? "X" : "-") + " ");
                }
                Console.WriteLine();
                lineCount++;
            }
            Console.WriteLine();
        }
    }
}
