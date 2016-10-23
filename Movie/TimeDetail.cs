using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Movie
{
    public class TimeDetail : SingleDetail
    {
        //[RegisterSingleDetailAttribute]
        //public static void RegisterTimeDetail()
        //{
        //    SingleDetail.RegisterDetailHandler("time", CreateStringDetail);
        //}
        //public static TimeDetail CreateStringDetail(XmlNode xNode)
        //{
        //    TimeDetail objStringDetail = new TimeDetail();
        //    objStringDetail.Parse(xNode);
        //    return objStringDetail;

        //}
        public TimeDetail() { }
        private string GetHour()
        {
            string st;
            int i;
            bool flag;
            do
            {
                flag = false;
                Console.WriteLine("Enter Hour");
                st = Console.ReadLine();
                bool bVlalid = Int32.TryParse(st, out i) && i > -1;
                //if(!string.IsNullOrEmpty(st) && !bVlalid)
                //{

                //}
                if ((IsMandatory || !string.IsNullOrEmpty(st)) && !bVlalid)
                //if ((IsMandatory && !bVlalid) || (!string.IsNullOrEmpty(st) && !bVlalid))
                {
                    Console.WriteLine(st + " is not a valid value for hour");
                    //Console.WriteLine("Enter Hour");
                    flag = true;
                }
            }
            while (flag);
            return i.ToString() + " hours";
        }
        private string GetMinute()
        {
            string st;
            int i = 0;
            bool flag;
            do
            {
                flag = false;
                Console.WriteLine("Enter minutes");
                st = Console.ReadLine();
                bool bVlalid = Int32.TryParse(st, out i) && i < 61 && i > -1;
                if ((IsMandatory || !string.IsNullOrEmpty(st)) && !bVlalid)
                {
                    Console.WriteLine(st + " is not a valid value for minutes");
                    //Console.WriteLine("Enter minutes");
                    flag = true;
                }
            }
            while (flag);
            return i.ToString() + " minutes";
        }

        protected override object GetValue()
        {

            string st = GetHour() + "," + GetMinute();
            if (string.IsNullOrEmpty(st.Trim()))
                return string.Empty;

            return st;//.Replace(" ",".");
        }

    }
}
