using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Movie
{
    public class PlainTextExporter :ExportMovieBase
    {
        private PlainTextExporter()
        { }
        [RegisterExportMovie]
        public static void RegisterPlainTextExporter()
        {
            RegisterExportMovieHandler("plain-text", () => new PlainTextExporter());
        }

        #region ExportMovieBase
        public override string DisplayText
        {
            get { return "Export as plain text"; }
        }

        public override void Export(IEnumerable<XElement> movies, string path)
        {
            StringBuilder sb = new StringBuilder();
            movies.Select(Helper.parse).ForEach(t=>{sb.Append(t);});
            File.WriteAllText(path, sb.ToString());
        }

        #endregion
    }
}
