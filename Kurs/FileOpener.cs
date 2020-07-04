//using Microsoft.Win32;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs
{
    class FileOpener
    {
        public FileOpener()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "D:\\Cryptology\\Lab3\\Lab3\\bin\\EncryptedInf.txt";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == true)
            {
                //Get the path of specified file
                GetFilePath = openFileDialog.FileName;

                //Read the contents of the file into a stream
                var fileStream = openFileDialog.OpenFile();

                using (StreamReader reader = new StreamReader(fileStream))
                {
                    GetFileString = reader.ReadToEnd();
                }
            }
            
        }
        public string GetFileString { get; private set; }
        public string GetFilePath { get; private set; }
    }
}
