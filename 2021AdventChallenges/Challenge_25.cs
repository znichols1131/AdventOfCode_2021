using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventChallenges
{
    public class Challenge_25
    {
        private List<List<SeaCucumber>> _map = new List<List<SeaCucumber>>();

        public void Challenge_A()
        {
            // Get input
            string path = @"C:\Users\Zach Nichols\ElevenFifty\Practice\2021Advent\2021AdventChallenges\Inputs\Challenge_25a_input.txt";
            string[] lines = System.IO.File.ReadAllLines(path);

            // Get sea cucumbers
            for(int row = 0; row < lines.Count(); row++)
            {
                List<SeaCucumber> rowList = new List<SeaCucumber>();
                for(int col = 0; col < lines[row].Length; col++)
                {
                    switch(lines[row].ToLower()[col])
                    {
                        case '>':
                            rowList.Add(new SeaCucumber(row, col, SeaCucumber.CucumberType.EastFacing));
                            break;
                        case 'v':
                            rowList.Add(new SeaCucumber(row, col, SeaCucumber.CucumberType.SouthFacing));
                            break;
                        case '.':
                            rowList.Add(new SeaCucumber(row, col, SeaCucumber.CucumberType.Empty));
                            break;
                        default:
                            break;
                    }
                }
                _map.Add(rowList);
            }

            // Move sea cucumbers
            int steps = 0;
            do
            {
                steps++;
            } while (AttemptMigration());


            // Output
            PrintMap();
            Console.WriteLine($"\nNumber of steps: {steps}");
            Console.ReadLine();

        }

        public void Challenge_B()
        {
            // Requires all other days/challenges to be complete
        }

        public bool AttemptMigration()
        {
            // Check which east-facing ones can move
            bool eastCanMove = PrepareCucumbers(SeaCucumber.CucumberType.EastFacing);

            // Move east-facing ones
            if(eastCanMove)
                MoveCucumbers(SeaCucumber.CucumberType.EastFacing);

            // Check which south-facing ones can move
            bool southCanMove = PrepareCucumbers(SeaCucumber.CucumberType.SouthFacing);

            // Move south-facing ones
            if (southCanMove)
                MoveCucumbers(SeaCucumber.CucumberType.SouthFacing);

            // Return if any were able to move
            return eastCanMove || southCanMove;
        }

        public bool PrepareCucumbers(SeaCucumber.CucumberType type)
        {
            bool SomeCanMove = false;
            foreach(var row in _map)
            {
                foreach(var sc in row.Where(c => c.CType == type))
                {
                    sc.WillMove = CanMove(sc);
                    SomeCanMove = sc.WillMove || SomeCanMove;
                }
            }

            return SomeCanMove;
        }

        public void MoveCucumbers(SeaCucumber.CucumberType type)
        {
            foreach (var row in _map)
            {
                foreach (var sc in row.Where(c => c.CType == type && c.WillMove == true))
                {
                    if(type == SeaCucumber.CucumberType.EastFacing)
                    {
                        // Move right

                        // Check if the sea cucumber will jump to the left side
                        int targetCol = sc.Col + 1;
                        if (targetCol >= row.Count)
                            targetCol = 0;

                        // Set destination
                        var target = _map[sc.Row][targetCol];
                        target.CType = sc.CType;
                        target.WillMove = false;

                        // Empty the current space
                        sc.CType = SeaCucumber.CucumberType.Empty;
                        target.WillMove = false;

                    }
                    else if(type == SeaCucumber.CucumberType.SouthFacing)
                    {
                        // Move down

                        // Check if the sea cucumber will jump to the top
                        int targetRow = sc.Row + 1;
                        if (targetRow >= _map.Count)
                            targetRow = 0;

                        // Set destination
                        var target = _map[targetRow][sc.Col];
                        target.CType = sc.CType;
                        target.WillMove = false;

                        // Empty the current space
                        sc.CType = SeaCucumber.CucumberType.Empty;
                        target.WillMove = false;
                    }
                }
            }
        }

        public bool CanMove(SeaCucumber sc)
        {
            if(sc.CType == SeaCucumber.CucumberType.EastFacing)
            {
                // Check to the right
                if(sc.Col+1 >= _map[sc.Row].Count)
                {
                    // The cucumber will reappear on the left side at an empty space.
                    return _map[sc.Row][0].CType == SeaCucumber.CucumberType.Empty;
                }
                else
                {
                    // The cucumber can move right to an empty space.
                    return _map[sc.Row][sc.Col + 1].CType == SeaCucumber.CucumberType.Empty;
                }
            }
            else if (sc.CType == SeaCucumber.CucumberType.SouthFacing)
            {
                // Check down/south
                if (sc.Row + 1 >= _map.Count)
                {
                    // The cucumber will reappear at the top at an empty space.
                    return _map[0][sc.Col].CType == SeaCucumber.CucumberType.Empty;
                }
                else
                {
                    // The cucumber can move down/south to an empty space.
                    return _map[sc.Row + 1][sc.Col].CType == SeaCucumber.CucumberType.Empty;
                }
            }

            return false;
        }

        public void PrintMap()
        {
            foreach(var row in _map)
            {
                foreach(var sc in row)
                {
                    switch(sc.CType)
                    {
                        case SeaCucumber.CucumberType.Empty:
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.Write(".");
                            break;
                        case SeaCucumber.CucumberType.EastFacing:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(">");
                            break;
                        case SeaCucumber.CucumberType.SouthFacing:
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("v");
                            break;
                        default:
                            break;
                    }
                }

                Console.WriteLine();
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        public class SeaCucumber
        {
            public int Row { get; set; }
            public int Col { get; set; }
            public CucumberType CType { get; set; }
            public bool WillMove { get; set; }

            public SeaCucumber() { }
            public SeaCucumber(int row, int col)
            {
                Row = row;
                Col = col;
            }
            public SeaCucumber(int row, int col, CucumberType cType)
            {
                Row = row;
                Col = col;
                CType = cType;
            }

            public void MoveTo(int row, int col)
            {
                Row = row;
                Col = col;
                WillMove = false;
            }

            public enum CucumberType
            {
                Empty = 0,
                EastFacing = 1,
                SouthFacing = 2
            }
        }
    }
}
