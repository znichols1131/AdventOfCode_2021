using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventChallenges
{
    public class Challenge_24
    {
        private List<string> _instructions = new List<string>();
        private int _w = 0;
        private int _x = 0;
        private int _y = 0;
        private int _z = 0;

        public void Challenge_A()
        {
            // Get input
            string path = @"C:\Users\Zach Nichols\ElevenFifty\Practice\2021Advent\2021AdventChallenges\Inputs\Challenge_24a_input.txt";
            _instructions = System.IO.File.ReadAllLines(path).ToList<string>();

            // Get highest valid model number
            //decimal model = FindLargestValidModelNum();

            // Used Excel spreadsheet to solve with help understanding from u/relativistic-turtle (Reddit)
            // Source: https://www.reddit.com/r/adventofcode/comments/rnejv5/2021_day_24_solutions/?utm_source=share&utm_medium=web2x&context=3
            decimal model = 99995969919326m;
            if(RunMonad(model.ToString()) == 0)
            {
                Console.WriteLine($"\n\nHighest valid model number: {model}");
            }else
            {
                Console.WriteLine($"\n\nError: this is not a valid number.");
            }
            // Output
            Console.ReadLine();
        }

        public void Challenge_B()
        {
            // Get input
            string path = @"C:\Users\Zach Nichols\ElevenFifty\Practice\2021Advent\2021AdventChallenges\Inputs\Challenge_24a_input.txt";
            _instructions = System.IO.File.ReadAllLines(path).ToList<string>();

            // Get highest valid model number
            //decimal model = FindLargestValidModelNum();

            // Used Excel spreadsheet to solve with help understanding from u/relativistic-turtle (Reddit)
            // Source: https://www.reddit.com/r/adventofcode/comments/rnejv5/2021_day_24_solutions/?utm_source=share&utm_medium=web2x&context=3
            decimal model = 48111514719111m;
            if (RunMonad(model.ToString()) == 0)
            {
                Console.WriteLine($"\n\nLowest valid model number: {model}");
            }
            else
            {
                Console.WriteLine($"\n\nError: this is not a valid number.");
            }
            // Output
            Console.ReadLine();
        }

        public decimal FindLargestValidModelNum()
        {
            // Model is a 14-digit number that cannot contain zeros
            decimal model = 99999977999999;
            while (model >= 11111111111111)
            {
                // Check if the number contains any zeros
                if(model.ToString().Contains('0'))
                {
                    // As a shortcut, find out where the '0' is.
                    // If it's in the 1's place, substract 1.
                    // If it's in the 10's place, subtract 10, and so on.
                    for(int i = model.ToString().Length-1; i >= 0; i--)
                    {
                        if(model.ToString()[i] == '0')
                        {
                            model -= (decimal)Math.Pow(10, model.ToString().Length - 1 - i);
                        }
                    }

                    // No need to run ALU on invalid model number
                    goto Try_Again;
                }

                // Test model number
                // Monad should return 0 if valid.
                Console.WriteLine("Attempting " + model);
                if (RunMonad(model.ToString()) == 0)
                    return model;

                // If model number wasn't valid, try next number
                model--;

            Try_Again:;
                // Jump to here if the model number was changed
            }

            // No valid model number found
            return -1;
        }

        public int RunMonad(string model)
        {
            // Keep track of next input index
            int index = 0;

            // Perform instructions
            foreach(string instruction in _instructions)
            {
                switch(instruction.Substring(0,3).ToLower())
                {
                    case "inp":
                        // Input a value to a variable
                        if (!ALU_Input(instruction, int.Parse(model[index].ToString())))
                            return -1;
                        index++;
                        break;

                    case "add":
                        // Add two values, store to first variable
                        if (!ALU_Add(instruction))
                            return -1;
                        break;

                    case "mul":
                        // Multiply two values, store to first variable
                        if (!ALU_Multiply(instruction))
                            return -1;
                        break;

                    case "div":
                        // Divide first number by second number, store to first variable
                        if (!ALU_Divide(instruction))
                            return -1;
                        break;

                    case "mod":
                        // Find remainder from dividing first number by second number, store to first variable
                        if (!ALU_Modulo(instruction))
                            return -1;
                        break;

                    case "eql":
                        // Find if numbers are equal, store 1 (true) or 0 (false) in first variable
                        if (!ALU_Equal(instruction))
                            return -1;
                        break;

                    default:
                        break;
                }
            }

            // Return z value
            return _z;
        } 
        
        public bool ALU_Input(string instruction, int value)
        {
            switch(instruction.Substring(4, 1).ToLower())
            {
                case "w":
                    _w = value;
                    return true;

                case "x":
                    _x = value;
                    return true;

                case "y":
                    _y = value;
                    return true;

                case "z":
                    _z = value;
                    return true;

                default:
                    return true;
            }
        }

        public bool ALU_Add(string instruction)
        {
            string firstVar = instruction.Substring(4, 1).ToLower();
            string secondVar = instruction.Substring(6, instruction.Length-6).ToLower();
            int value = 0;

            switch (secondVar)
            {
                case "w":
                    value = _w;
                    break;

                case "x":
                    value = _x;
                    break;

                case "y":
                    value = _y;
                    break;

                case "z":
                    value = _z;
                    break;

                default:
                    value = int.Parse(secondVar);
                    break;
            }

            switch (firstVar)
            {
                case "w":
                    _w += value;
                    break;

                case "x":
                    _x += value;
                    break;

                case "y":
                    _y += value;
                    break;

                case "z":
                    _z += value;
                    break;

                default:
                    break;
            }

            return true;
        }

        public bool ALU_Multiply(string instruction)
        {
            string firstVar = instruction.Substring(4, 1).ToLower();
            string secondVar = instruction.Substring(6, instruction.Length - 6).ToLower();
            int value = 0;

            switch (secondVar)
            {
                case "w":
                    value = _w;
                    break;

                case "x":
                    value = _x;
                    break;

                case "y":
                    value = _y;
                    break;

                case "z":
                    value = _z;
                    break;

                default:
                    value = int.Parse(secondVar);
                    break;
            }

            switch (firstVar)
            {
                case "w":
                    _w *= value;
                    break;

                case "x":
                    _x *= value;
                    break;

                case "y":
                    _y *= value;
                    break;

                case "z":
                    _z *= value;
                    break;

                default:
                    break;
            }

            return true;
        }

        public bool ALU_Divide(string instruction)
        {
            string firstVar = instruction.Substring(4, 1).ToLower();
            string secondVar = instruction.Substring(6, instruction.Length - 6).ToLower();
            int value = 0;

            switch (secondVar)
            {
                case "w":
                    value = _w;
                    break;

                case "x":
                    value = _x;
                    break;

                case "y":
                    value = _y;
                    break;

                case "z":
                    value = _z;
                    break;

                default:
                    value = int.Parse(secondVar);
                    break;
            }

            // Don't allow division by zero
            if (value == 0)
                return false;

            switch (firstVar)
            {
                case "w":
                    _w /= value;
                    break;

                case "x":
                    _x /= value;
                    break;

                case "y":
                    _y /= value;
                    break;

                case "z":
                    _z /= value;
                    break;

                default:
                    break;
            }

            return true;
        }

        public bool ALU_Modulo(string instruction)
        {
            string firstVar = instruction.Substring(4, 1).ToLower();
            string secondVar = instruction.Substring(6, instruction.Length - 6).ToLower();
            int value = 0;

            switch (secondVar)
            {
                case "w":
                    value = _w;
                    break;

                case "x":
                    value = _x;
                    break;

                case "y":
                    value = _y;
                    break;

                case "z":
                    value = _z;
                    break;

                default:
                    value = int.Parse(secondVar);
                    break;
            }

            // Don't allow division by zero
            if (value == 0)
                return false;

            switch (firstVar)
            {
                case "w":
                    _w %= value;
                    break;

                case "x":
                    _x %= value;
                    break;

                case "y":
                    _y %= value;
                    break;

                case "z":
                    _z %= value;
                    break;

                default:
                    break;
            }

            return true;
        }

        public bool ALU_Equal(string instruction)
        {
            string firstVar = instruction.Substring(4, 1).ToLower();
            string secondVar = instruction.Substring(6, instruction.Length - 6).ToLower();
            int value = 0;

            switch (secondVar)
            {
                case "w":
                    value = _w;
                    break;

                case "x":
                    value = _x;
                    break;

                case "y":
                    value = _y;
                    break;

                case "z":
                    value = _z;
                    break;

                default:
                    value = int.Parse(secondVar);
                    break;
            }

            switch (firstVar)
            {
                case "w":
                    _w = (_w == value) ? 1 : 0;
                    break;

                case "x":
                    _x = (_x == value) ? 1 : 0;
                    break;

                case "y":
                    _y = (_y == value) ? 1 : 0;
                    break;

                case "z":
                    _z = (_z == value) ? 1 : 0;
                    break;

                default:
                    break;
            }

            return true;
        }
    }
}
