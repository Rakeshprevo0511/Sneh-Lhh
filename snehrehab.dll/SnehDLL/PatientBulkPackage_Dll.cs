using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnehDLL
{
    public class PatientBulkPackage_Dll
    {
        public long BookingID { get; set; }
        public long BulkID { get; set; }
        public int SessionID { get; set; }
        public int PackageID { get; set; }
        public float AppointmentCharge { get; set; }
        public int AppointmentCount { get; set; }
        public float PackageAmount { get; set; }
        public float UsedAppointmentCount { get; set; }
        public double UsedAppointmentCharge { get; set; }
        public int PatientID { get; set; }
        public int ModePayment { get; set; }
        public int BankID { get; set; }
        public string BankBranch { get; set; }
        public string ChequeTxnNo { get; set; }
        public DateTime ChequeDate { get; set; }
        public DateTime PaidDate { get; set; }
        public string Narration { get; set; }
        public bool IsPackage { get; set; }
        public float Amount { get; set; }
        public string PackageCode { get; set; }
        public int MaximumTime { get; set; }
        public float OneTimeAmt { get; set; }
    }
}
