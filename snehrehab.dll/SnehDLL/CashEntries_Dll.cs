using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnehDLL
{
    public class CashEntries_Dll
    {
        public int LedgerID { get; set; }
        public string UniqueID { get; set; }
        public int AccountType { get; set; }
        public int AccountNameID { get; set; }
        public float CreditAmt { get; set; }
        public float DebitAmt { get; set; }
        public int LinkLedgerID { get; set; }
        public int PayMode { get; set; }
        public string Narration { get; set; }
        public DateTime PayDate { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public int AddedBy { get; set; }
        public int ModifyBy { get; set; }
        public int BankID { get; set; }
        public DateTime ChequeDate { get; set; }
        public int HeadID { get; set; }
        public int DoctorID { get; set; }
        public int PatientID { get; set; }
        public int ProductCatID { get; set; }
        public int ProductID { get; set; }
    }
}
