using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventChallenges
{
    public class Challenge_03
    {
        public void Challenge_A()
        {
            string gammaRateStr = "";
            string epsilonRateStr = "";

            string[] data = System.IO.File.ReadAllLines(@"C:\Users\Zach Nichols\ElevenFifty\Practice\2021Advent\2021AdventChallenges\Inputs\Challenge_03a_input.txt");
            int bitLength = data[0].Length;

            for(int i = 0; i < bitLength; i++)
            {
                int zeroCount = 0;

                foreach(string d in data)
                {
                    if (d[i] == '0')
                        zeroCount++;
                }
                
                gammaRateStr += (zeroCount > data.Length - zeroCount) ? "0" : "1";
                epsilonRateStr += (zeroCount > data.Length - zeroCount) ? "1" : "0";
            }

            Console.WriteLine("Gamma bit: " + gammaRateStr);
            Console.WriteLine("Epsilon bit: " + epsilonRateStr);

            int gammaRate = ValueForBinaryString(gammaRateStr);
            int epsilonRate = ValueForBinaryString(epsilonRateStr);
            int powerConsumption = gammaRate * epsilonRate;

            Console.WriteLine("\nGamma rate: " + gammaRate);
            Console.WriteLine("Epsilon rate: " + epsilonRate);
            Console.WriteLine("\nPower consumption: " + powerConsumption);
            Console.ReadLine();
        }

        public void Challenge_B()
        {
            string oxygenRatingStr = GetOxygenRating();
            string co2RatingStr = GetCO2Rating();

            Console.WriteLine("Oxygen bit: " + oxygenRatingStr);
            Console.WriteLine("CO2 bit: " + co2RatingStr);

            int oxygenRating = ValueForBinaryString(oxygenRatingStr);
            int co2Rating = ValueForBinaryString(co2RatingStr);
            int lifeSupportRating = oxygenRating * co2Rating;

            Console.WriteLine("\nOxygen rating: " + oxygenRating);
            Console.WriteLine("CO2 rating: " + co2Rating);
            Console.WriteLine("\nLife support rating: " + lifeSupportRating);
            Console.ReadLine();
        }

        public int ValueForBinaryString(string input)
        {
            int bitLength = input.Length;
            int output = 0;

            for(int i = 0; i < bitLength; i++)
            {
                int j = int.Parse(input[i].ToString());
                output += j * (int)Math.Pow(2, bitLength - i - 1);
            }

            return output;
        }

        public string GetOxygenRating()
        {
            List<string> data = System.IO.File.ReadAllLines(@"C:\Users\Zach Nichols\ElevenFifty\Practice\2021Advent\2021AdventChallenges\Inputs\Challenge_03a_input.txt").ToList<string>();
            int bitLength = data[0].Length;

            for (int i = 0; i < bitLength; i++)
            {
                int zeroCount = 0;

                foreach (string d in data)
                {
                    if (d[i] == '0')
                        zeroCount++;
                }

                char target = (zeroCount > data.Count - zeroCount) ? '0' : '1';

                for (int j = data.Count - 1; j >= 0; j--)
                {
                    if (data[j][i] != target)
                    {
                        if (data.Count > 1)
                        {
                            data.RemoveAt(j);
                        }
                        else
                        {
                            // If last string in data, this is the oxygen rating
                            return data[j];
                        }
                    }
                }
            }

            // Default = return the first of the remaining strings
            return data.First<string>();
        }

        public string GetCO2Rating()
        {
            List<string> data = System.IO.File.ReadAllLines(@"C:\Users\Zach Nichols\ElevenFiftyProjects\Practice\2021AdventChallenges\2021AdventChallenges\Inputs\Challenge_03a_input.txt").ToList<string>();
            int bitLength = data[0].Length;

            for (int i = 0; i < bitLength; i++)
            {
                int zeroCount = 0;

                foreach (string d in data)
                {
                    if (d[i] == '0')
                        zeroCount++;
                }

                char target = (zeroCount <= data.Count - zeroCount) ? '0' : '1';

                for (int j = data.Count - 1; j >= 0; j--)
                {
                    if (data[j][i] != target)
                    {
                        if (data.Count > 1)
                        {
                            data.RemoveAt(j);
                        }
                        else
                        {
                            // If last string in data, this is the oxygen rating
                            return data[j];
                        }
                    }
                }
            }

            // Default = return the first of the remaining strings
            return data.First<string>();
        }
    }
}
