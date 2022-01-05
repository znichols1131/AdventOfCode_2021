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
        public void Challenge_A()
        {
            // Get input
            string filePath = Path.Combine(@"..\..\Inputs\", "Challenge_23a_input.txt");
            List<string> instructions = System.IO.File.ReadAllLines(filePath).ToList<string>();
        }

        public void Challenge_B()
        {

        }
    }
}
