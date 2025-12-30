using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnehDLL
{
    public class Receiption_Dll
    {
        public int ReceiptionID { get; set; }
        public string UniqueID { get; set; }
        public string FullName { get; set; }
        public string Designation { get; set; }
        public string Qualifications { get; set; }
        public string MailID { get; set; }
        public string ContactNo { get; set; }
        public string Emergency_ContactNO { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime Anniversary_Date { get; set; }
        public string Reference_Documents { get; set; }
        public DateTime JoinDate { get; set; }
        public string PancardNo { get; set; }
        public string AadharcardNo { get; set; }
        public string Address { get; set; }
        public string Profile_Image_Path { get; set; }
        public int TYPEID { get; set; }
        public int ID { get; set; }
        public TimeSpan Clinic_Shift_TimeFrom { get; set; }
        public TimeSpan Clinic_Shift_TimeUpto { get; set; }
        public string Clinic_Shift_TimeFromChar { get; set; }
        public string Clinic_Shift_TimeUptoChar { get; set; }
        public string BloodGroup { get; set; }
    }
}
