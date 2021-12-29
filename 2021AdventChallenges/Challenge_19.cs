using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventChallenges
{
    public class Challenge_19
    {
        // Note on coordinates:
        // X+ = East
        // Y+ = North
        // Z+ = Up

        private List<Scanner> _scanners = new List<Scanner>();
        private List<Scanner> _alignedScanners = new List<Scanner>();
        private List<(Scanner.DirectionState, Scanner.DirectionState)> _orientations = new List<(Scanner.DirectionState, Scanner.DirectionState)>()
        {
            // First element is direction facing, second element is direction up
            (Scanner.DirectionState.XPos, Scanner.DirectionState.ZPos),
            (Scanner.DirectionState.XPos, Scanner.DirectionState.YPos),
            (Scanner.DirectionState.XPos, Scanner.DirectionState.ZNeg),
            (Scanner.DirectionState.XPos, Scanner.DirectionState.YNeg),

            (Scanner.DirectionState.XNeg, Scanner.DirectionState.ZPos),
            (Scanner.DirectionState.XNeg, Scanner.DirectionState.YPos),
            (Scanner.DirectionState.XNeg, Scanner.DirectionState.ZNeg),
            (Scanner.DirectionState.XNeg, Scanner.DirectionState.YNeg),

            (Scanner.DirectionState.YPos, Scanner.DirectionState.ZPos),
            (Scanner.DirectionState.YPos, Scanner.DirectionState.XPos),
            (Scanner.DirectionState.YPos, Scanner.DirectionState.ZNeg),
            (Scanner.DirectionState.YPos, Scanner.DirectionState.XNeg),

            (Scanner.DirectionState.YNeg, Scanner.DirectionState.ZPos),
            (Scanner.DirectionState.YNeg, Scanner.DirectionState.XPos),
            (Scanner.DirectionState.YNeg, Scanner.DirectionState.ZNeg),
            (Scanner.DirectionState.YNeg, Scanner.DirectionState.XNeg),

            (Scanner.DirectionState.ZPos, Scanner.DirectionState.XPos),
            (Scanner.DirectionState.ZPos, Scanner.DirectionState.YPos),
            (Scanner.DirectionState.ZPos, Scanner.DirectionState.XNeg),
            (Scanner.DirectionState.ZPos, Scanner.DirectionState.YNeg),

            (Scanner.DirectionState.ZNeg, Scanner.DirectionState.XPos),
            (Scanner.DirectionState.ZNeg, Scanner.DirectionState.YPos),
            (Scanner.DirectionState.ZNeg, Scanner.DirectionState.XNeg),
            (Scanner.DirectionState.ZNeg, Scanner.DirectionState.YNeg)
        };

        public void Challenge_A()
        {
            // Get input
            string path = @"C:\Users\Zach Nichols\ElevenFifty\Practice\2021Advent\2021AdventChallenges\Inputs\Challenge_19a_input.txt";
            List<string> lines = System.IO.File.ReadAllLines(path).ToList<string>();

            // Get scanners and probes from input
            Scanner currentScanner = new Scanner();
            foreach (string line in lines)
            {
                if(line.StartsWith("--- scanner"))
                {
                    // Get id
                    int id = int.Parse(line.Replace("--- scanner ", "").Replace(" ---", "").Trim());

                    // New scanner
                    currentScanner = new Scanner(id);
                    currentScanner.Location = new Location(0, 0, 0);

                    // Get scanner number
                }
                else if(line is null || line == "")
                {
                    // Previous scanner is finished
                    if(currentScanner != null)
                    {
                        _scanners.Add(currentScanner);
                        currentScanner = null;
                    }
                }else
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
                _scanners.Add(currentScanner);
                currentScanner = null;
            }

            // Align unaligned scanners
            while(_scanners.Any())
            {
                // Get first scanner in list
                var unalignedScanner = _scanners.First();

                // If aligned scanners is empty, add the first scanners to use as a reference
                if(!_alignedScanners.Any())
                {
                    _alignedScanners.Add(unalignedScanner);
                    continue;
                }

                // At this point, try to align scanner.
                if (AlignScanner(unalignedScanner))
                {
                    // If successful, remove the scanner from the list of unaligned scanners.
                    // AlignScanner() will automatically add it to list of aligned scanners.
                    _scanners.Remove(unalignedScanner);
                    continue;
                }


                // If unsuccessful, move scanner to end of list so that we don't pull it next.
                _scanners.Remove(unalignedScanner);
                _scanners.Add(unalignedScanner);
            }

            // Get unique probes from list of aligned scanners

            // Output
            Console.WriteLine("Number of scanners: " + _scanners.Count);
            Console.ReadLine();
        }

        public void Challenge_B()
        {

        }

        public bool AlignScanner(Scanner targetScanner)
        {
            // Check if 12 points in common with all already-aligned points
            List<Location> alignedProbes = new List<Location>();
            foreach (Scanner s in _alignedScanners)
            {
                s.Probes.ForEach(p => alignedProbes.Add(p));
            }
            alignedProbes = alignedProbes.Distinct().ToList();

            // Rotate the target scanner to get the most matches
            foreach ((Scanner.DirectionState, Scanner.DirectionState) orientation in _orientations)
            {
                // Align target scanner to this orientation
                targetScanner.RotateScannerToDirection(orientation.Item1, orientation.Item2);

                // Get offsets between targetScanner probes and aligned probes
                List<Location> offsets = new List<Location>();
                foreach(var p in targetScanner.Probes)
                {
                    foreach(var ap in alignedProbes)
                    {
                        offsets.Add(new Location(p.X - ap.X, p.Y - ap.Y, p.Z - ap.Z));
                    }
                }

                // If 12 or more offsets are the same, this alignment of targetScanner has 12+ points in common with the aligned probes (success)
                var sortedOffsets = from o in offsets
                                    group o by new
                                    {
                                        o.X,
                                        o.Y,
                                        o.Z,
                                    } into groupedOffsets
                                    select new
                                    {
                                        X = groupedOffsets.Key.X,
                                        Y = groupedOffsets.Key.Y,
                                        Z = groupedOffsets.Key.Z,
                                        Count = groupedOffsets.Count()
                                    };

                // For debugging
                Console.WriteLine($"Scanner {targetScanner.ID} aligned to {orientation.Item1}, {orientation.Item2} => {sortedOffsets.Max(x => x.Count)} points.");

                if(sortedOffsets.Any(o => o.Count >= 12))
                {
                    // Success

                    // Get the offset (targetProbe - alignedProbes)
                    var offset = sortedOffsets.Where(o => o.Count >= 12).First();

                    // Update coordinates of target scanner relative to reference scanner
                    targetScanner.Location.X -= offset.X;
                    targetScanner.Location.Y -= offset.Y;
                    targetScanner.Location.Z -= offset.Z;

                    // Update coordinates of target scanner's probes relative to reference scanner
                    foreach (Location p in targetScanner.Probes)
                    {
                        p.X -= offset.X;
                        p.Y -= offset.Y;
                        p.Z -= offset.Z;
                    }

                    // Add target scanner to list of aligned scanners
                    _alignedScanners.Add(targetScanner);
                    return true;
                }
            }

            // None of the orientations helped align the scanner
            return false;
        }

        public int NumberOfProbesInCommon(Scanner scannerOne, Scanner scannerTwo)
        {
            // For the given orientation of each scanner, find the number of probes in common between these scanners

            // For each probe in scannerOne, find the offsets it has to the probes in scannnerTwo
            List<(int, int, int)> offsets = new List<(int, int, int)>();
            foreach (Location probeOne in scannerOne.Probes)
            {
                foreach (Location probeTwo in scannerTwo.Probes)
                {
                    int deltaX = probeOne.X - probeTwo.X;
                    int deltaY = probeOne.Y - probeTwo.Y;
                    int deltaZ = probeOne.Z - probeTwo.Z;

                    offsets.Add((deltaX, deltaY, deltaZ));
                }
            }

            // Now find what the most common offset was by
            // using SQL to group the offsets by deltaX, deltaY, and deltaZ.
            // The query should contain a list of counts for each unique offset.
            var query = from o in offsets
                        group o by new { o.Item1, o.Item2, o.Item3 } into g
                        select new
                        {
                            Count = g.Count()
                        };

            // Now return the highest count of unqiue offsets
            return query.Max(g => g.Count);
        }

    }

    public class Scanner
    {
        public int ID { get; set; }
        public Location Location { get; set; }
        public DirectionState DirectionFacing { get; set; }
        public DirectionState DirectionUp { get; set; }
        public List<Location> Probes { get; set; } = new List<Location>();

        public Scanner() { }
        public Scanner(int id)
        {
            ID = id;
            DirectionFacing = DirectionState.YPos;
            DirectionUp = DirectionState.ZPos;
        }
        public Scanner(int id, Location location)
        {
            ID = id;
            Location = location;
            DirectionFacing = DirectionState.YPos;
            DirectionUp = DirectionState.ZPos;
        }

        public void RotateScannerToDirection(DirectionState targetFacing, DirectionState targetUp)
        {
            // Move to face target direction
            int degreesXY = GetDegreesInXYPlane(targetFacing) - GetDegreesInXYPlane(DirectionFacing);
            int degreesYZ = GetDegreesInYZPlane(targetFacing) - GetDegreesInYZPlane(DirectionFacing);
            RotateScanner(degreesXY, degreesYZ);

            // Move to correct up orientation
            degreesXY = GetDegreesInXYPlane(targetUp) - GetDegreesInXYPlane(DirectionUp);
            degreesYZ = GetDegreesInYZPlane(targetUp) - GetDegreesInYZPlane(DirectionUp);
            RotateScanner(degreesXY, degreesYZ);
        }

        public void RotateScanner(int xyRotation, int yzRotation)
        {
            // Deal with xy Rotation
            // Positive rotation = x+ to y+
            int currentXY = GetDegreesInXYPlane(DirectionFacing);
            currentXY += xyRotation;
            DirectionFacing = GetDirectionInXYPlane(currentXY);
            RotateProbesXY(xyRotation);

            // Now deal with yz rotation
            // Positive rotation = y+ to z+
            int currentYZ = GetDegreesInYZPlane(DirectionFacing);
            currentYZ += yzRotation;
            DirectionFacing = GetDirectionInYZPlane(currentYZ);
            RotateProbesYZ(yzRotation);
        }

        public void RotateProbesXY(int xyRotation)
        {
            // Get the number of quarter rotations (90 degrees turns)
            int quarterRotations = (int)(xyRotation / 90);
            while (quarterRotations >= 4)
                quarterRotations -= 4;
            while (quarterRotations < 0)
                quarterRotations += 4;

            if (quarterRotations == 0)
                return;

            // Iterate through all probes
            foreach(Location probe in Probes)
            {
                int previousY = probe.Y;
                switch (quarterRotations)
                {
                    case 1:
                        // x+ becomes y+, y+ becomes x-
                        probe.Y = probe.X;
                        probe.X = -previousY;
                        return;
                    case 2:
                        // x+ becomes x-, y+ becomes y-
                        probe.X *= -1;
                        probe.Y *= -1;
                        return;
                    case 3:
                        // x+ becomes y-, y+ becomes x+
                        probe.Y = -probe.X;
                        probe.X = previousY;
                        return;
                    default:
                        return;
                }
            }
        }

        public void RotateProbesYZ(int yzRotation)
        {
            // Get the number of quarter rotations (90 degrees turns)
            int quarterRotations = (int)(yzRotation / 90);
            while (quarterRotations >= 4)
                quarterRotations -= 4;
            while (quarterRotations < 0)
                quarterRotations += 4;

            if (quarterRotations == 0)
                return;

            // Iterate through all probes
            foreach (Location probe in Probes)
            {
                int previousZ = probe.Z;
                switch (quarterRotations)
                {
                    case 1:
                        // y+ becomes z+, z+ becomes y-
                        probe.Z = probe.Y;
                        probe.Y = -previousZ;
                        return;
                    case 2:
                        // y+ becomes y-, z+ becomes z-
                        probe.Y *= -1;
                        probe.Z *= -1;
                        return;
                    case 3:
                        // y+ becomes z-, z+ becomes y+
                        probe.Z = -probe.Y;
                        probe.Y = previousZ;
                        return;
                    default:
                        return;
                }
            }
        }

        public int GetDegreesInXYPlane(DirectionState direction)
        {
            switch(direction)
            {
                case DirectionState.XPos:
                    return 0;
                case DirectionState.YPos:
                    return 90;
                case DirectionState.XNeg:
                    return 180;
                case DirectionState.YNeg:
                    return 270;
                default:
                    return -360;
            }
        }

        public DirectionState GetDirectionInXYPlane(int degrees)
        {
            // Get rid of anything above 360 or below 0
            while (degrees >= 360)
                degrees -= 360;
            while (degrees < 0)
                degrees += 360;

            switch (degrees)
            {
                case 0:
                    return DirectionState.XPos;
                case 90:
                    return DirectionState.YPos;
                case 180:
                    return DirectionState.XNeg;
                case 270:
                    return DirectionState.YNeg;
                default:
                    return DirectionState.None;
            }
        }

        public int GetDegreesInYZPlane(DirectionState direction)
        {
            switch (direction)
            {
                case DirectionState.YPos:
                    return 0;
                case DirectionState.ZPos:
                    return 90;
                case DirectionState.YNeg:
                    return 180;
                case DirectionState.ZNeg:
                    return 270;
                default:
                    return -360;
            }
        }

        public DirectionState GetDirectionInYZPlane(int degrees)
        {
            // Get rid of anything above 360 or below 0
            while (degrees >= 360)
                degrees -= 360;
            while (degrees < 0)
                degrees += 360;

            switch (degrees)
            {
                case 0:
                    return DirectionState.YPos;
                case 90:
                    return DirectionState.ZPos;
                case 180:
                    return DirectionState.YNeg;
                case 270:
                    return DirectionState.ZNeg;
                default:
                    return DirectionState.None;
            }
        }

        public enum DirectionState
        {
            None = 0,
            XPos = 1,
            XNeg = 2,
            YPos = 3,
            YNeg = 4,
            ZPos = 5,
            ZNeg = 6
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
