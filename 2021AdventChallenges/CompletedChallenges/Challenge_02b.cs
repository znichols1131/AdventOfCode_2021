using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventChallenges
{
    public class Challenge_02b
    {
        public void Run()
        {
            int depth = 0;
            int horizontalPosition = 0;
            int aim = 0;

            string fwdStr = "forward";
            string dwnStr = "down";
            string upStr = "up";

            string[] instructions = System.IO.File.ReadAllLines(@"C:\Users\Zach Nichols\ElevenFiftyProjects\Practice\2021AdventChallenges\2021AdventChallenges\Inputs\Challenge_02a_input.txt");

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
