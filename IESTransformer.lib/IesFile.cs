using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IESTransformer.lib
{
    public class IesFile
    {
        public static List<string> iesFileContent = new List<string>(); 
        
        
        /// <summary>
        /// Метод для чтения содержимого IES файла из текстового файла
        /// </summary>
        public void ReadFile(string path)
        {
            Encoding win1251 = Encoding.GetEncoding("Windows-1251");
            string[] iesFileRaw = File.ReadAllLines(path, win1251);
            for (int i = 0; i < iesFileRaw.Length; i++)
            {
                iesFileContent.Add(iesFileRaw[i]);
            }
        }
    }
}
