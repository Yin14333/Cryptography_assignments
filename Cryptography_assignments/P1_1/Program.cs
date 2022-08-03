using System;
using System.Collections;

namespace P1_1
{
    class Program
    {
        private static byte[] HexToByteArray(string hexString)
        {
            int intFromHex = Convert.ToByte(hexString, 16);

            string intBinary = Convert.ToString(intFromHex, 2).PadLeft(8, '0');
            char[] digits = intBinary.ToCharArray();
            int[] digitsArray = Array.ConvertAll(digits, c => (int)Char.GetNumericValue(c));

            byte group1 = Convert.ToByte(digitsArray[0] * 2 + digitsArray[1] * 1);
            byte group2 = Convert.ToByte(digitsArray[2] * 2 + digitsArray[3] * 1);
            byte group3 = Convert.ToByte(digitsArray[4] * 2 + digitsArray[5] * 1);
            byte group4 = Convert.ToByte(digitsArray[6] * 2 + digitsArray[7] * 1);

            byte[] oneHexToBytes = { group1, group2, group3, group4 };

            return oneHexToBytes;
        }

        static void Main(string[] args)
        {
            // Add corner case check
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter a Hex string.");
            }

            string[] inputArray = args[0].Split(' ');
            //Prints out individual string from test 
            System.Collections.Generic.List<byte> inputByteList = new System.Collections.Generic.List<byte>();
            foreach (string item in inputArray)
            {

                inputByteList.AddRange(HexToByteArray(item));

            }

            byte[] bmpBytes = new byte[] {
             0x42 , 0x4D , 0x4C , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 ,
             0x00 , 0x00 , 0x1A , 0x00 , 0x00 , 0x00 , 0x0C , 0x00 ,
             0x00 , 0x00 , 0x04 , 0x00 , 0x04 , 0x00 , 0x01 , 0x00 ,
             0x18 , 0x00 , 0x00 , 0x00 , 0xFF , 0xFF , 0xFF , 0xFF ,
             0x00 , 0x00 , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF ,
             0xFF , 0x00 , 0x00 , 0x00 , 0xFF , 0xFF , 0xFF , 0x00 ,
             0x00 , 0x00 , 0xFF , 0x00 , 0x00 , 0xFF , 0xFF , 0xFF ,
             0xFF , 0x00 , 0x00 , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF ,
             0xFF , 0x00 , 0x00 , 0x00 , 0xFF , 0xFF , 0xFF , 0x00 ,
             0x00 , 0x00
            };


            //Convert bytes to a string
            string bmpStr = BitConverter.ToString(bmpBytes);
            string[] results = bmpStr.Split('-');
          
            //for loop body bytes
            for (byte i = 0; i < inputByteList.Count; i++)
            {
                int mask = Convert.ToByte(inputByteList[i]);

                int intFromHex = Convert.ToByte(results[26 + i], 16);
                intFromHex ^= mask;
                string result = intFromHex.ToString("X8").Substring(6, 2);
                results[26 + i] = result;
            }

            for (int i = 0; i < results.Length; i++)
            {
                if(i != results.Length - 1)
                {
                    Console.Write(results[i] + " ");
                }
                else
                {
                    Console.Write(results[i]);
                }                
            }
        }
    }
}
