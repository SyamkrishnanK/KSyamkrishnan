using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Movie
{
   public interface IExportMovie
    {
        string DisplayText
       { get; }

        void Export(IEnumerable<XElement> movies, string path);
    }
}
