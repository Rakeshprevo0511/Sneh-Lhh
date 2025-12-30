using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;

namespace SnehBLL
{
    public class PatientMast_Bll
    {
        DbHelper.SqlDb db;

        public PatientMast_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public static int Check(string _uniqueID)
        {
            int _patientID = 0;
            SqlCommand cmd = new SqlCommand("PatientMast_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = _uniqueID;
            cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = 0;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["PatientID"].ToString(), out _patientID);
            }
            return _patientID;
        }

        public static string Check(int _patientID)
        {
            string _uniqueID = "";
            SqlCommand cmd = new SqlCommand("PatientMast_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = "";
            cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = _patientID;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                _uniqueID = dt.Rows[0]["UniqueID"].ToString();
            }
            return _uniqueID;
        }

        public static string GetMail(int patientid, int type)
        {
            string mailid = string.Empty;
            SqlCommand cmd = new SqlCommand("MailID_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = patientid;
            cmd.Parameters.Add("@Type", SqlDbType.Int).Value = type;
            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd); string mailpatient = string.Empty; string fatmail = string.Empty; string motmail = string.Empty;
            if (dt.Rows.Count > 0)
            {
                mailid = dt.Rows[0]["MailID"].ToString();
            }
            return mailid;
        }

        public DataTable GetMailDropdown(int patientid, int type)
        {
            string mailid = string.Empty;
            SqlCommand cmd = new SqlCommand("MailID_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = patientid;
            cmd.Parameters.Add("@Type", SqlDbType.Int).Value = type;
            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd); return dt;
        }

        public int UpdateMail(int patientid, int type, string mail)
        {
            SqlCommand cmd = new SqlCommand("Update_Mail"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = patientid;
            cmd.Parameters.Add("@Type", SqlDbType.Int).Value = type;
            cmd.Parameters.Add("@Mail", SqlDbType.VarChar, 250).Value = mail;
            int i = db.DbUpdate(cmd); return i;
        }

        public DataTable SearchRegPayDetail(int paymode, string _fullName, DateTime _fromDate, DateTime _uptoDate)
        {
            SqlCommand cmd = new SqlCommand("Reg_Payment_Detail"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PayMode", SqlDbType.Int).Value = paymode;
            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 50).Value = _fullName;
            if (_fromDate > DateTime.MinValue)
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = _fromDate;
            else
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = DBNull.Value;
            if (_uptoDate > DateTime.MinValue)
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = _uptoDate;
            else
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = DBNull.Value;

            return db.DbRead(cmd);
        }

        public int Delete(int _patientID)
        {
            SqlCommand cmd = new SqlCommand("PatientMast_Delete"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = _patientID;

            SqlParameter Param = new SqlParameter(); Param.ParameterName = "@RetVal";
            Param.DbType = DbType.Int64; Param.Direction = ParameterDirection.Output;
            Param.Value = 0; cmd.Parameters.Add(Param);

            db.DbUpdate(cmd);

            int i = 0;
            if (cmd.Parameters["@RetVal"].Value != null)
            {
                int.TryParse(cmd.Parameters["@RetVal"].Value.ToString(), out i);
            }
            return i;
        }

        public SnehDLL.PatientMast_Dll Get(int _patientID)
        {
            SnehDLL.PatientMast_Dll D = null;
            SqlCommand cmd = new SqlCommand("PatientMast_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = _patientID;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                D = new SnehDLL.PatientMast_Dll(); int i = 0;
                D.PatientID = int.Parse(dt.Rows[i]["PatientID"].ToString());
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                D.PatientCode = dt.Rows[i]["PatientCode"].ToString();
                D.MrNo = dt.Rows[i]["MrNo"].ToString();
                int PatientTypeID = 0; int.TryParse(dt.Rows[i]["PatientTypeID"].ToString(), out PatientTypeID); D.PatientTypeID = PatientTypeID;
                D.PreFix = dt.Rows[i]["PreFix"].ToString();
                D.FullName = dt.Rows[i]["FullName"].ToString();
                DateTime BirthDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["BirthDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out BirthDate);
                D.BirthDate = BirthDate;
                int AgeYear = 0; int.TryParse(dt.Rows[i]["AgeYear"].ToString(), out AgeYear); D.AgeYear = AgeYear;
                int AgeMonth = 0; int.TryParse(dt.Rows[i]["AgeMonth"].ToString(), out AgeMonth); D.AgeMonth = AgeMonth;
                int Gender = 0; int.TryParse(dt.Rows[i]["Gender"].ToString(), out Gender); D.Gender = Gender;
                D.MobileNo = dt.Rows[i]["MobileNo"].ToString();
                D.MailID = dt.Rows[i]["MailID"].ToString();
                bool HasSchool = true; bool.TryParse(dt.Rows[i]["HasSchool"].ToString(), out HasSchool); D.HasSchool = HasSchool;
                D.SchoolingYear = dt.Rows[i]["SchoolingYear"].ToString();
                D.SchoolName = dt.Rows[i]["SchoolName"].ToString();
                D.SchoolRemark = dt.Rows[i]["SchoolRemark"].ToString();
                D.rAddress = dt.Rows[i]["rAddress"].ToString();
                D.cAddress = dt.Rows[i]["cAddress"].ToString();
                int CategoryID = 0; int.TryParse(dt.Rows[i]["CategoryID"].ToString(), out CategoryID); D.CategoryID = CategoryID;
                int CountryID = 0; int.TryParse(dt.Rows[i]["CountryID"].ToString(), out CountryID); D.CountryID = CountryID;
                int StateID = 0; int.TryParse(dt.Rows[i]["StateID"].ToString(), out StateID); D.StateID = StateID;
                int CityID = 0; int.TryParse(dt.Rows[i]["CityID"].ToString(), out CityID); D.CityID = CityID;
                D.ZipCode = dt.Rows[i]["ZipCode"].ToString();
                D.TelephoneNo = dt.Rows[i]["TelephoneNo"].ToString();
                D.FatherName = dt.Rows[i]["FatherName"].ToString();
                D.FatherOccupation = dt.Rows[i]["FatherOccupation"].ToString();
                D.FatherMailID = dt.Rows[i]["FatherMailID"].ToString();
                D.FatherMobileNo = dt.Rows[i]["FatherMobileNo"].ToString();
                D.MotherName = dt.Rows[i]["MotherName"].ToString();
                D.MotherOccupation = dt.Rows[i]["MotherOccupation"].ToString();
                D.MotherMailID = dt.Rows[i]["MotherMailID"].ToString();
                D.MotherMobileNo = dt.Rows[i]["MotherMobileNo"].ToString();
                D.RegistrationCode = dt.Rows[i]["RegistrationCode"].ToString();
                DateTime RegistrationDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["RegistrationDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out RegistrationDate);
                D.RegistrationDate = RegistrationDate;
                int ReferredBy = 0; int.TryParse(dt.Rows[i]["ReferredBy"].ToString(), out ReferredBy); D.ReferredBy = ReferredBy;
                int ConsultedBy = 0; int.TryParse(dt.Rows[i]["ConsultedBy"].ToString(), out ConsultedBy); D.ConsultedBy = ConsultedBy;
                D.VisitPurpose = dt.Rows[i]["VisitPurpose"].ToString();
                D.ChiefComplaints = dt.Rows[i]["ChiefComplaints"].ToString();
                D.MedicalHistory = dt.Rows[i]["MedicalHistory"].ToString();
                D.BriefHistory = dt.Rows[i]["BriefHistory"].ToString();
                D.PreferredTime = dt.Rows[i]["PreferredTime"].ToString();
                int AdultCaseID = 0; int.TryParse(dt.Rows[i]["AdultCaseID"].ToString(), out AdultCaseID); D.AdultCaseID = AdultCaseID;
                D.Investigation = dt.Rows[i]["Investigation"].ToString();
                D.MedicalAdvice = dt.Rows[i]["MedicalAdvice"].ToString();
                bool IsEnabled = true; bool.TryParse(dt.Rows[i]["IsEnabled"].ToString(), out IsEnabled); D.IsEnabled = IsEnabled;
                DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                D.AddedDate = AddedDate;
                DateTime ModifyDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["ModifyDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ModifyDate);
                D.ModifyDate = ModifyDate;
                int AddedBy = 0; int.TryParse(dt.Rows[i]["AddedBy"].ToString(), out AddedBy); D.AddedBy = AddedBy;
                int ModifyBy = 0; int.TryParse(dt.Rows[i]["ModifyBy"].ToString(), out ModifyBy); D.ModifyBy = ModifyBy;
                bool IsPrinted = false; bool.TryParse(dt.Rows[i]["IsPrinted"].ToString(), out IsPrinted); D.IsPrinted = IsPrinted;
                DateTime PrintedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["PrintedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out PrintedDate);
                D.PrintedDate = PrintedDate;
                int PaymentType = 0; int.TryParse(dt.Rows[i]["PaymentType"].ToString(), out PaymentType); D.PaymentType = PaymentType;
                D.ImagePath = dt.Rows[i]["ImagePath"].ToString();
                int refselected = 0; int.TryParse(dt.Rows[i]["Ref_Selected"].ToString(), out refselected);
                D.Ref_Selected = refselected;
                D.DiagnosisOther = dt.Rows[i]["DiagnosisOther"].ToString();
                D.DiagnosisID = dt.Rows[i]["DiagnosisID"].ToString();
            }
            return D;
        }

        public List<SnehDLL.PatientMast_Dll> GetList()
        {
            List<SnehDLL.PatientMast_Dll> DL = new List<SnehDLL.PatientMast_Dll>();
            SqlCommand cmd = new SqlCommand("PatientMast_GetList"); cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.PatientMast_Dll D = new SnehDLL.PatientMast_Dll();

                D.PatientID = int.Parse(dt.Rows[i]["PatientID"].ToString());
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                int PatientTypeID = 0; int.TryParse(dt.Rows[i]["PatientTypeID"].ToString(), out PatientTypeID); D.PatientTypeID = PatientTypeID;
                D.PreFix = dt.Rows[i]["PreFix"].ToString();
                D.FullName = dt.Rows[i]["FullName"].ToString();
                DateTime BirthDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["BirthDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out BirthDate);
                D.BirthDate = BirthDate;
                int AgeYear = 0; int.TryParse(dt.Rows[i]["AgeYear"].ToString(), out AgeYear); D.AgeYear = AgeYear;
                int AgeMonth = 0; int.TryParse(dt.Rows[i]["AgeMonth"].ToString(), out AgeMonth); D.AgeMonth = AgeMonth;
                int Gender = 0; int.TryParse(dt.Rows[i]["Gender"].ToString(), out Gender); D.Gender = Gender;
                D.MobileNo = dt.Rows[i]["MobileNo"].ToString();
                D.MailID = dt.Rows[i]["MailID"].ToString();
                bool HasSchool = true; bool.TryParse(dt.Rows[i]["HasSchool"].ToString(), out HasSchool); D.HasSchool = HasSchool;
                D.SchoolingYear = dt.Rows[i]["SchoolingYear"].ToString();
                D.SchoolName = dt.Rows[i]["SchoolName"].ToString();
                D.SchoolRemark = dt.Rows[i]["SchoolRemark"].ToString();
                D.rAddress = dt.Rows[i]["rAddress"].ToString();
                D.cAddress = dt.Rows[i]["cAddress"].ToString();
                int CategoryID = 0; int.TryParse(dt.Rows[i]["CategoryID"].ToString(), out CategoryID); D.CategoryID = CategoryID;
                int CountryID = 0; int.TryParse(dt.Rows[i]["CountryID"].ToString(), out CountryID); D.CountryID = CountryID;
                int StateID = 0; int.TryParse(dt.Rows[i]["StateID"].ToString(), out StateID); D.StateID = StateID;
                int CityID = 0; int.TryParse(dt.Rows[i]["CityID"].ToString(), out CityID); D.CityID = CityID;
                D.ZipCode = dt.Rows[i]["ZipCode"].ToString();
                D.TelephoneNo = dt.Rows[i]["TelephoneNo"].ToString();
                D.FatherName = dt.Rows[i]["FatherName"].ToString();
                D.FatherOccupation = dt.Rows[i]["FatherOccupation"].ToString();
                D.FatherMailID = dt.Rows[i]["FatherMailID"].ToString();
                D.FatherMobileNo = dt.Rows[i]["FatherMobileNo"].ToString();
                D.MotherName = dt.Rows[i]["MotherName"].ToString();
                D.MotherOccupation = dt.Rows[i]["MotherOccupation"].ToString();
                D.MotherMailID = dt.Rows[i]["MotherMailID"].ToString();
                D.MotherMobileNo = dt.Rows[i]["MotherMobileNo"].ToString();
                D.RegistrationCode = dt.Rows[i]["RegistrationCode"].ToString();
                DateTime RegistrationDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["RegistrationDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out RegistrationDate);
                D.RegistrationDate = RegistrationDate;
                int ReferredBy = 0; int.TryParse(dt.Rows[i]["ReferredBy"].ToString(), out ReferredBy); D.ReferredBy = ReferredBy;
                int ConsultedBy = 0; int.TryParse(dt.Rows[i]["ConsultedBy"].ToString(), out ConsultedBy); D.ConsultedBy = ConsultedBy;
                D.VisitPurpose = dt.Rows[i]["VisitPurpose"].ToString();
                D.ChiefComplaints = dt.Rows[i]["ChiefComplaints"].ToString();
                D.MedicalHistory = dt.Rows[i]["MedicalHistory"].ToString();
                D.BriefHistory = dt.Rows[i]["BriefHistory"].ToString();
                D.PreferredTime = dt.Rows[i]["PreferredTime"].ToString();
                int AdultCaseID = 0; int.TryParse(dt.Rows[i]["AdultCaseID"].ToString(), out AdultCaseID); D.AdultCaseID = AdultCaseID;
                D.Investigation = dt.Rows[i]["Investigation"].ToString();
                D.MedicalAdvice = dt.Rows[i]["MedicalAdvice"].ToString();
                bool IsEnabled = true; bool.TryParse(dt.Rows[i]["IsEnabled"].ToString(), out IsEnabled); D.IsEnabled = IsEnabled;
                DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                D.AddedDate = AddedDate;
                DateTime ModifyDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["ModifyDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ModifyDate);
                D.ModifyDate = ModifyDate;
                int AddedBy = 0; int.TryParse(dt.Rows[i]["AddedBy"].ToString(), out AddedBy); D.AddedBy = AddedBy;
                int ModifyBy = 0; int.TryParse(dt.Rows[i]["ModifyBy"].ToString(), out ModifyBy); D.ModifyBy = ModifyBy;
                bool IsPrinted = false; bool.TryParse(dt.Rows[i]["IsPrinted"].ToString(), out IsPrinted); D.IsPrinted = IsPrinted;
                DateTime PrintedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["PrintedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out PrintedDate);
                D.PrintedDate = PrintedDate;
                int PaymentType = 0; int.TryParse(dt.Rows[i]["PaymentType"].ToString(), out PaymentType); D.PaymentType = PaymentType;

                DL.Add(D);
            }
            return DL;
        }

        public DataTable Search(int _patientTypeID, string _fullName,
            DateTime _fromDate, DateTime _uptoDate)
        {
            SqlCommand cmd = new SqlCommand("PatientMast_GetSearch"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PatientTypeID", SqlDbType.Int).Value = _patientTypeID;
            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 50).Value = _fullName;
            if (_fromDate > DateTime.MinValue)
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = _fromDate;
            else
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = DBNull.Value;
            if (_uptoDate > DateTime.MinValue)
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = _uptoDate;
            else
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = DBNull.Value;

            return db.DbRead(cmd);
        }

        public static int PatientTypeID(int _patientID)
        {
            DbHelper.SqlDb db = new DbHelper.SqlDb(); int i = 0;
            SqlCommand cmd = new SqlCommand("PatientMast_GetPatientTypeID"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = _patientID;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["PatientTypeID"].ToString(), out i);
            }
            return i;
        }

        public int Set(SnehDLL.PatientMast_Dll D)
        {
            SqlCommand cmd = new SqlCommand("PatientMast_Set"); cmd.CommandType = CommandType.StoredProcedure;
           
            cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = D.PatientID;
            cmd.Parameters.Add("@PatientTypeID", SqlDbType.Int).Value = D.PatientTypeID;
            cmd.Parameters.Add("@MrNo", SqlDbType.VarChar, -1).Value = D.MrNo;
            cmd.Parameters.Add("@PreFix", SqlDbType.VarChar, 50).Value = D.PreFix;
            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 250).Value = D.FullName;
            if (D.BirthDate > DateTime.MinValue)
                cmd.Parameters.Add("@BirthDate", SqlDbType.DateTime).Value = D.BirthDate;
            else
                cmd.Parameters.Add("@BirthDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@AgeYear", SqlDbType.Int).Value = D.AgeYear;
            cmd.Parameters.Add("@AgeMonth", SqlDbType.Int).Value = D.AgeMonth;
            cmd.Parameters.Add("@Gender", SqlDbType.Int).Value = D.Gender;
            cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar, 250).Value = D.MobileNo;

            cmd.Parameters.Add("@MailID", SqlDbType.VarChar, 250).Value = D.MailID;
            cmd.Parameters.Add("@HasSchool", SqlDbType.Bit).Value = D.HasSchool;
            cmd.Parameters.Add("@SchoolingYear", SqlDbType.VarChar, 50).Value = D.SchoolingYear;
            cmd.Parameters.Add("@SchoolName", SqlDbType.VarChar, 250).Value = D.SchoolName;
            cmd.Parameters.Add("@SchoolRemark", SqlDbType.VarChar, 500).Value = D.SchoolRemark;
            cmd.Parameters.Add("@rAddress", SqlDbType.VarChar, 500).Value = D.rAddress;
            cmd.Parameters.Add("@cAddress", SqlDbType.VarChar, 500).Value = D.cAddress;
            cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = D.CategoryID;
            cmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = D.CountryID;
            cmd.Parameters.Add("@StateID", SqlDbType.Int).Value = D.StateID;

            cmd.Parameters.Add("@CityID", SqlDbType.Int).Value = D.CityID;
            cmd.Parameters.Add("@ZipCode", SqlDbType.VarChar, 50).Value = D.ZipCode;
            cmd.Parameters.Add("@TelephoneNo", SqlDbType.VarChar, 250).Value = D.TelephoneNo;
            cmd.Parameters.Add("@FatherName", SqlDbType.VarChar, 250).Value = D.FatherName;
            cmd.Parameters.Add("@FatherOccupation", SqlDbType.VarChar, 250).Value = D.FatherOccupation;
            cmd.Parameters.Add("@FatherMailID", SqlDbType.VarChar, 250).Value = D.FatherMailID;
            cmd.Parameters.Add("@FatherMobileNo", SqlDbType.VarChar, 50).Value = D.FatherMobileNo;
            cmd.Parameters.Add("@MotherName", SqlDbType.VarChar, 250).Value = D.MotherName;
            cmd.Parameters.Add("@MotherOccupation", SqlDbType.VarChar, 250).Value = D.MotherOccupation;
            cmd.Parameters.Add("@MotherMailID", SqlDbType.VarChar, 250).Value = D.MotherMailID;

            cmd.Parameters.Add("@MotherMobileNo", SqlDbType.VarChar, 50).Value = D.MotherMobileNo;
            if (D.RegistrationDate > DateTime.MinValue)
                cmd.Parameters.Add("@RegistrationDate", SqlDbType.DateTime).Value = D.RegistrationDate;
            else
                cmd.Parameters.Add("@RegistrationDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@ReferredBy", SqlDbType.Int).Value = D.ReferredBy;
            cmd.Parameters.Add("@ConsultedBy", SqlDbType.Int).Value = D.ConsultedBy;
            cmd.Parameters.Add("@VisitPurpose", SqlDbType.VarChar, -1).Value = D.VisitPurpose;
            cmd.Parameters.Add("@ChiefComplaints", SqlDbType.VarChar, -1).Value = D.ChiefComplaints;
            cmd.Parameters.Add("@MedicalHistory", SqlDbType.VarChar, -1).Value = D.MedicalHistory;
            cmd.Parameters.Add("@BriefHistory", SqlDbType.VarChar, -1).Value = D.BriefHistory;
            cmd.Parameters.Add("@PreferredTime", SqlDbType.VarChar, 250).Value = D.PreferredTime;
            cmd.Parameters.Add("@AdultCaseID", SqlDbType.Int).Value = D.AdultCaseID;

            cmd.Parameters.Add("@Investigation", SqlDbType.VarChar, 4000).Value = D.Investigation;
            cmd.Parameters.Add("@MedicalAdvice", SqlDbType.VarChar, 4000).Value = D.MedicalAdvice;
            cmd.Parameters.Add("@IsEnabled", SqlDbType.Bit).Value = D.IsEnabled;
            if (D.ModifyDate > DateTime.MinValue)
                cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = D.ModifyDate;
            else
                cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@ModifyBy", SqlDbType.Int).Value = D.ModifyBy;
            cmd.Parameters.Add("@PaymentType", SqlDbType.Int).Value = D.PaymentType;
            cmd.Parameters.Add("@ImagePath", SqlDbType.VarChar).Value = D.ImagePath;
            cmd.Parameters.Add("@Ref_Selected", SqlDbType.VarChar).Value = D.Ref_Selected;
            cmd.Parameters.Add("@DiagnosisID", SqlDbType.VarChar, -1).Value = D.DiagnosisID;
            cmd.Parameters.Add("@DiagnosisOther", SqlDbType.VarChar, -1).Value = D.DiagnosisOther;

            SqlParameter Param = new SqlParameter(); Param.ParameterName = "@RetVal";
            Param.DbType = DbType.Int64; Param.Direction = ParameterDirection.Output;
            Param.Value = 0; cmd.Parameters.Add(Param);

            db.DbUpdate(cmd);

            int i = 0;
            if (cmd.Parameters["@RetVal"].Value != null)
            {
                int.TryParse(cmd.Parameters["@RetVal"].Value.ToString(), out i);
            }
            return i;
        }

        public List<SnehDLL.PatientMast_Dll> GetForDropdown()
        {
            List<SnehDLL.PatientMast_Dll> DL = new List<SnehDLL.PatientMast_Dll>();
            SqlCommand cmd = new SqlCommand("PatientMast_GetForDropdown"); cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.PatientMast_Dll D = new SnehDLL.PatientMast_Dll();
                D.PatientID = int.Parse(dt.Rows[i]["PatientID"].ToString());
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                int PatientTypeID = 0; int.TryParse(dt.Rows[i]["PatientTypeID"].ToString(), out PatientTypeID); D.PatientTypeID = PatientTypeID;
                D.PreFix = dt.Rows[i]["PreFix"].ToString();
                D.FullName = dt.Rows[i]["FullName"].ToString();
                D.RegistrationCode = dt.Rows[i]["RegistrationCode"].ToString();

                DL.Add(D);
            }
            return DL;
        }

        public bool SaveFile(ref System.Web.UI.WebControls.FileUpload txtFile, string _fileName)
        {
            try
            {
                txtFile.SaveAs(Path(true) + _fileName);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static int BulkCheck(string _uniqueID)
        {
            int _patientID = 0;
            SqlCommand cmd = new SqlCommand("PatientMastBULK_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = _uniqueID;
            cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = 0;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["PatientID"].ToString(), out _patientID);
            }
            return _patientID;
        }
        public string Path(bool _full)
        {
            if (_full)
            {
                return HttpContext.Current.Server.MapPath("~/Files/");
            }
            else
            {
                return "/Files/";
            }
        }
        public int PatientType(int _patientID)
        {
            int PatientTypeID = 0;
            SqlCommand cmd = new SqlCommand("SELECT P.PatientTypeID FROM PatientMast P WHERE P.PatientID = @PatientID"); cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = _patientID;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["PatientTypeID"].ToString(), out PatientTypeID);
            }
            return PatientTypeID;
        }
        public int Transfer_PatientSet(SnehDLL.PatientMast_Dll D)
        {
            SqlCommand cmd = new SqlCommand("Transfer_PatientMast_Set"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = D.PatientID;
            cmd.Parameters.Add("@PatientTypeID", SqlDbType.Int).Value = D.PatientTypeID;
            cmd.Parameters.Add("@PreFix", SqlDbType.VarChar, 50).Value = D.PreFix;
            //cmd.Parameters.Add("@MrNo", SqlDbType.VarChar, -1).Value = D.MrNo;
            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 250).Value = D.FullName;
            if (D.BirthDate > DateTime.MinValue)
                cmd.Parameters.Add("@BirthDate", SqlDbType.DateTime).Value = D.BirthDate;
            else
                cmd.Parameters.Add("@BirthDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@AgeYear", SqlDbType.Int).Value = D.AgeYear;
            cmd.Parameters.Add("@AgeMonth", SqlDbType.Int).Value = D.AgeMonth;
            cmd.Parameters.Add("@Gender", SqlDbType.Int).Value = D.Gender;
            cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar, 250).Value = D.MobileNo;
            cmd.Parameters.Add("@MailID", SqlDbType.VarChar, 250).Value = D.MailID;
            cmd.Parameters.Add("@HasSchool", SqlDbType.Bit).Value = D.HasSchool;
            cmd.Parameters.Add("@SchoolingYear", SqlDbType.VarChar, 50).Value = D.SchoolingYear;
            cmd.Parameters.Add("@SchoolName", SqlDbType.VarChar, 250).Value = D.SchoolName;
            cmd.Parameters.Add("@SchoolRemark", SqlDbType.VarChar, 500).Value = D.SchoolRemark;
            cmd.Parameters.Add("@rAddress", SqlDbType.VarChar, 500).Value = D.rAddress;
            cmd.Parameters.Add("@cAddress", SqlDbType.VarChar, 500).Value = D.cAddress;
            cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = D.CategoryID;
            cmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = D.CountryID;
            cmd.Parameters.Add("@StateID", SqlDbType.Int).Value = D.StateID;
            cmd.Parameters.Add("@CityID", SqlDbType.Int).Value = D.CityID;
            cmd.Parameters.Add("@ZipCode", SqlDbType.VarChar, 50).Value = D.ZipCode;
            cmd.Parameters.Add("@TelephoneNo", SqlDbType.VarChar, 250).Value = D.TelephoneNo;
            cmd.Parameters.Add("@FatherName", SqlDbType.VarChar, 250).Value = D.FatherName;
            cmd.Parameters.Add("@FatherOccupation", SqlDbType.VarChar, 250).Value = D.FatherOccupation;
            cmd.Parameters.Add("@FatherMailID", SqlDbType.VarChar, 250).Value = D.FatherMailID;
            cmd.Parameters.Add("@FatherMobileNo", SqlDbType.VarChar, 50).Value = D.FatherMobileNo;
            cmd.Parameters.Add("@MotherName", SqlDbType.VarChar, 250).Value = D.MotherName;
            cmd.Parameters.Add("@MotherOccupation", SqlDbType.VarChar, 250).Value = D.MotherOccupation;
            cmd.Parameters.Add("@MotherMailID", SqlDbType.VarChar, 250).Value = D.MotherMailID;
            cmd.Parameters.Add("@MotherMobileNo", SqlDbType.VarChar, 50).Value = D.MotherMobileNo;
            if (D.RegistrationDate > DateTime.MinValue)
                cmd.Parameters.Add("@RegistrationDate", SqlDbType.DateTime).Value = D.RegistrationDate;
            else
                cmd.Parameters.Add("@RegistrationDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@ReferredBy", SqlDbType.Int).Value = D.ReferredBy;
            cmd.Parameters.Add("@ConsultedBy", SqlDbType.Int).Value = D.ConsultedBy;
            cmd.Parameters.Add("@VisitPurpose", SqlDbType.VarChar, -1).Value = D.VisitPurpose;
            cmd.Parameters.Add("@ChiefComplaints", SqlDbType.VarChar, -1).Value = D.ChiefComplaints;
            cmd.Parameters.Add("@MedicalHistory", SqlDbType.VarChar, -1).Value = D.MedicalHistory;
            cmd.Parameters.Add("@BriefHistory", SqlDbType.VarChar, -1).Value = D.BriefHistory;
            cmd.Parameters.Add("@PreferredTime", SqlDbType.VarChar, 250).Value = D.PreferredTime;
            cmd.Parameters.Add("@AdultCaseID", SqlDbType.Int).Value = D.AdultCaseID;
            cmd.Parameters.Add("@Investigation", SqlDbType.VarChar, 4000).Value = D.Investigation;
            cmd.Parameters.Add("@MedicalAdvice", SqlDbType.VarChar, 4000).Value = D.MedicalAdvice;
            cmd.Parameters.Add("@IsEnabled", SqlDbType.Bit).Value = D.IsEnabled;
            if (D.ModifyDate > DateTime.MinValue)
                cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = D.ModifyDate;
            else
                cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@ModifyBy", SqlDbType.Int).Value = D.ModifyBy;
            cmd.Parameters.Add("@PaymentType", SqlDbType.Int).Value = D.PaymentType;
            cmd.Parameters.Add("@ImagePath", SqlDbType.VarChar).Value = D.ImagePath;
            cmd.Parameters.Add("@Is_Transfer", SqlDbType.Bit).Value = D.Is_Transfer;
            cmd.Parameters.Add("@Ref_Selected", SqlDbType.VarChar).Value = D.Ref_Selected;
            cmd.Parameters.Add("@DiagnosisID", SqlDbType.VarChar, -1).Value = D.DiagnosisID;
            cmd.Parameters.Add("@DiagnosisOther", SqlDbType.VarChar, -1).Value = D.DiagnosisOther;
            SqlParameter Param = new SqlParameter(); Param.ParameterName = "@RetVal";
            Param.DbType = DbType.Int64; Param.Direction = ParameterDirection.Output;
            Param.Value = 0; cmd.Parameters.Add(Param);

            db.DbUpdate(cmd);

            int i = 0;
            if (cmd.Parameters["@RetVal"].Value != null)
            {
                int.TryParse(cmd.Parameters["@RetVal"].Value.ToString(), out i);
            }
            return i;
        }
    }
}
