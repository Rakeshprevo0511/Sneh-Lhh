using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnehDLL
{
    public class ApiMail_Dll
    {
        public int MailID { get; set; }
        public string SenderName { get; set; }
        public string Smtp { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public int PortNo { get; set; }
        public bool SslEnabled { get; set; }
    }
}
