using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace P3
{
    class Program
    {
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

        private static byte[] StrToByteArr(string str)
        {
            string[] strArr = str.Split(' ');
            List<byte> inputHexBytes = new List<Byte>();
            foreach (var hex in strArr)
            {
                byte hexByte = HexstrToByte(hex);
                inputHexBytes.Add(hexByte);
            }
            byte[] HexBytes = inputHexBytes.ToArray();

            return HexBytes;
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }
        
        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                Console.WriteLine("Please enter a Hex string.");
            }
            string inputIV = args[0];
            int nExponent = Int32.Parse(args[3]);
            BigInteger nBase = BigInteger.Parse(args[4]);
            int x = Int32.Parse(args[5]);
            BigInteger gYModN = BigInteger.Parse(args[6]);
            string encryptedTxt = args[7];
            string plainTxt = args[8];

            //Calculate DH key
            int two = 2;
            BigInteger nValue = BigInteger.Pow(two, nExponent) - nBase;
            BigInteger key = BigInteger.ModPow(gYModN, x, nValue);

            //Convert key to byte[]
            byte[] keyBytes = key.ToByteArray();

            //Convert IV hex string into byte[]
            byte[] ivBytes = StrToByteArr(inputIV);

            //Convert encrypted Hex string to byte[]
            byte[] encryptedBytes = StrToByteArr(encryptedTxt);
          
            using (Aes myAes = Aes.Create())
            {
                //Encrypt string to Hex string
                byte[] encrypted = EncryptStringToBytes_Aes(plainTxt, keyBytes, ivBytes);
                string encryptedHexStr = ByteArrayToString(encrypted);
                for (int i = 2; i < encryptedHexStr.Length; i += 2)
                {
                    if (i != encryptedHexStr.Length - 1)
                    {
                        encryptedHexStr = encryptedHexStr.Insert(i, " ");
                        i++;
                    }
                    else
                    {
                        Console.Write(encryptedHexStr[i]);
                    }
                }
                string cipherResult = encryptedHexStr.ToUpper();

                //Decrypted bytes to string
                string decryptedTxt = DecryptStringFromBytes_Aes(encryptedBytes, keyBytes, ivBytes);

                string result = decryptedTxt + "," + cipherResult;
                Console.Write(result);
            }
        }
    }
}
254 1223 251 1339 66536047120374145538916787981868004206438539248910734713495276883724693574434582104900978079701174539167102706725422582788481727619546235440508214694579 1756026041
