using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnehDLL
{
    public class PatientMast_Dll
    {
        public int PatientID { get; set; }
        public string UniqueID { get; set; }
        public string PatientCode { get; set; }
        public int PatientTypeID { get; set; }
        public string PreFix { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public int AgeYear { get; set; }
        public int AgeMonth { get; set; }
        public int Gender { get; set; }
        public string MobileNo { get; set; }
        public string MailID { get; set; }
        public bool HasSchool { get; set; }
        public string SchoolingYear { get; set; }
        public string SchoolName { get; set; }
        public string SchoolRemark { get; set; }
        public string rAddress { get; set; }
        public string cAddress { get; set; }
        public int CategoryID { get; set; }
        public int CountryID { get; set; }
        public int StateID { get; set; }
        public int CityID { get; set; }
        public string ZipCode { get; set; }
        public string TelephoneNo { get; set; }
        public string FatherName { get; set; }
        public string FatherOccupation { get; set; }
        public string FatherMailID { get; set; }
        public string FatherMobileNo { get; set; }
        public string MotherName { get; set; }
        public string MotherOccupation { get; set; }
        public string MotherMailID { get; set; }
        public string MotherMobileNo { get; set; }
        public string RegistrationCode { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int ReferredBy { get; set; }
        public int ConsultedBy { get; set; }
        public string VisitPurpose { get; set; }
        public string ChiefComplaints { get; set; }
        public string MedicalHistory { get; set; }
        public string BriefHistory { get; set; }
        public string PreferredTime { get; set; }
        public int AdultCaseID { get; set; }
        public string Investigation { get; set; }
        public string MedicalAdvice { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public int AddedBy { get; set; }
        public int ModifyBy { get; set; }
        public bool IsPrinted { get; set; }
        public DateTime PrintedDate { get; set; }
        public int PaymentType { get; set; }
        public string ImagePath { get; set; }
        public bool Is_Transfer { get; set; }
        public string MrNo { get; set; }
        public int Ref_Selected { get; set; }
        public string DiagnosisID { get; set; }
        public string DiagnosisOther { get; set; }
    }
}