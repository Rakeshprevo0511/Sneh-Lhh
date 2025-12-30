using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace SnehBLL
{
    public class DoctorMast_Bll
    {
        DbHelper.SqlDb db;

        public DoctorMast_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public static int Check(string _uniqueID)
        {
            int _doctorID = 0;
            SqlCommand cmd = new SqlCommand("DoctorMast_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = _uniqueID;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = 0;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["DoctorID"].ToString(), out _doctorID);
            }
            return _doctorID;
        }

        public static string Check(int _doctorID)
        {
            string _uniqueID = "";
            SqlCommand cmd = new SqlCommand("DoctorMast_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = "";
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = _doctorID;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                _uniqueID = dt.Rows[0]["UniqueID"].ToString();
            }
            return _uniqueID;
        }

        public int Delete(int _doctorID)
        {
            SqlCommand cmd = new SqlCommand("DoctorMast_Delete"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = _doctorID;

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

        public SnehDLL.DoctorMast_Dll Get(int _doctorID)
        {
            SnehDLL.DoctorMast_Dll D = null;
            SqlCommand cmd = new SqlCommand("DoctorMast_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = _doctorID;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                D = new SnehDLL.DoctorMast_Dll(); int i = 0;
                D.DoctorID = int.Parse(dt.Rows[i]["DoctorID"].ToString());
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                D.PreFix = dt.Rows[i]["PreFix"].ToString();
                D.FullName = dt.Rows[i]["FullName"].ToString();
                D.Qualification = dt.Rows[i]["Qualification"].ToString();
                int SpecialityID = 0; int.TryParse(dt.Rows[i]["SpecialityID"].ToString(), out SpecialityID); D.SpecialityID = SpecialityID;
                int WorkplaceID = 0; int.TryParse(dt.Rows[i]["WorkplaceID"].ToString(), out WorkplaceID); D.WorkplaceID = WorkplaceID;
                D.WorkplaceOther = dt.Rows[i]["WorkplaceOther"].ToString();
                D.cAddress = dt.Rows[i]["cAddress"].ToString();
                int CountryID = 0; int.TryParse(dt.Rows[i]["CountryID"].ToString(), out CountryID); D.CountryID = CountryID;
                int StateID = 0; int.TryParse(dt.Rows[i]["StateID"].ToString(), out StateID); D.StateID = StateID;
                int CityID = 0; int.TryParse(dt.Rows[i]["CityID"].ToString(), out CityID); D.CityID = CityID;
                D.ZipCode = dt.Rows[i]["ZipCode"].ToString();
                D.Telephone1 = dt.Rows[i]["Telephone1"].ToString();
                D.Telephone2 = dt.Rows[i]["Telephone2"].ToString();
                D.MailID = dt.Rows[i]["MailID"].ToString();
                D.MobileNo = dt.Rows[i]["MobileNo"].ToString();
                D.PanCard = dt.Rows[i]["PanCard"].ToString();
                D.Remarks = dt.Rows[i]["Remarks"].ToString();
                DateTime BirthDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["BirthDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out BirthDate);
                D.BirthDate = BirthDate;
                D.ClinicAddress = dt.Rows[i]["ClinicAddress"].ToString();
                D.ClinicTel1 = dt.Rows[i]["ClinicTel1"].ToString();
                D.ClinicTel2 = dt.Rows[i]["ClinicTel2"].ToString();
                int FacilitatorID = 0; int.TryParse(dt.Rows[i]["FacilitatorID"].ToString(), out FacilitatorID); D.FacilitatorID = FacilitatorID;
                DateTime JoinDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["JoinDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out JoinDate);
                D.JoinDate = JoinDate;
                bool IsEnabled = true; bool.TryParse(dt.Rows[i]["IsEnabled"].ToString(), out IsEnabled); D.IsEnabled = IsEnabled;
                TimeSpan Available1From = new TimeSpan(); DateTime Available1FromD = new DateTime();
                DateTime.TryParseExact(dt.Rows[i]["Available1From"].ToString(), DbHelper.Configuration.timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out Available1FromD);
                if (Available1FromD > DateTime.MinValue && Available1FromD < DateTime.MaxValue)
                {
                    Available1From = Available1FromD.TimeOfDay;
                }
                D.Available1From = Available1From;
                TimeSpan Available1Upto = new TimeSpan(); DateTime Available1UptoD = new DateTime();
                DateTime.TryParseExact(dt.Rows[i]["Available1Upto"].ToString(), DbHelper.Configuration.timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out Available1UptoD);
                if (Available1UptoD > DateTime.MinValue && Available1UptoD < DateTime.MaxValue)
                {
                    Available1Upto = Available1UptoD.TimeOfDay;
                }
                D.Available1Upto = Available1Upto;
                TimeSpan Available2From = new TimeSpan(); DateTime Available2FromD = new DateTime();
                DateTime.TryParseExact(dt.Rows[i]["Available2From"].ToString(), DbHelper.Configuration.timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out Available2FromD);
                if (Available2FromD > DateTime.MinValue && Available2FromD < DateTime.MaxValue)
                {
                    Available2From = Available2FromD.TimeOfDay;
                }
                D.Available2From = Available2From;
                TimeSpan Available2Upto = new TimeSpan(); DateTime Available2UptoD = new DateTime();
                DateTime.TryParseExact(dt.Rows[i]["Available2Upto"].ToString(), DbHelper.Configuration.timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out Available2UptoD);
                if (Available2UptoD > DateTime.MinValue && Available2UptoD < DateTime.MaxValue)
                {
                    Available2Upto = Available2UptoD.TimeOfDay;
                }
                D.Available2Upto = Available2Upto;
                D.Available1FromChar = dt.Rows[i]["Available1FromChar"].ToString();
                D.Available1UptoChar = dt.Rows[i]["Available1UptoChar"].ToString();
                D.Available2FromChar = dt.Rows[i]["Available2FromChar"].ToString();
                D.Available2UptoChar = dt.Rows[i]["Available2UptoChar"].ToString();
                DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                D.AddedDate = AddedDate;
                DateTime ModifyDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["ModifyDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ModifyDate);
                D.ModifyDate = ModifyDate;
                int AddedBy = 0; int.TryParse(dt.Rows[i]["AddedBy"].ToString(), out AddedBy); D.AddedBy = AddedBy;
                int ModifyBy = 0; int.TryParse(dt.Rows[i]["ModifyBy"].ToString(), out ModifyBy); D.ModifyBy = ModifyBy;
                D.Profile_Image_Path = dt.Rows[i]["Profile_Image_Path"].ToString();
                D.FatherName = dt.Rows[i]["FatherName"].ToString();
                D.MotherName = dt.Rows[i]["MotherName"].ToString();
                D.Husband_WifeName = dt.Rows[i]["Husband_WifeName"].ToString();
                int qualificationid = 0; int.TryParse(dt.Rows[i]["QualificationID"].ToString(), out qualificationid);
                D.QualificationID = qualificationid;
                D.BloodGroup = dt.Rows[i]["BloodGroup"].ToString();
                D.Aadharcard = dt.Rows[i]["Aadharcard"].ToString();
                DateTime anniversary = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["Anniversary_Date"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out anniversary);
                D.Anniversary_Date = anniversary;
            }
            return D;
        }

        public List<SnehDLL.DoctorMast_Dll> GetList()
        {
            List<SnehDLL.DoctorMast_Dll> DL = new List<SnehDLL.DoctorMast_Dll>();
            SqlCommand cmd = new SqlCommand("DoctorMast_GetList"); cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.DoctorMast_Dll D = new SnehDLL.DoctorMast_Dll();
                D.DoctorID = int.Parse(dt.Rows[i]["DoctorID"].ToString());
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                D.PreFix = dt.Rows[i]["PreFix"].ToString();
                D.FullName = dt.Rows[i]["FullName"].ToString();
                D.Qualification = dt.Rows[i]["Qualification"].ToString();
                int SpecialityID = 0; int.TryParse(dt.Rows[i]["SpecialityID"].ToString(), out SpecialityID); D.SpecialityID = SpecialityID;
                int WorkplaceID = 0; int.TryParse(dt.Rows[i]["WorkplaceID"].ToString(), out WorkplaceID); D.WorkplaceID = WorkplaceID;
                D.WorkplaceOther = dt.Rows[i]["WorkplaceOther"].ToString();
                D.cAddress = dt.Rows[i]["cAddress"].ToString();
                int CountryID = 0; int.TryParse(dt.Rows[i]["CountryID"].ToString(), out CountryID); D.CountryID = CountryID;
                int StateID = 0; int.TryParse(dt.Rows[i]["StateID"].ToString(), out StateID); D.StateID = StateID;
                int CityID = 0; int.TryParse(dt.Rows[i]["CityID"].ToString(), out CityID); D.CityID = CityID;
                D.ZipCode = dt.Rows[i]["ZipCode"].ToString();
                D.Telephone1 = dt.Rows[i]["Telephone1"].ToString();
                D.Telephone2 = dt.Rows[i]["Telephone2"].ToString();
                D.MailID = dt.Rows[i]["MailID"].ToString();
                D.MobileNo = dt.Rows[i]["MobileNo"].ToString();
                D.PanCard = dt.Rows[i]["PanCard"].ToString();
                D.Remarks = dt.Rows[i]["Remarks"].ToString();
                DateTime BirthDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["BirthDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out BirthDate);
                D.BirthDate = BirthDate;
                D.ClinicAddress = dt.Rows[i]["ClinicAddress"].ToString();
                D.ClinicTel1 = dt.Rows[i]["ClinicTel1"].ToString();
                D.ClinicTel2 = dt.Rows[i]["ClinicTel2"].ToString();
                int FacilitatorID = 0; int.TryParse(dt.Rows[i]["FacilitatorID"].ToString(), out FacilitatorID); D.FacilitatorID = FacilitatorID;
                DateTime JoinDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["JoinDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out JoinDate);
                D.JoinDate = JoinDate;
                bool IsEnabled = true; bool.TryParse(dt.Rows[i]["IsEnabled"].ToString(), out IsEnabled); D.IsEnabled = IsEnabled;
                TimeSpan Available1From = new TimeSpan(); DateTime Available1FromD = new DateTime();
                DateTime.TryParseExact(dt.Rows[i]["Available1From"].ToString(), DbHelper.Configuration.timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out Available1FromD);
                if (Available1FromD > DateTime.MinValue && Available1FromD < DateTime.MaxValue)
                {
                    Available1From = Available1FromD.TimeOfDay;
                }
                D.Available1From = Available1From;
                TimeSpan Available1Upto = new TimeSpan(); DateTime Available1UptoD = new DateTime();
                DateTime.TryParseExact(dt.Rows[i]["Available1Upto"].ToString(), DbHelper.Configuration.timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out Available1UptoD);
                if (Available1UptoD > DateTime.MinValue && Available1UptoD < DateTime.MaxValue)
                {
                    Available1Upto = Available1UptoD.TimeOfDay;
                }
                D.Available1Upto = Available1Upto;
                TimeSpan Available2From = new TimeSpan(); DateTime Available2FromD = new DateTime();
                DateTime.TryParseExact(dt.Rows[i]["Available2From"].ToString(), DbHelper.Configuration.timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out Available2FromD);
                if (Available2FromD > DateTime.MinValue && Available2FromD < DateTime.MaxValue)
                {
                    Available2From = Available2FromD.TimeOfDay;
                }
                D.Available2From = Available2From;
                TimeSpan Available2Upto = new TimeSpan(); DateTime Available2UptoD = new DateTime();
                DateTime.TryParseExact(dt.Rows[i]["Available2Upto"].ToString(), DbHelper.Configuration.timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out Available2UptoD);
                if (Available2UptoD > DateTime.MinValue && Available2UptoD < DateTime.MaxValue)
                {
                    Available2Upto = Available2UptoD.TimeOfDay;
                }
                D.Available2Upto = Available2From;
                D.Available1FromChar = dt.Rows[i]["Available1FromChar"].ToString();
                D.Available1UptoChar = dt.Rows[i]["Available1UptoChar"].ToString();
                D.Available2FromChar = dt.Rows[i]["Available2FromChar"].ToString();
                D.Available2UptoChar = dt.Rows[i]["Available2UptoChar"].ToString();
                DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                D.AddedDate = AddedDate;
                DateTime ModifyDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["ModifyDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ModifyDate);
                D.ModifyDate = ModifyDate;
                int AddedBy = 0; int.TryParse(dt.Rows[i]["AddedBy"].ToString(), out AddedBy); D.AddedBy = AddedBy;
                int ModifyBy = 0; int.TryParse(dt.Rows[i]["ModifyBy"].ToString(), out ModifyBy); D.ModifyBy = ModifyBy;

                DL.Add(D);
            }
            return DL;
        }

        public List<SnehDLL.DoctorMast_Dll> GetForDropdown()
        {
            List<SnehDLL.DoctorMast_Dll> DL = new List<SnehDLL.DoctorMast_Dll>();
            SqlCommand cmd = new SqlCommand("DoctorMast_GetForDropdown"); cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.DoctorMast_Dll D = new SnehDLL.DoctorMast_Dll();
                D.DoctorID = int.Parse(dt.Rows[i]["DoctorID"].ToString());
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                D.PreFix = dt.Rows[i]["PreFix"].ToString();
                D.FullName = dt.Rows[i]["FullName"].ToString();
                bool IsEnabled = true; bool.TryParse(dt.Rows[i]["IsEnabled"].ToString(), out IsEnabled); D.IsEnabled = IsEnabled;

                DL.Add(D);
            }
            return DL;
        }

        public List<SnehDLL.DoctorMast_Dll> Search(string _fullName, DateTime _fromDate, DateTime _uptoDate)
        {
            List<SnehDLL.DoctorMast_Dll> DL = new List<SnehDLL.DoctorMast_Dll>();
            SqlCommand cmd = new SqlCommand("DoctorMast_GetSearch"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 50).Value = _fullName;
            if (_fromDate > DateTime.MinValue)
                cmd.Parameters.Add("@JoinFrom", SqlDbType.DateTime).Value = _fromDate;
            else
                cmd.Parameters.Add("@JoinFrom", SqlDbType.DateTime).Value = DBNull.Value;
            if (_uptoDate > DateTime.MinValue)
                cmd.Parameters.Add("@JoinUpto", SqlDbType.DateTime).Value = _uptoDate;
            else
                cmd.Parameters.Add("@JoinUpto", SqlDbType.DateTime).Value = DBNull.Value;

            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.DoctorMast_Dll D = new SnehDLL.DoctorMast_Dll();
                D.DoctorID = int.Parse(dt.Rows[i]["DoctorID"].ToString());
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                D.PreFix = dt.Rows[i]["PreFix"].ToString();
                D.FullName = dt.Rows[i]["FullName"].ToString();
                D.Qualification = dt.Rows[i]["Qualification"].ToString();
                int SpecialityID = 0; int.TryParse(dt.Rows[i]["SpecialityID"].ToString(), out SpecialityID); D.SpecialityID = SpecialityID;
                int WorkplaceID = 0; int.TryParse(dt.Rows[i]["WorkplaceID"].ToString(), out WorkplaceID); D.WorkplaceID = WorkplaceID;
                D.WorkplaceOther = dt.Rows[i]["WorkplaceOther"].ToString();
                D.cAddress = dt.Rows[i]["cAddress"].ToString();
                int CountryID = 0; int.TryParse(dt.Rows[i]["CountryID"].ToString(), out CountryID); D.CountryID = CountryID;
                int StateID = 0; int.TryParse(dt.Rows[i]["StateID"].ToString(), out StateID); D.StateID = StateID;
                int CityID = 0; int.TryParse(dt.Rows[i]["CityID"].ToString(), out CityID); D.CityID = CityID;
                D.ZipCode = dt.Rows[i]["ZipCode"].ToString();
                D.Telephone1 = dt.Rows[i]["Telephone1"].ToString();
                D.Telephone2 = dt.Rows[i]["Telephone2"].ToString();
                D.MailID = dt.Rows[i]["MailID"].ToString();
                D.MobileNo = dt.Rows[i]["MobileNo"].ToString();
                D.PanCard = dt.Rows[i]["PanCard"].ToString();
                D.Remarks = dt.Rows[i]["Remarks"].ToString();
                DateTime BirthDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["BirthDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out BirthDate);
                D.BirthDate = BirthDate;
                D.ClinicAddress = dt.Rows[i]["ClinicAddress"].ToString();
                D.ClinicTel1 = dt.Rows[i]["ClinicTel1"].ToString();
                D.ClinicTel2 = dt.Rows[i]["ClinicTel2"].ToString();
                int FacilitatorID = 0; int.TryParse(dt.Rows[i]["FacilitatorID"].ToString(), out FacilitatorID); D.FacilitatorID = FacilitatorID;
                DateTime JoinDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["JoinDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out JoinDate);
                D.JoinDate = JoinDate;
                bool IsEnabled = true; bool.TryParse(dt.Rows[i]["IsEnabled"].ToString(), out IsEnabled); D.IsEnabled = IsEnabled;
                TimeSpan Available1From = new TimeSpan(); DateTime Available1FromD = new DateTime();
                DateTime.TryParseExact(dt.Rows[i]["Available1From"].ToString(), DbHelper.Configuration.timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out Available1FromD);
                if (Available1FromD > DateTime.MinValue && Available1FromD < DateTime.MaxValue)
                {
                    Available1From = Available1FromD.TimeOfDay;
                }
                D.Available1From = Available1From;
                TimeSpan Available1Upto = new TimeSpan(); DateTime Available1UptoD = new DateTime();
                DateTime.TryParseExact(dt.Rows[i]["Available1Upto"].ToString(), DbHelper.Configuration.timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out Available1UptoD);
                if (Available1UptoD > DateTime.MinValue && Available1UptoD < DateTime.MaxValue)
                {
                    Available1Upto = Available1UptoD.TimeOfDay;
                }
                D.Available1Upto = Available1Upto;
                TimeSpan Available2From = new TimeSpan(); DateTime Available2FromD = new DateTime();
                DateTime.TryParseExact(dt.Rows[i]["Available2From"].ToString(), DbHelper.Configuration.timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out Available2FromD);
                if (Available2FromD > DateTime.MinValue && Available2FromD < DateTime.MaxValue)
                {
                    Available2From = Available2FromD.TimeOfDay;
                }
                D.Available2From = Available2From;
                TimeSpan Available2Upto = new TimeSpan(); DateTime Available2UptoD = new DateTime();
                DateTime.TryParseExact(dt.Rows[i]["Available2Upto"].ToString(), DbHelper.Configuration.timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out Available2UptoD);
                if (Available2UptoD > DateTime.MinValue && Available2UptoD < DateTime.MaxValue)
                {
                    Available2Upto = Available2UptoD.TimeOfDay;
                }
                D.Available2Upto = Available2From;
                D.Available1FromChar = dt.Rows[i]["Available1FromChar"].ToString();
                D.Available1UptoChar = dt.Rows[i]["Available1UptoChar"].ToString();
                D.Available2FromChar = dt.Rows[i]["Available2FromChar"].ToString();
                D.Available2UptoChar = dt.Rows[i]["Available2UptoChar"].ToString();
                DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                D.AddedDate = AddedDate;
                DateTime ModifyDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["ModifyDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ModifyDate);
                D.ModifyDate = ModifyDate;
                int AddedBy = 0; int.TryParse(dt.Rows[i]["AddedBy"].ToString(), out AddedBy); D.AddedBy = AddedBy;
                int ModifyBy = 0; int.TryParse(dt.Rows[i]["ModifyBy"].ToString(), out ModifyBy); D.ModifyBy = ModifyBy;

                DL.Add(D);
            }
            return DL;
        }

        public int Set(SnehDLL.DoctorMast_Dll D)
        {
            SqlCommand cmd = new SqlCommand("DoctorMast_Set"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = D.DoctorID;
            cmd.Parameters.Add("@PreFix", SqlDbType.VarChar, 50).Value = D.PreFix;
            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 250).Value = D.FullName;
            cmd.Parameters.Add("@Qualification", SqlDbType.VarChar, 250).Value = D.Qualification;
            cmd.Parameters.Add("@SpecialityID", SqlDbType.Int).Value = D.SpecialityID;
            cmd.Parameters.Add("@WorkplaceID", SqlDbType.Int).Value = D.WorkplaceID;
            cmd.Parameters.Add("@WorkplaceOther", SqlDbType.VarChar, 250).Value = D.WorkplaceOther;
            cmd.Parameters.Add("@cAddress", SqlDbType.VarChar, 500).Value = D.cAddress;
            cmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = D.CountryID;
            cmd.Parameters.Add("@StateID", SqlDbType.Int).Value = D.StateID;
            cmd.Parameters.Add("@CityID", SqlDbType.Int).Value = D.CityID;
            cmd.Parameters.Add("@ZipCode", SqlDbType.VarChar, 50).Value = D.ZipCode;
            cmd.Parameters.Add("@Telephone1", SqlDbType.VarChar, 50).Value = D.Telephone1;
            cmd.Parameters.Add("@Telephone2", SqlDbType.VarChar, 50).Value = D.Telephone2;
            cmd.Parameters.Add("@MailID", SqlDbType.VarChar, 500).Value = D.MailID;
            cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar, 50).Value = D.MobileNo;
            cmd.Parameters.Add("@PanCard", SqlDbType.VarChar, 50).Value = D.PanCard;
            cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 500).Value = D.Remarks;
            if (D.BirthDate > DateTime.MinValue)
                cmd.Parameters.Add("@BirthDate", SqlDbType.DateTime).Value = D.BirthDate;
            else
                cmd.Parameters.Add("@BirthDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@ClinicAddress", SqlDbType.VarChar, 500).Value = D.ClinicAddress;
            cmd.Parameters.Add("@ClinicTel1", SqlDbType.VarChar, 50).Value = D.ClinicTel1;
            cmd.Parameters.Add("@ClinicTel2", SqlDbType.VarChar, 50).Value = D.ClinicTel2;
            cmd.Parameters.Add("@FacilitatorID", SqlDbType.Int).Value = D.FacilitatorID;
            if (D.JoinDate > DateTime.MinValue)
                cmd.Parameters.Add("@JoinDate", SqlDbType.DateTime).Value = D.JoinDate;
            else
                cmd.Parameters.Add("@JoinDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@IsEnabled", SqlDbType.Bit).Value = D.IsEnabled;

            if (D.Available1From > TimeSpan.MinValue && D.Available1From < TimeSpan.MaxValue && D.Available1FromChar.Length > 0)
                cmd.Parameters.Add("@Available1From", SqlDbType.Time).Value = D.Available1From;
            else
                cmd.Parameters.Add("@Available1From", SqlDbType.Time).Value = DBNull.Value;

            if (D.Available1Upto > TimeSpan.MinValue && D.Available1Upto < TimeSpan.MaxValue && D.Available1UptoChar.Length > 0)
                cmd.Parameters.Add("@Available1Upto", SqlDbType.Time).Value = D.Available1Upto;
            else
                cmd.Parameters.Add("@Available1Upto", SqlDbType.Time).Value = DBNull.Value;

            if (D.Available2From > TimeSpan.MinValue && D.Available2From < TimeSpan.MaxValue && D.Available2FromChar.Length > 0)
                cmd.Parameters.Add("@Available2From", SqlDbType.Time).Value = D.Available2From;
            else
                cmd.Parameters.Add("@Available2From", SqlDbType.Time).Value = DBNull.Value;

            if (D.Available2Upto > TimeSpan.MinValue && D.Available2Upto < TimeSpan.MaxValue && D.Available2UptoChar.Length > 0)
                cmd.Parameters.Add("@Available2Upto", SqlDbType.Time).Value = D.Available2Upto;
            else
                cmd.Parameters.Add("@Available2Upto", SqlDbType.Time).Value = DBNull.Value;

            cmd.Parameters.Add("@Available1FromChar", SqlDbType.VarChar, 50).Value = D.Available1FromChar;
            cmd.Parameters.Add("@Available1UptoChar", SqlDbType.VarChar, 50).Value = D.Available1UptoChar;
            cmd.Parameters.Add("@Available2FromChar", SqlDbType.VarChar, 50).Value = D.Available2FromChar;
            cmd.Parameters.Add("@Available2UptoChar", SqlDbType.VarChar, 50).Value = D.Available2UptoChar;

            if (D.ModifyDate > DateTime.MinValue)
                cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = D.ModifyDate;
            else
                cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = DBNull.Value;

            cmd.Parameters.Add("@ModifyBy", SqlDbType.Int).Value = D.ModifyBy;

            cmd.Parameters.Add("@Profile_Image_Path", SqlDbType.VarChar, 4000).Value = D.Profile_Image_Path;
            cmd.Parameters.Add("@FatherName", SqlDbType.VarChar, 250).Value = D.FatherName;
            cmd.Parameters.Add("@MotherName", SqlDbType.VarChar, 250).Value = D.MotherName;
            cmd.Parameters.Add("@Husband_WifeName", SqlDbType.VarChar, 250).Value = D.Husband_WifeName;
            cmd.Parameters.Add("@QualificationID", SqlDbType.Int).Value = D.QualificationID;
            cmd.Parameters.Add("@BloodGroup", SqlDbType.VarChar, 50).Value = D.BloodGroup;
            cmd.Parameters.Add("@Aadharcard", SqlDbType.VarChar, 250).Value = D.Aadharcard;
            if (D.Anniversary_Date > DateTime.MinValue)
                cmd.Parameters.Add("@Anniversary_Date", SqlDbType.DateTime).Value = D.Anniversary_Date;
            else
                cmd.Parameters.Add("@Anniversary_Date", SqlDbType.DateTime).Value = DBNull.Value;

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

        public int SetCharges(int _doctorID, int _sessionID, int _chargeType, float _chargeAmt)
        {
            SqlCommand cmd = new SqlCommand("DoctorCharges_Set"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = _doctorID;
            cmd.Parameters.Add("@SessionID", SqlDbType.Int).Value = _sessionID;
            cmd.Parameters.Add("@ChargeType", SqlDbType.Int).Value = _chargeType;
            cmd.Parameters.Add("@ChargeAmt", SqlDbType.Decimal).Value = _chargeAmt;

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

        public DataTable getCharges(int _doctorID)
        {
            SqlCommand cmd = new SqlCommand("DoctorCharges_GetList"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = _doctorID;

            return db.DbRead(cmd);
        }

        public int SetCharges(int _chargeID)
        {
            SqlCommand cmd = new SqlCommand("DoctorCharges_Del"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ChargeID", SqlDbType.Int).Value = _chargeID;

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
        public List<SnehDLL.DoctorMast_Dll> get_new(int _appointmentID)
        {
            List<SnehDLL.DoctorMast_Dll> DL = new List<SnehDLL.DoctorMast_Dll>();
            SqlCommand cmd = new SqlCommand("DoctorMast_Get_new"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.DoctorMast_Dll D = new SnehDLL.DoctorMast_Dll();
                D.AppointmentID = int.Parse(dt.Rows[i]["AppointmentID"].ToString());
                D.DoctorID = int.Parse(dt.Rows[i]["DoctorID"].ToString());
                D.FullName = dt.Rows[i]["Therapist"].ToString();
                //D.ScheduleType = dt.Rows[i]["ScheduleType"].ToString();
                DL.Add(D);
            }
            return DL;
        }
        public List<SnehDLL.DoctorMast_Dll> get_newapp(int _appointmentID)
        {
            List<SnehDLL.DoctorMast_Dll> DL = new List<SnehDLL.DoctorMast_Dll>();
            SqlCommand cmd = new SqlCommand("DoctorMast_Get_newapp"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.DoctorMast_Dll D = new SnehDLL.DoctorMast_Dll();
                D.AppointmentID = int.Parse(dt.Rows[i]["AppointmentID"].ToString());
                D.DoctorID = int.Parse(dt.Rows[i]["DoctorID"].ToString());
                D.FullName = dt.Rows[i]["Therapist"].ToString();
                //D.ScheduleType = dt.Rows[i]["ScheduleType"].ToString();
                DL.Add(D);
            }
            return DL;
        }
    }
}
