using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnehDLL
{
    public class SupportTicket_Dll
    {
        public int TicketID { get; set; }
        public string TicketCode { get; set; }
        public string UniqueID { get; set; }
        public int UserID { get; set; }
        public string tMessage { get; set; }
        public string aFile { get; set; }
        public string uFile { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public int AddedBy { get; set; }
        public int ModifyBy { get; set; }
        public int cStatus { get; set; }
        public string Remark { get; set; }
        public DateTime StatusDate { get; set; }
        public string yNarration { get; set; }
        public string yRemark { get; set; }
        public int yStatus { get; set; }
        public string yes_no { get; set; }
        public string yes_no_value { get; set; }
    }
}
