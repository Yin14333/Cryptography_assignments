using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace P2
{
    class Program
    {
        private const int MAXTRY = 10000000;
        private static Random random = new Random();
        
        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static byte[] StrToByteArr(string str)
        {
            byte[] strToBytes = Encoding.UTF8.GetBytes(str);
            return strToBytes;
        }

        private static byte HexstrToByte(string str)
        {
            byte saltByte = 0;
            try
            {
                saltByte = Byte.Parse(str,
                System.Globalization.NumberStyles.HexNumber);
            }
            catch (OverflowException)
            {
                Console.WriteLine("'{0}' is out of range of a byte.", str);
            }
            return saltByte;
        }

        /// <summary>
        /// Resource from https://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        //input string, output string, MD5 generated salted random string
        public static string SaltedHashInStr(string randomstr, byte salt)
        {
            //convert random string to byte array
            byte[] tempBytes = StrToByteArr(randomstr);

            //add salt byte to it
            List<byte> inputStringList = new List<Byte>(tempBytes);
            inputStringList.Add(salt);
            byte[] listToBytes = inputStringList.ToArray();

            //use MD5 to get the hash
            MD5 md5 = MD5.Create();
            byte[] saltedHashBytes = md5.ComputeHash(listToBytes);

            //convert the hash to string
            string saltedHashStr = ByteArrayToString(saltedHashBytes);

            //extract first 10 chars from the string with substring
            string first10Chars = saltedHashStr.Substring(0, 10);

            return first10Chars;
        }


        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter a Hex string.");
            }

            byte salt = HexstrToByte(args[0]);
            // byte salt = HexstrToByte("C5");

            Dictionary<string, string> cache = new Dictionary<string, string>();

            for (int i = 0; i < MAXTRY; i++)
            {
                string tempStr = RandomString(16);
                string tempKey = SaltedHashInStr(tempStr, salt);

                if (cache.ContainsKey(tempKey))
                {
                    string plainText1 = cache[tempKey];
                    if (!plainText1.Equals(tempStr, StringComparison.OrdinalIgnoreCase))
                    {
                        string result = plainText1 + "," + tempStr;
                        Console.WriteLine(result);
                        return;
                    }
                }

                cache.Add(tempKey, tempStr);
            }
        }
    }
}
