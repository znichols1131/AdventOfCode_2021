using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventChallenges
{
    public class Challenge_20
    {
        private string _imgAlgorithm = "";
        private List<string> _image = new List<string>();

        private int _border = 0;
        private char _marginChar = '.';

        public void Challenge_A()
        {
            // Get input
            string filePath = Path.Combine(@"..\..\Inputs\", "Challenge_20a_input.txt");
            List<string> lines = System.IO.File.ReadAllLines(filePath).ToList<string>();

            // Get image algorithm
            _imgAlgorithm = lines.First();

            // Populate image
            for(int i = 1; i < lines.Count; i++)
            {
                string line = lines[i];

                if (line != null && line != "")
                    _image.Add(line);
            }

            // Print original image
            //PrintImage();

            // Prepare image
            int numberOfEnhancements = 2;
            _border = numberOfEnhancements + 3;
            PrepareImage(_border);
            PrintImage();

            // Enhance image
            for (int i = numberOfEnhancements; i > 0; i--)
            {
                EnhanceImage();
                PrintImage();
                _border--;
            }

            // Trim
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            TrimImage(_border);
            PrintImage();

            // Output
            int pixelsLit = 0;
            foreach(string line in _image)
            {
                pixelsLit += line.Count(c => c=='#'); 
            }
            Console.WriteLine("\nNumber of lit pixels = " + pixelsLit);
            Console.ReadLine();
        }

        public void Challenge_B()
        {
            // Get input
            string filePath = Path.Combine(@"..\..\Inputs\", "Challenge_20a_input.txt");
            List<string> lines = System.IO.File.ReadAllLines(filePath).ToList<string>();

            // Get image algorithm
            _imgAlgorithm = lines.First();

            // Populate image
            for (int i = 1; i < lines.Count; i++)
            {
                string line = lines[i];

                if (line != null && line != "")
                    _image.Add(line);
            }

            // Print original image
            //PrintImage();

            // Prepare image
            int numberOfEnhancements = 50;
            _border = numberOfEnhancements +1;
            PrepareImage(_border);
            //PrintImage();

            // Enhance image
            for (int i = numberOfEnhancements; i > 0; i--)
            {
                EnhanceImage();
                //PrintImage();
                Console.WriteLine("Iteration #" + (numberOfEnhancements-i+1));
                _border--;
            }

            // Trim
            //PrintImage();
            //Console.WriteLine();
            //Console.WriteLine();
            Console.WriteLine();
            TrimImage(_border);
            PrintImage();

            // Output
            int pixelsLit = 0;
            foreach (string line in _image)
            {
                pixelsLit += line.Count(c => c == '#');
            }
            Console.WriteLine("\nNumber of lit pixels = " + pixelsLit);
            Console.ReadLine();
        }

        public void EnhanceImage()
        {
            // Ignore the first # pixels on each edge

            // Create an output image
            List<string> outputImage = _image.ToList();

            // Prepare existing image
            //PrepareImage();

            // Parse existing image
            for(int row = 0; row < _image.Count; row++)
            {
                for(int col = 0; col < _image[row].Length; col++)
                {
                    var surroundingPoints = GetSurroundingPoints(row, col);
                    char light = GetLightValue(surroundingPoints);
                    outputImage[row] = outputImage[row].Remove(col, 1).Insert(col, light.ToString());
                }
            }

            _image = outputImage;

            if(_marginChar == '.')
            {
                _marginChar = '#';
            }else
            {
                _marginChar = '.';
            }
        }

        public void TrimImage(int border)
        {
            for (int i = 0; i < border; i++)
            {
                // Remove top and bottom
                _image.RemoveAt(0);
                _image.RemoveAt(_image.Count - 1);

                // Remove sides
                for (int row = 0; row < _image.Count; row++)
                {
                    _image[row] = _image[row].Substring(1, _image[row].Length - 2);
                }
            }

            _border = 0;
        }

        public void PrepareImage(int padding)
        {
            for(int i = 0; i < padding; i++)
            {
                string blankRow = _image.First().Replace('#', '.').Replace('.', _marginChar);

                // Pad top and bottom
                _image.Insert(0, blankRow);
                _image.Add(blankRow);

                // Pad sides
                for(int row = 0; row < _image.Count; row++)
                {
                    _image[row] = _marginChar + _image[row] + _marginChar;
                }
            }
        }

        public List<string> GetSurroundingPoints(int row, int col)
        {
            List<string> surroundingPoints = new List<string>();

            // Get row above
            if(row - 1 >= 0)
            {
                string topRow = "";

                // Get left
                if(col - 1 >= 0)
                {
                    topRow += _image[row - 1][col - 1];
                }
                else
                {
                    topRow += _marginChar;
                }

                // Get middle
                topRow += _image[row - 1][col];

                // Get right
                if (col + 1 < _image[row-1].Length)
                {
                    topRow += _image[row - 1][col + 1];
                }
                else
                {
                    topRow += _marginChar;
                }

                surroundingPoints.Add(topRow);
            }
            else
            {
                surroundingPoints.Add("" + _marginChar + _marginChar + _marginChar);
            }

            // Get current row
            string middleRow = "";

            // Get left
            if (col - 1 >= 0)
            {
                middleRow += _image[row][col - 1];
            }
            else
            {
                middleRow += _marginChar;
            }

            // Get middle
            middleRow += _image[row][col];

            // Get right
            if (col + 1 < _image[row].Length)
            {
                middleRow += _image[row][col + 1];
            }
            else
            {
                middleRow += _marginChar;
            }

            surroundingPoints.Add(middleRow);

            // Get row below
            if (row + 1 < _image.Count)
            {
                string bottomRow = "";

                // Get left
                if (col - 1 >= 0)
                {
                    bottomRow += _image[row + 1][col - 1];
                }
                else
                {
                    bottomRow += _marginChar;
                }

                // Get middle
                bottomRow += _image[row + 1][col];

                // Get right
                if (col + 1 < _image[row + 1].Length)
                {
                    bottomRow += _image[row + 1][col + 1];
                }
                else
                {
                    bottomRow += _marginChar;
                }

                surroundingPoints.Add(bottomRow);
            }
            else
            {
                surroundingPoints.Add("" + _marginChar + _marginChar + _marginChar);
            }

            return surroundingPoints;
        }

        public char GetLightValue(List<string> surroundingPoints)
        {
            // Get string into one line
            string binaryInput = "";
            foreach(string line in surroundingPoints)
            {
                binaryInput += line;
            }

            // Replace . with 0, # with 1
            binaryInput = binaryInput.Replace('.', '0').Replace('#', '1');
            int decimalInput = (int)Convert.ToInt64(binaryInput, 2);

            // Return matching character from algorithm
            return _imgAlgorithm[decimalInput];
        }

        public void PrintImage()
        {
            for(int row = 0; row < _image.Count; row++)
            {
                for(int col = 0; col < _image[row].Length; col++)
                {
                    if(!(row >= _border && row < _image.Count-_border && col >= _border && col < _image[row].Length-_border))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else
                    {
                        Console.ForegroundColor = _image[row][col] == '#' ? ConsoleColor.Green : ConsoleColor.White;
                    }
                    Console.Write(_image[row][col]);
                }
                Console.WriteLine();
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
