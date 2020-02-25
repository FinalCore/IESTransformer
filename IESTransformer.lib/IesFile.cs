﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

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

        /// <summary>
        /// Метод, извлекающий количество углов фотометрирования, заданных в ies файле.
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public void ExtractAnglesDim(out int alphaCount, out int bethaCount)
        {
            /* Извлекаем строку IES файла, в которой содержится информация о количестве углов, под которыми производились
               измерения силы света */
            string pattern = @"(\d{ 3,5}.\d{ 1,2})\s\d{ 1,3}.\d{ 1,3}";
            Regex searchKey = new Regex(pattern);
            for (int i = 0; i < iesFileContent.Count; i++)
            {
                if (searchKey.IsMatch(iesFileContent[i]))
                {
                    string anglesString = iesFileContent[i];
                    break;
                }
            }
            pattern = @"\s\d{1,3}\s\d{1,3}\s";
            searchKey = new Regex(pattern);
            string anglesDim = searchKey.Match(pattern).Value;
            //Извлекаем численное значение углов из подстроки
            char[] anglesDimChar = anglesDim.ToCharArray();
            string anglesVert = "";
            string anglesHor = "";
            bool flagVH = true; // флаг, отвечающий за выбор вертикального или горизонтального угла
            for (int i = 1; i < anglesDimChar.Length; i++)
            {
                if (Char.IsDigit(anglesDimChar[i])) // ошибка в этой строке (в anglesDimChar.Length - 2)
                {
                    if (flagVH) anglesVert += anglesDimChar[i];
                    else anglesHor += anglesDimChar[i];
                }
                else flagVH = false;
            }
            alphaCount = int.Parse(anglesVert);
            bethaCount = int.Parse(anglesHor);           
        }
    }
}
