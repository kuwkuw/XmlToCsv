using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLtoCSV
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Convert XML to CSV");
            Converter converter = new Converter();
            try
            {
                converter.Convert("FEHPackageLevelDetail.xml");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }
    }
}
