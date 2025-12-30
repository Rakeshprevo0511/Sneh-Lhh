using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace SnehBLL
{
    public class Receiption_Bll
    {
        DbHelper.SqlDb db = null;
        public Receiption_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public static int Check(string _uniqueID)
        {
            int ReceiptionID = 0;
            SqlCommand cmd = new SqlCommand("ReceiptionMast_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = _uniqueID;
            cmd.Parameters.Add("@ReceiptionID", SqlDbType.Int).Value = 0;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["ReceiptionID"].ToString(), out ReceiptionID);
            }
            return ReceiptionID;
        }

        public static string Check(int ReceiptionID)
        {
            string _uniqueID = "";
            SqlCommand cmd = new SqlCommand("ReceiptionMast_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = "";
            cmd.Parameters.Add("@ReceiptionID", SqlDbType.Int).Value = ReceiptionID;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                _uniqueID = dt.Rows[0]["UniqueID"].ToString();
            }
            return _uniqueID;
        }

        public int set(SnehDLL.Receiption_Dll RD)
        {
            SqlCommand cmd = new SqlCommand("Set_ReceiptionMast"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ReceiptionID", SqlDbType.Int).Value = RD.ReceiptionID;
            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 250).Value = RD.FullName;
            cmd.Parameters.Add("@Designation", SqlDbType.VarChar, 250).Value = RD.Designation;
            cmd.Parameters.Add("@Qualifications", SqlDbType.VarChar, 500).Value = RD.Qualifications;
            cmd.Parameters.Add("@MailID", SqlDbType.VarChar, 250).Value = RD.MailID;
            cmd.Parameters.Add("@ContactNo", SqlDbType.VarChar, 250).Value = RD.ContactNo;
            cmd.Parameters.Add("@Emergency_ContactNO", SqlDbType.VarChar, 250).Value = RD.Emergency_ContactNO;
            if (RD.BirthDate > DateTime.MinValue)
                cmd.Parameters.Add("@BirthDate", SqlDbType.DateTime).Value = RD.BirthDate;
            else
                cmd.Parameters.Add("@BirthDate", SqlDbType.DateTime).Value = DBNull.Value;
            if (RD.Anniversary_Date > DateTime.MinValue)
                cmd.Parameters.Add("@Anniversary_Date", SqlDbType.DateTime).Value = RD.Anniversary_Date;
            else
                cmd.Parameters.Add("@Anniversary_Date", SqlDbType.DateTime).Value = DBNull.Value;

            cmd.Parameters.Add("@Reference_Documents", SqlDbType.VarChar, 500).Value = RD.Reference_Documents;

            if (RD.JoinDate > DateTime.MinValue)
                cmd.Parameters.Add("@JoinDate", SqlDbType.DateTime).Value = RD.JoinDate;
            else
                cmd.Parameters.Add("@JoinDate", SqlDbType.DateTime).Value = DBNull.Value;

            if (RD.Clinic_Shift_TimeFrom > TimeSpan.MinValue && RD.Clinic_Shift_TimeFrom < TimeSpan.MaxValue && RD.Clinic_Shift_TimeFromChar.Length > 0)
                cmd.Parameters.Add("@Clinic_Shift_TimeFrom", SqlDbType.Time).Value = RD.Clinic_Shift_TimeFrom;
            else
                cmd.Parameters.Add("@Clinic_Shift_TimeFrom", SqlDbType.Time).Value = DBNull.Value;

            if (RD.Clinic_Shift_TimeUpto > TimeSpan.MinValue && RD.Clinic_Shift_TimeUpto < TimeSpan.MaxValue && RD.Clinic_Shift_TimeUptoChar.Length > 0)
                cmd.Parameters.Add("@Clinic_Shift_TimeUpto", SqlDbType.Time).Value = RD.Clinic_Shift_TimeUpto;
            else
                cmd.Parameters.Add("@Clinic_Shift_TimeUpto", SqlDbType.Time).Value = DBNull.Value;

            cmd.Parameters.Add("@Clinic_Shift_TimeFromChar", SqlDbType.VarChar, 50).Value = RD.Clinic_Shift_TimeFromChar;
            cmd.Parameters.Add("@Clinic_Shift_TimeUptoChar", SqlDbType.VarChar, 50).Value = RD.Clinic_Shift_TimeUptoChar;
            cmd.Parameters.Add("@BloodGroup", SqlDbType.VarChar, 50).Value = RD.BloodGroup;

            cmd.Parameters.Add("@PancardNo", SqlDbType.VarChar, 250).Value = RD.PancardNo;
            cmd.Parameters.Add("@AadharcardNo", SqlDbType.VarChar, 250).Value = RD.AadharcardNo;
            cmd.Parameters.Add("@Address", SqlDbType.VarChar, 500).Value = RD.Address;
            cmd.Parameters.Add("@Profile_Image_Path", SqlDbType.NVarChar, 4000).Value = RD.Profile_Image_Path;

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

        public SnehDLL.Receiption_Dll Get(int receiptionid)
        {
            SnehDLL.Receiption_Dll RD = null;
            SqlCommand cmd = new SqlCommand("ReceiptionMast_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ReceiptionID", SqlDbType.Int).Value = receiptionid;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                RD = new SnehDLL.Receiption_Dll(); int i = 0;
                RD.ReceiptionID = int.Parse(dt.Rows[i]["ReceiptionID"].ToString());
                RD.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                RD.FullName = dt.Rows[i]["FullName"].ToString();
                RD.Designation = dt.Rows[i]["Designation"].ToString();
                RD.Qualifications = dt.Rows[i]["Qualifications"].ToString();
                RD.MailID = dt.Rows[i]["MailID"].ToString();
                RD.ContactNo = dt.Rows[i]["ContactNo"].ToString();
                RD.Emergency_ContactNO = dt.Rows[i]["Emergency_ContactNO"].ToString();
                DateTime BirthDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["BirthDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out BirthDate);
                RD.BirthDate = BirthDate;
                DateTime Anniversary_Date = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["Anniversary_Date"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out Anniversary_Date);
                RD.Anniversary_Date = Anniversary_Date;
                RD.Reference_Documents = dt.Rows[i]["Reference_Documents"].ToString();
                DateTime JoinDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["JoinDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out JoinDate);
                RD.JoinDate = JoinDate;
                RD.PancardNo = dt.Rows[i]["PancardNo"].ToString();
                RD.AadharcardNo = dt.Rows[i]["AadharcardNo"].ToString();
                RD.Address = dt.Rows[i]["Address"].ToString();
                RD.Profile_Image_Path = dt.Rows[i]["Profile_Image_Path"].ToString();
                TimeSpan ClinicShiftTimeFrom = new TimeSpan(); DateTime ClinicShiftTimeD = new DateTime();
                DateTime.TryParseExact(dt.Rows[i]["Clinic_Shift_TimeFrom"].ToString(), DbHelper.Configuration.timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ClinicShiftTimeD);
                if (ClinicShiftTimeD > DateTime.MinValue && ClinicShiftTimeD < DateTime.MaxValue)
                {
                    ClinicShiftTimeFrom = ClinicShiftTimeD.TimeOfDay;
                }
                RD.Clinic_Shift_TimeFrom = ClinicShiftTimeFrom;
                TimeSpan ClinicShiftTimeUpto = new TimeSpan(); DateTime ClinicShiftTimeUptoD = new DateTime();
                DateTime.TryParseExact(dt.Rows[i]["Clinic_Shift_TimeUpto"].ToString(), DbHelper.Configuration.timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ClinicShiftTimeUptoD);
                if (ClinicShiftTimeUptoD > DateTime.MinValue && ClinicShiftTimeUptoD < DateTime.MaxValue)
                {
                    ClinicShiftTimeUpto = ClinicShiftTimeUptoD.TimeOfDay;
                }
                RD.Clinic_Shift_TimeUpto = ClinicShiftTimeUpto;
                RD.Clinic_Shift_TimeFromChar = dt.Rows[i]["Clinic_Shift_TimeFromChar"].ToString();
                RD.Clinic_Shift_TimeUptoChar = dt.Rows[i]["Clinic_Shift_TimeUptoChar"].ToString();
                RD.BloodGroup = dt.Rows[i]["BloodGroup"].ToString();
            }
            return RD;
        }

        public List<SnehDLL.Receiption_Dll> GetList(int id, string _fullName, DateTime _fromDate, DateTime _uptoDate)
        {
            List<SnehDLL.Receiption_Dll> RD = new List<SnehDLL.Receiption_Dll>();
            SqlCommand cmd = new SqlCommand("Get_Search_All"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
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
                SnehDLL.Receiption_Dll D = new SnehDLL.Receiption_Dll();
                D.TYPEID = int.Parse(dt.Rows[i]["TYPEID"].ToString());
                D.ReceiptionID = int.Parse(dt.Rows[i]["ReceiptionID"].ToString());
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                D.FullName = dt.Rows[i]["FullName"].ToString();
                D.Designation = dt.Rows[i]["Designation"].ToString();
                D.Qualifications = dt.Rows[i]["Qualifications"].ToString();
                D.MailID = dt.Rows[i]["MailID"].ToString();
                D.ContactNo = dt.Rows[i]["ContactNo"].ToString();
                D.Emergency_ContactNO = dt.Rows[i]["Emergency_ContactNO"].ToString();
                DateTime BirthDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["BirthDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out BirthDate);
                D.BirthDate = BirthDate;
                DateTime Anniversary_Date = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["Anniversary_Date"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out Anniversary_Date);
                D.Anniversary_Date = Anniversary_Date;
                D.Reference_Documents = dt.Rows[i]["Reference_Documents"].ToString();
                DateTime JoinDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["JoinDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out JoinDate);
                D.JoinDate = JoinDate;
                D.PancardNo = dt.Rows[i]["PancardNo"].ToString();
                D.AadharcardNo = dt.Rows[i]["AadharcardNo"].ToString();
                D.Address = dt.Rows[i]["Address"].ToString();
                D.Profile_Image_Path = dt.Rows[i]["Profile_Image_Path"].ToString();
                RD.Add(D);
            }
            return RD;
        }

        public DataTable GetListNew(int id, string _fullName, DateTime _fromDate, DateTime _uptoDate)
        {
            SqlCommand cmd = new SqlCommand("Get_Search_All"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
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
            return dt;
        }

        public int Delete(int receiptionid)
        {
            SqlCommand cmd = new SqlCommand("Receiption_Mast_Delete"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ReceiptionID", SqlDbType.Int).Value = receiptionid;

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

        public bool SaveFileNew(ref HttpPostedFile txtFile, string _fileName)
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

        public DataTable GetListNew()
        {
            SqlCommand cmd = new SqlCommand("Get_ReceiptionList"); cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = db.DbRead(cmd); return dt;
        }
    }
}
