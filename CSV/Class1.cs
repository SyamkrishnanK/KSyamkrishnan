using Movie;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CSV
{
    public class CSVExporter : IExportMovie
    {
        public CSVExporter()
        { }
        //[RegisterExportMovie]
        //public static void RegisterPlainTextExporter()
        //{
        //    ExportMovieBase.RegisterExportMovieHandler("csv", () => new CSVExporter());
        //}

        public string DisplayText
        {
            get { return "Export as CSV"; }
        }
        private static string CSVFormat(string t)
        {
            return "\"" + t.Replace('"', '\'') + "\"";
        }

        public void Export(IEnumerable<System.Xml.Linq.XElement> movies, string path)
        {
            IEnumerable<string> titles = movies.SelectMany(t => t.Descendants()).Select(t => t.Name.ToString());
            StringBuilder sb = new StringBuilder();
            titles.ForEach(t => sb.Append(t).Append(","));
            sb.AppendLine();
            foreach (XElement movie in movies)
            {
                foreach (string st in titles)
                {
                    XElement el = movie.Element(st);
                    if (el != null)
                    {
                        var att = el.Attribute("multiple");

                        if (el != null && el.Value == "1")
                        {
                            sb.Append(String.Join("||", el.Descendants("value").Select(t => t.Value).Select(CSVFormat)));
                        }
                        else
                            sb.Append(CSVFormat(el.Value));

                    }
                    sb.Append(",");

                }
                sb.AppendLine();
            }
            File.WriteAllText(path, sb.ToString());
        }
    }
}
