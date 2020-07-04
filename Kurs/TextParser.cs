using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Kurs
{
    class TextParser
    {
        //_______________PATTERNS_______________
        Regex Pattern = new Regex(@"^(?<letter>\w+):<(?<word>\w+)>$");
        //______________________________________
        public TextParser(string Text)
        {
            string[] buf1 = Text.Split(new char[] {' ','\t', '\n' },StringSplitOptions.RemoveEmptyEntries) ;
            string[] buf2 = Text.Split(new string[] { "<<<<<" }, StringSplitOptions.RemoveEmptyEntries);


            if (buf2.Length == 2)
            {
                    var tempbuf = buf2[1].
                        Trim(new char[] { ' ','\t','\n' }).
                        Split(new string[] { ">>>>>" }, StringSplitOptions.RemoveEmptyEntries);
                if (tempbuf.Length != 0)
                {    
                        text = tempbuf[0];
                }
            }

            //}
            foreach (var i in buf1)
            {
                
                if(Pattern.IsMatch(i))
                {
                    switch(Pattern.Match(i).Groups["letter"].Value)
                    {
                        case ("p"):
                            p = BigInteger.Parse(Pattern.Match(i).Groups["word"].Value);
                            break;
                        case ("q"):
                            q = BigInteger.Parse(Pattern.Match(i).Groups["word"].Value);
                            break;
                        case ("y"):
                            y = BigInteger.Parse(Pattern.Match(i).Groups["word"].Value);
                            break;
                        case ("r"):
                            r = BigInteger.Parse(Pattern.Match(i).Groups["word"].Value);
                            break;
                        case ("s"):
                            s = BigInteger.Parse(Pattern.Match(i).Groups["word"].Value);
                            break;
                        case ("g"):
                            g = BigInteger.Parse(Pattern.Match(i).Groups["word"].Value);
                            break;
                        case ("hash"):
                            hash = Pattern.Match(i).Groups["word"].Value;
                            break;
                        case ("publickey"):
                            publickey = BigInteger.Parse(Pattern.Match(i).Groups["word"].Value);
                            break;
                        case ("text"):

                            break;
                        default:
                            throw new ArgumentException("Невизначений формат");
                    }

                }
            }
            Check();
        }
        public BigInteger p { get; private set; }
        public BigInteger q { get; private set; }
        public BigInteger y { get; private set; }
        public BigInteger r { get; private set; }
        public BigInteger s { get; private set; }
        public BigInteger g { get; private set; }
        public BigInteger publickey { get; private set; }
        public string hash { get; private set; }
        public string text { get; private set; }

        private void Check()
        {
            if (p != 0 && q != 0 && g != 0)
                return;
            else if (r != 0 && s != 0)
                return;
            else if (hash != "")
                return;
            else if (text != "")
                return;
            else if (publickey != 0)
                return;
            else throw new FormatException();
        }

    }
    class TextDeParser
    {
        //_______________PATTERNS_______________
        Regex Pattern = new Regex(@"^(?<letter>\w+):<(?<word>\w+)>$");
        //______________________________________
        public TextDeParser(string Text)
        {
            string[] buf = Text.Split(new char[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var i in buf)
            {
                if (Pattern.IsMatch(i))
                {

                }
            }
            
        }


    }

}
