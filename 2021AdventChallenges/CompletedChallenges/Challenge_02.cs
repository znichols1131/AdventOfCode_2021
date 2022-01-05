using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventChallenges
{
    public class Challenge_02
    {
        public void Challenge_A()
        {
            int depth = 0;
            int horizontalPosition = 0;

            string fwdStr = "forward";
            string dwnStr = "down";
            string upStr = "up";

            string filePath = Path.Combine(@"..\..\Inputs\", "Challenge_02a_input.txt");
            string[] instructions = System.IO.File.ReadAllLines(filePath);

            foreach (string instruction in instructions)
            {
                if (instruction.ToLower().StartsWith(fwdStr))
                {
                    string distance = instruction.Replace(fwdStr, "");
                    distance = distance.Trim();
                    horizontalPosition += int.Parse(distance);
                }
                else if (instruction.ToLower().StartsWith(dwnStr))
                {
                    string distance = instruction.Replace(dwnStr, "");
                    distance = distance.Trim();
                    depth += int.Parse(distance);
                }
                else if (instruction.ToLower().StartsWith(upStr))
                {
                    string distance = instruction.Replace(upStr, "");
                    distance = distance.Trim();
                    depth -= int.Parse(distance);
                }
            }

            Console.WriteLine("Final position: " + horizontalPosition);
            Console.WriteLine("Final depth: " + depth);
            Console.WriteLine("\nProduct: " + horizontalPosition * depth);
            Console.ReadLine();
        }

        public void Challenge_B()
        {
            int depth = 0;
            int horizontalPosition = 0;
            int aim = 0;

            string fwdStr = "forward";
            string dwnStr = "down";
            string upStr = "up";

            string filePath = Path.Combine(@"..\..\Inputs\", "Challenge_02a_input.txt");
            string[] instructions = System.IO.File.ReadAllLines(filePath);

            foreach (string instruction in instructions)
            {
                if (instruction.ToLower().StartsWith(fwdStr))
                {
                    string distance = instruction.Replace(fwdStr, "");
                    distance = distance.Trim();

                    int x = int.Parse(distance);
                    horizontalPosition += x;
                    depth += aim * x;
                }
                else if (instruction.ToLower().StartsWith(dwnStr))
                {
                    string degree = instruction.Replace(dwnStr, "");
                    degree = degree.Trim();
                    aim += int.Parse(degree);
                }
                else if (instruction.ToLower().StartsWith(upStr))
                {
                    string degree = instruction.Replace(upStr, "");
                    degree = degree.Trim();
                    aim -= int.Parse(degree);
                }
            }

            Console.WriteLine("Final position: " + horizontalPosition);
            Console.WriteLine("Final depth: " + depth);
            Console.WriteLine("Final aim: " + aim);
            Console.WriteLine("\nProduct: " + horizontalPosition * depth);
            Console.ReadLine();
        }
    }
}
