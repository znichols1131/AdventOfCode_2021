using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventChallenges
{
    public class Challenge_05
    {
        private List<Point> _points = new List<Point>();

        public void Challenge_A()
        {
            // Get all lines from file
            string path = @"C:\Users\Zach Nichols\ElevenFifty\Practice\2021Advent\2021AdventChallenges\Inputs\Challenge_05a_input.txt";
            List<string> listOfLines = System.IO.File.ReadAllLines(path).ToList<string>();

            // Mark the lines and points
            int count = 0;
            foreach (string line in listOfLines)
            {
                count++;
                Console.WriteLine("Line #" + count + " of " + listOfLines.Count);

                string[] pointStrings = line.Split((new string[] { " -> "}), StringSplitOptions.None);
                Point pointA = new Point(pointStrings[0]);
                Point pointB = new Point(pointStrings[1]);
                MarkLine(pointA, pointB);
            }

            // Find how many points have two or more "marks"
            // AKA: where at least two lines intersect
            List<Point> intersections = _points.FindAll(p => p.Count > 1);

            Console.WriteLine("Total number of intersections: " + intersections.Count);
            Console.ReadLine();
        }

        public void Challenge_B()
        {
            // Get all lines from file
            string path = @"C:\Users\Zach Nichols\ElevenFifty\Practice\2021Advent\2021AdventChallenges\Inputs\Challenge_05a_input.txt";
            List<string> listOfLines = System.IO.File.ReadAllLines(path).ToList<string>();

            // Mark the lines and points
            int count = 0;
            foreach (string line in listOfLines)
            {
                count++;
                Console.WriteLine("Line #" + count + " of " + listOfLines.Count);

                string[] pointStrings = line.Split((new string[] { " -> " }), StringSplitOptions.None);
                Point pointA = new Point(pointStrings[0]);
                Point pointB = new Point(pointStrings[1]);
                MarkLine(pointA, pointB);
            }

            // Find how many points have two or more "marks"
            // AKA: where at least two lines intersect
            List<Point> intersections = _points.FindAll(p => p.Count > 1);

            Console.WriteLine("Total number of intersections: " + intersections.Count);
            Console.ReadLine();
        }

        public void MarkLine(Point pointA, Point pointB)
        {
            if(pointA.X == pointB.X)
            {
                // Vertical line
                for(int y = Math.Min(pointA.Y, pointB.Y); y <= Math.Max(pointA.Y, pointB.Y); y++)
                {
                    MarkPointWithCoordinates(pointA.X, y);
                }

            }else if(pointA.Y == pointB.Y)
            {
                // Horizontal line
                for (int x = Math.Min(pointA.X, pointB.X); x <= Math.Max(pointA.X, pointB.X); x++)
                {
                    MarkPointWithCoordinates(x, pointA.Y);
                }
            }
        }

        public void MarkPointWithCoordinates(int x, int y)
        {
            // Try to find existing point
            Point existing = _points.Find(p => p.X == x && p.Y == y);

            if(existing is null)
            {
                // If it doesn't exist, add a new point
                _points.Add(new Point(x, y));
            }else
            {
                // If it does exist, increase the count
                existing.Count += 1;
            }
        }
        
    }

    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Count { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
            Count = 1;
        }

        public Point(string input)
        {
            try
            {
                string[] coordinates = input.Split(',');
                X = int.Parse(coordinates[0]);
                Y = int.Parse(coordinates[1]);
                Count = 1;
            }
            catch { }
        }
    }
}
