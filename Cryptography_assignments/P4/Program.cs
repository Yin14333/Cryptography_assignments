using System;
using System.Numerics;

namespace P4
{
    class Program
    {
        //Encryption 
        private static BigInteger EncryptRSA(BigInteger m, int e, BigInteger n)
        {
            BigInteger enResult = BigInteger.ModPow(m, e, n);
            return enResult;
        }

        //Decryption
        public static BigInteger DecryptRSA(BigInteger m, BigInteger n, BigInteger d)
        {
            //BigInteger mPowe = BigInteger.Pow(m, e);
            BigInteger deResult = BigInteger.ModPow(m, d, n);
            return deResult;
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter a Hex string.");
            }
            
            int pExponent = Int32.Parse(args[0]);
            BigInteger pBase = BigInteger.Parse(args[1]);
            int qExponent = Int32.Parse(args[2]);
            BigInteger qBase = BigInteger.Parse(args[3]);
            BigInteger cipherT = BigInteger.Parse(args[4]);
            BigInteger plainT = BigInteger.Parse(args[5]);
            int e = 65537;

            
            //Calculate p, q, N
            BigInteger pValue = BigInteger.Pow(2, pExponent) - pBase;
            BigInteger qValue = BigInteger.Pow(2, qExponent) - qBase;
            BigInteger nValue = pValue * qValue;
            BigInteger phiN = (pValue - 1) * (qValue - 1);

            //Calculate d
            int k;
            BigInteger dTemp;
            BigInteger dValue = 0;

            for(k = 0; k < e; k++)
            {
                dTemp = (1 + k * phiN) / e;
                if ((e * dTemp % phiN) == 1)
                {
                    dValue = dTemp;
                }
            }
 
            //Encrypt
            BigInteger encryptResult = EncryptRSA(plainT, e, nValue);

            //Decrypt
            BigInteger decryptResult = DecryptRSA(cipherT, nValue, dValue);

            Console.Write(decryptResult + "," + encryptResult);
        }
    }
}
