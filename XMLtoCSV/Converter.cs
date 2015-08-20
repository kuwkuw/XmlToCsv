using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Collections.Generic;

namespace XMLtoCSV
{
    /// <summary>
    /// Converts files in XML format to CSV format.
    /// </summary>
    /// <remarks>
    /// CODE REVIEW SUMMARY:
    /// 0) Read documentation and task more accurate. The task says that path to the file is provided 
    ///    as the first command line argument. But you hardcoded it in the Program.cs.
    /// 1) Performance. Do not use heavy operations. Especially inside of a loop. Search for better/faster 
    ///	   approach.
    /// 2) Better names that help to understand better and faster the purpose of each class/method.
    ///	   With first look on the Converter it's not clear what it does. There are two reasons for it.
    ///	   First:  the name of the class or it's Convert method doesn't says anything about it's purpose. We 
    ///			   only know that it converts something into something. Better class name would help.
    ///	   Second: comments could help with understanding it's purpose. Although better naming would be much 
    ///			   better approach.
    /// 3) Comments:
    ///		- lack of comments for the Converter class and two of it's methods.
    ///		- not enough comments inside of methods. Although they are not so complicated but still
    ///		  it requires some time to understand the algorithm of each method. When you comment 
    ///		  algorithm other developer can read through all comments in the method and understand 
    ///		  it much faster.
    /// 4) Exception. Use more specific exceptions.
    /// 5) "var" keyword. Best practice: 
    ///		DO use it when a type is obvious
    ///			Example: var converter = new Converter();
    /// 
    ///		DO NOT use it when a type is not obvious.
    ///			Example: IEnumerable<XElement> rows = xElement.Elements().Elements( "row" );
    /// 6) Not all lines or code were alighned in a straight line. Use only tabs (Tab key) for formatting.
    ///	   Use Edit -> Advance -> Format Document (Ctrl + K, Ctrl + D) to reformat the document.
    /// </remarks>
    public class Converter
    {
        /// <summary>
        /// Converts a file in XML format to a new file in CSV format.
        /// </summary>
        /// <param name="path">Path to the file to convert.</param>
        public void ConvertToCsv(string path)
        {
            // checking path to file exists.
            if (!File.Exists(path))
                throw new FileNotFoundException(string.Format("File doesn't exist. File: {0}", path));
            // load XML document.
            XElement xElement = XElement.Load(path);
            // read all rows from the document.
            IEnumerable<XElement> rows = xElement.Elements().Elements("row");
            // read headers from the first row.
            IEnumerable<string> header = rows.First().Elements().Select(i => i.Attribute("name").Value);
            // write items in to CSV file.
            using (var file = File.Create(Path.GetFileNameWithoutExtension(path) + ".csv"))
            // IMPORTANT: StreamWriter implements IDisposable so you need to dispose it.
            using (var sw = new StreamWriter(file))
            {
                // write headers.
                sw.WriteLine(ToCsvRow(header.ToArray()));
                // write data.
                foreach (XElement item in rows)
                {
                    sw.WriteLine(ToCsvRow(item.Elements().Select(i => i.Value).ToArray()));
                }
            }
        }
        /// <summary>
        /// Create CSV row.
        /// </summary>
        /// <param name="list">Elements in the row.</param>
        /// <returns>CSV row(string)</returns>
        private string ToCsvRow(string[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                list[i] = CsvItemFormater(list[i]);
            }
            return string.Join(",", list);
        }
        /// <summary>
        /// Format xml item to csv item
        /// </summary>
        /// <param name="xmlItem">Element from xml</param>
        /// <returns>formated item</returns>
        private string CsvItemFormater(string xmlItem)
        {
            // IMPORTANT: this if condition can be optimized. See: https://msdn.microsoft.com/en-us/library/11w09h50(v=vs.110).aspx
            // IMPORTANT: what if a value will contain \n or \r\n? Or a "?
            //			if ( xmlItem.Split( ' ' ).Count() > 1 || xmlItem.Contains( ',' ) )
            //				return String.Format( "\"{0}\"", xmlItem );
            // usually it's better to check for empty/null values as the first condition of a method. To prevent 
            // errors and to not perform other operations on empty values.
            // the better way to check for empty values is String.IsNullOrEmpty or String.IsNullOrWhiteSpace.


            // tokens cen't contain line breacks  
            char[] spesialChars = new[] {'"', ',' };

            // check for empti item
            if (!String.IsNullOrEmpty(xmlItem))
            {
                // check for spasial chars
                if (xmlItem.IndexOfAny(spesialChars) !=-1)
                {
                    xmlItem = xmlItem.Replace("\"", "\"\"");
                    xmlItem = String.Format("\"{0}\"", xmlItem);
                }
                return xmlItem;
            }
            return String.Empty;
        }
    }
}
