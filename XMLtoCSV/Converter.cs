using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XMLtoCSV
{
    public class Converter
    {
        public void Convert(string path)
        {
            //Checking path to file exists.
            if(!File.Exists(path))
                throw new ApplicationException("File don`t exists");

            XElement xElement = XElement.Load(path);

            var rows = xElement.Elements().Elements("row");

            var heade = rows.First().Elements().Select(i => i.Attribute("name").Value);
            //Write items in to CSV file.
            using (var file = File.Create(Path.GetFileNameWithoutExtension(path)+".csv"))
            {
                StreamWriter sw = new StreamWriter(file);
                sw.WriteLine(ToStr(heade));
                foreach (var item in rows)
                {
                    sw.WriteLine(ToStr(item.Elements().Select(i => i.Value)));
                }
            }
        }
        /// <summary>
        /// Create CSV row.
        /// </summary>
        /// <param name="list">Elemets list</param>
        /// <returns>CSV row(string)</returns>
       private string ToStr(IEnumerable<string> list)
        {
            StringBuilder itemStringBuilder = new StringBuilder();
            var itemCounter = 0;
            foreach (var item in list)
            {

                if (itemCounter != list.Count() - 1)//Check for last item
                {
                    itemStringBuilder.Append(XmlItemConverToCsvItem(item) + ",");
                }
                else
                {
                    itemStringBuilder.Append(XmlItemConverToCsvItem(item));
                }
                itemCounter++;
            }
           
            return itemStringBuilder.ToString();
        }

        private string XmlItemConverToCsvItem(string xmlItem)
        {
            if (xmlItem.Split(' ').Count()>1 || xmlItem.Contains(','))
                return String.Format("\"{0}\"",  xmlItem);
            

            if (xmlItem.Equals(String.Empty))
                return "\"\"";
            

            return xmlItem;
        }
    }
}
