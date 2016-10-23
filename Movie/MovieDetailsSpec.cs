using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Reflection;

namespace Movie
{
    public class MovieDetailsSpec
    {
        IEnumerable<ISingleDetail> Details;
        
        private MovieDetailsSpec(IEnumerable<ISingleDetail> xDetails)
        {
            Details = xDetails;
            
        }

        public XElement EnterAMovie()
        {
            return new XElement("movie", Details.Select(t => t.AddMovieDetail()));
        }

        public static MovieDetailsSpec LoadMovieDetailsSpecFromXml(string SpecPath)
        {
            Helper.RegisterImplementations(typeof(ISingleDetail), t => t.GetCustomAttribute<RegisterSingleDetailAttribute>());
            Console.WriteLine(string.Format("Reading Detais Configuration from '{0}'", SpecPath));
            XmlDocument XDoc = new XmlDocument();

            XDoc.Load(SpecPath);
            IEnumerable<ISingleDetail> xDetails = XDoc.DocumentElement.SelectNodes("movie-detail").Cast<XmlNode>().Select(t => SingleDetail.ReadSpec(t)).ToList();
            return new MovieDetailsSpec(xDetails);

        }

    }
}
