using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventChallenges
{
    public class Challenge_22
    {
        private Cuboid _bounds = new Cuboid((50, -50), (50, -50), (50, -50), false);
        private List<Cuboid> _cuboids = new List<Cuboid>();
        public void Challenge_A()
        {
            // Get input
            string path = @"C:\Users\Zach Nichols\ElevenFifty\Practice\2021Advent\2021AdventChallenges\Inputs\Challenge_22a_input.txt";
            List<string> instructions = System.IO.File.ReadAllLines(path).ToList<string>();
            int id = 1;

            // Parse instructions
            foreach(string instruction in instructions)
            {
                if (instruction is null || instruction == "")
                    continue;

                //Console.WriteLine(instruction);
                Console.WriteLine($"Working on instruction {id} of {instructions.Count}.");
                string[] info = instruction.Replace(" x=", ",").Replace("y=", "").Replace("z=", "").Replace("..", "~").Replace(" ", ",").Split(',');
                
                bool turnOn = (info[0].ToLower() == "on");
                int xMin = int.Parse(info[1].Split('~')[0]);
                int xMax = int.Parse(info[1].Split('~')[1]);
                int yMin = int.Parse(info[2].Split('~')[0]);
                int yMax = int.Parse(info[2].Split('~')[1]);
                int zMin = int.Parse(info[3].Split('~')[0]);
                int zMax = int.Parse(info[3].Split('~')[1]);

                Cuboid newCuboid = new Cuboid((xMax, xMin), (yMax, yMin), (zMax, zMin), turnOn);

                //Console.WriteLine($"Before {_cuboids.Count} cuboids, volume on = {CountActiveCubesInBounds(_bounds)}");
                //Console.WriteLine($"Introducing cube with {newCuboid.Volume} and light value {newCuboid.On}.");
                ToggleCube(newCuboid);
                //Console.WriteLine($"After  {_cuboids.Count} cuboids, volume on = {CountActiveCubesInBounds(_bounds)}\n");

                id++;
            }

            // Output
            Console.WriteLine("Number of cubes on in boundary: " + CountActiveCubesInBounds(_bounds));
            Console.ReadLine();
        }

        public void Challenge_B()
        {
            // Get input
            string path = @"C:\Users\Zach Nichols\ElevenFifty\Practice\2021Advent\2021AdventChallenges\Inputs\Challenge_22b_input.txt";
            List<string> instructions = System.IO.File.ReadAllLines(path).ToList<string>();
            int id = 1;

            // Parse instructions
            foreach (string instruction in instructions)
            {
                if (instruction is null || instruction == "")
                    continue;

                //Console.WriteLine(instruction);
                Console.WriteLine($"Working on instruction {id} of {instructions.Count}.");
                string[] info = instruction.Replace(" x=", ",").Replace("y=", "").Replace("z=", "").Replace("..", "~").Replace(" ", ",").Split(',');

                bool turnOn = (info[0].ToLower() == "on");
                int xMin = int.Parse(info[1].Split('~')[0]);
                int xMax = int.Parse(info[1].Split('~')[1]);
                int yMin = int.Parse(info[2].Split('~')[0]);
                int yMax = int.Parse(info[2].Split('~')[1]);
                int zMin = int.Parse(info[3].Split('~')[0]);
                int zMax = int.Parse(info[3].Split('~')[1]);

                Cuboid newCuboid = new Cuboid((xMax, xMin), (yMax, yMin), (zMax, zMin), turnOn);

                //Console.WriteLine($"Before {_cuboids.Count} cuboids, volume on = {CountActiveCubesInBounds(null)}");
                //Console.WriteLine($"Introducing cube with {newCuboid.Volume} and light value {newCuboid.On}.");
                ToggleCube(newCuboid);
                //Console.WriteLine($"After  {_cuboids.Count} cuboids, volume on = {CountActiveCubesInBounds(null)}\n");

                id++;
            }

            // Output
            Console.WriteLine("Number of cubes on in bounds: " + CountActiveCubesInBounds(_bounds));
            Console.WriteLine("Number of cubes on (anywhere): " + CountActiveCubesInBounds(null));
            Console.ReadLine();
        }

        public void ToggleCube(Cuboid input)
        {
            // Because new cuboid could get split into smaller cuboids, we should store it as a list.
            List<Cuboid> newCuboids = new List<Cuboid>() { input };

            while(newCuboids.Any())
            {
                // Get first new cuboid
                Cuboid newestCuboid = newCuboids.First();

                for (int exIndex = 0; exIndex < _cuboids.Count; exIndex++)
                {
                    Cuboid existingCuboid = _cuboids[exIndex];

                    // Find any cuboids intersected by new cuboid(s)
                    if (newestCuboid.OverlapsWithCuboid(existingCuboid))
                    {
                        // Check if newest cuboid is contained by existing cuboid
                        if (newestCuboid.ContainedByCuboid(existingCuboid) && newestCuboid.On == existingCuboid.On)
                        {
                            // The newest cuboid is fully contained by this cuboid AND the on/off is already the same.
                            // Therefore, not only is the intersection irrelevant, the new cuboid can be removed from the
                            // new list entirely.
                            newCuboids.Remove(newestCuboid);
                            goto Try_Again;
                        }

                        // Get the intersection shared by the new cuboid and the old cuboid.
                        // Add it to the list of existing cuboids now that it's been processed.
                        Cuboid intersection = newestCuboid.GetIntersectedCuboid(newestCuboid, existingCuboid, newestCuboid.On);                       

                        // Replace the existing cuboid with subcuboids and add them to the list of existing cuboids.
                        foreach (var nc in existingCuboid.SubcuboidsAfterRemovingCuboid(intersection))
                        {
                            // Only add back non-empty cuboids
                            if(nc.On)
                                _cuboids.Add(nc);
                        }
                        _cuboids.Remove(existingCuboid);

                        goto Try_Again;
                    }
                }

                // At this point, the new cuboid has no intersects, so we can add it to the cuboids list and try again.
                // Only save cuboid if it's ON. They're assumed off by default.
                if (newestCuboid.On)
                    _cuboids.Add(newestCuboid);
                newCuboids.Remove(newestCuboid);                

            Try_Again:;
                // Jump to here if we made any changes. That way, we can check any new subcuboids for intersections.

            }
        }

        public decimal CountActiveCubesInBounds(Cuboid bounds)
        {
            decimal totalVolume = 0m;

            foreach (var c in _cuboids)
            {
                if(bounds is null && c.On)
                {
                    // Count the volume
                    totalVolume += c.Volume;
                }
                else if(bounds != null && (c.OverlapsWithCuboid(bounds) || c.ContainedByCuboid(bounds) || bounds.ContainedByCuboid(c)))
                {
                    // Count the volume within that bounds
                    var intersect = c.GetIntersectedCuboid(c, bounds, c.On);
                    var onVolume = intersect.On ? intersect.Volume : 0;
                    totalVolume += onVolume;
                }
            }

            return totalVolume;
        }
    }

    public class Cuboid
    {
        public (int Hi, int Lo) X { get; set; }
        public (int Hi, int Lo) Y { get; set; }
        public (int Hi, int Lo) Z { get; set; }
        public bool On { get; set; }

        public decimal Volume {
            get
            {
                return ((decimal)(X.Hi - X.Lo + 1)) * ((decimal)(Y.Hi - Y.Lo + 1)) * ((decimal)(Z.Hi - Z.Lo + 1));
            }
        }

        public Cuboid() { }

        public Cuboid((int hi, int lo) x, (int hi, int lo) y, (int hi, int lo) z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Cuboid((int hi, int lo) x, (int hi, int lo) y, (int hi, int lo) z, bool on)
        {
            X = x;
            Y = y;
            Z = z;
            On = on;
        }


        public bool ContainsPoint(int x, int y, int z)
        {
            int containingPlanes = 0;

            if (x >= X.Lo && x <= X.Hi)
                containingPlanes++;

            if (y >= Y.Lo && y <= Y.Hi)
                containingPlanes++;

            if (z >= Z.Lo && z <= Z.Hi)
                containingPlanes++;

            // If the point is within all three planes, it is contained by this cuboid
            return containingPlanes == 3;
        }

        public bool OverlapsWithCuboid(Cuboid otherCuboid)
        {
            int planesOfIntersection = 0;

            if ((X.Hi >= otherCuboid.X.Lo && X.Hi <= otherCuboid.X.Hi) ||
                (X.Lo >= otherCuboid.X.Lo && X.Lo <= otherCuboid.X.Hi) ||
                otherCuboid.X.Lo >= X.Lo && otherCuboid.X.Lo <= X.Hi ||
                otherCuboid.X.Hi >= X.Lo && otherCuboid.X.Hi <= X.Hi)
                planesOfIntersection++;

            if ((Y.Hi >= otherCuboid.Y.Lo && Y.Hi <= otherCuboid.Y.Hi) ||
                (Y.Lo >= otherCuboid.Y.Lo && Y.Lo <= otherCuboid.Y.Hi) ||
                otherCuboid.Y.Lo >= Y.Lo && otherCuboid.Y.Lo <= Y.Hi ||
                otherCuboid.Y.Hi >= Y.Lo && otherCuboid.Y.Hi <= Y.Hi)
                planesOfIntersection++;

            if ((Z.Hi >= otherCuboid.Z.Lo && Z.Hi <= otherCuboid.Z.Hi) ||
                (Z.Lo >= otherCuboid.Z.Lo && Z.Lo <= otherCuboid.Z.Hi) ||
                otherCuboid.Z.Lo >= Z.Lo && otherCuboid.Z.Lo <= Z.Hi ||
                otherCuboid.Z.Hi >= Z.Lo && otherCuboid.Z.Hi <= Z.Hi)
                planesOfIntersection++;

            // If there's overlap in all 3 planes, we have a volumetric overlap.
            // Note: overlap on an edge (X1.Hi = X2.Lo) is still overlap since each position (x,y,z) represents a cube.
            return planesOfIntersection == 3;
        }

        public Cuboid GetIntersectedCuboid(Cuboid cuboidOne, Cuboid cuboidTwo, bool lightSetting)
        {
            // This assumes that the cuboids have already been confirmed to intersect.

            // Start with cuboid one dimensions
            (int Hi, int Lo) x = (Math.Min(cuboidOne.X.Hi, cuboidTwo.X.Hi), Math.Max(cuboidOne.X.Lo, cuboidTwo.X.Lo));
            (int Hi, int Lo) y = (Math.Min(cuboidOne.Y.Hi, cuboidTwo.Y.Hi), Math.Max(cuboidOne.Y.Lo, cuboidTwo.Y.Lo));
            (int Hi, int Lo) z = (Math.Min(cuboidOne.Z.Hi, cuboidTwo.Z.Hi), Math.Max(cuboidOne.Z.Lo, cuboidTwo.Z.Lo));

            // Create new cuboid, giving it the latest cuboid's on/off value
            return new Cuboid(x, y, z, lightSetting);
        }

        public List<Cuboid> SubcuboidsAfterRemovingCuboid(Cuboid removed)
        {
            List<Cuboid> subcuboids = new List<Cuboid>();

            // Store moving boundaries, initiate with parent boundaries
            (int Hi, int Lo) x = X;
            (int Hi, int Lo) y = Y;
            (int Hi, int Lo) z = Z;

            // Check in x+
            if(removed.X.Hi < x.Hi)
            {
                Cuboid cuboid = new Cuboid((x.Hi, removed.X.Hi+1), y, z, On);
                subcuboids.Add(cuboid);
                x.Hi = removed.X.Hi;
            }

            // Check in x-
            if (removed.X.Lo > x.Lo)
            {
                Cuboid cuboid = new Cuboid((removed.X.Lo-1, x.Lo), y, z, On);
                subcuboids.Add(cuboid);
                x.Lo = removed.X.Lo;
            }

            // Check in y+
            if (removed.Y.Hi < y.Hi)
            {
                Cuboid cuboid = new Cuboid(x, (y.Hi, removed.Y.Hi+1), z, On);
                subcuboids.Add(cuboid);
                y.Hi = removed.Y.Hi;
            }

            // Check in y-
            if (removed.Y.Lo > y.Lo)
            {
                Cuboid cuboid = new Cuboid(x, (removed.Y.Lo-1, y.Lo), z, On);
                subcuboids.Add(cuboid);
                y.Lo = removed.Y.Lo;
            }

            // Check in z+
            if (removed.Z.Hi < z.Hi)
            {
                Cuboid cuboid = new Cuboid(x, y, (z.Hi, removed.Z.Hi+1), On);
                subcuboids.Add(cuboid);
                z.Hi = removed.Z.Hi;
            }

            // Check in z-
            if (removed.Z.Lo > z.Lo)
            {
                Cuboid cuboid = new Cuboid(x, y, (removed.Z.Lo-1, z.Lo), On);
                subcuboids.Add(cuboid);
                z.Lo = removed.Z.Lo;
            }

            // At this point, we've sliced off any un-intersected parts of the original cuboid.
            // The remaining boundaries are those of the removed cuboid.
            return subcuboids;
        }

        public bool ContainedByCuboid(Cuboid parent)
        {
            int planesContained = 0;

            if ((X.Hi >= parent.X.Lo && X.Hi <= parent.X.Hi) &&
                (X.Lo >= parent.X.Lo && X.Lo <= parent.X.Hi))
                planesContained++;

            if ((Y.Hi >= parent.Y.Lo && Y.Hi <= parent.Y.Hi) &&
                (Y.Lo >= parent.Y.Lo && Y.Lo <= parent.Y.Hi))
                planesContained++;

            if ((Z.Hi >= parent.Z.Lo && Z.Hi <= parent.Z.Hi) &&
                (Z.Lo >= parent.Z.Lo && Z.Lo <= parent.Z.Hi))
                planesContained++;

            // If all 3 planes are contained by the parent, this cuboid is a subset of the parent.
            // Note: boundaries are inclusive.
            return planesContained == 3;
        }

    }
}
