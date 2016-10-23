using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Movie
{
    public abstract class SingleDetail : ISingleDetail
    {
        #region static members
        static Dictionary<string, Func< ISingleDetail>> _dict = new Dictionary<string, Func< ISingleDetail>>();

        //public static void RegisterDetailHandler(string type, Func<XmlNode, ISingleDetail> fnDetailConstrucor, bool bOverWrite = false)
        //{
        //    _dict.RegisterDetailHandler( type, fnDetailConstrucor, bOverWrite);
        //    //if (_dict.ContainsKey(type))
        //    //{
        //    //    if (bOverWrite)
        //    //        _dict[type] = fnDetailConstrucor;
        //    //}
        //    //else
        //    //    _dict.Add(type, fnDetailConstrucor);
        //}

        internal static void RegisterAllDetailHandlers()
        {
            //Helper.RegisterImplementations(typeof(IExportMovie), t => t.GetCustomAttribute<RegisterExportMovieAttribute>());
            _dict = new Dictionary<string, Func<ISingleDetail>>();
            _dict.RegisterAllHandlers(ConfigurationManager.AppSettings["SingleDetailHandlerPath"]);
        }

        public static ISingleDetail ReadSpec(XmlNode xSpec)
        {
            string stType = xSpec.GetAttributeValue("type");
            ISingleDetail sd;
            if (!string.IsNullOrEmpty(stType) && _dict.ContainsKey(stType.ToLower()))
                sd = _dict[stType]();
            else
                sd = new StringDetail();
            sd.Parse(xSpec);
            return sd;
        }

        #endregion

        public string Name
        {
            get;
            private set;
        }

        string _DisplayText;
        public string DisplayText
        {
            private set
            {
                _DisplayText = value;
            }
            get
            {
                return _DisplayText ?? Name;
            }

        }
        public bool IsMandatory
        {
            get;
            private set;
        }

        public bool AllowMultiple
        {
            get;
            private set;
        }

        string _PersistedText;
        public string PersistedText
        {
            get
            {
                string st = _PersistedText ?? Name;
                return st.ToLower().Replace(" ", "-");
            }
            private set
            {
                _PersistedText = value;
            }
        }

        Func<string, bool> fnStringToBool = t => t.Trim() == "1" ? true : false;
        public virtual void Parse(XmlNode xNode)
        {
            Name = xNode.InnerText;
            DisplayText = xNode.GetAttributeValue("display-text");
            IsMandatory = xNode.GetAttributeValue("mandatory", fnStringToBool);
            AllowMultiple = xNode.GetAttributeValue("allow-multiple", fnStringToBool);
            PersistedText = xNode.GetAttributeValue("persisted-text");
        }
        #region ISIngleDetail

        public virtual XElement AddMovieDetail()
        {
            List<Object> ls = new List<object>();
            string p = string.Empty;
            do
            {
                ls.Add(PromptAndGetValue());
                if (AllowMultiple)
                {
                    Console.WriteLine("If you want to add another press y");
                    p = Console.ReadLine();
                }
            } while (p.ToLower() == "y");
            return ls.Count > 1 ? new XElement(PersistedText, new XAttribute("multiple", "1"), ls.Select(t => new XElement("value", t)))
                : new XElement(PersistedText, ls.First());

        }

        #endregion
        private Object PromptAndGetValue()
        {
            Console.WriteLine(DisplayText);
            return GetValue();
        }
        protected abstract object GetValue();

    }
}
