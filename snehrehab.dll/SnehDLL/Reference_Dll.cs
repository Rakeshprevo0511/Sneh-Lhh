using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnehDLL
{
    public class Reference_Dll
    {
        public string UniqueID { get; set; }
        public string Prefix { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        public string Website { get; set; }
        public string Address { get; set; }
        public DateTime AddedDate { get; set; }
        public int ID { get; set; }
        public int PatientID { get; set; }
        public int HospitalID { get; set; }
        public int OnlineID { get; set; }
        public int OtherID { get; set; }
        public int SchoolID { get; set; }
        public int TeacherID { get; set; }
        public int DoctorID { get; set; }
        public int ReferredBy { get; set; }
        public int Ref_SelectID { get; set; }
    }
}
