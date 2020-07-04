using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHA256Alg
{
    class SHA256
    {
        Encoding Enc = Encoding.Default;
        private readonly uint[] s_K =
        {
            0x428a2f98, 0x71374491, 0xb5c0fbcf, 0xe9b5dba5, 0x3956c25b, 0x59f111f1, 0x923f82a4, 0xab1c5ed5,
            0xd807aa98, 0x12835b01, 0x243185be, 0x550c7dc3, 0x72be5d74, 0x80deb1fe, 0x9bdc06a7, 0xc19bf174,
            0xe49b69c1, 0xefbe4786, 0x0fc19dc6, 0x240ca1cc, 0x2de92c6f, 0x4a7484aa, 0x5cb0a9dc, 0x76f988da,
            0x983e5152, 0xa831c66d, 0xb00327c8, 0xbf597fc7, 0xc6e00bf3, 0xd5a79147, 0x06ca6351, 0x14292967,
            0x27b70a85, 0x2e1b2138, 0x4d2c6dfc, 0x53380d13, 0x650a7354, 0x766a0abb, 0x81c2c92e, 0x92722c85,
            0xa2bfe8a1, 0xa81a664b, 0xc24b8b70, 0xc76c51a3, 0xd192e819, 0xd6990624, 0xf40e3585, 0x106aa070,
            0x19a4c116, 0x1e376c08, 0x2748774c, 0x34b0bcb5, 0x391c0cb3, 0x4ed8aa4a, 0x5b9cca4f, 0x682e6ff3,
            0x748f82ee, 0x78a5636f, 0x84c87814, 0x8cc70208, 0x90befffa, 0xa4506ceb, 0xbef9a3f7, 0xc67178f2
        };

        private readonly uint[] h_K =
        {
            0x6A09E667,
            0xBB67AE85,
            0x3C6EF372,
            0xA54FF53A,
            0x510E527F,
            0x9B05688C,
            0x1F83D9AB,
            0x5BE0CD19,
        };

        public SHA256()
        {

        }

        private byte[] PreparingOfMessage(string Message)
        {

            List<byte> temp = new List<byte>(Enc.GetBytes(Message));

            byte initialadding = 0b_1000_0000;
            byte postadding = 0b_0000_0000;
           
            temp.Add(initialadding);

            while ((temp.Count*8) % 512 != 448)
                temp.Add(postadding);

            temp.AddRange(BitConverter.GetBytes((long)Message.Length));

            return temp.ToArray();
        }

        public byte[] GetHashinBytes(string Message)
        {
            string hash = GetHash(Message);
            byte[] 
                outbytes = new byte[32],
                buf = new byte[4];

            UInt32 temp=0;
            for (int i = 0; i < 8; i++)
            {
                temp = Convert.ToUInt32(hash.Substring((hash.Length / 8) * i, hash.Length / 8), 16);
                buf = BitConverter.GetBytes(temp);
                Array.Copy(buf, 0, outbytes, i * 4, 4);
            }
            return outbytes;
        }
        public string GetHash(string Message)
        {
            string outmessage = "";
            byte[] ExtendedMess = PreparingOfMessage(Message);

            uint[] datawords = new uint[64];
            uint
                s0 = 0,
                s1 = 0,
                t1 = 0,
                t2 = 0,
                a = 0,
                b = 0,
                c = 0,
                d = 0,
                e = 0,
                f = 0,
                g = 0,
                h = 0;



            for (int Segment = 0; Segment < ExtendedMess.Length/64; Segment++)
            {
                //Separating text to 16 words
                for (int i = 0; i < 16; i++)
                {
                    datawords[i] = BitConverter.ToUInt32(ExtendedMess, i * 4);
                }

                //Generating of 48 words
                for (int i = 16; i < 64; i++)
                {
                    s0 = BitRShift(datawords[i - 15], 7) ^ BitRShift(datawords[i - 15], 18) ^ (datawords[i - 15] >> 3);
                    s1 = BitRShift(datawords[i - 2], 17) ^ BitRShift(datawords[i - 2], 19) ^ (datawords[i - 2] >> 10);
                    datawords[i] = datawords[i - 16] + s0 + datawords[i - 7] + s1;
                 
                }

                //Init of helping elements
                a = h_K[0];
                b = h_K[1];
                c = h_K[2];
                d = h_K[3];
                e = h_K[4];
                f = h_K[5];
                g = h_K[6];
                h = h_K[7];

                //Main cycle
                for (int i = 0; i < 64; i++)
                {
                    t2 = Sum0(a) + Ma(a, b, c);
                    t1 = h + Sum1(e) + Ch(e, f, g) + s_K[i] + datawords[i];
                    h = g;
                    g = f;
                    f = e;
                    e = d + t1;
                    d = c;
                    c = b;
                    b = a;
                    a = t1 + t2;
                }

                h_K[0] = h_K[0] + a;
                h_K[1] = h_K[1] + b;
                h_K[2] = h_K[2] + c;
                h_K[3] = h_K[3] + d;
                h_K[4] = h_K[4] + e;
                h_K[5] = h_K[5] + f;
                h_K[6] = h_K[6] + g;
                h_K[7] = h_K[7] + h;


            }   

            for(int i = 0;i< 8;i++)
            {
                outmessage += Convert.ToString(h_K[i], 16);
            }

            ReInitializeHash();
                
            return outmessage;
        }

        private uint BitRShift(uint Value,int Valueofperm)
        {
            return ((Value >> Valueofperm) | (Value << (32 - Valueofperm)));

        }

        private void ReInitializeHash()
        {
            h_K[0] = 0x6A09E667;
            h_K[1] = 0xBB67AE85;
            h_K[2] = 0x3C6EF372;
            h_K[3] = 0xA54FF53A;
            h_K[4] = 0x510E527F;
            h_K[5] = 0x9B05688C;
            h_K[6] = 0x1F83D9AB;
            h_K[7] = 0x5BE0CD19;
        }

        private uint Ch(uint E, uint F, uint G)
        {
            return (E & F) ^ (~E & G);
        }

        private uint Ma(uint A, uint B, uint C)
        {
            return (A & B) ^ (A & C) ^ (B & C);
        }

        private uint Sum0(uint A)
        {
            return BitRShift(A, 2) ^ BitRShift(A, 13) ^ BitRShift(A, 22);
        }

        private uint Sum1(uint E)
        {
            return BitRShift(E, 6) ^ BitRShift(E, 11) ^ BitRShift(E, 25);
        }
    }
}
