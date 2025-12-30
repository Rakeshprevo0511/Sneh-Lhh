using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace snehrehab
{
    public class rModel
    {
        public bool status { get; set; }
        public string msg { get; set; }
        public dynamic data { get; set; }

        public rModel()
        {
            status = false; msg = null; data = null;
        }

        public class optionMdel
        {
            public optionMdel()
            {
                Option = string.Empty;
                Option1 = string.Empty;
                Option2 = string.Empty;
                Option3 = string.Empty;
                Option4 = string.Empty;
                Option5 = string.Empty;
            }
            public string Option { get; set; }
            public string Option1 { get; set; }
            public string Option2 { get; set; }
            public string Option3 { get; set; }
            public string Option4 { get; set; }
            public string Option5 { get; set; }
        }

        public string Encode(string base64Decoded)
        {
            if (String.IsNullOrEmpty(base64Decoded)) { return string.Empty; }
            string base64Encoded = string.Empty;
            byte[] data = System.Text.ASCIIEncoding.ASCII.GetBytes(base64Decoded);
            base64Encoded = System.Convert.ToBase64String(data);
            return base64Encoded;
        }

        public string Decode(string base64Encoded)
        {
            if (String.IsNullOrEmpty(base64Encoded)) { return string.Empty; }
            string base64Decoded = string.Empty;
            byte[] data = System.Convert.FromBase64String(base64Encoded);
            base64Decoded = System.Text.ASCIIEncoding.ASCII.GetString(data);
            return base64Decoded;
        }
    }

    public class sModel
    {
        public sModel()
        {
            IsEnabled = false;
        }

        public string TokenID { get; set; }
        public int UserID { get; set; }
        public DateTime StartDate { get; set; }
        public bool IsPwdReset { get; set; }
        public bool IsForced { get; set; }
        public int DoctorID { get; set; }
        public string FullName { get; set; }
        public string MailID { get; set; }
        public string MobileNo { get; set; }
        public bool IsEnabled { get; set; }
        public string Msg { get; set; }
    }
}