using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventChallenges
{
    public class Challenge_01b
    {
        public void Run()
        {
            List<int> depths = new List<int>();

            string[] depthTexts = System.IO.File.ReadAllLines(@"C:\Users\Zach Nichols\ElevenFiftyProjects\Practice\2021AdventChallenges\2021AdventChallenges\Inputs\Challenge_01b_input.txt");
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
