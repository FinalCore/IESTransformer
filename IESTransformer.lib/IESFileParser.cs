using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace IESTransformer.lib
{
    public static class IESFileParser
    {
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
    }
}
