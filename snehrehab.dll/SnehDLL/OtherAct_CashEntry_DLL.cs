using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnehDLL
{
    public class OtherAct_CashEntry_DLL
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
        public string ChequeTxnNo { get; set; }
        public string BankBranch { get; set; }
        public DateTime ChequeDate { get; set; }
        public int HeadID { get; set; }
        public int Account_NameDoctorID { get; set; }
        public int Account_NamePatientID { get; set; }
        public int ProductCatID { get; set; }
        public int ProductID { get; set; }
        public string Doctor { get; set; }
        public int DoctorID { get; set; }
        public string Ass_Doctor { get; set; }
        public int Ass_DoctorID { get; set; }
        public string AccountName { get; set; }
        public string ProductCategory { get; set; }
        public string ProductName { get; set; }
        public string Online_TransactionID { get; set; }
        public DateTime Online_TransactionDate { get; set; }
    }
}
