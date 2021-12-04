using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventChallenges
{
    public class Challenge_03a
    {
        public void Run()
        {
            string gammaRateStr = "";
            string epsilonRateStr = "";

            string[] data = System.IO.File.ReadAllLines(@"C:\Users\Zach Nichols\ElevenFiftyProjects\Practice\2021AdventChallenges\2021AdventChallenges\Inputs\Challenge_03a_input.txt");
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
    }
}
