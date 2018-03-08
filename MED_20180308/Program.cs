using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MED_20180308
{
    class Program
    {
        static void Main(string[] args)
        {
            string refString = "abcdefgh";
            string hypString = "axcdfgyhz";
            CallMED(refString, hypString);
        }
        
        static void CallMED(string refString, string hypString)
        {
            MED med = new MED();
            med.Run(refString, hypString);
            Console.WriteLine("Ref: " + refString);
            Console.WriteLine("Hyp: " + hypString);
            Console.WriteLine("INS = " + med.INS);
            Console.WriteLine("DEL = " + med.DEL);
            Console.WriteLine("SUB = " + med.SUB);
            Console.WriteLine("REF = " + med.REF);
            Console.WriteLine("ErrorRate = " + med.ErrorRate);
        }
    }
}
