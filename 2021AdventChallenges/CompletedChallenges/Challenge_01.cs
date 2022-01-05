using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventChallenges
{
    public class Challenge_01
    {
        public void Challenge_A()
        {
            List<int> depths = new List<int>();

            string filePath = Path.Combine(@"..\..\Inputs\", "Challenge_01a_input.txt");
            string[] depthTexts = System.IO.File.ReadAllLines(filePath);
            if (depthTexts.Length == 0)
                return;

            foreach (string depth in depthTexts)
            {
                try
                {
                    depths.Add(int.Parse(depth));
                }
                catch { }
            }

            if (depths.Count == 0)
                return;

            // Print the first measurement
            Console.WriteLine(depths[0]);

            // Counter for how many time the depth increases
            int count = 0;
            for (int i = 1; i < depths.Count; i++)
            {
                if (depths[i] > depths[i - 1])
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(depths[i] + " (Depth Increased)");
                    count++;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(depths[i] + " (Depth Decreased)");
                }
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\nFinal count: {count}");
            Console.ReadLine();
        }

        public void Challenge_B()
        {
            List<int> depths = new List<int>();

            string filePath = Path.Combine(@"..\..\Inputs\", "Challenge_01b_input.txt");
            string[] depthTexts = System.IO.File.ReadAllLines(filePath);
            if (depthTexts.Length == 0)
                return;

            foreach (string depth in depthTexts)
            {
                try
                {
                    depths.Add(int.Parse(depth));
                }
                catch { }
            }

            if (depths.Count < 3)
                return;

            // Counter for how many time the depth increases
            int count = 0;
            int window = 0;
            int sum = depths[2] + depths[1] + depths[0];
            Console.WriteLine("{0, -5}{1, -10}", window, sum);

            for (int i = 3; i < depths.Count; i++)
            {
                int previousSum = sum;
                sum = depths[i - 2] + depths[i - 1] + depths[i];
                window++;

                if (sum > previousSum)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("{0, -5}{1, -10}{2,-30}", window, sum, "Increased");
                    count++;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("{0, -5}{1, -10}{2,-30}", window, sum, "Decreased");
                }
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\nFinal count: {count}");
            Console.ReadLine();
        }
    }
}
