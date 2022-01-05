using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventChallenges
{
    public class Challenge_17
    {
        public enum TrajectoryResult
        {
            FellShort = 0,
            Skipped = 1,
            Overshot = 2,
            Success = 3,
            LostSpeed = 4
        }

        private int _trenchBottom = 0;
        private int _trenchTop = 0;
        private int _trenchLeft = 0;
        private int _trenchRight = 0;

        public void Challenge_A()
        {
            // Get line
            string filePath = Path.Combine(@"..\..\Inputs\", "Challenge_17a_input.txt");
            string input = System.IO.File.ReadAllLines(filePath).First();

            // Get trench coordinates relative to sub (0,0)
            input = input.Replace("target area: x=", "");
            input = input.Replace(" y=", "");
            input = input.Replace("..", ",");
            string[] inputs = input.Split(',');

            _trenchBottom = Math.Min(int.Parse(inputs[2]), int.Parse(inputs[3]));
            _trenchTop = Math.Max(int.Parse(inputs[2]), int.Parse(inputs[3]));
            _trenchLeft = Math.Min(int.Parse(inputs[0]), int.Parse(inputs[1]));
            _trenchRight = Math.Max(int.Parse(inputs[0]), int.Parse(inputs[1]));

            // Test all possible starting x velocities within reason.
            // Keep track of highest y value.
            int highestY = 0;
            int maxVX = _trenchRight;
            for(int vx = 1; vx <= maxVX; vx++)
            {
                // Test y velocities until failure.                
                bool hasOvershot = false;
                bool hasLostSpeed = false;
                int vy = 0;
                int skipCount = 0;
                do
                {
                    // Try next starting velocity
                    vy++;
                    // See if it was successful
                    int latestY;
                    TrajectoryResult result;
                    (result, latestY) = TestVelocity(vx, vy);

                    Console.WriteLine($"Trying ({vx},{vy}) = {latestY}");

                    // Was it successful
                    if (result == TrajectoryResult.Success)
                    {
                        // Update highest Y value if necessary
                        highestY = Math.Max(highestY, latestY);

                    }
                    else if (result == TrajectoryResult.Overshot)
                    {
                        hasOvershot = true;
                    }
                    else if (result == TrajectoryResult.LostSpeed)
                    {
                        hasLostSpeed = true;
                    }
                    else if (result == TrajectoryResult.Skipped)
                    {
                        skipCount++;
                    }

                } while (!hasOvershot && !hasLostSpeed && skipCount <= 1000);
            }

            // Output
            Console.WriteLine("\nHighest y: " + highestY);
            Console.ReadLine();
        }

        public void Challenge_B()
        {
            // Get line
            string filePath = Path.Combine(@"..\..\Inputs\", "Challenge_17a_input.txt");
            string input = System.IO.File.ReadAllLines(filePath).First();

            // Get trench coordinates relative to sub (0,0)
            input = input.Replace("target area: x=", "");
            input = input.Replace(" y=", "");
            input = input.Replace("..", ",");
            string[] inputs = input.Split(',');

            _trenchBottom = Math.Min(int.Parse(inputs[2]), int.Parse(inputs[3]));
            _trenchTop = Math.Max(int.Parse(inputs[2]), int.Parse(inputs[3]));
            _trenchLeft = Math.Min(int.Parse(inputs[0]), int.Parse(inputs[1]));
            _trenchRight = Math.Max(int.Parse(inputs[0]), int.Parse(inputs[1]));

            // Test all possible starting x velocities within reason.
            // Keep track of highest y value.
            List<(int, int)> successfulVelocities = new List<(int, int)>();
            int maxVX = _trenchRight;
            for (int vx = 1; vx <= maxVX; vx++)
            {
                // Test y velocities until failure.                
                bool hasOvershot = false;
                bool hasLostSpeed = false;
                int vy = -vx * (Math.Abs(_trenchBottom / _trenchLeft) +1);
                int skipCount = 0;
                do
                {
                    // Try next starting velocity
                    vy++;
                    // See if it was successful
                    int latestY;
                    TrajectoryResult result;
                    (result, latestY) = TestVelocity(vx, vy);

                    Console.WriteLine($"Trying ({vx},{vy}) = {latestY}");

                    // Was it successful
                    if (result == TrajectoryResult.Success)
                    {
                        // Add successful starting velocities to list
                        successfulVelocities.Add((vx, vy));
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"({vx},{ vy}) = success");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (result == TrajectoryResult.Overshot)
                    {
                        hasOvershot = true;
                    }
                    else if (result == TrajectoryResult.LostSpeed)
                    {
                        hasLostSpeed = true;
                    }
                    else if (result == TrajectoryResult.Skipped)
                    {
                        skipCount++;
                    }

                } while (!hasOvershot && !hasLostSpeed && skipCount <= 1000);
            }

            // Output
            Console.WriteLine("\nNumber of unique velocities: " + successfulVelocities.Count);
            Console.ReadLine();
        }

        public (TrajectoryResult, int) TestVelocity(int vx, int vy)
        {
            // Keep track of location
            int x = 0;
            int y = 0;
            int highestY = 0;

            // Fire projectile
            while (true)
            {
                // Update position
                x += vx;
                y += vy;
                highestY = Math.Max(highestY, y);

                // Update velocities
                vy--;
                if (vx > 0)
                {
                    vx--;
                }
                else if (vx < 0)
                {
                    vx++;
                }

                // Check if it fell short
                if (x < _trenchLeft && y < _trenchBottom)
                {
                    if(vx == 0)
                    {
                        return (TrajectoryResult.LostSpeed, 0);
                    }
                    else
                    {
                        return (TrajectoryResult.FellShort, 0);
                    }
                }

                // Check if it overshot
                if (x > _trenchRight)
                    return (TrajectoryResult.Overshot, 0);

                // Check if it skipped over the target zone
                if (x >= _trenchLeft && x <= _trenchRight && y < _trenchBottom)
                    return (TrajectoryResult.Skipped, 0);

                // Check if success
                if (x >= _trenchLeft && x <= _trenchRight && y >= _trenchBottom && y <= _trenchTop)
                    return (TrajectoryResult.Success, highestY);
            }
        }


    }
}
