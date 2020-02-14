using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TransformerIES
{
    class Program
    {
        static void Main(string[] args)
        {
            //string path = @"..\..\IES test files\IntiBEAM IRF9-3W30-5H.ies"; // Путь к ies файлу IntiLED
            //string path = @"..\..\IES test files\ALS_PRS_418.ies";
            //string path = @"..\..\IES test files\IS-AC-AURORA-50 (D60) (2300.RGB.L) (2-2.10.4).ies";
            //string path = @"..\..\IES test files\GALAD Аврора LED-28-Wide_W2200.ies";
            string path = @"..\..\IES test files\ALS_PRS_418.ies";
            IESReader.iesFileRead(path);
            //foreach (var item in IESReader.iesFileContent)
            //Console.WriteLine(item);
            //IESReader.iesFileCut();
            Console.WriteLine("\n");
            IESReader.ExtractAnglesDim(out int alphaCount, out int bethaCount);
            //Console.WriteLine($"{alpha} {betha}");
            IESReader.PurgeIES(alphaCount, bethaCount);
            List<double[]> output = IESReader.ExtractIntensity(alphaCount);
            Display(output);
            Console.ReadKey();
        } 
        

        static void Display<T>(List<T[]> arg)
        {
            for(int i = 0; i < arg.Count; i++)
            {
                foreach (var item in arg[i])
                { Console.Write(item.ToString() + " "); }
                Console.Write("\n");
            }
                
        }
    }
}
