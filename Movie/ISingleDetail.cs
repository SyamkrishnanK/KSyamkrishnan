using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Movie
{
    public interface ISingleDetail
    {
       XElement AddMovieDetail();
       void Parse(XmlNode xNode);
       
    }
}
