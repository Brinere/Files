using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DESAlg
{
    class DES
    {
        //Sbox
        private readonly int[,,] SBoxTabl =   
        {
            { // 0
                {14, 4 , 13, 1 , 2 , 15, 11, 8 , 3 , 10, 6 , 12, 5 , 9 , 0 , 7 },
                {0 , 15, 7 , 4 , 14, 2 , 13, 1 , 10, 6 , 12, 11, 9 , 5 , 3 , 8 },
                {4 , 1 , 14, 8 , 13, 6 , 2 , 11, 15, 12, 9 , 7 , 3 , 10, 5 , 0 },
                {15, 12, 8 , 2 , 4 , 9 , 1 , 7 , 5 , 11, 3 , 14, 10, 0 , 6 , 13},
            },
            { // 1
                {15, 1 , 8 , 14, 6 , 11, 3 , 4 , 9 , 7 , 2 , 13, 12, 0 , 5 , 10},
                {3 , 13, 4 , 7 , 15, 2 , 8 , 14, 12, 0 , 1 , 10, 6 , 9 , 11, 5 },
                {0 , 14, 7 , 11, 10, 4 , 13, 1 , 5 , 8 , 12, 6 , 9 , 3 , 2 , 15},
                {13, 8 , 10, 1 , 3 , 15, 4 , 2 , 11, 6 , 7 , 12, 0 , 5 , 14, 9 },
            },
            { // 2
                {10, 0 , 9 , 14, 6 , 3 , 15, 5 , 1 , 13, 12, 7 , 11, 4 , 2 , 8 },
                {13, 7 , 0 , 9 , 3 , 4 , 6 , 10, 2 , 8 , 5 , 14, 12, 11, 15, 1 },
                {13, 6 , 4 , 9 , 8 , 15, 3 , 0 , 11, 1 , 2 , 12, 5 , 10, 14, 7 },
                {1 , 10, 13, 0 , 6 , 9 , 8 , 7 , 4 , 15, 14, 3 , 11, 5 , 2 , 12},
            },
            { // 3
                {7 , 13, 14, 3 , 0 , 6 , 9 , 10, 1 , 2 , 8 , 5 , 11, 12, 4 , 15},
                {13, 8 , 11, 5 , 6 , 15, 0 , 3 , 4 , 7 , 2 , 12, 1 , 10, 14, 9 },
                {10, 6 , 9 , 0 , 12, 11, 7 , 13, 15, 1 , 3 , 14, 5 , 2 , 8 , 4 },
                {3 , 15, 0 , 6 , 10, 1 , 13, 8 , 9 , 4 , 5 , 11, 12, 7 , 2 , 14},
            },
            { // 4
                {2 , 12, 4 , 1 , 7 , 10, 11, 6 , 8 , 5 , 3 , 15, 13, 0 , 14, 9 },
                {14, 11, 2 , 12, 4 , 7 , 13, 1 , 5 , 0 , 15, 10, 3 , 9 , 8 , 6 },
                {4 , 2 , 1 , 11, 10, 13, 7 , 8 , 15, 9 , 12, 5 , 6 , 3 , 0 , 14},
                {11, 8 , 12, 7 , 1 , 14, 2 , 13, 6 , 15, 0 , 9 , 10, 4 , 5 , 3 },
            },
            { // 5
                {12, 1 , 10, 15, 9 , 2 , 6 , 8 , 0 , 13, 3 , 4 , 14, 7 , 5 , 11},
                {10, 15, 4 , 2 , 7 , 12, 9 , 5 , 6 , 1 , 13, 14, 0 , 11, 3 , 8 },
                {9 , 14, 15, 5 , 2 , 8 , 12, 3 , 7 , 0 , 4 , 10, 1 , 13, 11, 6 },
                {4 , 3 , 2 , 12, 9 , 5 , 15, 10, 11, 14, 1 , 7 , 6 , 0 , 8 , 13},
            },
            { // 6
                {4 , 11, 2 , 14, 15, 0 , 8 , 13, 3 , 12, 9 , 7 , 5 , 10, 6 , 1 },
                {13, 0 , 11, 7 , 4 , 9 , 1 , 10, 14, 3 , 5 , 12, 2 , 15, 8 , 6 },
                {1 , 4 , 11, 13, 12, 3 , 7 , 14, 10, 15, 6 , 8 , 0 , 5 , 9 , 2 },
                {6 , 11, 13, 8 , 1 , 4 , 10, 7 , 9 , 5 , 0 , 15, 14, 2 , 3 , 12},
            },
            { // 7
                {13, 2 , 8 , 4 , 6 , 15, 11, 1 , 10, 9 , 3 , 14, 5 , 0 , 12, 7 },
                {1 , 15, 13, 8 , 10, 3 , 7 , 4 , 12, 5 , 6 , 11, 0 , 14, 9 , 2 },
                {7 , 11, 4 , 1 , 9 , 12, 14, 2 , 0 , 6 , 10, 13, 15, 3 , 5 , 8 },
                {2 , 1 , 14, 7 , 4 , 10, 8 , 13, 15, 12, 9 , 0 , 3 , 5 , 6 , 11},
            },

        };

        //IP
        private readonly int[] InitialPermTabl = 
        {
            58, 50, 42, 34, 26, 18, 10, 2, 60, 52, 44, 36, 28, 20, 12, 4,
            62, 54, 46, 38, 30, 22, 14, 6, 64, 56, 48, 40, 32, 24, 16, 8,
            57, 49, 41, 33, 25, 17, 9 , 1, 59, 51, 43, 35, 27, 19, 11, 3,
            61, 53, 45, 37, 29, 21, 13, 5, 63, 55, 47, 39, 31, 23, 15, 7,
        };

        //IP-1
        private readonly int[] InitialPerm_1Tabl =
        {
            40, 8, 48, 16, 56, 24, 64, 32, 39, 7, 47, 15, 55, 23, 63, 31,
            38, 6, 46, 14, 54, 22, 62, 30, 37, 5, 45, 13, 53, 21, 61, 29,
            36, 4, 44, 12, 52, 20, 60, 28, 35, 3, 43, 11, 51, 19, 59, 27,
            34, 2, 42, 10, 50, 18, 58, 26, 33, 1, 41, 9 , 49, 17, 57, 25,
        };

        //PboxExtent
        private readonly int[] PboxExtentTabl =
        {
            32,  1,  2,  3,  4,  5,
             4,  5,  6,  7,  8,  9,
             8,  9, 10, 11, 12, 13,
            12, 13, 14, 15, 16, 17,
            16, 17, 18, 19, 20, 21,
            20, 21, 22, 23, 24, 25,
            24, 25, 26, 27, 28, 29,
            28, 29, 30, 31, 32,  1
        };

        //PboxStraight
        private readonly int[] PboxStraightTabl =
        {
            16, 7 , 20, 21, 29, 12, 28, 17, 1 , 15, 23, 26, 5 , 18, 31, 10,
            2 , 8 , 24, 14, 32, 27, 3 , 9 , 19, 13, 30, 6 , 22, 11, 4 , 25,
        };

        //Deleting Of Key Bits And Permutation
        private readonly int[] C0 =
        {
            57, 49, 41, 33, 25, 17, 9 , 1 , 58, 50, 42, 34, 26, 18,
            10, 2 , 59, 51, 43, 35, 27, 19, 11, 3 , 60, 52, 44, 36,
        };
        private readonly int[] D0 =
        {
            63, 55, 47, 39, 31, 23, 15, 7 , 62, 54, 46, 38, 30, 22,
            14, 6 , 61, 53, 45, 37, 29, 21, 13, 5 , 28, 20, 12, 4 ,
        };

        //Cyclic permutation of key
        private readonly int[] CyclicPerm =
            {1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1};

        //Permutation of key compression
        private readonly int[] CompressKeyPerm =
        {
            14, 17, 11, 24, 1 , 5 , 3 , 28, 15, 6 , 21, 10,
            23, 19, 12, 4 , 26, 8 , 16, 7 , 27, 20, 13, 2 ,
            41, 52, 31, 37, 47, 55, 30, 40, 51, 45, 33, 48,
            44, 49, 39, 56, 34, 53, 46, 42, 50, 36, 29, 32,
        };

        //LocalKeyforiRaund
        BitArray[] LocalKeysBit = new BitArray[16];
        string[] LocalKeysStr = new string[16];

        //MainKey
        public readonly string MainKey = "";

        //Encoding tabl
        Encoding enc = Encoding.Default;


        public DES(string Key)
        {
            if (Key.Length != 7)
                throw new ArgumentException("Key size must be 7 bytes or 7 symbols");
            MainKey = Key;
            KeysGenerator(Key);
        }

        public static string GenerateKey()
        {
            byte[] buf = new byte[7];
            new Random().NextBytes(buf);
            return Encoding.Default.GetString(buf);
        }

        public BitArray Encrypt(BitArray Bits)
        {
            if (Bits.Length % 8 != 0)
                throw new ArgumentException();
            BitArray 
                outbits = new BitArray(Bits.Length),
                temp64bits = new BitArray(64);
            for (int i = 0; i < Bits.Length; i+=64)
            {
                for (int j = 0; j < 64; j++)
                {
                    temp64bits[j] = Bits[i + j];
                }
                temp64bits = FeistBlockCipher(temp64bits);
                for (int j = 0; j < 64; j++)
                {
                    outbits[i + j] = temp64bits[j];
                }
            }
            return outbits;
        }

        public BitArray Decrypt(BitArray Bits)
        {
            if (Bits.Length % 8 != 0)
                throw new ArgumentException();
            BitArray
                outbits = new BitArray(Bits.Length),
                temp64bits = new BitArray(64);
            for (int i = 0; i < Bits.Length; i += 64)
            {
                for (int j = 0; j < 64; j++)
                {
                    temp64bits[j] = Bits[i + j];
                }
                temp64bits = FeistBlockDeCipher(temp64bits);
                for (int j = 0; j < 64; j++)
                {
                    outbits[i + j] = temp64bits[j];
                }
            }
            return outbits;
        }

        public string Encrypt(string Text)
        {
            while (Text.Length % 8 != 0)
                Text += "0";
            string outmessage = "";
            byte[] tempbytes = enc.GetBytes(Text);
            var a = Encrypt(new BitArray(tempbytes));
            a.CopyTo(tempbytes, 0);
            outmessage = enc.GetString(tempbytes);
            return outmessage;           
        }                                                     
                                                              
        public string Decrypt(string Text)
        {
            while (Text.Length % 8 != 0)
                Text += "0";
            string outmessage = "";
            byte[] tempbytes = enc.GetBytes(Text);
            var a = Decrypt(new BitArray(tempbytes));
            a.CopyTo(tempbytes, 0);
            outmessage = enc.GetString(tempbytes);
            return outmessage;
        }

        private BitArray[] ByteToArrOf64bitSegments(byte[] InValues)
        {
            List<BitArray> bitArrays = new List<BitArray>();
            byte[] bytetmp = new byte[8];
            var temp = new BitArray(64);
            if (InValues.Length % 8 != 0)
                throw new ArgumentException();

            for (int i = 0; i < InValues.Length/8; i++)
            {
                Array.Copy(InValues, i * 8, bytetmp, 0, 8);

                bitArrays.Add(new BitArray(bytetmp));
            }
            return bitArrays.ToArray();
        }

        private BitArray SboxCipher48to32bit(BitArray InBits48)
        {
            if (InBits48.Length != 48)
                throw new ArgumentException("A count of in bits must be 48");
            var outbits32 = new BitArray(32);
            var bitbuf4 = new BitArray(4);
            //var bitbuf6 = new BitArray(4);
            var bitbuf2 = new BitArray(2);

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (j == 0 || j == 5)
                        bitbuf2[j % 2] = InBits48[i * 6 + j];
                    else
                        bitbuf4[j - 1] = InBits48[i * 6 + j];
                }
                int[] a = new int[1];
                int[] b = new int[1];
                bitbuf2.CopyTo(a, 0);
                bitbuf4.CopyTo(b, 0);
                //Finding of new element in tabl
                int newvalue = SBoxTabl[i, a[0], b[0]];
                bitbuf4 = new BitArray(new int[] { newvalue });
                for (int j = 0; j < 4; j++)
                {
                    outbits32[i * 4 + j] = bitbuf4[j];
                }
            }

           return outbits32;
        }

        private BitArray KeyConverter56to48bit(BitArray InBits56)
        {
            var OutBits48 = new BitArray(48);
            for (int i = 0; i < 48; i++)
            {
                OutBits48[i] = InBits56[CompressKeyPerm[i]-1];
            }
            return OutBits48;
        }

        private BitArray Bit64Permutator(BitArray InBits64,int[] PermTab)
        {
            var OutBits48 = new BitArray(64);
            for (int i = 0; i < 64; i++)
            {
                OutBits48[i] = InBits64[PermTab[i] - 1];
            }
            return OutBits48;
        }

        private BitArray DESFunction(BitArray InBits32, BitArray Key48bit)
        {
            if (InBits32.Length != 32 || Key48bit.Length != 48)
                throw new ArgumentException();

            var OutBits32 = new BitArray(32);
            var buf48bit = new BitArray(48);
            
            for (int i = 0; i < 48; i++)
            {   //PboxExt
                buf48bit[i] = InBits32[PboxExtentTabl[i]-1];
                //XOR with key
                buf48bit[i] ^= Key48bit[i];
            }

            //Sbox
            buf48bit = SboxCipher48to32bit(buf48bit);

            //Straight Sbox
            for (int i = 0; i < 32; i++)
            {
                OutBits32[i] = buf48bit[PboxStraightTabl[i]-1];
            }
            return OutBits32;
        }

        private BitArray FeistBlockCipher(BitArray InBlock64bit)
        {
            BitArray
                Li32bit = new BitArray(32),
                Ri32bit = new BitArray(32),
                temp = new BitArray(32);

            BitArray
                OutBlock64bit = new BitArray(64),
                buf64bit = new BitArray(64);

            //Initial permutation
            buf64bit = Bit64Permutator(InBlock64bit, InitialPermTabl);

            //Separation to 2 parts
            BitSeparator(buf64bit, out Li32bit, out Ri32bit);


            //Rounds
            for (int i = 0;i < 16;i++)
            {
                temp = new BitArray(DESFunction(Li32bit, LocalKeysBit[i]).Xor(Ri32bit));
                Ri32bit = new BitArray(Li32bit);
                Li32bit = new BitArray(temp);

            }


            //Joining of 2 parts
            buf64bit = BitJoiner(Li32bit, Ri32bit);

            //Final permutation
            OutBlock64bit = Bit64Permutator(buf64bit, InitialPerm_1Tabl);

            return OutBlock64bit;
        }

        private BitArray FeistBlockDeCipher(BitArray InBlock64bit)
        {
            BitArray
                Li32bit = new BitArray(32),
                Ri32bit = new BitArray(32),
                temp = new BitArray(32);

            BitArray
                OutBlock64bit = new BitArray(64),
                buf64bit = new BitArray(64);

            //Initial permutation
            buf64bit = Bit64Permutator(InBlock64bit, InitialPermTabl);

            //Separation to 2 parts
            BitSeparator(buf64bit, out Li32bit, out Ri32bit);

            //Rounds
            for (int i = 15; i >= 0; i--)
            {
                temp = new BitArray(DESFunction(Ri32bit, LocalKeysBit[i]).Xor(Li32bit));
                Li32bit = new BitArray(Ri32bit);
                Ri32bit = new BitArray(temp);
            }


            //Joining of 2 parts
            buf64bit = BitJoiner(Li32bit, Ri32bit);

            //Final permutation
            OutBlock64bit = Bit64Permutator(buf64bit, InitialPerm_1Tabl);

            return OutBlock64bit;
        }

        private void KeysGenerator(string Key)
        {
            var key56bit = new BitArray(enc.GetBytes(Key));
            if (key56bit.Length != 56)
                throw new ArgumentException("Key must be 56 bit");

            var key64bit = new BitArray(64);
            //Adding of checking bits, ext to 64 bit
            for (int i = 0, j = 0; i < 64; i++)
            {
                if ((i + 1) % 8 == 0)
                {
                    key64bit[i] = false;
                    j--;
                }
                else
                    key64bit[i] = key56bit[i + j];
            }

            BitArray
                Ci = new BitArray(28),
                Di = new BitArray(28),
                PreRaundKey = new BitArray(56);

            //InitPerm
            for (int i = 0; i < 28; i++)
            {
                Ci[i] = key64bit[C0[i]];
                Di[i] = key64bit[D0[i]];
            }
            Encoding ascii = Encoding.ASCII;
            byte[] a = new byte[6];
            //RaundsOfKeyPerm
            for (int i = 0; i < 16; i++)
            {
                Ci = BitLShift(Ci, CyclicPerm[i]);
                Di = BitLShift(Di, CyclicPerm[i]);
                PreRaundKey = BitJoiner(Ci, Di);
                LocalKeysBit[i] = KeyConverter56to48bit(PreRaundKey);
                LocalKeysBit[i].CopyTo(a, 0);
                LocalKeysStr[i] = ascii.GetString(a);
            }

        }

        private BitArray BitLShift (BitArray Bits,int ValueofPerm)
        {
            var outbits = new BitArray(Bits.Length);
            
            for (int i = ValueofPerm; i < Bits.Length; i++)
            {
                outbits[i - ValueofPerm] = Bits[i];
            }

            for (int i = 0; i < ValueofPerm; i++)
                outbits[Bits.Length- ValueofPerm+i] = Bits[i];

            return outbits;
        }

        private void BitSeparator(BitArray AllBits,out BitArray LeftPart, out BitArray RightPart)
        {
            if (AllBits.Length % 2 != 0)
                throw new ArgumentException();
            LeftPart = new BitArray(AllBits.Length / 2);
            RightPart = new BitArray(AllBits.Length / 2);
            for (int i = 0; i < AllBits.Length / 2; i++)
            {
                LeftPart[i] = AllBits[i];
                RightPart[i] = AllBits[i+ AllBits.Length / 2];
            }
        }

        private BitArray BitJoiner(BitArray LeftPart,BitArray RightPart)
        {
            if (LeftPart.Length != RightPart.Length)
                throw new ArgumentException("Size of 2 parts must be the same");
            var OutBits = new BitArray(LeftPart.Length * 2);
            for (int i = 0; i < LeftPart.Length; i++)
            {
                OutBits[i] = LeftPart[i];
                OutBits[i+ LeftPart.Length] = RightPart[i]; 
            }
            return OutBits;
        }

    }
}
