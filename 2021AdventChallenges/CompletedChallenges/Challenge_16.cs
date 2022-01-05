using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2021AdventChallenges
{
    public class Challenge_16
    {
        private static readonly Dictionary<char, string> hexCharacterToBinary = new Dictionary<char, string> {
            { '0', "0000" },
            { '1', "0001" },
            { '2', "0010" },
            { '3', "0011" },
            { '4', "0100" },
            { '5', "0101" },
            { '6', "0110" },
            { '7', "0111" },
            { '8', "1000" },
            { '9', "1001" },
            { 'a', "1010" },
            { 'b', "1011" },
            { 'c', "1100" },
            { 'd', "1101" },
            { 'e', "1110" },
            { 'f', "1111" }
        };

        private List<Packet> _packets = new List<Packet>();

        public void Challenge_A()
        {
            // Get line
            string filePath = Path.Combine(@"..\..\Inputs\", "Challenge_16a_input.txt");
            string hexLine = System.IO.File.ReadAllLines(filePath).First();
            //hexLine = "A0016C880162017C3686B18A3D4780";

            // Convert hex to binary
            string binaryLine = "";
            foreach(char c in hexLine)
            {
                binaryLine += hexCharacterToBinary[char.ToLower(c)];
            }

            // Parse binary input
            _packets = ParseBinaryInput_A(binaryLine);

            // Get all version numbers
            int output = _packets.Sum(p => p.NetVersion);

            // Output
            Console.WriteLine("Answer: " + output);
            Console.ReadLine();
        }

        public void Challenge_B()
        {
            // Get line
            string filePath = Path.Combine(@"..\..\Inputs\", "Challenge_16a_input.txt");
            string hexLine = System.IO.File.ReadAllLines(filePath).First();
            //hexLine = "F600BC2D8F";

            // Convert hex to binary
            string binaryLine = "";
            foreach (char c in hexLine)
            {
                binaryLine += hexCharacterToBinary[char.ToLower(c)];
            }

            // Parse binary input
            _packets = ParseBinaryInput_B(binaryLine);

            // Get all version numbers
            Int64 output = EvaluatePacket(_packets.First());

            // Output
            Console.WriteLine("Answer: " + output);
            Console.ReadLine();
        }

        public List<Packet> ParseBinaryInput_A(string input)
        {
            List<Packet> packets = new List<Packet>();

            // Make sure the input meets the minimum length of a packet (6 bit header, 7th bit can begin a literal)
            while (input.Length >= 7)
            {
                Packet p = GetPacketFromInput_A(ref input);
                if (p is null)
                    break;

                packets.Add(p);
            }          

            return packets;
        }

        public Packet GetPacketFromInput_A(ref string input)
        {
            // Keep track of this packet
            Packet packet = new Packet();

            // Get version
            packet.Version = (int)ConvertBinaryToInt(input.Substring(0, 3));

            // Get type
            packet.Type = (int)ConvertBinaryToInt(input.Substring(3, 3));
            if (packet.Type == 4)
            {
                // Literal value

                // Pad the original input with zeros until it's length is a multiple of 4
                while (input.Length % 4 > 0)
                {
                    input += "0";
                }

                // Get packet body
                string packetBody = input.Substring(6, input.Length - 6);

                // Parse packet body
                string binaryAnswer = "";
                int lastIndex = packetBody.Length - 1;
                for (int i = 4; i < packetBody.Length; i += 5)
                {
                    lastIndex = i;

                    // Get the number for that group of bits
                    binaryAnswer += packetBody.Substring(i - 3, 4);

                    // Check if this is the last group of bits
                    if (packetBody[i - 4] == '0')
                        break;
                }

                // Add this subanswer to the overall answer
                //packet.Value += ConvertBinaryToInt(binaryAnswer);

                // Remove the packet from input
                input = input.Remove(0, lastIndex + 1 + 6);
            }
            else
            {
                // Operator packet

                // Make sure there's at least 7 digits
                if (input.Length < 7)
                    return null;

                // Get length type ID
                packet.PacketLengthType = (int)ConvertBinaryToInt(input.Substring(6, 1));
                if (packet.PacketLengthType == 0)
                {
                    // Make sure there's at least 15 more digits
                    if (input.Length < 7 + 15)
                        return null;

                    // Total length in bits
                    int lengthInBits = (int)ConvertBinaryToInt(input.Substring(7, 15));
                    input = input.Remove(0, 22);

                    string tempStr = input.Substring(0, lengthInBits);
                    input = input.Remove(0, lengthInBits);

                    // Recursively solve subpackets
                    bool success = true;
                    while (tempStr.Length > 7 && success)
                    {
                        Packet p = GetPacketFromInput_A(ref tempStr);
                        if (p is null)
                            success = false;

                        packet.Subpackets.Add(p);

                        if (!tempStr.Contains('1'))
                            success = false;
                    }
                }
                else
                {
                    // Make sure there's at least 11 more digits
                    if (input.Length < 7 + 11)
                        return null;

                    // Number of subpackets
                    int numberOfSubpackets = (int)ConvertBinaryToInt(input.Substring(7, 11));
                    input = input.Remove(0, 18);

                    // Recursively solve subpackets
                    for(int i = 0; i < numberOfSubpackets; i++)
                    {
                        if (!input.Contains('1'))
                            continue;

                        Packet p = GetPacketFromInput_A(ref input);
                        if (p is null)
                            continue;

                        packet.Subpackets.Add(p);
                    }
                }
            }

            return packet;
        }

        public List<Packet> ParseBinaryInput_B(string input)
        {
            List<Packet> packets = new List<Packet>();

            // Make sure the input meets the minimum length of a packet (6 bit header, 7th bit can begin a literal)
            while (input.Length >= 7)
            {
                Packet p = GetPacketFromInput_B(ref input);
                if (p is null)
                    break;

                packets.Add(p);
            }

            return packets;
        }

        public Packet GetPacketFromInput_B(ref string input)
        {
            // Keep track of this packet
            Packet packet = new Packet();

            // Get version
            packet.Version = (int)ConvertBinaryToInt(input.Substring(0, 3));

            // Get type
            packet.Type = (int)ConvertBinaryToInt(input.Substring(3, 3));
            if (packet.Type == 4)
            {
                // Literal value

                // Pad the original input with zeros until it's length is a multiple of 4
                while (input.Length % 4 > 0)
                {
                    input += "0";
                }

                // Get packet body
                string packetBody = input.Substring(6, input.Length - 6);

                // Parse packet body
                string binaryAnswer = "";
                int lastIndex = packetBody.Length - 1;
                for (int i = 4; i < packetBody.Length; i += 5)
                {
                    lastIndex = i;

                    // Get the number for that group of bits
                    binaryAnswer += packetBody.Substring(i - 3, 4);

                    // Check if this is the last group of bits
                    if (packetBody[i - 4] == '0')
                        break;
                }

                // Add this subanswer to the overall answer
                packet.Value += ConvertBinaryToInt(binaryAnswer);

                // Remove the packet from input
                input = input.Remove(0, lastIndex + 1 + 6);
            }
            else
            {
                // Operator packet

                // Make sure there's at least 7 digits
                if (input.Length < 7)
                    return null;

                // Get length type ID
                packet.PacketLengthType = (int)ConvertBinaryToInt(input.Substring(6, 1));
                if (packet.PacketLengthType == 0)
                {
                    // Make sure there's at least 15 more digits
                    if (input.Length < 7 + 15)
                        return null;

                    // Total length in bits
                    int lengthInBits = (int)ConvertBinaryToInt(input.Substring(7, 15));
                    input = input.Remove(0, 22);

                    string tempStr = input.Substring(0, lengthInBits);
                    input = input.Remove(0, lengthInBits);

                    // Recursively solve subpackets
                    bool success = true;
                    while (tempStr.Length > 7 && success)
                    {
                        Packet p = GetPacketFromInput_B(ref tempStr);
                        if (p is null)
                            success = false;

                        packet.Subpackets.Add(p);

                        if (!tempStr.Contains('1'))
                            success = false;
                    }
                }
                else
                {
                    // Make sure there's at least 11 more digits
                    if (input.Length < 7 + 11)
                        return null;

                    // Number of subpackets
                    int numberOfSubpackets = (int)ConvertBinaryToInt(input.Substring(7, 11));
                    input = input.Remove(0, 18);

                    // Recursively solve subpackets
                    for (int i = 0; i < numberOfSubpackets; i++)
                    {
                        if (!input.Contains('1'))
                            continue;

                        Packet p = GetPacketFromInput_B(ref input);
                        if (p is null)
                            continue;

                        packet.Subpackets.Add(p);
                    }
                }
            }

            return packet;
        }

        public Int64 ConvertBinaryToInt(string binaryInput)
        {
            return Convert.ToInt64(binaryInput, 2);
        }

        public Int64 EvaluatePacket(Packet p)
        {
            switch(p.Type)
            {
                case 0:
                    // Sum the subpackets
                    Int64 sum = 0;
                    foreach(Packet sub in p.Subpackets)
                    {
                        sum += EvaluatePacket(sub);
                    }
                    return sum;

                case 1:
                    // Multiple the subpackets
                    Int64 product = 1;
                    foreach (Packet sub in p.Subpackets)
                    {
                        product *= EvaluatePacket(sub);
                    }
                    return product; 

                case 2:
                    // Take the mininum of the subpackets
                    Int64 min = Int64.MaxValue;
                    foreach (Packet sub in p.Subpackets)
                    {
                        min = Math.Min(min, EvaluatePacket(sub));
                    }
                    return min;

                case 3:
                    // Take the maximum of the subpackets
                    Int64 max = Int64.MinValue;
                    foreach (Packet sub in p.Subpackets)
                    {
                        max = Math.Max(max, EvaluatePacket(sub));
                    }
                    return max;

                case 4:
                    // Just return the value of the literal packet
                    return p.Value;

                case 5:
                    // Return 1 if first packet is greater than second packet, 0 otherwise

                    return (EvaluatePacket(p.Subpackets[0]) > EvaluatePacket(p.Subpackets[1])) ? 1 : 0;

                case 6:
                    // Return 1 if first packet is less than second packet, 0 otherwise

                    return (EvaluatePacket(p.Subpackets[0]) < EvaluatePacket(p.Subpackets[1])) ? 1 : 0;

                case 7:
                    // Return 1 if first packet is equal to second packet, 0 otherwise

                    return (EvaluatePacket(p.Subpackets[0]) == EvaluatePacket(p.Subpackets[1])) ? 1 : 0;

                default:
                    break;
            }

            // Something went wrong
            return 0;
        }
    }

    public class Packet
    {
        public int Version { get; set; }
        public int Type { get; set; }
        public int PacketLengthType { get; set; }
        public List<Packet> Subpackets { get; set; } = new List<Packet>();
        public Int64 Value { get; set; }
        public Int64 NetValue
        {
            get
            {
                return Value + Subpackets.Sum(p => p.Value);
            }
        }

        public int NetVersion
        {
            get
            {
                return Version + Subpackets.Sum(p => p.NetVersion);
            }
        }

        public Packet() { }

        public Packet(int version, int type)
        {
            Version = version;
            Type = type;
        }
    }
}
