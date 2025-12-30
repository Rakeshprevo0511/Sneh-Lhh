using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnehDLL
{
    public class PatientPackage_Dll
    {
        public int BookingID { get; set; }
        public string UniqueID { get; set; }
        public int PatientID { get; set; }
        public int SessionID { get; set; }
        public int PackageID { get; set; }
        public float AppointmentCharge { get; set; }
        public float ExtraCharge { get; set; }
        public float AppointmentCount { get; set; }
        public float PackageAmount { get; set; }
        public int ModePayment { get; set; }
        public int BankID { get; set; }
        public string Narration { get; set; }
        public DateTime ChequeDate { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public int AddedBy { get; set; }
        public int ModifyBy { get; set; }
        public bool IsDiscounted { get; set; }
        public int DiscountType { get; set; }
        public float DiscountValue { get; set; }
        public float DiscountAmt { get; set; }
        public float NetAmt { get; set; }
        public int DiscountedOn { get; set; }
        public float NewSessionCharge { get; set; }
        public string ChequeTxnNo { get; set; }
        public string BankBranch { get; set; }
        public string HospitalReceiptID { get; set; }
        public DateTime HospitalReceiptDate { get; set; }
    }
}
