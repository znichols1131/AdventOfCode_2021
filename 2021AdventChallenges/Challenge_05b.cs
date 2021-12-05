using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventChallenges
{
    public class Challenge_05b
    {
        private List<Point> _points = new List<Point>();

        public void Run()
        {
            // Get all lines from file
            string path = @"C:\Users\Zach Nichols\ElevenFiftyProjects\Practice\2021AdventChallenges\2021AdventChallenges\Inputs\Challenge_05a_input.txt";
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
            if (pointA.X == pointB.X)
            {
                // Vertical line
                for (int y = Math.Min(pointA.Y, pointB.Y); y <= Math.Max(pointA.Y, pointB.Y); y++)
                {
                    MarkPointWithCoordinates(pointA.X, y);
                }

            }
            else if (pointA.Y == pointB.Y)
            {
                // Horizontal line
                for (int x = Math.Min(pointA.X, pointB.X); x <= Math.Max(pointA.X, pointB.X); x++)
                {
                    MarkPointWithCoordinates(x, pointA.Y);
                }
            }
            else
            {
                // Diagonal line (assume slope = 1 or -1
                int slope = (pointA.Y - pointB.Y) / (pointA.X - pointB.X);
                int startX = Math.Min(pointA.X, pointB.X);
                int startY = pointA.X == startX ? pointA.Y : pointB.Y;

                for (int x = startX; x <= Math.Max(pointA.X, pointB.X); x++)
                {
                    MarkPointWithCoordinates(x, startY + slope*(x-startX));
                }
            }
        }

        public void MarkPointWithCoordinates(int x, int y)
        {
            // Try to find existing point
            Point existing = _points.Find(p => p.X == x && p.Y == y);

            if (existing is null)
            {
                // If it doesn't exist, add a new point
                _points.Add(new Point(x, y));
            }
            else
            {
                // If it does exist, increase the count
                existing.Count += 1;
            }
        }

    }
}
