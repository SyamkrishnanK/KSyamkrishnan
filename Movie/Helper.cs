using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Movie
{
    public static class Helper
    {
        private static void RegisterDetailHandler<T>(this Dictionary<string, T> dict, string type, T fnDetailConstrucor, bool bOverWrite)
        {
            if (dict.ContainsKey(type))
            {
                if (bOverWrite)
                    dict[type] = fnDetailConstrucor;
            }
            else
                dict.Add(type, fnDetailConstrucor);
        }

        public static void RegisterAllHandlers<T>(this Dictionary<string,Func< T>> dict, string path)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(path);



            IEnumerable<XmlNode> handlerNodes = xDoc.DocumentElement.SelectNodes("handler").Cast<XmlNode>();
            foreach (XmlNode x in handlerNodes)
            {
                string name = x.Attributes["name"].Value;
                string assembly = x.Attributes["assembly-name"].Value;
                string handlerType = x.InnerText;
                var instantiator = GetInstantiatorFn<T>(assembly, handlerType);
                if (instantiator == null)
                    continue;
                dict.RegisterDetailHandler(name, instantiator, true);



            }
        }
        private static Func<T> GetInstantiatorFn<T>(string assembly,string handlerType)
        {

            Type hType = Type.GetType(handlerType + "," + assembly);
            if (!typeof(T).IsAssignableFrom(hType))
                return null;
            return ()=> (T)Activator.CreateInstance(hType);
        }
        //public static IEnumerable<Func<Object[], T>> GetTypeImplementations<T>( string path)
        //{
        
        //    return null;

        //    //IEnumerable<MethodInfo> types = AppDomain.CurrentDomain.GetAssemblies()
        //    //    .SelectMany(s => s.GetTypes())
        //    //    .Where(p => type.IsAssignableFrom(p) && !p.IsInterface)
        //    //    .SelectMany(t => t.GetMethods())
        //    //    .Where(t => t.IsStatic && fnCustomAttributeSelector != null
        //    //    && t.GetParameters().Count() == 0);

        //    //types.ForEach(t => t.Invoke(null, null));


        //}

        public static string parse(XElement movie, int index)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Movie Number ");
            sb.AppendLine((index + 1).ToString());
            movie.Descendants().Select(parseDetail).ForEach(t => { sb.Append(t); });
            return sb.ToString();

        }

        internal static string parseDetail(XElement xEl, int index)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            if (xEl.Attribute("multiple") != null && xEl.Attribute("multiple").Value == "1")
            {
                sb.AppendLineWithPreceedingTab(xEl.Name.ToString(), 1);
                xEl.Descendants().ForEach(t => sb.AppendLineWithPreceedingTab(t.Value, 2));
            }
            else
            {
                sb.AppendWithPreceedingTab(string.Empty, 1);
                sb.AppendFormat("{0} : {1}", xEl.Name.ToString(), xEl.Value);
                sb.AppendLine();
            }
            return sb.ToString();

        }
    }
}
