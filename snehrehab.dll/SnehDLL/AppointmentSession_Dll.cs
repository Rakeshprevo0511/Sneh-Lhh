using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnehDLL
{
    public class AppointmentSession_Dll
    {
        public int AppointmentID { get; set; }
        public int BookingID { get; set; }
        public int PackageID { get; set; }
        public float PackageTotalBalance { get; set; }
        public float AppointmentCharge { get; set; }
        public float PackageNewBalance { get; set; }
        public int PaymentType { get; set; }
        public float AmountToPay { get; set; }
        public int BankID { get; set; }
        public string BankBranch { get; set; }
        public string ChequeTxnNo { get; set; }
        public DateTime ChequeDate { get; set; }
        public string Narration { get; set; }
        public int OtherBookingID { get; set; }
        public float OtherBookingTotalBalance { get; set; }
        public float OtherBookingNewBalance { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public int AddedBy { get; set; }
        public int ModifyBy { get; set; }
        public int DeductBookingID { get; set; }
        public int BulkBookingID { get; set; }
        public int BulkPackageID { get; set; }
    }
}
