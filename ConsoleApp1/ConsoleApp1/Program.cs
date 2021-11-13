using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string val = "123.45";
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("de-DE");
            //  decimal dblval = decimal.Parse(val, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
             double dblval = double.Parse(val, System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat);
          
            Console.WriteLine(dblval);

            Console.ReadLine();
        }
    }
}
