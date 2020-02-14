using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;

namespace TransformerIES
{ //Разбить метод ExtractAnglesDim на две части для удобства отладки
  //TODO разобраться с чтением количества углов измерения силы света вместо 181 и 73 выдает 1817 и 3
    class IESReader
    {

        public static List<string> iesFileContent = new List<string>();
        public static List<string> intensityMatrix = iesFileContent;
        public static string[] iesFileRaw;
        public static string dataIES;
        

        /// <summary>
        /// Метод для чтения содержимого IES файла из текстового файла
        /// </summary>
        public static void iesFileRead(string path)
        {
            iesFileRaw = File.ReadAllLines(path);            
            for (int i = 0; i < iesFileRaw.Length; i++)
            {
                iesFileContent.Add(iesFileRaw[i]);
            }
        }

        /// <summary>
            /// Метод, извлекающий количество углов, заданных в IES файле
            /// </summary>
            /// <returns></returns>
        public static void ExtractAnglesDim(out int alphaCount, out int bethaCount)
        {
            // вырезаем часть ies файла, содержащую в себе информацию о количестве углов фотометрирования 
            CutAnglesData();
            //Извлекаем подстроку, в которой записано количество углов
            string anglePattern = @"\s\d{1,3}\s\d{1,3}\s";
            Regex searchKey = new Regex(anglePattern);
            Match match = searchKey.Match(dataIES);
            string anglesDim = match.Value;
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

        /// <summary>
        ///Метод, удаляющий лишнюю информацию из ies файла
        /// </summary>
        public static void PurgeIES(int alphaCount, int bethaCount)
        {
            //удаляем все строки текстового представления IES файла, идущие до значений углов фотометрирования
            string pattern = @"(\d{1,5}.\d.{1,4}){10,}";
            Regex searchKey = new Regex(pattern);
            for (int i = 1; i < iesFileContent.Count; i++)
            {
                if (searchKey.IsMatch(iesFileRaw[i]))
                {
                    intensityMatrix.RemoveRange(0, i);
                    break;
                }
            }           
            //удаляем значения углов фотометрирования
            int counter = CountValues(intensityMatrix[0]);
            int j = 1;
            while (counter < alphaCount)
            {
                counter += CountValues(intensityMatrix[j]);
                j++;
            }
            counter = CountValues(intensityMatrix[j]);
            while (counter < bethaCount)
            {
                counter += CountValues(intensityMatrix[j]);
                j++;
            }
            intensityMatrix.RemoveRange(0, j + 1);            
        }

        /// <summary>
        /// Метод, извлекющий значения силы света в коллекцию типа List<double[]>
        /// </summary>
        /// <param name="alphaCount"></param>
        /// <returns></returns>
        public static List<double[]> ExtractIntensity(int alphaCount)
        {
            //Считываем значения силы света и записываем их в новую переменную типа List<string>
            List<double[]> intensityValues = new List<double[]>();
            for (int i = 0; i < intensityMatrix.Count; i++)
            {
                int counter = CountValues(intensityMatrix[i]);
                string tempValues = intensityMatrix[i];
                int j = i + 1;
                while(counter < alphaCount)
                {   
                    counter += CountValues(intensityMatrix[j]);
                    tempValues += intensityMatrix[j];
                    j++;
                }
                intensityValues.Add(StringToDouble(tempValues));
                i = j - 1;
            }
            return intensityValues;
        }
        /// <summary>
        /// Метод подчета количества значений силы света в строке
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static int CountValues(string line)
        {
            string[] splittedLine = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int blankStrings = 0;
            for (int i = 0; i < splittedLine.Length; i++)
            {
                char[] lineSymbols = splittedLine[i].ToCharArray();
                if (lineSymbols[0] == ' ')
                    blankStrings++;
            }
            return splittedLine.Length - blankStrings;
        }

        /// <summary>
        /// Метод, преобразующий строку в числовой массив типа double 
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static double[] StringToDouble (string line)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            string[] splittedLine = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            double[] outputValues = new double[splittedLine.Length];
            for (int i = 0; i < splittedLine.Length; i++)
            {
                outputValues[i] = double.Parse(splittedLine[i]);
            }
            return outputValues;            
        }

        public static void CutAnglesData ()
        {
            //Извлекаем строку IES файла, в которой содержится информация о количестве углов, под которыми производились
            //измерения силы света
            string pattern = @"\d\s{1,4}(\d{3,5}|\d{3,5}.\d{1,2})\s\d{1,3}.\d{1,3}\s\d{1,3}\s\d{1,3}";
            Regex searchKey = new Regex(pattern);
            for (int i = 0; i < iesFileContent.Count; i++)
            {
                if (searchKey.IsMatch(iesFileRaw[i]))
                {
                    dataIES = iesFileRaw[i];
                    break;
                }
            }
        }
    }
}
