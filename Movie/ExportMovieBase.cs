using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Movie
{
    public abstract class ExportMovieBase : IExportMovie
    {
        static Dictionary<string, Func<IExportMovie>> _dict = new Dictionary<string, Func<IExportMovie>>();


        public static void RegisterExportMovieHandler(string type, Func<IExportMovie> fnDetailConstrucor, bool bOverWrite = false)
        {
            _dict.RegisterDetailHandler(type, fnDetailConstrucor, bOverWrite);
        }

        internal static void RegisterAllExportHandlers()
        {
            Helper.RegisterImplementations(typeof(IExportMovie), t => t.GetCustomAttribute<RegisterExportMovieAttribute>());
        }
        internal static IEnumerable<Func<IExportMovie>> GetAllHandlers()
        {
            return _dict.Values;
        }
       

        #region IExportMovie
        public abstract string DisplayText
        {
            get;
        }

        public abstract void Export(IEnumerable<System.Xml.Linq.XElement> movies, string path);

        #endregion
    }
}
