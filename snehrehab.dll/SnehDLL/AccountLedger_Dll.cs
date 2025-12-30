using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnehDLL
{
    public class AccountLedger_Dll
    {
        public int LedgerID { get; set; }
        public string UniqueID { get; set; }
        public int HeadID { get; set; }
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
    }
}
