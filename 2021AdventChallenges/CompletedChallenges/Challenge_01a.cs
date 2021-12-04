using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventChallenges
{
    public class Challenge_01a
    {
        public void Run()
        {
            List<int> depths = new List<int>();

            string[] depthTexts = System.IO.File.ReadAllLines(@"C:\Users\Zach Nichols\ElevenFiftyProjects\Practice\2021AdventChallenges\2021AdventChallenges\Inputs\Challenge_01a_input.txt");
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
    }
}
