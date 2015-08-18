using System;

namespace XMLtoCSV
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine( "Convert XML to CSV" );
			var converter = new Converter();
			try {
				// IMPORTANT: you should have taken path to the file 
				// from command line argument. What if I will rename 
				// the XML file? What if I want to convert another file?
				converter.Convert( "FEHPackageLevelDetail.xml" );
			} catch ( Exception ex ) {
				Console.WriteLine( ex.Message );
			}
			Console.ReadLine();
		}
	}
}
