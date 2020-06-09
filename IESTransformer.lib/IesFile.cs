using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Dynamic;

namespace IESTransformer.lib
{
    public class IesFile
    {
        static List<string> iesFileContent = new List<string>();
        //string name;
        //int lampFlux, numberOfLamps, outFlux, alphaCount, bethaCount;
        //double fluxRatio, power, length, width, height;

        public string Name { get; set; }
        public int LampFlux { get; set; }
        public int NumberOfLamps { get; set; }
        public int OutFlux { get; set; }
        public int AlphaCount { get; set; }
        public int BethaCount { get; set; }
        public double FluxRatio { get; set; }
        public double Power { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public List<double[]> IntencityMatrix = new List<double[]>();

        //Конструктор по умолчанию
        public IesFile() { }

        // Тестовый конструктор
        public IesFile(string name, int outFlux, double power)
        {
            Name = name;
            OutFlux = outFlux;
            Power = power;
        }

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
        /// Метод, извлекающий основные данные о светильнике из ies файла
        /// </summary>
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

            NumberOfLamps = int.Parse(temp[0]);
            LampFlux = int.Parse(temp[1]);
            FluxRatio = double.Parse(temp[2]);
            AlphaCount = int.Parse(temp[3]);  
            BethaCount = int.Parse(temp[4]);
            Width = double.Parse(temp[7]);
            Length = double.Parse(temp[8]);
            Height = double.Parse(temp[9]);            
        }       

        /// <summary>
        /// Метод, извлекающий массив значений сил света из ies файлы
        /// </summary>
        public void ExtractIntencity()
        {
            // отсекаем часть ies файла, выше массива значений сил света
            string pattern = @"360.0";
            Regex searchKey = new Regex(pattern);
            for (int i = 0; i < iesFileContent.Count; i++)
            {
                if (searchKey.IsMatch(iesFileContent[i]))
                {
                    iesFileContent.RemoveRange(0, i + 1);
                    break;
                }
            }

            // переформируем оставшуюсячасть так, чтобы значения сил света в одной плоскости измерений были записаны строго в одной строке
            // Для этого преобразуем коллекцию строк в коллекцию значений типа double
            List<double> intencityVector = new List<double>();
           
            for(int i = 0; i < iesFileContent.Count; i++)
            {
                string[] splittedString = iesFileContent[i].Split(' ');
                for (int j = 0; j < splittedString.Length - 1; j++)
                {
                    intencityVector.Add(Double.Parse(splittedString[j]));
                }
            }
            // "Нарезаем" коллекцию значений сил света по плоскостям измерений
            //double initialValue = intencityVector[0];            
            for(int i = 0; i < BethaCount; i++)
            {
                IntencityMatrix.Add(new double[AlphaCount]);
                intencityVector.CopyTo(i * AlphaCount, IntencityMatrix[i], 0, AlphaCount);               
            }
        }     
    }
}
