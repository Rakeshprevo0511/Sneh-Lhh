using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnehDLL
{
    public class UserAccount_Dll
    {
        public int UserID { get; set; }
        public string UniqueID { get; set; }
        public int DoctorID { get; set; }
        public string FullName { get; set; }
        public string MobileNo { get; set; }
        public string MailID { get; set; }
        public string LoginName { get; set; }
        public string LoginPwd { get; set; }
        public string LastLogin { get; set; }
        public int UserCatID { get; set; }
        public bool IsMain { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public int AddedBy { get; set; }
        public int ModifyBy { get; set; }
        public int IsLoggedIn { get; set; }
        public int IsPwdReset { get; set; }
    }
}
