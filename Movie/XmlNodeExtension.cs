using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Movie
{
    public static class XmlNodeExtension
    {
        public static T GetAttributeValue<T>(this XmlNode xNode, string name, Func<string, T> fnConvertor)
        {

            XmlAttribute xDisplay = xNode.Attributes[name];
            if (xDisplay != null)
                return fnConvertor(xDisplay.Value);

            return default(T);
        }

        public static string GetAttributeValue(this XmlNode xNode, string name)
        {
            return xNode.GetAttributeValue(name, t => t);
        }

        public static void ForEach<T>(this IEnumerable<T> ls, Action<T> act)
        {
            ls.ToList().ForEach(act);
        }
        //public static void ForEach<T,X>(this IEnumerable<T> ls, Func<T,X> act)
        //{
        //    ls.ToList().ForEach(act);
        //}
        public static void Each<T>(this IEnumerable<T> ls, Action<T, int> act)
        {
            ls.Select((t, i) => { act(t, i); return false; });
        }
    }
}
