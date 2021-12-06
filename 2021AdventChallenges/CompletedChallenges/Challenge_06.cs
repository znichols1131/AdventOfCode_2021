using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventChallenges
{
    public class Challenge_06
    {
        public void Challenge_A()
        {
            List<int> _listOfFish = new List<int>();

            // Get all lines from file
            string path = @"C:\Users\Zach Nichols\ElevenFiftyProjects\Practice\2021AdventChallenges\2021AdventChallenges\Inputs\Challenge_06a_input.txt";
            List<string> lines = System.IO.File.ReadAllLines(path).ToList<string>();

            // Fish attributes
            int adultReset = 6;
            int childStart = 8;

            // Populate fish array
            foreach (string line in lines)
            {
                string[] input = line.Split(',');
                foreach(string s in input)
                {
                    _listOfFish.Add(int.Parse(s.Trim()));
                }
            }

            // Simulate each day
            for(int day = 0; day<80; day++)
            {
                // Parse each fish
                int count = _listOfFish.Count;
                for(int f = 0; f < count; f++)
                {
                    // Update fish counter
                    _listOfFish[f] -= 1;

                    // If the fish is ready to spawn, reset the fish and add a new fish
                    if(_listOfFish[f] < 0)
                    {
                        _listOfFish[f] = adultReset;
                        _listOfFish.Add(childStart);
                    }
                }
            }

            // Output number of fish
            Console.WriteLine("Number of fish: " + _listOfFish.Count);
            Console.ReadLine();
        }

        public void Challenge_B()
        {
            List<decimal> _listOfFishCounts = new List<decimal>();

            // Fish attributes
            int adultReset = 6;
            int childStart = 8;

            // Get all lines from file
            string path = @"C:\Users\Zach Nichols\ElevenFiftyProjects\Practice\2021AdventChallenges\2021AdventChallenges\Inputs\Challenge_06a_input.txt";
            List<string> lines = System.IO.File.ReadAllLines(path).ToList<string>();

            // Set up fish count array
            for(int i = 0; i < 9; i++)
            {
                _listOfFishCounts.Add(0);
            }

            // Populate
            foreach (string line in lines)
            {
                string[] input = line.Split(',');
                foreach (string s in input)
                {
                    int age = int.Parse(s.Trim());
                    _listOfFishCounts[age]++;
                }
            }

            // Simulate each day
            for (int day = 0; day < 256; day++)
            {
                // How many fish will spawn today
                decimal fishToSpawn = _listOfFishCounts[0];

                // Parse each age of fish
                for (int f = 1; f < _listOfFishCounts.Count; f++)
                {
                    _listOfFishCounts[f - 1] = _listOfFishCounts[f];
                    _listOfFishCounts[f] = 0.0m;
                }

                // Spawn new fish and reset adults
                _listOfFishCounts[adultReset] += fishToSpawn;
                _listOfFishCounts[childStart] += fishToSpawn;
            }

            // Output number of fish
            decimal sum = 0.0m;
            for (int i = 0; i < _listOfFishCounts.Count; i++)
            {
                sum += _listOfFishCounts[i];
            }

            Console.WriteLine("Number of fish: " + sum);
            Console.ReadLine();
        }
    }
}
