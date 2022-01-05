using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventChallenges
{
    public class Challenge_07
    {
        public void Challenge_A()
        {
            // Get all lines from file
            string filePath = Path.Combine(@"..\..\Inputs\", "Challenge_07a_input.txt");
            List<string> lines = System.IO.File.ReadAllLines(filePath).ToList<string>();

            // Get crab positions
            List<int> positions = new List<int>();
            foreach (string pos in lines[0].Split(','))
            {
                positions.Add(int.Parse(pos));
            }

            // Loop through fuel counts (starting at average position)
            int previousFuelCount = int.MaxValue;
            int maxIndex = positions.Count;
            int minIndex = -1;
            int target = positions.Sum() / positions.Count;
            bool successful = false;
            FuelCountState state = FuelCountState.Starting;
            while (!successful)
            {
                int newFuelCount = FuelCountForTarget(target, positions);
                Console.ForegroundColor = newFuelCount > previousFuelCount ? ConsoleColor.Red : ConsoleColor.Green;
                Console.WriteLine("Position: " + target + ", fuel: " + newFuelCount);

                switch (state)
                {
                    case FuelCountState.Starting:

                        if(target + 1 < maxIndex)
                        {
                            // Make sure we don't exceed max index
                            target++;
                            state = FuelCountState.Increasing;

                        }else if (target - 1 > minIndex)
                        {
                            // Make sure we don't hit the min index
                            target--;
                            state = FuelCountState.Decreasing;
                        }
                        else
                        {
                            // Array only has one item
                            successful = true;
                        }
                        break;

                    case FuelCountState.Increasing:

                        if(newFuelCount > previousFuelCount)
                        {
                            // Heading the wrong direction
                            maxIndex = target;

                            if (target - 1 > minIndex)
                            {
                                // Make sure we don't hit the min index
                                target--;
                                state = FuelCountState.Decreasing;
                            }
                            else
                            {
                                target = minIndex;
                                successful = true;
                            }
                        }
                        else
                        {
                            // Heading the right direction
                            minIndex = target;

                            if (target + 1 < maxIndex)
                            {
                                // Make sure we don't exceed max index
                                target++;
                                state = FuelCountState.Increasing;

                            }
                            else
                            {
                                successful = true;
                            }
                        }

                        break;

                    case FuelCountState.Decreasing:

                        if (newFuelCount > previousFuelCount)
                        {
                            // Heading the wrong direction
                            minIndex = target;

                            if (target + 1 < maxIndex)
                            {
                                // Make sure we don't exceed max index
                                target++;
                                state = FuelCountState.Increasing;

                            }
                            else
                            {
                                target = maxIndex;
                                successful = true;
                            }
                        }
                        else
                        {
                            // Heading the right direction
                            maxIndex = target;

                            if (target - 1 > minIndex)
                            {
                                // Make sure we don't hit the min index
                                target--;
                                state = FuelCountState.Decreasing;
                            }
                            else
                            {
                                successful = true;
                            }
                        }

                        break;

                    default:
                        break;
                }

                if(!successful)
                    previousFuelCount = newFuelCount;
            }

            // Output
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nPosition: " + target);
            Console.WriteLine("Fuel used: " + previousFuelCount);
            Console.ReadLine();
        }

        public void Challenge_B()
        {
            // Get all lines from file
            string filePath = Path.Combine(@"..\..\Inputs\", "Challenge_07a_input.txt");
            List<string> lines = System.IO.File.ReadAllLines(filePath).ToList<string>();

            // Get crab positions
            List<int> positions = new List<int>();
            foreach (string pos in lines[0].Split(','))
            {
                positions.Add(int.Parse(pos));
            }

            // Loop through fuel counts (starting at average position)
            int previousFuelCount = int.MaxValue;
            int maxIndex = positions.Count;
            int minIndex = -1;
            int target = positions.Sum() / positions.Count;
            bool successful = false;
            FuelCountState state = FuelCountState.Starting;
            while (!successful)
            {
                int newFuelCount = ExpFuelCountForTarget(target, positions);
                Console.ForegroundColor = newFuelCount > previousFuelCount ? ConsoleColor.Red : ConsoleColor.Green;
                Console.WriteLine("Position: " + target + ", fuel: " + newFuelCount);

                switch (state)
                {
                    case FuelCountState.Starting:

                        if (target + 1 < maxIndex)
                        {
                            // Make sure we don't exceed max index
                            target++;
                            state = FuelCountState.Increasing;

                        }
                        else if (target - 1 > minIndex)
                        {
                            // Make sure we don't hit the min index
                            target--;
                            state = FuelCountState.Decreasing;
                        }
                        else
                        {
                            // Array only has one item
                            successful = true;
                        }
                        break;

                    case FuelCountState.Increasing:

                        if (newFuelCount > previousFuelCount)
                        {
                            // Heading the wrong direction
                            maxIndex = target;

                            if (target - 1 > minIndex)
                            {
                                // Make sure we don't hit the min index
                                target--;
                                state = FuelCountState.Decreasing;
                            }
                            else
                            {
                                target = minIndex;
                                successful = true;
                            }
                        }
                        else
                        {
                            // Heading the right direction
                            minIndex = target;

                            if (target + 1 < maxIndex)
                            {
                                // Make sure we don't exceed max index
                                target++;
                                state = FuelCountState.Increasing;

                            }
                            else
                            {
                                successful = true;
                            }
                        }

                        break;

                    case FuelCountState.Decreasing:

                        if (newFuelCount > previousFuelCount)
                        {
                            // Heading the wrong direction
                            minIndex = target;

                            if (target + 1 < maxIndex)
                            {
                                // Make sure we don't exceed max index
                                target++;
                                state = FuelCountState.Increasing;

                            }
                            else
                            {
                                target = maxIndex;
                                successful = true;
                            }
                        }
                        else
                        {
                            // Heading the right direction
                            maxIndex = target;

                            if (target - 1 > minIndex)
                            {
                                // Make sure we don't hit the min index
                                target--;
                                state = FuelCountState.Decreasing;
                            }
                            else
                            {
                                successful = true;
                            }
                        }

                        break;

                    default:
                        break;
                }

                if (!successful)
                    previousFuelCount = newFuelCount;
            }

            // Output
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nPosition: " + target);
            Console.WriteLine("Fuel used: " + previousFuelCount);
            Console.ReadLine();
        }

        public int FuelCountForTarget(int target, List<int> positions)
        {
            int fuel = 0;

            if (positions.Count == 0)
                return fuel;

            foreach (int pos in positions)
            {
                fuel += Math.Abs(pos - target);
            }

            return fuel;
        }

        public int ExpFuelCountForTarget(int target, List<int> positions)
        {
            int fuel = 0;

            if (positions.Count == 0)
                return fuel;

            foreach (int pos in positions)
            {
                for (int i = 1; i <= Math.Abs(pos - target); i++)
                {
                    fuel += i;
                }
            }

            return fuel;
        }

        public enum FuelCountState
        {
            Starting = 0,
            Increasing = 1,
            Decreasing = 2
        }
    }
}
