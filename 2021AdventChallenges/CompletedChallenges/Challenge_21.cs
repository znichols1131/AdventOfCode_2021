using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventChallenges
{
    public class Challenge_21
    {
        private List<Player> _players = new List<Player>();
        private int _nextDiceRoll = 1;
        private int _totalDiceRolls = 0;
        
        private List<KeyValuePair<int, int>> _diracOutcomes = new List<KeyValuePair<int, int>>();

        public void Challenge_A()
        {
            // Get input
            string filePath = Path.Combine(@"..\..\Inputs\", "Challenge_21a_input.txt");
            List<string> lines = System.IO.File.ReadAllLines(filePath).ToList<string>();

            // Create players
            foreach(string line in lines)
            {
                if (line is null || line == "")
                    continue;

                string[] playerInfo = line.Replace("Player ", "").Replace(" starting position: ", "#").Split('#');

                _players.Add(new Player(int.Parse(playerInfo[0]), int.Parse(playerInfo[1])));
            }

            // Play game while players are below 1000 score
            while(!_players.Any(p => p.Score >= 1000))
            {
                // Play round
                foreach(Player p in _players)
                {
                    // Player makes move by rolling dice 3 times
                    int move = _nextDiceRoll * 3 + 3;
                    _totalDiceRolls += 3;

                    _nextDiceRoll += 3;
                    if (_nextDiceRoll > 100)
                        _nextDiceRoll -= 100;

                    p.Move(move);

                    // Check if player won
                    if (p.Score >= 1000)
                        goto someone_won;
                }
            }

            someone_won:

            // Output
            Console.WriteLine("Total dice rolls: " + _totalDiceRolls);
            Console.WriteLine("Final score:");
            foreach (Player p in _players)
            {
                Console.WriteLine($"Player {p.ID} score = {p.Score}");
            }

            Player loser = _players.Where(p => p.Score < 1000).First();

            Console.WriteLine();
            Console.WriteLine("Puzzle solution = " + (loser.Score * _totalDiceRolls));

            Console.ReadLine();
        }

        public void Challenge_B()
        {
            // Get input
            string filePath = Path.Combine(@"..\..\Inputs\", "Challenge_21a_input.txt");
            List<string> lines = System.IO.File.ReadAllLines(filePath).ToList<string>();

            // Create players
            foreach (string line in lines)
            {
                if (line is null || line == "")
                    continue;

                string[] playerInfo = line.Replace("Player ", "").Replace(" starting position: ", "#").Split('#');

                _players.Add(new Player(int.Parse(playerInfo[0]), int.Parse(playerInfo[1])));
            }

            // Populate dirac outcomes
            PopulateDiracOutcomes();

            // Play game while players are below 21 score
            PlayDiracDice(_players, 0, 1);

            // Output
            Console.WriteLine("Final score:");
            foreach (Player p in _players)
            {
                Console.WriteLine($"Player {p.ID} score = {p.NumberOfWins}");
            }

            Console.WriteLine();
            Console.WriteLine("Puzzle solution = " + _players.Max(p => p.NumberOfWins));

            Console.ReadLine();
        }

        public void PlayDiracDice(List<Player> currentPlayers, int nextPlayerIndex, decimal numberOfUniverses)
        {
            // Player makes move by rolling dice 3 times, each roll branching into a different universe
            for (int i = 0; i < _diracOutcomes.Count; i++)
            {
                // Clone current player
                Player tempPlayer = new Player(currentPlayers[nextPlayerIndex].ID,
                                                currentPlayers[nextPlayerIndex].Position,
                                                currentPlayers[nextPlayerIndex].Score);

                // Move player
                tempPlayer.Move(_diracOutcomes[i].Key);

                // Check if winner
                if (tempPlayer.Score >= 21)
                {
                    // Add a win for that player, don't iterate further
                    Player winner = _players.Find(w => w.ID == tempPlayer.ID);
                    winner.NumberOfWins += numberOfUniverses * _diracOutcomes[i].Value;
                    continue;
                }
                else
                {
                    // If not a winner, start a new iteration
                    List<Player> newPlayers = new List<Player>();
                    foreach (Player cp in currentPlayers)
                    {
                        if (cp.ID == tempPlayer.ID)
                        {
                            newPlayers.Add(tempPlayer);
                        }
                        else
                        {
                            newPlayers.Add(new Player(cp.ID, cp.Position, cp.Score));
                        }
                    }
                    PlayDiracDice(newPlayers, (nextPlayerIndex + 1 < currentPlayers.Count) ? nextPlayerIndex + 1 : 0, numberOfUniverses * _diracOutcomes[i].Value);
                }       
            }            
        }

        public void PopulateDiracOutcomes()
        {
            for(int i = 1; i <= 3; i++)
            {
                for (int j = 1; j <= 3; j++)
                {
                    for (int k = 1; k <= 3; k++)
                    {
                        int sum = i + j + k;
                        
                        // Check if it exists
                        if(_diracOutcomes.Any(o => o.Key == sum))
                        {
                            var entry = _diracOutcomes.Find(o => o.Key == sum);
                            var newEntry = new KeyValuePair<int, int>(entry.Key, entry.Value + 1);
                            _diracOutcomes.Remove(entry);
                            _diracOutcomes.Add(newEntry);
                        }
                        else
                        {
                            var newEntry = new KeyValuePair<int, int>(sum, 1);
                            _diracOutcomes.Add(newEntry);
                        }
                    }
                }
            }
        }
    }

    public class Player
    {
        public int ID { get; set; }
        public int Position { get; set; }
        public int Score { get; set; }
        public decimal NumberOfWins { get; set; }

        public Player() { }
        public Player(int id, int position)
        {
            ID = id;
            Position = position;
        }
        
        public Player(int id, int position, int score)
        {
            ID = id;
            Position = position;
            Score = score;
        }


        public void Move(int spaces)
        {
            Position = (Position + spaces - 1) % 10 + 1;
            Score += Position;
        }
    }
}
