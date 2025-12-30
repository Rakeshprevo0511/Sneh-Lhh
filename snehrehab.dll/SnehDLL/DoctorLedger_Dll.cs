using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnehDLL
{
    public class DoctorLedger_Dll
    {
        public int LedgerID { get; set; }
        public string UniqueID { get; set; }
        public int DoctorID { get; set; }
        public string LedgerHead { get; set; }
        public int LedgerHeadID { get; set; }
        public float CreditAmt { get; set; }
        public float DebitAmt { get; set; }
        public int LinkLedgerID { get; set; }
        public int PayMode { get; set; }
        public string Narration { get; set; }
        public DateTime PayDate { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public int AddedBy { get; set; }
        public int ModifyBy { get; set; }
        public int AppointmentID { get; set; }
        public int PatientID { get; set; }
        public int SessionID { get; set; }
        public int PackageID { get; set; }
        public bool IsSessionEntry { get; set; }
    }
}
