using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSALibrary
{
    internal class GenerateNumber
    {
        private Random _random = new Random();

        public BigInteger GenerateRandomPrimeInRange(BigInteger min, BigInteger max)
        {
            while (true)
            {
                BigInteger candidate = GenerateRandomBigInteger(min, max);
                if (IsPrime(candidate))
                    return candidate;
            }
        }

        private BigInteger GenerateRandomBigInteger(BigInteger min, BigInteger max)
        {
            byte[] bytes = max.ToByteArray();
            BigInteger result;

            do
            {
                _random.NextBytes(bytes);
                result = new BigInteger(bytes);
            } while (result < min || result > max);

            return result;
        }

        private bool IsPrime(BigInteger number, int k = 10)
        {
            if (number <= 1) return false;
            if (number <= 3) return true;
            if (number % 2 == 0 || number % 3 == 0) return false;

            BigInteger d = number - 1;
            int s = 0;

            while (d % 2 == 0)
            {
                d /= 2;
                s += 1;
            }

            for (int i = 0; i < k; i++)
            {
                BigInteger a = GenerateRandomBigInteger(2, number - 2);
                BigInteger x = BigInteger.ModPow(a, d, number);

                if (x == 1 || x == number - 1)
                    continue;

                bool continueOuterLoop = false;

                for (int r = 0; r < s - 1; r++)
                {
                    x = BigInteger.ModPow(x, 2, number);
                    if (x == 1) return false;
                    if (x == number - 1)
                    {
                        continueOuterLoop = true;
                        break;
                    }
                }

                if (!continueOuterLoop)
                    return false;
            }

            return true;
        }
    }

    
}
