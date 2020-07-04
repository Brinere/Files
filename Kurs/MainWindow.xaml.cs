using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ElGamalSA;
using SHA256Alg;
using DESAlg;

namespace Kurs
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        ElGamal DSA = new ElGamal();

        string textbuf1 = "";
        string textbuf2 = "";
        //________________________________
        string MainDirPath = "D:\\Cryptology\\Kurs\\InfoFile\\";
        string CurrentPath = "";
        string CurrentFileName = "";
        string CurrentFileNameWE = "";
        //________________________________
        public MainWindow()
        {
            InitializeComponent();

        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            //FileOpener a = new FileOpener();
            //CurrentPath = a.GetFilePath;
            //FilePathTB.Text = "Адрес відчиненого файлу: " + CurrentPath;
            //MainOutTB.Text = a.GetFileString;
            FileOpener();
        }

        private void SignClick(object sender, RoutedEventArgs e)
        {
            try
            {
                FileOpener();
                //var des = new DES(KeyForEncriptionTB.Text);
                string textbuf = MainOutTB.Text;
                textbuf1 = textbuf;
                string
                    hash = new SHA256().GetHash(textbuf);
                string OpenkeyStr = File.ReadAllText(MainDirPath + "OpenKey.ok");
                var tp = new TextParser(OpenkeyStr);

                var pkey = new PublicKey<BigInteger, BigInteger, BigInteger>(tp.p, tp.g, tp.q);

                var sign = DSA.GenerateSign(hash, pkey);
                string outstring =
                    $"r:<{sign.r}>\n\n" +
                    $"s:<{sign.s}>\n\n" +
                    $"hash:<{hash}>\n\n" +
                    $"text:<<<<<{textbuf}>>>>> ";

                BigInteger privatekey = DSA.GetPrivateKey;
                BigInteger publickey = DSA.GenerateOpenKey(pkey, privatekey);

                //OpenKeysList.Items.Add(publickey.ToString());
                File.AppendAllText(MainDirPath + "PublicKey.pk", $"publickey:<{publickey}> \n");
                ListBoxUpdater();

                MainOutTB.Text = 
                    outstring +
                    "\n________________________________________________________________________________________\n" +
                    $"Ваш особистий ключ - {privatekey}\n" +
                    $"Ваш відкритий ключ - {publickey}" +
                    $"\n________________________________________________________________________________________\n";
                File.WriteAllText(MainDirPath + CurrentFileName + "SignedInf.si", outstring);
                //ListBoxUpdater();
            }
            catch (FormatException ex)
            { MessageBox.Show("Невірний формат відкритого ключа");}

        }
        private void SignCheckClick(object sender, RoutedEventArgs e)
        {
            try
            {
                FileOpener();
                if(OpenKeysList.SelectedIndex != -1)
                {
                    string textbuf = MainOutTB.Text;

                    var tpRSHT = new TextParser(textbuf);
                    textbuf2 = tpRSHT.text;
                    bool a = textbuf1 == textbuf2;
                    string
                        hash1 = tpRSHT.hash,
                        hash2 = new SHA256().GetHash(tpRSHT.text);
                    string OpenkeyStr = File.ReadAllText(MainDirPath + "OpenKey.ok");

                    var tpPGQ = new TextParser(OpenkeyStr);

                    var pkey = new PublicKey<BigInteger, BigInteger, BigInteger>(tpPGQ.p, tpPGQ.g, tpPGQ.q);
                    var sign = new Sign<BigInteger, BigInteger>(tpRSHT.r, tpRSHT.s);

                    BigInteger openkey = BigInteger.Parse((string)OpenKeysList.Items[OpenKeysList.SelectedIndex]);

                    if (hash1 == hash2)
                    {
                        if(DSA.CheckSign(hash2,pkey,sign,openkey))
                        {
                            MessageBox.Show("Підпис перевірено успішно");
                        }
                        else
                        {
                            MessageBox.Show("Підпис не збігається");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Хеш не співпадає");
                    }
                }
                else
                {
                    MessageBox.Show("Повинно обрати публічний ключ");
                }
            }
            catch
            {
                MessageBox.Show("Невірний формат підпису");
            }




        }

        private void GenerateNewKey(object sender, RoutedEventArgs e)
        {
            var buf = DSA.GeneratePublicKey();
            string outstring =
                $"p:<{buf.p}> \n" +
                $"g:<{buf.g}> \n" +
                $"q:<{buf.q}> \n";
            MainOutTB.Text = "Сгенерований відкритий ключ:\n" + outstring;
            File.WriteAllText(MainDirPath + "OpenKey.ok", outstring);
        }
        private void EncryptButton(object sender, RoutedEventArgs e)
        { 
            if(KeyForEncriptionTB.Text.Length == 7)
            {
                FileOpener();
                var des = new DES(KeyForEncriptionTB.Text);
                MainOutTB.Text = des.Encrypt(MainOutTB.Text);
                File.WriteAllText(MainDirPath + CurrentFileName + "EncryptInf.enc", MainOutTB.Text);
            }
            else
                MessageBox.Show("Розмір ключа має бути 7 символів або 56 біт");

        }

        private void KeyGenerate_Click(object sender, RoutedEventArgs e)
        {
            KeyForEncriptionTB.Text = DES.GenerateKey();
            //MainOutTB.Text = "Ключ сгенеровано.";
        }

        private void DESDecryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (KeyForEncriptionTB.Text.Length == 7)
            {
                
                FileOpener();
                //MainOutTB.Text = "";
                var des = new DES(KeyForEncriptionTB.Text);
                var tp = new TextParser(MainOutTB.Text);
                string textbuf = "";
                if (tp.text != null)
                {
                    textbuf = des.Decrypt(tp.text);
                }
                else
                {
                    textbuf = des.Decrypt(MainOutTB.Text);
                }
                MainOutTB.Text = "";
                var buf = textbuf.Split(new char[] { '.'});
                for (int i = 0; i < buf.Length-1; i++)
                {
                    MainOutTB.Text += buf[i] + ".";
                }
                //MainOutTB.Text = des.Decrypt(MainOutTB.Text);
                File.WriteAllText(MainDirPath + CurrentFileName + "DecryptInf.txt", MainOutTB.Text);
            }
            else
                MessageBox.Show("Розмір ключа має бути 7 символів або 56 біт");
        }

        private void FileOpener()
        {
            FileOpener a = new FileOpener();
            CurrentPath = a.GetFilePath;
            CurrentFileName = CurrentPath.Split(new char[] {'\\' })[CurrentPath.Split(new char[] { '\\' }).Length-1] ;
            CurrentFileNameWE = CurrentPath.Split(new char[] { '.' })[0];
            FilePathTB.Text = "Адрес відчиненого файлу: " + CurrentPath;
            MainOutTB.Text = a.GetFileString;
        }

        private void ListBoxUpdater()
        {
            OpenKeysList.Items.Clear();
            string buf = File.ReadAllText(MainDirPath + "PublicKey.pk");
            var strbuf = buf.Split(new char[] { ' ','\n', '\t' });
            foreach (var item in strbuf)
            {
                if(item != "")
                {
                    var tp = new TextParser(item);
                    OpenKeysList.Items.Add(tp.publickey.ToString());
                }

            }
        }
    }
}
