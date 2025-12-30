using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnehDLL
{
    public class PatientBulk_Dll
    {
        public long BulkID { get; set; }
        public string UniqueID { get; set; }
        public int PatientID { get; set; }
        public float Amount { get; set; }
        public int ModePayment { get; set; }
        public int BankID { get; set; }
        public string BankBranch { get; set; }
        public string ChequeTxnNo { get; set; }
        public DateTime ChequeDate { get; set; }
        public string Narration { get; set; }
        public DateTime PaidDate { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public int AddedBy { get; set; }
        public int ModifyBy { get; set; }
        public float BalanceAmount { get; set; }
        public bool IsPackage { get; set; }
    }
}
