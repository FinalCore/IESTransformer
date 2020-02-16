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
            string[] iesFileRaw = File.ReadAllLines(path);
            //for (int i = 0; i < iesFileRaw.Length; i++)
            //{
            //    iesFileContent.Add(iesFileRaw[i]);
            //}
        }
    }
}
