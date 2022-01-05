using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventChallenges
{
    public class Challenge_11
    {
        private int _rowCount = 10;
        private int _colCount = 10;
        private Octopus[,] octopi;

        public void Challenge_A()
        {
            // Get lines
            string filePath = Path.Combine(@"..\..\Inputs\", "Challenge_11a_input.txt");
            List<string> lines = System.IO.File.ReadAllLines(filePath).ToList<string>();

            // Set up array
            octopi = new Octopus[_rowCount, _colCount];
            for (int row = 0; row < lines.Count; row++)
            {
                for(int col = 0; col < lines[row].Length; col++)
                {
                    int lightLevel = int.Parse(lines[row][col].ToString());
                    octopi[row, col] = new Octopus(row, col, lightLevel);
                }
            }

            // Parse through each step and count flashes
            int targetSteps = 100;
            int flashes = 0;
            for(int step = 1; step <= targetSteps; step++)
            {
                //Console.Clear();
                //PrintOctopi();

                // Reset all octopi, increment light level by 1
                PrepareAllOctopi();
                //Console.WriteLine("\n");
                //PrintOctopi();

                // Now check if they flash starting with a seed 
                for (int row = 0; row < lines.Count; row++)
                {
                    for (int col = 0; col < lines[row].Length; col++)
                    {
                        // Check if octopus flashes (and if any surrounding octopi flash)
                        flashes += GetNewFlashesForSeed(octopi[row, col]);
                    }
                }

                //Console.WriteLine("\n\nAfter step: " + (step + 1));
                //Console.WriteLine("Flashes: " + flashes + "\n\n");
                //PrintOctopi();
                //Console.ReadLine();

            }

            // Output
            Console.WriteLine("\n\nFlashes: " + flashes);
            Console.ReadLine();
        }

        public void Challenge_B()
        {
            // Get lines
            string filePath = Path.Combine(@"..\..\Inputs\", "Challenge_11a_input.txt");
            List<string> lines = System.IO.File.ReadAllLines(filePath).ToList<string>();

            // Set up array
            octopi = new Octopus[_rowCount, _colCount];
            for (int row = 0; row < lines.Count; row++)
            {
                for (int col = 0; col < lines[row].Length; col++)
                {
                    int lightLevel = int.Parse(lines[row][col].ToString());
                    octopi[row, col] = new Octopus(row, col, lightLevel);
                }
            }

            // Parse through each step and count flashes
            int step = 0;
            bool success = false;
            while(!success)
            {
                step++;
                PrepareAllOctopi();

                // Now check if they flash starting with a seed 
                for (int row = 0; row < lines.Count; row++)
                {
                    for (int col = 0; col < lines[row].Length; col++)
                    {
                        // Check if octopus flashes (and if any surrounding octopi flash)
                        GetNewFlashesForSeed(octopi[row, col]);
                    }
                }

                success = CheckIfAllOctopiFlashed();
            }

            // Output
            Console.WriteLine("\n\nAll octopi flash on step: " + step);
            Console.ReadLine();
        }

        public void PrepareAllOctopi()
        {
            for (int row = 0; row < _rowCount; row++)
            {
                for (int col = 0; col < _colCount; col++)
                {
                    // Unflash octpus for this step
                    if (octopi[row, col].Flashed)
                    {
                        octopi[row, col].Flashed = false;
                        octopi[row, col].LightLevel = 0;
                    }

                    octopi[row, col].LightLevel++;
                }
            }
        }

        public bool CheckIfAllOctopiFlashed()
        {
            bool success = true;

            for (int row = 0; row < _rowCount; row++)
            {
                for (int col = 0; col < _colCount; col++)
                {
                    success = success && octopi[row, col].Flashed;
                }
            }

            return success;
        }

        public int GetNewFlashesForSeed(Octopus o)
        {
            int flashes = 0;

            // If it flashed, increment surrounding octopi
            if(o.CheckFlash())
            {
                flashes++;

                // Check row above
                if (o.Row -1 >= 0)
                {
                    // Check top
                    octopi[o.Row - 1, o.Col].LightLevel++;
                    flashes += GetNewFlashesForSeed(octopi[o.Row - 1, o.Col]);

                    // Check top left
                    if (o.Col -1 >= 0)
                    {
                        octopi[o.Row - 1, o.Col - 1].LightLevel++;
                        flashes += GetNewFlashesForSeed(octopi[o.Row - 1, o.Col - 1]);
                    }

                    // Check top right
                    if (o.Col + 1 < _colCount)
                    {
                        octopi[o.Row - 1, o.Col + 1].LightLevel++;
                        flashes += GetNewFlashesForSeed(octopi[o.Row - 1, o.Col + 1]);
                    }
                }

                // Check left
                if (o.Col - 1 >= 0)
                {
                    octopi[o.Row, o.Col - 1].LightLevel++;
                    flashes += GetNewFlashesForSeed(octopi[o.Row, o.Col - 1]);
                }

                // Check right
                if (o.Col + 1 < _colCount)
                {
                    octopi[o.Row, o.Col + 1].LightLevel++;
                    flashes += GetNewFlashesForSeed(octopi[o.Row, o.Col + 1]);
                }

                // Check row below
                if (o.Row + 1 < _rowCount)
                {
                    // Check bottom
                    octopi[o.Row + 1, o.Col].LightLevel++;
                    flashes += GetNewFlashesForSeed(octopi[o.Row + 1, o.Col]);

                    // Check bottom left
                    if (o.Col - 1 >= 0)
                    {
                        octopi[o.Row + 1, o.Col - 1].LightLevel++;
                        flashes += GetNewFlashesForSeed(octopi[o.Row + 1, o.Col - 1]);
                    }

                    // Check bottom right
                    if (o.Col + 1 < _colCount)
                    {
                        octopi[o.Row + 1, o.Col + 1].LightLevel++;
                        flashes += GetNewFlashesForSeed(octopi[o.Row + 1, o.Col + 1]);
                    }
                }
            }

            return flashes;
        }

        public class Octopus
        {
            public int Row { get; set; }
            public int Col { get; set; }
            public int LightLevel { get; set; }
            public bool Flashed { get; set; }

            public Octopus()
            {
                Flashed = false;
                LightLevel = 0;
            }

            public Octopus(int row, int col)
            {
                Flashed = false;
                LightLevel = 0;
                Row = row;
                Col = col;
            }

            public Octopus(int row, int col, int lightLevel)
            {
                Flashed = false;
                LightLevel = lightLevel;
                Row = row;
                Col = col;
            }

            // Checks if this octopus is ready to flash
            public bool CheckFlash()
            {
                if (LightLevel > 9 && !Flashed)
                {
                    Flashed = true;
                    LightLevel = 0;
                    return true;
                }

                return false;
            }
        }

        public void PrintOctopi()
        {
            for (int row = 0; row < _rowCount; row++)
            {
                for (int col = 0; col < _colCount; col++)
                {
                    Console.ForegroundColor = octopi[row, col].Flashed ? ConsoleColor.Green : ConsoleColor.White;
                    Console.Write(octopi[row, col].LightLevel);
                }
                Console.WriteLine();
            }
        }
    }
}
