using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnehDLL
{
    public class LeaveApplications_Dll
    {
        public int LeaveID { get; set; }
        public string UniqueID { get; set; }
        public int UserID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime UptoDate { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan UptoTime { get; set; }
        public int TypeID { get; set; }
        public string Reason { get; set; }
        public string cAddress { get; set; }
        public string cNumber { get; set; }
        public int LeaveStatus { get; set; } //0 - On Hold, 1 - Approved, 2 - Rejected
        public DateTime StatusDate { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public int AddedBy { get; set; }
        public int ModifyBy { get; set; }
    }
}
