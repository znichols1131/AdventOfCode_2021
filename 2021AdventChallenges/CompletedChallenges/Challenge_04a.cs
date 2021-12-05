using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventChallenges
{
    public class Challenge_04a
    {
        public void Run()
        {
            // Get all non-blank lines from file
            string path = @"C:\Users\Zach Nichols\ElevenFiftyProjects\Practice\2021AdventChallenges\2021AdventChallenges\Inputs\Challenge_04a_input.txt";
            List<string> listOfLines = System.IO.File.ReadAllLines(path)
                      .Where(x => !string.IsNullOrWhiteSpace(x))
                      .ToList<string>();

            // First line will be the numbers drawn in random order
            string chosenNumbersStr = listOfLines.First<string>();
            listOfLines.Remove(chosenNumbersStr);

            // Get list of randomly drawn numbers
            List<int> chosenNumbers = new List<int>();
            foreach (string str in chosenNumbersStr.Split(',').ToList<string>())
            {
                chosenNumbers.Add(int.Parse(str.Trim()));
            }

            // Get list of boards
            List<List<int>> boards = new List<List<int>>();
            while(listOfLines.Count > 5)
            {
                List<int> currentBoard = new List<int>();
                for(int i = 0; i < 5; i++)
                {
                    foreach (string str in listOfLines[i].Split(' '))
                    {
                        if(str != null && str != "")
                            currentBoard.Add(int.Parse(str.Trim()));
                    }
                }

                boards.Add(currentBoard);
                listOfLines.RemoveRange(0, 5);
            }

            // Play the game until someone wins
            for(int i = 4; i < chosenNumbers.Count; i++)
            {

                List<List<int>> result = ReturnWinningBoard(boards, chosenNumbers);

                // If a board won
                if (result != null)
                {
                    List<int> winningBoard = result[0];
                    int finalIndex = result[1][0];

                    Console.WriteLine("Winner!\n");
                    PrintWinningNumbers(chosenNumbers.GetRange(0, finalIndex + 1));
                    Console.WriteLine();
                    PrintBoard(winningBoard, chosenNumbers.GetRange(0, finalIndex + 1));
                    Console.WriteLine("\nFinal score: " + GetScoreForBoard(winningBoard, chosenNumbers.GetRange(0, finalIndex + 1), chosenNumbers[finalIndex]));
                    Console.ReadLine();
                    return;
                }
            }

            Console.WriteLine("No winners");
            Console.ReadLine();
        }

        public List<List<int>> ReturnWinningBoard(List<List<int>> boards, List<int> chosenNumbers)
        {
            for(int i = 4; i < chosenNumbers.Count; i++)
            {
                foreach (List<int> b in boards)
                {
                    //PrintBoard(b, chosenNumbers.GetRange(0, i + 1));
                    //Console.WriteLine();

                    if (CheckBoard(b, chosenNumbers.GetRange(0, i+1)))
                    {
                        List<List<int>> result = new List<List<int>>();
                        result.Add(b);

                        List<int> index = new List<int>();
                        index.Add(i);
                        result.Add(index);

                        return result;
                    }
                }
            }

            return null;
        }

        public bool CheckBoard(List<int> board, List<int> chosenNumbers)
        {
            // Check horizontal rows
            for(int i = 0; i < board.Count; i+=5)
            {
                for (int j = i; j < i + 5; j++)
                {
                    if (!chosenNumbers.Contains(board[j]))
                        goto NextRow;
                }

                // At this point, all 5 in this row succeed
                return true;

            // Jump to here if the row fails at any point
            NextRow:;
            }

            // Check vertical columns
            for (int i = 0; i < 5; i++)
            {
                for (int j = i; j < board.Count; j+=5)
                {
                    if (!chosenNumbers.Contains(board[j]))
                        goto NextColumn;
                }

                // At this point, all 5 in this column succeed
                return true;

            // Jump to here if the column fails at any point
            NextColumn:;
            }

            return false;

            // Don't check diagonals yet
            //// Check downward diagonal
            //{
            //    for (int j = 0; j < board.Count; j += 6)
            //    {
            //        if (!chosenNumbers.Contains(board[j]))
            //            goto NextDiagonal;
            //    }

            //    // At this point, all 5 in this diagonal succeed
            //    return true;

            //// Jump to here if the diagonal fails at any point
            //NextDiagonal:;
            //}

            //// Check downward diagonal
            //for (int j = 4; j < board.Count; j += 4)
            //{
            //    if (!chosenNumbers.Contains(board[j]))
            //        return false;
            //}

            //// At this point, all 5 in this diagonal succeed
            //return true;
        }

        public int GetScoreForBoard(List<int> board, List<int> chosenNumbers, int lastNumberCalled)
        {
            int sum = 0;
            foreach(int i in chosenNumbers)
            {
                try
                {
                    board.Remove(i);
                }
                catch { }
            }

            foreach(int i in board)
            {
                sum += i;
            }

            return sum * lastNumberCalled;
        }

        public void PrintWinningNumbers(List<int> winningNumbers)
        {
            foreach(int i in winningNumbers)
            {
                Console.Write(i + ", ");
            }
            Console.WriteLine();
        }

        public void PrintBoard(List<int> board, List<int> chosenNumbers)
        {
            for (int i = 0; i < board.Count; i+=5)
            {
                for (int j = i; j < i+5; j++)
                {
                    if (chosenNumbers.Contains(board[j]))
                        Console.ForegroundColor = ConsoleColor.Green;

                    Console.Write(board[j] + " ");
                    Console.ForegroundColor = ConsoleColor.White;

                }

                Console.WriteLine();
            }
        }
    }

    //public class Board
    //{
    //    public Tile[,] tiles = new Tile[5,5];

    //    public Board(List<int> values)
    //    {
    //        for(int row = 0; row < 5; row ++)
    //        {
    //            for(int col = 0; col < 5; col++)
    //            {
    //                tiles[row, col] = new Tile(values[row*5 + col]);
    //            }
    //        }
    //    }

    //    public void MarkTile(int value)
    //    {
    //        for (int row = 0; row < 5; row++)
    //        {
    //            for (int col = 0; col < 5; col++)
    //            {
    //                if (tiles[row, col].Value == value)
    //                {
    //                    tiles[row, col].Marked = true;
    //                    return;
    //                }
    //            }
    //        }
    //    }

    //    public bool CheckBingo()
    //    {
    //        // Check horizontal rows
    //        for (int row = 0; row < 5; row++)
    //        {
    //            for (int col = 0; col < 5; col++)
    //            {
    //                if (!tiles[row, col].Marked)
    //                    goto NextRow;
    //            }

    //            // At this point, all 5 in this row succeed
    //            return true;

    //        // Jump to here if the row fails at any point
    //        NextRow:;
    //        }

    //        // Check vertical columns
    //        for (int col = 0; col < 5; col++)
    //        {
    //            for (int row = 0; row < 5; row++)
    //            {
    //                if (!tiles[row, col].Marked)
    //                    goto NextColumn;
    //            }

    //            // At this point, all 5 in this column succeed
    //            return true;

    //        // Jump to here if the column fails at any point
    //        NextColumn:;
    //        }

    //        // Check downward diagonal
    //        for (int row = 0; row < 5; row++)
    //        {
    //            if (!tiles[row, row].Marked)
    //                goto NextDiagonal;
    //        }

    //        // At this point, all 5 in this row succeed
    //        return true;

    //    // Jump to here if the diagonal fails at any point
    //    NextDiagonal:;

    //        // Check downward diagonal
    //        for (int row = 0; row < 5; row++)
    //        {
    //            if (!tiles[row, 4 - row].Marked)
    //                return false;
    //        }

    //        // At this point, all 5 in this diagonal succeed
    //        return true;
    //    }
    //}

    //public class Tile
    //{
    //    public int Value { get; set; }
    //    public bool Marked { get; set; }

    //    public Tile(int value)
    //    {
    //        Value = value;
    //        Marked = false;
    //    }

    //    public Tile(int value, bool marked)
    //    {
    //        Value = value;
    //        Marked = marked;
    //    }
    //}
}
