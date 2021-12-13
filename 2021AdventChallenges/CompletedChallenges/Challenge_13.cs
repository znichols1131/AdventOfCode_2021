using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventChallenges
{
    public class Challenge_13
    {
        private List<Point_13> _points = new List<Point_13>();
        private int _maxX = 0;
        private int _maxY = 0;
        public void Challenge_A()
        {
            // Get lines
            string path = @"C:\Users\Zach Nichols\ElevenFifty\Practice\2021Advent\2021AdventChallenges\Inputs\Challenge_13a_input.txt";
            List<string> lines = System.IO.File.ReadAllLines(path).ToList<string>();

            // Set up list of points
            foreach (string line in lines)
            {
                if (!(line is null || line == "" || line.StartsWith("fold")))
                {
                    string[] coordinates = line.Split(',');
                    int x = int.Parse(coordinates[0]);
                    int y = int.Parse(coordinates[1]);

                    if (x > _maxX)
                        _maxX = x;

                    if (y > _maxY)
                        _maxY = y;

                    _points.Add(new Point_13(x, y));
                }
            }

            // Follow the first instruction
            foreach (string line in lines)
            {
                if (line.StartsWith("fold"))
                {
                    PerformInstruction(line);

                    _maxX = 0;
                    _maxY = 0;
                    
                    // Get latest boundaries
                    foreach (Point_13 p in _points)
                    {
                        if (p.X > _maxX)
                            _maxX = p.X;

                        if (p.Y > _maxY)
                            _maxY = p.Y;
                    }

                    // Output right away for Challenge A (not more instructions)
                    //PrintPoints();
                    Console.WriteLine("Visible dots: " + _points.Count);
                    Console.ReadLine();
                    return;
                }
            }
        }

        public void Challenge_B()
        {
            // Get lines
            string path = @"C:\Users\Zach Nichols\ElevenFifty\Practice\2021Advent\2021AdventChallenges\Inputs\Challenge_13a_input.txt";
            List<string> lines = System.IO.File.ReadAllLines(path).ToList<string>();

            // Set up list of points
            foreach (string line in lines)
            {
                if (!(line is null || line == "" || line.StartsWith("fold")))
                {
                    string[] coordinates = line.Split(',');
                    int x = int.Parse(coordinates[0]);
                    int y = int.Parse(coordinates[1]);

                    if (x > _maxX)
                        _maxX = x;

                    if (y > _maxY)
                        _maxY = y;

                    _points.Add(new Point_13(x, y));
                }
            }

            // Follow the first instruction
            foreach (string line in lines)
            {
                if (line.StartsWith("fold"))
                {
                    PerformInstruction(line);                                    
                }
            }

            // Get latest boundaries
            Transpose();
            
            _maxX = 0;
            _maxY = 0;
            foreach (Point_13 p in _points)
            {
                if (p.X > _maxX)
                    _maxX = p.X;

                if (p.Y > _maxY)
                    _maxY = p.Y;
            }

            // Output
            PrintPoints();
            Console.ReadLine();
        }

        public void PerformInstruction(string instruction)
        {
            string[] parts = instruction.Split('=');

            int number = int.Parse(parts[1]);

            if (parts[0].EndsWith("x"))
            {
                // Vertical line, fold to the left
                Fold(number, false);
            }
            else if (parts[0].EndsWith("y"))
            {
                // Horizontal line, fold up
                Fold(number, true);
            }
        }

        public void Fold(int value, bool shouldFoldUp)
        {
            // Use revserse for loop since we might be removing points as we go (count changes)
            for(int i = _points.Count - 1; i >= 0; i--)
            {
                Point_13 p = _points[i];

                // Horizontal line, fold up
                if (shouldFoldUp && p.Y > value)
                    MovePoint(p, p.X, value - (p.Y - value));

                // Vertical line, fold to the left
                if (!shouldFoldUp && p.X > value)
                    MovePoint(p, value - (p.X - value), p.Y);
            }
        }

        public void MovePoint(Point_13 p, int targetX, int targetY)
        {
            // Check if that point already exists
            Point_13 existingPoint = _points.Find(pt => pt.X == targetX && pt.Y == targetY);
            if(existingPoint is null)
            {
                // Space was blank, move point
                p.X = targetX;
                p.Y = targetY;
            }
            else
            {
                // Space is occupied, remove duplicate
                _points.Remove(p);
            }
        }

        public void Transpose()
        {
            List <Point_13> newPoints = new List<Point_13>();
            foreach(Point_13 p in _points)
            {
                newPoints.Add(new Point_13(p.Y, p.X));
            }

            _points.Clear();
            _points.AddRange(newPoints);
        }

        public void PrintPoints()
        {
            // Create an array to hold the dots
            //bool[,] dots = new bool[_maxX + 1, _maxY + 1];
            
            for(int row = 0; row < _maxX + 1; row++)
            {
                for(int col = 0; col < _maxY + 1; col++)
                {
                    Point_13 existingPoint = _points.Find(pt => pt.X == row && pt.Y == col);
                    if(existingPoint is null)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(".");
                    }else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("X");
                    }
                }
                Console.WriteLine();
            }
        }
    }

    public class Point_13
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point_13() { }
        public Point_13(int x, int y)
        {
            X = x;
            Y = y;
        }

    }
}
