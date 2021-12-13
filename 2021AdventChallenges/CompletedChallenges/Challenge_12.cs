using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventChallenges
{
    public class Challenge_12
    {
        private List<Cave> _caves = new List<Cave>();
        private List<string> _paths = new List<string>();

        public void Challenge_A()
        {
            // Get lines
            string path = @"C:\Users\Zach Nichols\ElevenFifty\Practice\2021Advent\2021AdventChallenges\Inputs\Challenge_12a_input.txt";
            List<string> lines = System.IO.File.ReadAllLines(path).ToList<string>();

            // Set up caves
            foreach (string line in lines)
            {
                string[] points = line.Split('-');
                AddConnection(points[0], points[1]);
            }

            // Delete any obsolete caves (only connected to small caves)
            for(int i = _caves.Count -1; i>=0; i--)
            {
                if (_caves[i].IsCaveObsolete())
                    _caves.RemoveAt(i);
            }

            // Get starting point, check if null
            Cave start = GetCaveForID("start");
            if(start is null)
            {
                Console.WriteLine("Error: no starting point.");
                Console.ReadLine();
                return;
            }

            // Find all paths, do not allow sub to revisit small caves
            FindAllPathsFromSeed(start, "", 0);

            // Output
            PrintPaths();
            Console.WriteLine("\n\nNumber of paths: " + _paths.Count);
            Console.ReadLine();
        }

        public void Challenge_B()
        {
            // Get lines
            string path = @"C:\Users\Zach Nichols\ElevenFifty\Practice\2021Advent\2021AdventChallenges\Inputs\Challenge_12a_input.txt";
            List<string> lines = System.IO.File.ReadAllLines(path).ToList<string>();

            // Set up caves
            foreach (string line in lines)
            {
                string[] points = line.Split('-');
                AddConnection(points[0], points[1]);
            }

            // Delete any obsolete caves (only connected to small caves)
            for (int i = _caves.Count - 1; i >= 0; i--)
            {
                if (_caves[i].IsCaveObsolete())
                    _caves.RemoveAt(i);
            }

            // Get starting point, check if null
            Cave start = GetCaveForID("start");
            if (start is null)
            {
                Console.WriteLine("Error: no starting point.");
                Console.ReadLine();
                return;
            }

            // Find all paths, all sub to revisit a single small cave if needed
            FindAllPathsFromSeed(start, "", 1);

            // Output
            PrintPaths();
            Console.WriteLine("\n\nNumber of paths: " + _paths.Count);
            Console.ReadLine();
        }

        public Cave GetCaveForID(string id)
        {
            foreach(Cave c in _caves)
            {
                if (c.ID == id)
                    return c;
            }

            return null;
        }

        public void AddConnection (string caveAId, string caveBId)
        {
            // Get first cave or set it up
            Cave caveA = GetCaveForID(caveAId);
            if (caveA is null)
            {
                caveA = new Cave(caveAId);
                _caves.Add(caveA);
            }

            // Get second cave or set it up
            Cave caveB = GetCaveForID(caveBId);
            if (caveB is null)
            {
                caveB = new Cave(caveBId);
                _caves.Add(caveB);
            }
            // Add each cave to the other's list of connections
            caveA.Connections.Add(caveB);
            caveB.Connections.Add(caveA);
        }

        public void FindAllPathsFromSeed(Cave seed, string pathSoFar, int revisitsAllowed)
        {
            if(pathSoFar is null || pathSoFar=="")
            {
                // Starting string
                pathSoFar = "start";
            }else
            {
                pathSoFar += ("," + seed.ID);
            }

            // Get caves visited so far
            string[] visitedCaveIds = pathSoFar.Split(',');

            // Parse through IDs
            foreach(Cave c in seed.Connections)
            {
                // Check if it's NOT the starting point AND
                // If it's either a large cave or hasn't been visited before
                if (c.ID != "start" && (c.IsLargeCave || (!pathSoFar.Contains(c.ID) || revisitsAllowed>0) ))
                {
                    if(c.ID == "end")
                    {
                        // This is a valid path, add it to the list
                        _paths.Add(pathSoFar + "," + c.ID);
                    }else
                    {
                        // Use this as a new iterative seed
                        // If it wasn't a large cave and we had to revisit, remove one allowable revisit
                        FindAllPathsFromSeed(c, pathSoFar, (!c.IsLargeCave && pathSoFar.Contains(c.ID)) ? revisitsAllowed - 1 : revisitsAllowed);
                    }
                }
            }
        }

        public void PrintPaths()
        {
            Console.WriteLine();
            foreach (string path in _paths)
            {
                Console.WriteLine(path);
            }
            Console.WriteLine();
        }
    }

    public class Cave
    {
        public string ID { get; set; }
        public bool IsLargeCave { get; set; }
        public List<Cave> Connections { get; set; } = new List<Cave>();

        public Cave() { }

        public Cave(string id)
        {
            ID = id;

            // If uppercase, it's a large cave
            if (ID == id.ToUpper())
                IsLargeCave = true;
        }

        public bool IsCaveObsolete()
        {
            // If a cave is only connected to a single small cave,
            // It would be impossible to visit this cave without
            // needing to revisit the small cave. Thus, the cave
            // in question cannot be visited.
            return (Connections.Count == 1 && !Connections[0].IsLargeCave);
        }
    }
}
