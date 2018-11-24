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
            string refString = "abcdefghijklmnop";
            string hypString = "abbcxefxhxjkllmnp";
            MED med = new MED();
            med.Run(refString, hypString);
        }        
    }
}