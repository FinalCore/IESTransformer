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
        static List<string> iesFileContent = new List<string>();
        string name;
        int lampFlux, numberOfLamps, outFlux, alphaCount, bethaCount;
        double fluxRatio, power, length, width, height;

        public string Name { get; set; }
        public int LampFlux { get; set; }
        public int NumberOfLamps { get; set; }
        public int OutFlux { get; set; }
        public int AlphaCount { get; set; }
        public int BethaCount { get; set; }
        public double FluxRatio { get; set; }
        public double Power { get; set; }

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

        public void ExtractData()
        {
            string dataString = "";
            // Извлекаем строку IES файла, в которой содержится информация о параметрах светильника
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            string pattern = @"\d{1,3}\s{1,4}\d{1,4}\s[\d{1,4}.\d{1-3}]";
            Regex searchKey = new Regex(pattern);
            for (int i = 0; i < iesFileContent.Count; i++)
            {
                if (searchKey.IsMatch(iesFileContent[i]))
                {
                    dataString = iesFileContent[i];
                    break;
                }
            }

            // Извлекаем основные параметры светильника из строки dataString
            pattern = @"\s{2,}";
            dataString = Regex.Replace(dataString, pattern, " ");
            string[] temp = dataString.Split(' ');

            numberOfLamps = int.Parse(temp[0]);
            lampFlux = int.Parse(temp[1]);
            fluxRatio = double.Parse(temp[2]);
            alphaCount = int.Parse(temp[3]);  
            bethaCount = int.Parse(temp[4]);
            width = double.Parse(temp[7]);
            length = double.Parse(temp[8]);
            height = double.Parse(temp[9]);

            //for(int i = 0; i < dataString.Length; i++)
            //{
            //    if (dataString[i] == ' ')
            //    {
            //        string substring = dataString.Substring(0, i);
            //        NumberOfLamps = int.Parse(substring);
            //        //отсекаем исследованную часть строки dataString
            //    }
            //}


        }

        /// <summary>
        /// Метод, извлекающий количество углов фотометрирования, заданных в ies файле.
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public void ExtractAnglesDim(ref int alphaCount, ref int bethaCount)
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
