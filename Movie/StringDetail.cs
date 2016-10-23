using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Movie
{
    public class StringDetail : SingleDetail
    {
        [RegisterSingleDetailAttribute]
        public static void RegisterStringDetail()
        {
            SingleDetail.RegisterDetailHandler("string", CreateStringDetail);
        }

        private StringDetail() { }

        public static StringDetail CreateStringDetail(XmlNode xNode)
        {
            StringDetail objStringDetail = new StringDetail();
            objStringDetail.Parse(xNode);
            return objStringDetail;

        }


        protected override object GetValue()
        {
            bool flag;
            string st;
            do
            {
                flag = false;
                st = Console.ReadLine();
                if (IsMandatory && string.IsNullOrEmpty(st))
                {
                    Console.WriteLine(string.Format("{0} cannot be empty.", Name));
                    Console.WriteLine(DisplayText);
                    flag = true;
                }

            }
            while (flag);
            return st;
        }
    }
}
