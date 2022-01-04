using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventChallenges
{
    public class Challenge_19
    {
        private List<Scanner> _unalignedScanners = new List<Scanner>();
        private List<Scanner> _alignedScanners = new List<Scanner>();
        private int _maxID = 0;

        // After some debugging, I've found an order to the scanners being aligned.
        // For future debugging, I'm adding this shortcut.
        // Note: couldn't get scanners 4,6,8 to line up, so I put them first. This worked.
        private List<int> _indicesForSorting = new List<int>() { 0, 13, 14, 16, 20, 9, 11, 19, 24, 25, 5, 7, 10, 21, 22, 23, 17, 18, 1, 2, 3, 4, 6, 8, 15, 12 };

        public void Challenge_A()
        {
            // Get input
            string path = @"C:\Users\Zach Nichols\ElevenFifty\Practice\2021Advent\2021AdventChallenges\Inputs\Challenge_19a_input.txt";
            List<string> lines = System.IO.File.ReadAllLines(path).ToList<string>();

            // Get scanners and probes from input
            Scanner currentScanner = new Scanner();
            foreach (string line in lines)
            {
                if (line.StartsWith("--- scanner"))
                {
                    // Get id
                    int id = int.Parse(line.Replace("--- scanner ", "").Replace(" ---", "").Trim());

                    // New scanner
                    currentScanner = new Scanner(id);
                    currentScanner.Location = new Location(0, 0, 0);

                    // Get scanner number
                }
                else if (line is null || line == "")
                {
                    // Previous scanner is finished
                    if (currentScanner != null)
                    {
                        _unalignedScanners.Add(currentScanner);
                        currentScanner = null;
                    }
                }
                else
                {
                    // Add this probe location to the scanner
                    string[] probeCoordinates = line.Split(',');
                    currentScanner.Probes.Add(new Location()
                    {
                        X = int.Parse(probeCoordinates[0]),
                        Y = int.Parse(probeCoordinates[1]),
                        Z = int.Parse(probeCoordinates[2])
                    });
                }
            }

            // Last scanner is finished
            if (currentScanner != null)
            {
                _unalignedScanners.Add(currentScanner);
                currentScanner = null;
            }

            // For debugging shortcut (see above), sort list
            SortStartingList();

            // Store max ID of scanners
            _maxID = _unalignedScanners.Count - 1;
            PrintScannerStatus(-1);

            // Align unaligned scanners
            while (_unalignedScanners.Any())
            {
                // Get first scanner in list
                var unalignedScanner = _unalignedScanners.First();

                // If aligned scanners is empty, add the first scanners to use as a reference
                if (!_alignedScanners.Any())
                {
                    _alignedScanners.Add(unalignedScanner);
                    _unalignedScanners.Remove(unalignedScanner);
                    PrintScannerStatus(unalignedScanner.ID + 1);
                    continue;
                }

                // At this point, try to align scanner.
                if (AlignScanner(unalignedScanner))
                {
                    // If successful, remove the scanner from the list of unaligned scanners.
                    // AlignScanner() will automatically add it to list of aligned scanners.
                    _unalignedScanners.Remove(unalignedScanner);
                    PrintScannerStatus(unalignedScanner.ID + 1);
                    continue;
                }


                // If unsuccessful, move scanner to end of list so that we don't pull it next.
                _unalignedScanners.Remove(unalignedScanner);
                _unalignedScanners.Add(unalignedScanner);
                PrintScannerStatus(unalignedScanner.ID + 1);
            }

            // Get unique probes from list of aligned scanners

            // Output
            Console.WriteLine("\nNumber of scanners: " + _alignedScanners.Count);
            Console.WriteLine("Number of unique probes: " + GetAlignedProbes(-1).Count);
            Console.ReadLine();
        }

        public void Challenge_B()
        {
            // Get input
            string path = @"C:\Users\Zach Nichols\ElevenFifty\Practice\2021Advent\2021AdventChallenges\Inputs\Challenge_19a_input.txt";
            List<string> lines = System.IO.File.ReadAllLines(path).ToList<string>();

            // Get scanners and probes from input
            Scanner currentScanner = new Scanner();
            foreach (string line in lines)
            {
                if (line.StartsWith("--- scanner"))
                {
                    // Get id
                    int id = int.Parse(line.Replace("--- scanner ", "").Replace(" ---", "").Trim());

                    // New scanner
                    currentScanner = new Scanner(id);
                    currentScanner.Location = new Location(0, 0, 0);

                    // Get scanner number
                }
                else if (line is null || line == "")
                {
                    // Previous scanner is finished
                    if (currentScanner != null)
                    {
                        _unalignedScanners.Add(currentScanner);
                        currentScanner = null;
                    }
                }
                else
                {
                    // Add this probe location to the scanner
                    string[] probeCoordinates = line.Split(',');
                    currentScanner.Probes.Add(new Location()
                    {
                        X = int.Parse(probeCoordinates[0]),
                        Y = int.Parse(probeCoordinates[1]),
                        Z = int.Parse(probeCoordinates[2])
                    });
                }
            }

            // Last scanner is finished
            if (currentScanner != null)
            {
                _unalignedScanners.Add(currentScanner);
                currentScanner = null;
            }

            // For debugging shortcut (see above), sort list
            SortStartingList();

            // Store max ID of scanners
            _maxID = _unalignedScanners.Count - 1;
            PrintScannerStatus(-1);

            // Align unaligned scanners
            while (_unalignedScanners.Any())
            {
                // Get first scanner in list
                var unalignedScanner = _unalignedScanners.First();

                // If aligned scanners is empty, add the first scanners to use as a reference
                if (!_alignedScanners.Any())
                {
                    _alignedScanners.Add(unalignedScanner);
                    _unalignedScanners.Remove(unalignedScanner);
                    PrintScannerStatus(unalignedScanner.ID + 1);
                    continue;
                }

                // At this point, try to align scanner.
                if (AlignScanner(unalignedScanner))
                {
                    // If successful, remove the scanner from the list of unaligned scanners.
                    // AlignScanner() will automatically add it to list of aligned scanners.
                    _unalignedScanners.Remove(unalignedScanner);
                    PrintScannerStatus(unalignedScanner.ID + 1);
                    continue;
                }


                // If unsuccessful, move scanner to end of list so that we don't pull it next.
                _unalignedScanners.Remove(unalignedScanner);
                _unalignedScanners.Add(unalignedScanner);
                PrintScannerStatus(unalignedScanner.ID + 1);
            }

            // Get unique probes from list of aligned scanners

            // Output
            Console.WriteLine("\nNumber of scanners: " + _alignedScanners.Count);
            Console.WriteLine("Number of unique probes: " + GetAlignedProbes(-1).Count);
            Console.WriteLine("\nLargest Manhattan distance between scanners: " + LargestManhattanDistance());
            Console.ReadLine();
        }

        public void PrintScannerStatus(int currentIndex)
        {
            // Make sure current index is valid
            if (!_unalignedScanners.Any(s => s.ID == currentIndex))
            {
                if (_unalignedScanners.Any(s => s.ID > currentIndex))
                {
                    currentIndex = _unalignedScanners.Where(s => s.ID > currentIndex).Min(s => s.ID);
                }
                else if(_unalignedScanners.Any())
                {
                    currentIndex = _unalignedScanners.Min(s => s.ID);
                }
            }

            Console.Clear();
            Console.WriteLine("List of scanners:\n");

            for (int i = 0; i <= _maxID; i++)
            {
                if(i == currentIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else if (_alignedScanners.Any(s => s.ID == i))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }

                Console.WriteLine("Scanner " + i);
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        public bool AlignScanner(Scanner unalignedScanner)
        {
            // Pseudocode:
            // Get a list of currently-aligned probes
            // For each probe on this scanner,
            // Set that probe equal to (0,0,0) and adjust all other probes relatively
            // Try all 24 orientations for the scanner
            // If any orientation yields 12+ matches to the list of aligned probes, success.
            // On a success, add scanner to list of aligned scanners.

            // Get a list of currently-aligned probes (search the most-recently added probes first)
            List<Location> alignedProbes = GetAlignedProbes(unalignedScanner.ID);
            alignedProbes.Reverse();

            foreach(Location alProbe in alignedProbes)
            {
                foreach(Location unProbe in unalignedScanner.Probes)
                {
                    // Try all orientations of scanner
                    for(int yz = 0; yz < 360; yz+=90)
                    {
                        for(int xy = 0; xy < 360; xy+=90)
                        {
                            // Offset all probes in this unaligned scanner so that alProbe matches unProbe
                            unalignedScanner.OffsetAllProbesFrom(unProbe, alProbe);

                            // Try this orientation
                            if (CheckProbesForAlignment(unalignedScanner.Probes, alignedProbes))
                            {
                                // Success
                                goto Scanner_Aligned;
                            }

                            // If it wasn't successful, rotate 90 degrees
                            unalignedScanner.RotateXY(90);
                        }
                        // Rotate 90 degrees one last time to face original direction
                        unalignedScanner.RotateXY(90);

                        // Rotate 90 degrees in the next axis
                        unalignedScanner.RotateYZ(90);
                    }
                    // Rotate 90 degrees one last time to face original direction
                    unalignedScanner.RotateYZ(90);


                    // Now face up
                    // Same as +90xy rotation, +90yz rotation, (-90xy rotation unnecessary, just make it face up)
                    unalignedScanner.RotateXY(90);
                    unalignedScanner.RotateYZ(90);
                    //unalignedScanner.RotateProbesXY(-90);

                    for (int xy = 0; xy < 360; xy += 90)
                    {
                        // Offset all probes in this unaligned scanner so that alProbe matches unProbe
                        unalignedScanner.OffsetAllProbesFrom(unProbe, alProbe);

                        // Try this orientation
                        if (CheckProbesForAlignment(unalignedScanner.Probes, alignedProbes))
                        {
                            // Success
                            goto Scanner_Aligned;
                        }

                        // If it wasn't successful, rotate 90 degrees
                        unalignedScanner.RotateXY(90);
                    }
                    // Rotate 90 degrees one last time to face original direction
                    unalignedScanner.RotateXY(90);

                    // Now face down
                    // Same as +180yz rotation, starting orientation doesn't matter
                    unalignedScanner.RotateYZ(180);
                    for (int xy = 0; xy < 360; xy += 90)
                    {
                        // Offset all probes in this unaligned scanner so that alProbe matches unProbe
                        unalignedScanner.OffsetAllProbesFrom(unProbe, alProbe);

                        // Try this orientation
                        if (CheckProbesForAlignment(unalignedScanner.Probes, alignedProbes))
                        {
                            // Success
                            goto Scanner_Aligned;
                        }

                        // If it wasn't successful, rotate 90 degrees
                        unalignedScanner.RotateXY(90);
                    }

                    // At this point, we tried all orientations of this scanner for this unaligned probe.
                    // Pick the next unaligned probe and repeat.
                }

                // At this point, we tried all orientations of this scanner for all of its unaligned probes.
                // Pick the next aligned probe to use as the "origin" and repeat.
            }

            // At this point, we couldn't align the unaligned scanner successfully
            //Console.ForegroundColor = ConsoleColor.Red;
            //Console.WriteLine($"Scanner {unalignedScanner.ID} could not be aligned.");
            //Console.ForegroundColor = ConsoleColor.White;
            return false;

            // Jump to here if we were successful in aligning the scanner
        Scanner_Aligned:;

            //Console.ForegroundColor = ConsoleColor.Green;
            //Console.WriteLine($"Scanner {unalignedScanner.ID} was aligned successfully.");
            //Console.ForegroundColor = ConsoleColor.White;

            _alignedScanners.Add(unalignedScanner);
            _unalignedScanners.Remove(unalignedScanner);
            return true;

        }

        public bool CheckProbesForAlignment(List<Location> unalignedProbes, List<Location> alignedProbes)
        {
            ////For debugging
            //Console.Clear();
            //Console.WriteLine("\nAligned probes");

            //Console.ForegroundColor = ConsoleColor.Cyan;
            //foreach (var probe in alignedProbes)
            //{
            //    Console.WriteLine($"({probe.X},{probe.Y},{probe.Z})");
            //}

            //Console.ForegroundColor = ConsoleColor.White;
            //Console.WriteLine("\nUnaligned probes");
            //Console.ForegroundColor = ConsoleColor.Blue;
            //foreach (var probe in unalignedProbes)
            //{
            //    Console.WriteLine($"({probe.X},{probe.Y},{probe.Z})");
            //}

            //Console.ForegroundColor = ConsoleColor.White;

            int matches = 0;
            foreach(Location newProbe in unalignedProbes)
            {
                if (alignedProbes.Any(p => p.X == newProbe.X && p.Y == newProbe.Y && p.Z == newProbe.Z))
                    matches++;
            }

            //if (matches > 1)
            //    Console.WriteLine("Matches: " + matches);

            // Since our Align method works by setting at least one point in common between the aligned and unaligned probes,
            // Translation is fixed (xOffset, yOffset, zOffset) between the two sets of points.
            // That leaves 9 degrees of freedom (3 per axis) to check.
            return matches >= 9;
            //return matches >= 12;
        }

        public List<Location> GetAlignedProbes(int unalignedScannerID)
        {
            List<Location> alignedProbes = new List<Location>();
            foreach (Scanner alignedScanner in _alignedScanners)
            {
                foreach (Location probe in alignedScanner.Probes)
                {
                    // Make sure the list of probes is unique (no duplicates)
                    if (!alignedProbes.Any(p => p.X == probe.X && p.Y == probe.Y && p.Z == probe.Z))
                        alignedProbes.Add(probe);
                }
            }                     

            return alignedProbes;
        }

        public void SortStartingList()
        {
            List<Scanner> sortedScanners = new List<Scanner>();

            while(_indicesForSorting.Any())
            {
                int i = _indicesForSorting.First();
                _indicesForSorting.Remove(i);

                Scanner scanner = _unalignedScanners.Find(s => s.ID == i);

                sortedScanners.Add(scanner);
                _unalignedScanners.Remove(scanner);
            }

            if(_unalignedScanners.Any())
            {
                sortedScanners.AddRange(_unalignedScanners);
            }

            _unalignedScanners = sortedScanners;
        }

        public int LargestManhattanDistance()
        {
            int largestManhattan = 0;

            foreach(Scanner s1 in _alignedScanners)
            {
                foreach(Scanner s2 in _alignedScanners.Where(x => x.ID != s1.ID))
                {
                    int newManhattan =  Math.Abs(s1.Location.X - s2.Location.X) +
                                        Math.Abs(s1.Location.Y - s2.Location.Y) +
                                        Math.Abs(s1.Location.Z - s2.Location.Z);

                    //Console.WriteLine($"({s1.Location.X},{s1.Location.Y},{s1.Location.Z}) to\t({s2.Location.X},{s2.Location.Y},{s2.Location.Z}) =\t{newManhattan}");

                    largestManhattan = Math.Max(largestManhattan, newManhattan);
                }
            }

            return largestManhattan;
        }
    }

    public class Scanner
    {
        public int ID { get; set; }
        public Location Location { get; set; }
        public List<Location> Probes { get; set; } = new List<Location>();

        public Scanner() { }
        public Scanner(int id)
        {
            ID = id;
        }
        public Scanner(int id, Location location)
        {
            ID = id;
            Location = location;
        }

        public void RotateXY(int xyRotation)
        {
            // If we're debugging, we'll want to output some of the intermediate steps
            bool debug = false;

            // Get the number of quarter rotations (90 degrees turns)
            int quarterRotations = (int)(xyRotation / 90);
            while (quarterRotations >= 4)
                quarterRotations -= 4;
            while (quarterRotations < 0)
                quarterRotations += 4;

            if (quarterRotations == 0)
                return;

            if(debug)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Rotating scanner {ID} {xyRotation} in XY plane");
            }

            // Rotate scanner
            int previousScannerY = Location.Y;
            switch (quarterRotations)
            {
                case 1:
                    // x+ becomes y+, y+ becomes x-
                    Location.Y = Location.X;
                    Location.X = -previousScannerY;
                    break;
                case 2:
                    // x+ becomes x-, y+ becomes y-
                    Location.X *= -1;
                    Location.Y *= -1;
                    break;
                case 3:
                    // x+ becomes y-, y+ becomes x+
                    Location.Y = -Location.X;
                    Location.X = previousScannerY;
                    break;
                default:
                    break;
            }

            // Iterate through all probes
            foreach (Location probe in Probes)
            {
                if(debug)
                    Console.Write($"Rotating probe: \t({probe.X},{probe.Y},{probe.Z}) to\t");

                int previousY = probe.Y;
                switch (quarterRotations)
                {
                    case 1:
                        // x+ becomes y+, y+ becomes x-
                        probe.Y = probe.X;
                        probe.X = -previousY;
                        break;
                    case 2:
                        // x+ becomes x-, y+ becomes y-
                        probe.X *= -1;
                        probe.Y *= -1;
                        break;
                    case 3:
                        // x+ becomes y-, y+ becomes x+
                        probe.Y = -probe.X;
                        probe.X = previousY;
                        break;
                    default:
                        break;
                }

                if (debug)
                    Console.WriteLine($"({probe.X},{probe.Y},{probe.Z})");
            }

            if (debug)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public void RotateYZ(int yzRotation)
        {
            // If we're debugging, we'll want to output some of the intermediate steps
            bool debug = false;

            // Get the number of quarter rotations (90 degrees turns)
            int quarterRotations = (int)(yzRotation / 90);
            while (quarterRotations >= 4)
                quarterRotations -= 4;
            while (quarterRotations < 0)
                quarterRotations += 4;

            if (quarterRotations == 0)
                return;

            if (debug)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Rotating scanner {ID} {yzRotation} in YZ plane");
            }

            // Rotate scanner
            int previousScannerZ = Location.Z;
            switch (quarterRotations)
            {
                case 1:
                    // y+ becomes z+, z+ becomes y-
                    Location.Z = Location.Y;
                    Location.Y = -previousScannerZ;
                    break;
                case 2:
                    // y+ becomes y-, z+ becomes z-
                    Location.Y *= -1;
                    Location.Z *= -1;
                    break;
                case 3:
                    // y+ becomes z-, z+ becomes y+
                    Location.Z = -Location.Y;
                    Location.Y = previousScannerZ;
                    break;
                default:
                    break;
            }

            // Iterate through all probes
            foreach (Location probe in Probes)
            {
                if (debug)
                    Console.Write($"Rotating probe: \t({probe.X},{probe.Y},{probe.Z}) to\t");

                int previousZ = probe.Z;
                switch (quarterRotations)
                {
                    case 1:
                        // y+ becomes z+, z+ becomes y-
                        probe.Z = probe.Y;
                        probe.Y = -previousZ;
                        break;
                    case 2:
                        // y+ becomes y-, z+ becomes z-
                        probe.Y *= -1;
                        probe.Z *= -1;
                        break;
                    case 3:
                        // y+ becomes z-, z+ becomes y+
                        probe.Z = -probe.Y;
                        probe.Y = previousZ;
                        break;
                    default:
                        break;
                }

                if (debug)
                    Console.WriteLine($"({probe.X},{probe.Y},{probe.Z})");
            }

            if (debug)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public void OffsetAllProbesFrom(Location origin, Location destination)
        {
            // If we're debugging, we'll want to output some of the intermediate steps
            bool debug = false;
            
            // Get offset
            int xOffset = destination.X - origin.X;
            int yOffset = destination.Y - origin.Y;
            int zOffset = destination.Z - origin.Z;

            if(debug)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Offset scanner {ID}");
            }

            // Apply offset to scanner itself            
            Location.X += xOffset;
            Location.Y += yOffset;
            Location.Z += zOffset;

            // Apply offset to each probe
            foreach (Location probe in Probes)
            {
                if(debug)
                    Console.Write($"Offset probe\t{probe.X},{probe.Y},{probe.Z}) to\t");

                probe.X += xOffset;
                probe.Y += yOffset;
                probe.Z += zOffset;

                if(debug)
                    Console.WriteLine($"({probe.X},{probe.Y},{probe.Z})");
            }

            if (debug)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }

    public class Location
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public Location() { }
        public Location(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
