using System;
using System.IO;
using System.Security.Cryptography;

namespace steganography_yin
{
    public class Program
    {
  
        private static string Encrypt(byte[] key, string secretString)
        {
            DESCryptoServiceProvider csp = new DESCryptoServiceProvider();
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms,
            csp.CreateEncryptor(key, key), CryptoStreamMode.Write);
            StreamWriter sw = new StreamWriter(cs);
            sw.Write(secretString);
            sw.Flush();
            cs.FlushFinalBlock();
            sw.Flush();
            return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
        }

        public static void Main(string[] args)
        {
            string plainText = args[0];
            string cipherText = args[1];
            int seed = 0;

            DateTime startTime = new DateTime(2020, 7, 3, 11, 0, 0);
            TimeSpan ts = startTime.Subtract(new DateTime(1970, 1, 1));
            int seedStart = (int)ts.TotalMinutes;


            DateTime endTime = new DateTime(2020, 7, 4, 11, 0, 0);
            TimeSpan ts1 = endTime.Subtract(new DateTime(1970, 1, 1));
            int seedEnd = (int)ts1.TotalMinutes;

            for(int i = seedStart; i <= seedEnd; i++)
            {
                Random rng = new Random(i);
                byte[] key = BitConverter.GetBytes(rng.NextDouble());
                string tempResult = Encrypt(key, plainText);
                if (tempResult == cipherText)
                {
                    seed = i;
                    break;
                }
            }
   
            Console.WriteLine(seed);
        }
    }
}
