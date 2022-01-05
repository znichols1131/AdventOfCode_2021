using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventChallenges
{
    public class Challenge_09
    {
        public void Challenge_A()
        {
            // Get lines
            string filePath = Path.Combine(@"..\..\Inputs\", "Challenge_09a_input.txt");
            List<string> lines = System.IO.File.ReadAllLines(filePath).ToList<string>();

            // Convert to integers
            List<List<int>> depths = new List<List<int>>();
            foreach (string line in lines)
            {
                List<int> lineOfDepths = new List<int>();
                foreach(char d in line)
                {
                    lineOfDepths.Add(int.Parse(d.ToString()));
                }
                depths.Add(lineOfDepths);
            }

            // Check for lowest points
            List<int> lowestDepths = new List<int>();
            for(int row = 0; row < depths.Count; row++)
            {
                for(int col = 0; col < depths[row].Count; col++)
                {
                    bool lowest = true;

                    // Check left
                    if(col - 1 >= 0)
                        lowest = lowest && (depths[row][col] < depths[row][col - 1]);

                    // Check right
                    if (col + 1 < depths[row].Count)
                        lowest = lowest && (depths[row][col] < depths[row][col + 1]);

                    // Check up
                    if (row - 1 >= 0 && col < depths[row - 1].Count)
                        lowest = lowest && (depths[row][col] < depths[row - 1][col]);

                    // Check down
                    if (row + 1 < depths.Count && col < depths[row + 1].Count)
                        lowest = lowest && (depths[row][col] < depths[row + 1][col]);

                    // Add the lowest points to the list
                    if (lowest)
                    {
                        lowestDepths.Add(depths[row][col]);
                        Console.ForegroundColor = ConsoleColor.Red;
                    }

                    Console.Write(depths[col][row]);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine();
            }

            // Get risk level
            int risk = 0;
            foreach(int d in lowestDepths)
            {
                risk += (d + 1);
            }

            // Output
            Console.WriteLine("\nRisk level: " + risk);
            Console.ReadLine();
        }

        private List<List<DepthPoint>> _depths = new List<List<DepthPoint>>();

        public void Challenge_B()
        {
            // Get lines
            string filePath = Path.Combine(@"..\..\Inputs\", "Challenge_09a_input.txt");
            List<string> lines = System.IO.File.ReadAllLines(filePath).ToList<string>();

            // Convert to integers
            for(int row = 0; row < lines.Count; row++)
            {
                List<DepthPoint> lineOfDepths = new List<DepthPoint>();
                for (int col = 0; col < lines[row].Length; col++)
                {
                    lineOfDepths.Add(new DepthPoint(row, col, int.Parse( lines[row][col].ToString() )));
                }
                _depths.Add(lineOfDepths);
            }

            // Find basins
            int nextBasinId = 0;
            List<DepthPoint> checkedPoints = new List<DepthPoint>();
            List<List<DepthPoint>> basins = new List<List<DepthPoint>>();
            basins.Add(new List<DepthPoint>());

            for (int row = 0; row < lines.Count; row++)
            {
                for (int col = 0; col < lines[row].Length; col++)
                {
                    // Check if basin is already assigned and is not a peak
                    if(_depths[row][col].BasinId == -1 && _depths[row][col].Depth != 9)
                    {
                        // Try to loop through surrounding points to check for basin ID
                        int basinId = GetBasinIdForSeed(row, col, checkedPoints);

                        if(basinId != -1)
                        {
                            // Successfully found an attached basin
                            _depths[row][col].BasinId = basinId;
                        }
                        else
                        {
                            // Unsuccessful, give it a new unique basin ID and create a new basin
                            _depths[row][col].BasinId = nextBasinId;
                            nextBasinId++;
                            basins.Add(new List<DepthPoint>());
                        }

                        // Add that point to a basin
                        basins[_depths[row][col].BasinId].Add(_depths[row][col]);
                        checkedPoints.Clear();
                    }
                }
            }

            // Find 3 largest basins
            List<int> basinAreas = new List<int>();
            for(int b = 0; b < basins.Count; b++)
            {
                basinAreas.Add(basins[b].Count);

                // Remove lowest areas until there are only 3 remaining
                while (basinAreas.Count > 3)
                {
                    basinAreas.Remove(basinAreas.Min());
                }
            }

            // Output
            Console.WriteLine("Largest basins: ");
            int product = 1;
            for(int i = 0; i < basinAreas.Count; i++)
            {
                product *= basinAreas[i];
                Console.WriteLine(basinAreas[i]);
            }
            Console.WriteLine("Product: " + product);
            Console.ReadLine();

        }

        public int GetBasinIdForSeed(int row, int col, List<DepthPoint>checkedPoints)
        {
            // Try to grab the basin ID of the nearest connected depth that isn't a peak
            bool success = false;

            // For each direction
            // Check that there hasn't been a success so far
            // Check that the rows and columns are within bounds
            // Check that the target point is not a peak (9)
            // Check that the target point is not already checked

            // If the target point has a basin ID, return it to the parent of the method
            // Otherwise, start a new recursive loop
            // Worst case: loop ends when all remaining points are peaks (9) and all others are checked.

            // Check up
            if (!success && row - 1 >= 0 && col < _depths[row - 1].Count && _depths[row-1][col].Depth != 9 && !checkedPoints.Contains(_depths[row-1][col]))
            {
                // If the target cell has a basin already, return basin ID
                if (_depths[row - 1][col].BasinId != -1)
                    return _depths[row - 1][col].BasinId;

                // Otherwise, recursively start new seed
                checkedPoints.Add(_depths[row - 1][col]);
                int basinId = GetBasinIdForSeed(row - 1, col, checkedPoints);

                if (basinId != -1)
                    return basinId;

                success = false;
            }

            // Check left
            if (!success && col - 1 >= 0 && _depths[row][col - 1].Depth != 9 && !checkedPoints.Contains(_depths[row][col-1]))
            {
                // If the target cell has a basin already, return basin ID
                if (_depths[row][col - 1].BasinId != -1)
                    return _depths[row][col-1].BasinId;

                // Otherwise, recursively start new seed
                checkedPoints.Add(_depths[row][col - 1]);
                int basinId = GetBasinIdForSeed(row, col-1, checkedPoints);

                if (basinId != -1)
                    return basinId;

                success = false;
            }

            // Check right
            if (!success && col + 1 < _depths[row].Count && _depths[row][col + 1].Depth != 9 && !checkedPoints.Contains(_depths[row][col+1]))
            {
                // If the target cell has a basin already, return basin ID
                if (_depths[row][col + 1].BasinId != -1)
                    return _depths[row][col + 1].BasinId;

                // Otherwise, recursively start new seed
                checkedPoints.Add(_depths[row][col + 1]);
                int basinId = GetBasinIdForSeed(row, col + 1, checkedPoints);

                if (basinId != -1)
                    return basinId;

                success = false;
            }

            // Check down
            if (!success && row + 1 < _depths.Count && col < _depths[row + 1].Count && _depths[row + 1][col].Depth != 9 && !checkedPoints.Contains(_depths[row + 1][col]))
            {
                // If the target cell has a basin already, return basin ID
                if (_depths[row + 1][col].BasinId != -1)
                    return _depths[row + 1][col].BasinId;

                // Otherwise, recursively start new seed
                checkedPoints.Add(_depths[row + 1][col]);
                int basinId = GetBasinIdForSeed(row + 1, col, checkedPoints);

                if (basinId != -1)
                    return basinId;

                success = false;
            }

            // No basin ID found
            return -1;
        }
    }

    public class DepthPoint
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public int Depth { get; set; }
        public int BasinId { get; set; }

        public DepthPoint() 
        {
            BasinId = -1;
        }

        public DepthPoint(int row, int col, int depth)
        {
            Row = row;
            Col = col;
            Depth = depth;
            BasinId = -1;
        }
    }
}
