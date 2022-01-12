using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventChallenges
{
    public class Challenge_23
    {
        private int _hallwayRow = -1;
        private int _hallwayMinCol = -1;
        private int _hallwayMaxCol = -1;

        private List<string> _bestMap = new List<string>();
        private List<Amphipod> _bestAmphipods = new List<Amphipod>();
        private List<string> _bestListOfMoves = new List<string>();
        private int _lowestEnergy = int.MaxValue;
        
        private int _iterationCount = 0;

        private Dictionary<char, int> _targetRooms = new Dictionary<char, int>();


        public void Challenge_A()
        {
            // Get input
            string filePath = Path.Combine(@"..\..\Inputs\", "Challenge_23a_input.txt");
            List<string> inputMap = System.IO.File.ReadAllLines(filePath).ToList<string>();
            List<Amphipod> amphipods = new List<Amphipod>();

            // Set up dictionary
            _targetRooms.Add('a', 3);
            _targetRooms.Add('b', 5);
            _targetRooms.Add('c', 7);
            _targetRooms.Add('d', 9);

            // Set up board
            for(int row = 0; row < inputMap.Count; row++)
            {
                if (inputMap[row].Contains(".."))
                    _hallwayRow = row;

                for(int col = 0; col < inputMap[row].Length; col++)
                {
                    if(row == _hallwayRow && inputMap[row][col] == '.')
                    {
                        // Hallway space
                        if (_hallwayMinCol < 0)
                            _hallwayMinCol = col;

                        if (_hallwayMaxCol < col)
                            _hallwayMaxCol = col;

                        // Replace '.' with ',' above rooms
                        if ("ABCDabcd".Contains(inputMap[row+1][col]))
                            inputMap[row] = inputMap[row].Remove(col, 1).Insert(col, ",");

                    }
                    else if ("ABCDabcd".Contains(inputMap[row][col]))
                    {
                        // Amphipod

                        // Create new amphipod, add to bottom of the room
                        Amphipod amphipod = new Amphipod(Char.ToLower(inputMap[row][col]), (row, col));
                        amphipod.CalculateHeuristicForMap(inputMap, _targetRooms);
                        amphipods.Add(amphipod);
                    }
                }
            }

            // Print map for user to see
            PrintMap(inputMap);
            Console.WriteLine();

            // Try to find solution
            Iterate(inputMap, amphipods, new List<string>());

            // Output
            Console.Clear();
            Console.WriteLine("Original map:\n");
            PrintMap(inputMap);
            
            Console.WriteLine($"\n\nMost efficient solution:\nEnergy used: {_lowestEnergy}\n");
            PrintInstructions(_bestListOfMoves);
            Console.WriteLine();
            PrintMap(_bestMap);
            Console.ReadLine();
        }

        public void Challenge_B()
        {
            // Get input
            string filePath = Path.Combine(@"..\..\Inputs\", "Challenge_23b_input.txt");
            List<string> inputMap = System.IO.File.ReadAllLines(filePath).ToList<string>();
            List<Amphipod> amphipods = new List<Amphipod>();

            // Set up dictionary
            _targetRooms.Add('a', 3);
            _targetRooms.Add('b', 5);
            _targetRooms.Add('c', 7);
            _targetRooms.Add('d', 9);

            // Set up board
            for (int row = 0; row < inputMap.Count; row++)
            {
                if (inputMap[row].Contains(".."))
                    _hallwayRow = row;

                for (int col = 0; col < inputMap[row].Length; col++)
                {
                    if (row == _hallwayRow && inputMap[row][col] == '.')
                    {
                        // Hallway space
                        if (_hallwayMinCol < 0)
                            _hallwayMinCol = col;

                        if (_hallwayMaxCol < col)
                            _hallwayMaxCol = col;

                        // Replace '.' with ',' above rooms
                        if ("ABCDabcd".Contains(inputMap[row + 1][col]))
                            inputMap[row] = inputMap[row].Remove(col, 1).Insert(col, ",");

                    }
                    else if ("ABCDabcd".Contains(inputMap[row][col]))
                    {
                        // Amphipod

                        // Create new amphipod, add to bottom of the room
                        Amphipod amphipod = new Amphipod(Char.ToLower(inputMap[row][col]), (row, col));
                        amphipod.CalculateHeuristicForMap(inputMap, _targetRooms);
                        amphipods.Add(amphipod);
                    }
                }
            }

            // Print map for user to see
            PrintMap(inputMap);
            Console.WriteLine();

            // Try to find solution
            Iterate(inputMap, amphipods, new List<string>());

            // Output
            Console.Clear();
            Console.WriteLine("Original map:\n");
            PrintMap(inputMap);

            Console.WriteLine($"\n\nMost efficient solution:\nEnergy used: {_lowestEnergy}\n");
            PrintInstructions(_bestListOfMoves);
            Console.WriteLine();
            PrintMap(_bestMap);
            Console.ReadLine();
        }

        private void Iterate(List<string> map, List<Amphipod> amphipods, List<string> previousMoves)
        {
            _iterationCount++;
            if (_iterationCount % 100000 == 0)
            {
                Console.WriteLine($"Now performing iteration {_iterationCount}");
                //if (_iterationCount / _iterationFactor > 20)
                //    _iterationFactor *= 10;
            }

            // Check if it's already failed
            if (amphipods.Sum(a => a.TotalEnergy) > _lowestEnergy)
                return;

            //PrintMap(map)

            // Check if successful
            if (!amphipods.Any(a => a.Location.Row == _hallwayRow))
            {
                // None are in the hallway, now check if any are in mismatched room
                bool success = true;
                foreach(Amphipod a in amphipods)
                {
                    int r = a.Location.Row;
                    int c = a.Location.Col;
                    char above = char.ToLower(map[r - 1][c]);
                    char below = char.ToLower(map[r + 1][c]);
                    string acceptable = ",#" + char.ToLower(a.AmphipodType);

                    // Will remain true if the spaces above and below are A) empty corridor, B) walls, or C) the same amphipod type
                    success = success && acceptable.Contains(above) && acceptable.Contains(below);
                }

                // Successful!
                if(success)
                {
                    int energyUsed = amphipods.Sum(a => a.EnergyUsed);

                    if (energyUsed < _lowestEnergy)
                    {
                        _bestMap = map;
                        _bestAmphipods = amphipods;
                        _lowestEnergy = energyUsed;
                        _bestListOfMoves = previousMoves;

                        Console.Clear();
                        Console.WriteLine($"Solution found using {_lowestEnergy} energy:");
                        PrintMap(_bestMap);
                        Console.WriteLine();
                    }                    
                    return;
                }
            }            

            // Consider all moves for each amphipod
            List<(List<string>, List<Amphipod>, List<string>)> iterationsToTry = new List<(List<string>, List<Amphipod>,List<string>)>();
            foreach(Amphipod amphipod in amphipods)
            {
                if(amphipod.Location.Col == _targetRooms[amphipod.AmphipodType] && !IsTrappingSomeoneElse(amphipod, map))
                {
                    // If amphipod is in the correct room and isn't trapping anything else. It can stay!
                    continue;
                }


                else if ((amphipod.Location.Row != _hallwayRow && amphipod.Location.Col != _targetRooms[amphipod.AmphipodType]) ||
                    (amphipod.Location.Col == _targetRooms[amphipod.AmphipodType] && IsTrappingSomeoneElse(amphipod, map)))
                {
                    // If amphipod is in the incorrect room or needs to move to free someone else

                    // Try to move to correct room if possible
                    MoveToCorrectRoomIfPossible(amphipod, map, amphipods, previousMoves);

                    // Check above amphipod to make sure we aren't trapped in a room
                    if (!CanLeaveRoom(amphipod, map))
                        goto Trapped_In_Room;

                    // Get all available spaces in corridor that don't involve going through another amphipod or stopping above a room
                    List<int> targetColumns = new List<int>();

                    // Check to right
                    for (int c = amphipod.Location.Col; c <= _hallwayMaxCol; c++)
                    {
                        if (map[_hallwayRow][c] == '.')
                            targetColumns.Add(c);

                        if ("ABCDabcd".Contains(map[_hallwayRow][c]))
                            break;
                    }

                    // Check to left
                    for (int c = amphipod.Location.Col; c >= _hallwayMinCol; c--)
                    {
                        if (map[_hallwayRow][c] == '.')
                            targetColumns.Add(c);

                        if ("ABCDabcd".Contains(map[_hallwayRow][c]))
                            break;
                    }

                    // Try all possible moves from here
                    foreach(int c in targetColumns)
                    {
                        string instruction = $"{amphipod.AmphipodType}\tat ({amphipod.Location.Row},{amphipod.Location.Col})\tmoves to ({_hallwayRow},{c})";

                        // Get new map
                        List<string> newMap = new List<string>(map);
                        newMap[_hallwayRow] = newMap[_hallwayRow].Remove(c, 1).Insert(c, amphipod.AmphipodType.ToString());
                        newMap[amphipod.Location.Row] = newMap[amphipod.Location.Row].Remove(amphipod.Location.Col, 1).Insert(amphipod.Location.Col, ".");

                        // Get new list of amphipods
                        List<Amphipod> newAmphipods = new List<Amphipod>();
                        foreach(Amphipod aOld in amphipods)
                        {
                            if (aOld != amphipod)
                            {
                                Amphipod aNew = new Amphipod(aOld.AmphipodType, aOld.Location);
                                aNew.EnergyUsed = aOld.EnergyUsed;
                                aNew.MinEnergyExpectedToUse = aOld.MinEnergyExpectedToUse;
                                newAmphipods.Add(aNew);
                            }
                            else
                            {
                                Amphipod aNew = new Amphipod(amphipod.AmphipodType, (_hallwayRow, c));
                                aNew.EnergyUsed = amphipod.EnergyUsed;
                                aNew.Move(Math.Abs(c - amphipod.Location.Col) + Math.Abs(amphipod.Location.Row - _hallwayRow), newMap, _targetRooms);
                                newAmphipods.Add(aNew);
                            }
                        }

                        List<string> newMoves = new List<string>(previousMoves);
                        newMoves.Add(instruction);
                        iterationsToTry.Add((newMap, newAmphipods, newMoves));
                    }

                Trapped_In_Room:;
                }


                else if (amphipod.Location.Row == _hallwayRow && RoomIsAvailableForType(amphipod.AmphipodType, _targetRooms[amphipod.AmphipodType], map))
                {
                    // If amphipod is in corridor, move to correct room.
                    MoveToCorrectRoomIfPossible(amphipod, map, amphipods, previousMoves);
                }
            }

            // Now recursively enter those in the most promising order
            while(iterationsToTry.Any())
            {
                iterationsToTry = iterationsToTry.OrderBy(i => i.Item2.Sum(a => a.TotalEnergy)).ToList();
                List<string> newMap = iterationsToTry.First().Item1;
                List<Amphipod> newAmphipods = iterationsToTry.First().Item2;
                List<string> newMoves = iterationsToTry.First().Item3;
                iterationsToTry.RemoveAt(0);
                Iterate(newMap, newAmphipods, newMoves);
            }
        }

        private void MoveToCorrectRoomIfPossible(Amphipod amphipod, List<string> map, List<Amphipod> amphipods, List<string> previousMoves)
        {
            // Get all available rooms to move to that don't involve going through another amphipod or stopping in a room occupied by a different amphipod type
            int targetCol = _targetRooms[char.ToLower(amphipod.AmphipodType)];

            // Check if room is available
            if (!RoomIsAvailableForType(amphipod.AmphipodType, targetCol, map))
                return;

            // Check for any obstructions
            bool noObstacles = CanLeaveRoom(amphipod, map);
            if(targetCol > amphipod.Location.Col)
            {
                // Move right
                for(int c = amphipod.Location.Col + 1; c <= targetCol; c++)
                {
                    noObstacles = noObstacles && !"ABCDabcd".Contains(map[_hallwayRow][c]);
                }
            }else
            {
                // Move left
                for (int c = amphipod.Location.Col - 1; c >= targetCol; c--)
                {
                    noObstacles = noObstacles && !"ABCDabcd".Contains(map[_hallwayRow][c]);
                }
            }

            if (!noObstacles)
                return;

            // Move to lowest spot in target room
            int targetRow = BottomOfRoom(targetCol, map);
            if (targetRow < 1)
                return;

            string instruction = $"{amphipod.AmphipodType}\tat ({amphipod.Location.Row},{amphipod.Location.Col})\tmoves to room {targetCol}";

            // Get new map
            List<string> newMap = new List<string>(map);
            newMap[targetRow] = newMap[targetRow].Remove(targetCol, 1).Insert(targetCol, amphipod.AmphipodType.ToString());
            newMap[amphipod.Location.Row] = newMap[amphipod.Location.Row].Remove(amphipod.Location.Col, 1).Insert(amphipod.Location.Col, ".");

            // Get new list of amphipods
            List<Amphipod> newAmphipods = new List<Amphipod>();
            foreach (Amphipod aOld in amphipods)
            {
                if (aOld != amphipod)
                {
                    Amphipod aNew = new Amphipod(aOld.AmphipodType, aOld.Location);
                    aNew.EnergyUsed = aOld.EnergyUsed;
                    aNew.MinEnergyExpectedToUse = aOld.MinEnergyExpectedToUse;
                    newAmphipods.Add(aNew);
                }
                else
                {
                    Amphipod aNew = new Amphipod(amphipod.AmphipodType, (targetRow, targetCol));
                    aNew.EnergyUsed = amphipod.EnergyUsed;
                    aNew.Move(Math.Abs(targetCol - amphipod.Location.Col) + Math.Abs(targetRow - _hallwayRow) + Math.Abs(_hallwayRow - amphipod.Location.Row), newMap, _targetRooms);
                    newAmphipods.Add(aNew);
                }
            }

            List<string> newMoves = new List<string>(previousMoves);
            newMoves.Add(instruction);

            Iterate(newMap, newAmphipods, newMoves);
        }

        public void PrintMap(List<string> map)
        {
            for(int row = 0; row < map.Count; row++)
            {
                for(int col = 0; col < map[row].Length; col++)
                {
                    switch(char.ToLower(map[row][col]))
                    {
                        case '.': Console.ForegroundColor = ConsoleColor.White; break;
                        case ',': Console.ForegroundColor = ConsoleColor.White; break;
                        case '#': Console.ForegroundColor = ConsoleColor.DarkGray; break;
                        case 'a': Console.ForegroundColor = ConsoleColor.Red; break;
                        case 'b': Console.ForegroundColor = ConsoleColor.Green; break;
                        case 'c': Console.ForegroundColor = ConsoleColor.Cyan; break;
                        case 'd': Console.ForegroundColor = ConsoleColor.Yellow; break;
                        default: break;
                    }
                    Console.Write(char.ToUpper(map[row][col]));
                }
                Console.WriteLine();
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        public void PrintInstructions(List<string> instructions)
        {
            for (int i = 0; i < instructions.Count; i++)
            {
                switch (char.ToLower(instructions[i][0]))
                {
                    case 'a': Console.ForegroundColor = ConsoleColor.Red; break;
                    case 'b': Console.ForegroundColor = ConsoleColor.Green; break;
                    case 'c': Console.ForegroundColor = ConsoleColor.Cyan; break;
                    case 'd': Console.ForegroundColor = ConsoleColor.Yellow; break;
                    default: break;
                }
                Console.WriteLine(instructions[i]);
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        private bool CanLeaveRoom(Amphipod amphipod, List<string> map)
        {
            string acceptable = ".,";

            for(int row = amphipod.Location.Row - 1; row > _hallwayRow; row--)
            {
                if (!acceptable.Contains(map[row][amphipod.Location.Col]))
                    return false;   // Trapped
            }

            return true;
        }

        private bool IsTrappingSomeoneElse(Amphipod amphipod, List<string> map)
        {
            string acceptable = ".#" + amphipod.AmphipodType;

            for (int row = amphipod.Location.Row+1; row < map.Count; row++)
            {
                if (!acceptable.Contains(map[row][amphipod.Location.Col]))
                    return true;   // Trapping something else
            }

            return false;
        }

        public bool RoomIsAvailableForType(char amphipodType, int column, List<string> map)
        {
            string acceptable = ".#," + char.ToUpper(amphipodType) + char.ToLower(amphipodType);

            for (int row = _hallwayRow + 1; row < map.Count; row++)
            {
                if (!acceptable.Contains(map[row][column]))
                    return false;
            }

            return true;
        }

        public int BottomOfRoom(int column, List<string> map)
        {
            for (int row = map.Count - 1; row > _hallwayRow; row--)
            {
                if (map[row][column] == '.')
                    return row;
            }

            return -1;
        }

        private class Amphipod
        {
            public (int Row, int Col) Location { get; set; }
            public char AmphipodType { get; set; }
            public int EnergyUsed { get; set; }
            public int MinEnergyExpectedToUse { get; set; }
            public int TotalEnergy { get { return EnergyUsed + MinEnergyExpectedToUse; } }
            private int _energyPerMove { get; set; }

            // Constructors
            public Amphipod() { }
            public Amphipod(char amphipodType)
            {
                AmphipodType = amphipodType;
                switch(amphipodType)
                {
                    case 'a': _energyPerMove = 1; break;        // Amber
                    case 'b': _energyPerMove = 10; break;       // Bronze
                    case 'c': _energyPerMove = 100; break;      // Copper
                    case 'd': _energyPerMove = 1000; break;     // Desert
                    default: break;
                }
            }

            public Amphipod(char amphipodType, (int row, int col) location)
            {
                AmphipodType = amphipodType;
                switch (amphipodType)
                {
                    case 'a': _energyPerMove = 1; break;        // Amber
                    case 'b': _energyPerMove = 10; break;       // Bronze
                    case 'c': _energyPerMove = 100; break;      // Copper
                    case 'd': _energyPerMove = 1000; break;     // Desert
                    default: break;
                }
                Location = location;
            }

            // Methods
            public void Move(int spaces, List<string> map, Dictionary<char, int> rooms)
            {
                EnergyUsed += _energyPerMove * spaces;
                CalculateHeuristicForMap(map, rooms);
            }

            public void CalculateHeuristicForMap(List<string> map, Dictionary<char, int> rooms)
            {
                int hallwayRow = -1;
                int targetCol = rooms[char.ToLower(AmphipodType)];

                for (int row = 0; row < map.Count; row++)
                {
                    if (map[row].Contains(','))
                        hallwayRow = row;
                }

                // Return expected energy to spend

                if(Location.Col == targetCol)
                {
                    // If in correct room

                    // Check if anyone is trapped below
                    string acceptable = ".#" + char.ToLower(AmphipodType) + char.ToUpper(AmphipodType);
                    for(int r = Location.Row; r < map.Count; r++)
                    {
                        if(!acceptable.Contains(map[r][Location.Col]))
                        {
                            // Something is trapped below, will need to move into hallway and back into correct room
                            int movement = 2 * (Math.Abs(hallwayRow - Location.Row) + 1) + 1;
                            MinEnergyExpectedToUse = movement * _energyPerMove;
                            return;
                        }
                    }

                    MinEnergyExpectedToUse = 0;
                    return;
                }


                else if(Location.Row == hallwayRow)
                {
                    // If in hallway, = movement to target room
                    // Allowed to count through occupied spaces.
                    int movesToClosest = Math.Abs(Location.Col - targetCol) + Math.Abs(Location.Row - hallwayRow) + 1;
                    MinEnergyExpectedToUse = movesToClosest * _energyPerMove;
                    return;
                }


                else if(Location.Row != hallwayRow && Location.Col != targetCol)
                {
                    // If in incorrect room, = 1 space + movement to nearest open room + 1 space
                    int movesToClosest = Math.Abs(Location.Col - targetCol) + Math.Abs(Location.Row - hallwayRow) + 2;
                    MinEnergyExpectedToUse = movesToClosest * _energyPerMove;
                    return;
                }
            }
        }
    }
}
