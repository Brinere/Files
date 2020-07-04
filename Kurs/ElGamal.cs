using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
//using SHA256Alg;

namespace ElGamalSA
{
    class ElGamal
    {

        Random _random = new Random();

        BigInteger 
            k,
            x,
            p,
            q,
            g;



        public ElGamal()
        {


            //string str = "dghdg";
            //var pkey = GeneratePublicKey();
            //var sign1 = GenerateSign(str, pkey);
            ////var sign2 = GenerateSign("", pkey);
            //var privatekey = GetPrivateKey;
            //var okey = GenerateOpenKey(pkey, privatekey);
            //var okey1 = GenerateOpenKey(pkey, privatekey);
            //var a = CheckSign(str, pkey, sign1, okey);
            //var b = CheckSign(str, pkey, sign1, okey1);


            //////////

        }

        #region PublicMethods
        public PublicKey<BigInteger, BigInteger, BigInteger> GeneratePublicKey()
        {
            //BigInteger
            q = GenerateQ();
            p = GenerateP();
            BigInteger h = GenerateH();
            g = CalculateG(h,p,q);
            return new PublicKey<BigInteger, BigInteger, BigInteger>(p, g, q);

        }

        public BigInteger GetPrivateKey
        {
            get { return x; }
        }

        public Sign<BigInteger, BigInteger> GenerateSign(string hash, PublicKey<BigInteger, BigInteger, BigInteger> pkey)
        {
            p = pkey.p;
            q = pkey.q;
            x = GenerateX();
            k = GenerateK();
            BigInteger
                m = CalculateM(hash),
                r = CalculateR(pkey.g, k, pkey.p, pkey.q),
                s = CalculateS(m, x,r,k, pkey.q);

            return new Sign<BigInteger, BigInteger>(r,s);
        }

        public BigInteger GenerateOpenKey(PublicKey<BigInteger, BigInteger, BigInteger> pkey, BigInteger privatekey)
        {
            return CalculateY(pkey.g, privatekey, pkey.p);
        }

        public bool CheckSign(string hash, PublicKey<BigInteger, BigInteger, BigInteger> pkey, Sign<BigInteger, BigInteger> sign, BigInteger openkey)
        {
            BigInteger
                m = CalculateM(hash);

            BigInteger w = BigInteger.ModPow(sign.s, q - 2, q);
            BigInteger u1 = (m * w) % q;
            BigInteger u2 = (sign.r * w) % q;
            BigInteger v = ((BigInteger.ModPow(pkey.g, u1, p) * BigInteger.ModPow(openkey, u2, p)) % p) % q;

            if (v == sign.r)
                return true;
            return false;

        }
        #endregion

        #region PrivateMethods

        #region CalculateMethods
        private BigInteger CalculateG(BigInteger h, BigInteger p, BigInteger q)
        {
            return BigInteger.ModPow(h,(p-1)/q,p);
        }


        private BigInteger CalculateS(BigInteger m, BigInteger x, BigInteger r, BigInteger k, BigInteger q)
        {
            BigInteger k_1 = BigInteger.ModPow(k, q-2, q);
            return k_1*((m+x*r)%q);
        }

        private BigInteger CalculateR(BigInteger g, BigInteger k, BigInteger p, BigInteger q)
        {
            return BigInteger.ModPow(g, k, p)%q;/*buf%p*///BigInteger.Remainder(BigInteger.Pow(g, (int)k),p);
        }

        private BigInteger CalculateY(BigInteger g, BigInteger x, BigInteger p)
        {
            return BigInteger.ModPow(g, x, p);/*buf%p*///BigInteger.Remainder(BigInteger.Pow(g, (int)k),p);
        }

        private BigInteger CalculateM(string hash)
        {
            byte[] hashinbytes = new SHA256Alg.SHA256().GetHashinBytes(hash);
            Array.Resize<byte>(ref hashinbytes, hashinbytes.Length - 1);
            BigInteger outvalue = BigInteger.Abs( new BigInteger(hashinbytes));
            return outvalue;
        }
        #endregion

        #region GenerateMethods
        private BigInteger GenerateK()
        {
            var numberBytes = new byte[20];
            BigInteger number = 0;

            while (number > q || 1 > number)
            {
                _random.NextBytes(numberBytes);
                number = BigInteger.Abs(new BigInteger(numberBytes));
            }
            return number;
        }

        private BigInteger GenerateQ()
        {

            byte[] temp = new byte[20];
            BigInteger outnumber = 0;
            do
            {
                _random.NextBytes(temp);
                outnumber = BigInteger.Abs(new BigInteger(temp));
            } while (!fermaIsPrime(outnumber));
            return outnumber;
        }

        private BigInteger GenerateP()
        {
            BigInteger P = 0;
            byte[] temp = new byte[128];
            _random.NextBytes(temp);
            BigInteger i = BigInteger.Abs( (new BigInteger(temp)/q)+1);
            while (true)
            {
                P = q * i++;
                //if (P > m)
                //{
                    if (fermaIsPrime(P + 1))
                    {
                        return P + 1;
                    }
                //}
            }
            
        }

        private BigInteger GenerateH()
        {
            var numberBytes = new byte[100];
            BigInteger number = 0;

            while(number > p-2 || 2 > number)
            {
                _random.NextBytes(numberBytes);
                number = new BigInteger(numberBytes);
            }
            return number;
        }

        private BigInteger GenerateX()
        {
            var numberBytes = new byte[20];
            BigInteger number = 0;

            while (number > q || 1 > number)
            {
                _random.NextBytes(numberBytes);
                number = BigInteger.Abs( new BigInteger(numberBytes));
            }
            return number;
        }
        #endregion

        #region HelpingMethods
        bool fermaIsPrime(BigInteger x)
        {
            if (x == 2)
                return true;
            byte[] bytes = new byte[100];
            _random.NextBytes(bytes);
            //srand(time(NULL));
            for (int i = 0; i < 100; i++)
            {
                BigInteger a = (new BigInteger(bytes) % (x - 2)) + 2;
                if (BigInteger.GreatestCommonDivisor(a, x) != 1)
                    return false;
                if (BigInteger.ModPow(a, x - 1, x) != 1)
                    return false;
            }
            return true;
        }

        private static bool IsPrime(BigInteger number)
        {
            if ((number & 1) == 0) return (number == 2);
            UInt64[] initial =
            {
                  2,   3,   5,   7,  11,  13,  17,  19,  23,  29,
                 31,  37,  41,  43,  47,  53,  59,  61,  67,  71,
                 73,  79,  83,  89,  97, 101, 103, 107, 109, 113,
                127, 131, 137, 139, 149, 151, 157, 163, 167, 173,
                179, 181, 191, 193, 197, 199, 211, 223, 227, 229,
                233, 239, 241, 251, 257
            };
            for (int i = 0; i < initial.Length; i++)
            {
                if (BigInteger.GreatestCommonDivisor(number, initial[i]) != 1)
                    return false;
            }
            UInt64 limit = (UInt64)Math.Sqrt((UInt64)number);
            for (UInt64 i = 3; i <= limit; i += 2)
            {
                if ((number % i) == 0) return false;
            }
            return true;
        }
        #endregion

        #endregion





        


    }

    public struct PublicKey<T1,T2,T3>
    {
        public PublicKey(T1 p,T2 g,T3 q)
        {
            this.p = p;
            this.g = g;
            this.q = q;
        }

        public T1 p { get; private set; }
        public T2 g { get; private set; }
        public T3 q { get; private set; }
    }

    public struct Sign<T1, T2>
    {
        public Sign(T1 r, T2 s)
        {
            this.r = r;
            this.s = s;
        }

        public T1 r { get; private set; }
        public T2 s { get; private set; }
   
    }
}
