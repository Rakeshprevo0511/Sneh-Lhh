using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnehDLL
{
    public class DoctorMast_Dll
    {
        public int DoctorID { get; set; }
        public string UniqueID { get; set; }
        public string PreFix { get; set; }
        public string FullName { get; set; }
        public string Qualification { get; set; }
        public int SpecialityID { get; set; }
        public int WorkplaceID { get; set; }
        public string WorkplaceOther { get; set; }
        public string cAddress { get; set; }
        public int CountryID { get; set; }
        public int StateID { get; set; }
        public int CityID { get; set; }
        public string ZipCode { get; set; }
        public string Telephone1 { get; set; }
        public string Telephone2 { get; set; }
        public string MailID { get; set; }
        public string MobileNo { get; set; }
        public string PanCard { get; set; }
        public string Remarks { get; set; }
        public DateTime BirthDate { get; set; }
        public string ClinicAddress { get; set; }
        public string ClinicTel1 { get; set; }
        public string ClinicTel2 { get; set; }
        public int FacilitatorID { get; set; }
        public DateTime JoinDate { get; set; }
        public bool IsEnabled { get; set; }
        public TimeSpan Available1From { get; set; }
        public TimeSpan Available1Upto { get; set; }
        public TimeSpan Available2From { get; set; }
        public TimeSpan Available2Upto { get; set; }
        public string Available1FromChar { get; set; }
        public string Available1UptoChar { get; set; }
        public string Available2FromChar { get; set; }
        public string Available2UptoChar { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public int AddedBy { get; set; }
        public int ModifyBy { get; set; }
        public string Profile_Image_Path { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string Husband_WifeName { get; set; }
        public int QualificationID { get; set; }
        public string BloodGroup { get; set; }
        public string Aadharcard { get; set; }
        public DateTime Anniversary_Date { get; set; }
        public int AppointmentID { get; set; }
        public string ScheduleType { get; set; }
    }
}
