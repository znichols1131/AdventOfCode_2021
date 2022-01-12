using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventChallenges
{
    public class Challenge_15
    {
        private Node[,] _grid;
        private int _endRow = 0;
        private int _endCol = 0;
        private Node _start;
        private Node _end;

        public void Challenge_A()
        {
            // Get lines
            string filePath = Path.Combine(@"..\..\Inputs\", "Challenge_15a_input.txt");
            List<string> lines = System.IO.File.ReadAllLines(filePath).ToList<string>();

            // Get target coordinates
            _endRow = lines.Count - 1;
            _endCol = lines[_endRow].Length - 1;

            // Create grid
            _grid = new Node[_endRow + 1, _endCol + 1];
            for (int row = 0; row < lines.Count; row++)
            {
                for (int col = 0; col < lines[row].Length; col++)
                {
                    _grid[row, col] = new Node(row, col, int.Parse(lines[row][col].ToString()));
                    _grid[row, col].SetDistance(_endRow, _endCol);
                }
            }

            // Get start and end nodes
            _start = new Node(0, 0, _grid[0,0].Risk);
            _start.SetDistance(_endRow, _endCol);
            _end = new Node(_endRow, _endCol, _grid[_endRow,_endCol].Risk);
            _end.SetDistance(_endRow, _endCol);

            // Find route with lowest risk
            int risk = FindPath();

            // Output
            Console.WriteLine("Route found:");
            Console.WriteLine("Risk: " + risk);
            Console.ReadLine();
        }

        public void Challenge_B()
        {
            // Get lines
            string filePath = Path.Combine(@"..\..\Inputs\", "Challenge_15a_input.txt");
            List<string> lines = System.IO.File.ReadAllLines(filePath).ToList<string>();

            // Get target coordinates
            int expansionFactor = 5;
            int totalRows = lines.Count * expansionFactor;
            int totalCols = lines[0].Length * expansionFactor;
            _endRow = totalRows - 1;
            _endCol = totalCols - 1;
            _grid = new Node[totalRows, totalCols];

            // Seed the original grid
            for (int row = 0; row < lines.Count; row++)
            {
                for (int col = 0; col < lines[row].Length; col++)
                {
                    _grid[row, col] = new Node(row, col, int.Parse(lines[row][col].ToString()));
                    _grid[row, col].SetDistance(_endRow, _endCol);
                }
            }

            // Expand right
            int originalColumns = totalCols / 5;
            for(int col = originalColumns; col < totalCols; col++)
            {
                int sourceCol = col - originalColumns;

                for(int row = 0; row < lines.Count; row++)
                {
                    _grid[row, col] = new Node(row, col, _grid[row, sourceCol].Risk + 1);
                    if (_grid[row, col].Risk > 9)
                        _grid[row, col].Risk = 1;

                    _grid[row, col].SetDistance(_endRow, _endCol);
                }
            }

            // Expand down
            int originalRows = totalRows / 5;
            for (int row = originalRows; row < totalRows; row++)
            {
                int sourceRow = row - originalRows;

                for (int col = 0; col < totalCols; col++)
                {
                    _grid[row, col] = new Node(row, col, _grid[sourceRow, col].Risk + 1);
                    if (_grid[row, col].Risk > 9)
                        _grid[row, col].Risk = 1;

                    _grid[row, col].SetDistance(_endRow, _endCol);
                }
            }

            // Get start and end nodes
            _start = new Node(0, 0, _grid[0, 0].Risk);
            _start.SetDistance(_endRow, _endCol);
            _end = new Node(_endRow, _endCol, _grid[_endRow, _endCol].Risk);
            _end.SetDistance(_endRow, _endCol);

            // Find route with lowest risk
            int risk = FindPath();

            // Output
            Console.WriteLine("Route found:");
            Console.WriteLine("Risk: " + risk);
            Console.ReadLine();
        }

        public int FindPath()
        {
            // Declare lists of nodes
            var nodesToCheck = new List<Node>();
            var visitedNodes = new List<Node>();

            // Add seed node
            nodesToCheck.Add(_start);

            // While there are any nodes to check
            while(nodesToCheck.Any())
            {
                // Get the next node, start with the one with the lowest cost
                var activeNode = nodesToCheck.OrderBy(n => n.TotalCost).First();

                // Check if this node is the end goal
                if (activeNode.Row == _end.Row && activeNode.Col == _end.Col)
                {
                    // Success! Print route and return risk of route.
                    List<Node> route = GetFinalRoute(activeNode);
                    PrintRoute(route);
                    return GetRouteRisk(route);
                }

                // Otherwise, mark this node as visited
                visitedNodes.Add(activeNode);
                nodesToCheck.Remove(activeNode);

                // Now get neighboring nodes and go through them
                List<Node> neighbors = GetNeighboringNodes(activeNode);
                foreach(Node neighbor in neighbors)
                {
                    // Check if we've already visited it, skip to next iteration of foreach loop
                    if (visitedNodes.Any(n => n.Row == neighbor.Row && n.Col == neighbor.Col))
                        continue;

                    // If it's already in the active list, check if this neighbor has a better route
                    if(nodesToCheck.Any(n => n.Row == neighbor.Row && n.Col == neighbor.Col))
                    {
                        var comparisonNode = nodesToCheck.Find(n => n.Row == neighbor.Row && n.Col == neighbor.Col);

                        // If the neighbor node has a better route, replace the comparison node
                        if(neighbor.TotalCost < comparisonNode.TotalCost)
                        {
                            nodesToCheck.Remove(comparisonNode);
                            nodesToCheck.Add(neighbor);
                        }
                    }
                    else
                    {
                        nodesToCheck.Add(neighbor);
                    }
                }
            }

            // Failed to find a successful route
            return -1;
        }

        public List<Node> GetFinalRoute(Node finalNode)
        {
            List<Node> route = new List<Node>();
            route.Add(finalNode);

            var node = finalNode;
            while(node.Parent != null)
            {
                node = node.Parent;
                route.Add(node);
            }

            return route;
        }

        public void PrintRoute(List<Node> route)
        {
            for(int row = 0; row < _endRow+1; row++)
            {
                for(int col = 0; col < _endCol+1; col++)
                {
                    if(route.Any(n => n.Row == row && n.Col == col))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }

                    Console.Write($"{_grid[row, col].Risk} ");
                }

                Console.WriteLine();
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        public int GetRouteRisk(List<Node> route)
        {
            int risk = 0;

            // We can't trust the risks stored in the route since they're altered by A*
            // So we'll have to go back to the original grid
            foreach(Node n in route)
            {
                // Don't count starting space
                if (n.Row == 0 && n.Col == 0)
                    continue;

                risk += _grid[n.Row, n.Col].Risk;
            }

            return risk;
        }

        public List<Node> GetNeighboringNodes(Node currentNode)
        {
            // Get all possible neighboring nodes
            List<Node> possibleNodes = new List<Node>()
            {
                new Node { Row = currentNode.Row+1, Col = currentNode.Col, Parent = currentNode},    // Down
                new Node { Row = currentNode.Row-1, Col = currentNode.Col, Parent = currentNode},    // Up
                new Node { Row = currentNode.Row, Col = currentNode.Col+1, Parent = currentNode},    // Right
                new Node { Row = currentNode.Row, Col = currentNode.Col-1, Parent = currentNode}     // Left

                //new Node { Row = currentNode.Row-1, Col = currentNode.Col-1, Parent = currentNode},    // Top left
                //new Node { Row = currentNode.Row-1, Col = currentNode.Col+1, Parent = currentNode},    // Top right
                //new Node { Row = currentNode.Row+1, Col = currentNode.Col-1, Parent = currentNode},    // Bottom left
                //new Node { Row = currentNode.Row+1, Col = currentNode.Col+1, Parent = currentNode}     // Bottom right
            };

            // Weed out ineligible coordinates
            possibleNodes = possibleNodes
                                        .Where(n => n.Row >= 0 && n.Row <= _endRow)
                                        .Where(n => n.Col >= 0 && n.Col <= _endCol)
                                        .ToList();

            // Assign risks and distances
            foreach(Node n in possibleNodes)
            {
                n.Risk = _grid[n.Row, n.Col].Risk + currentNode.Risk;
                n.SetDistance(_endRow, _endCol);
            }

            // Return remaining nodes
            return possibleNodes;
        }
    }

    public class Node
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public int Risk { get; set; }
        public int Distance { get; set; }
        public double TotalCost { get { return this.Risk + this.Distance; }  }    // ignoring distance = Dijkstra, including = A* algorithm
        public Node Parent { get; set; }

        public Node() { }
        
        public Node(int row, int col, int risk)
        {
            Row = row;
            Col = col;
            Risk = risk;
        }

        public void SetDistance(int targetRow, int targetCol)
        {
            Distance = Math.Abs(Col - targetCol) + Math.Abs(Row - targetRow);
        }
    }
}