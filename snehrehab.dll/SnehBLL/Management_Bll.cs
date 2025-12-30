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
    public class Management_Bll
    {
        DbHelper.SqlDb db = null;
        public Management_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public static int Check(string _uniqueID)
        {
            int ManagerID = 0;
            SqlCommand cmd = new SqlCommand("ManagementMast_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = _uniqueID;
            cmd.Parameters.Add("@ManagerID", SqlDbType.Int).Value = 0;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["ManagerID"].ToString(), out ManagerID);
            }
            return ManagerID;
        }

        public static string Check(int managerid)
        {
            string _uniqueID = "";
            SqlCommand cmd = new SqlCommand("ManagementMast_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = "";
            cmd.Parameters.Add("@ManagerID", SqlDbType.Int).Value = managerid;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                _uniqueID = dt.Rows[0]["UniqueID"].ToString();
            }
            return _uniqueID;
        }

        public int set(SnehDLL.Management_Dll MD)
        {
            SqlCommand cmd = new SqlCommand("Set_ManagerMast"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ManagerID", SqlDbType.Int).Value = MD.ManagerID;
            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 250).Value = MD.FullName;
            cmd.Parameters.Add("@Designation", SqlDbType.VarChar, 250).Value = MD.Designation;
            cmd.Parameters.Add("@Qualifications", SqlDbType.VarChar, 500).Value = MD.Qualifications;
            cmd.Parameters.Add("@MailID", SqlDbType.VarChar, 250).Value = MD.MailID;
            cmd.Parameters.Add("@ContactNo", SqlDbType.VarChar, 250).Value = MD.ContactNo;
            cmd.Parameters.Add("@Emergency_ContactNO", SqlDbType.VarChar, 250).Value = MD.Emergency_ContactNO;
            if (MD.BirthDate > DateTime.MinValue)
                cmd.Parameters.Add("@BirthDate", SqlDbType.DateTime).Value = MD.BirthDate;
            else
                cmd.Parameters.Add("@BirthDate", SqlDbType.DateTime).Value = DBNull.Value;
            if (MD.Anniversary_Date > DateTime.MinValue)
                cmd.Parameters.Add("@Anniversary_Date", SqlDbType.DateTime).Value = MD.Anniversary_Date;
            else
                cmd.Parameters.Add("@Anniversary_Date", SqlDbType.DateTime).Value = DBNull.Value;

            cmd.Parameters.Add("@Reference_Documents", SqlDbType.VarChar, 500).Value = MD.Reference_Documents;

            if (MD.JoinDate > DateTime.MinValue)
                cmd.Parameters.Add("@JoinDate", SqlDbType.DateTime).Value = MD.JoinDate;
            else
                cmd.Parameters.Add("@JoinDate", SqlDbType.DateTime).Value = DBNull.Value;

            if (MD.Clinic_Shift_TimeFrom > TimeSpan.MinValue && MD.Clinic_Shift_TimeFrom < TimeSpan.MaxValue && MD.Clinic_Shift_TimeFromChar.Length > 0)
                cmd.Parameters.Add("@Clinic_Shift_TimeFrom", SqlDbType.Time).Value = MD.Clinic_Shift_TimeFrom;
            else
                cmd.Parameters.Add("@Clinic_Shift_TimeFrom", SqlDbType.Time).Value = DBNull.Value;

            if (MD.Clinic_Shift_TimeUpto > TimeSpan.MinValue && MD.Clinic_Shift_TimeUpto < TimeSpan.MaxValue && MD.Clinic_Shift_TimeUptoChar.Length > 0)
                cmd.Parameters.Add("@Clinic_Shift_TimeUpto", SqlDbType.Time).Value = MD.Clinic_Shift_TimeUpto;
            else
                cmd.Parameters.Add("@Clinic_Shift_TimeUpto", SqlDbType.Time).Value = DBNull.Value;

            cmd.Parameters.Add("@Clinic_Shift_TimeFromChar", SqlDbType.VarChar, 50).Value = MD.Clinic_Shift_TimeFromChar;
            cmd.Parameters.Add("@Clinic_Shift_TimeUptoChar", SqlDbType.VarChar, 50).Value = MD.Clinic_Shift_TimeUptoChar;
            cmd.Parameters.Add("@BloodGroup", SqlDbType.VarChar, 50).Value = MD.BloodGroup;
            cmd.Parameters.Add("@PancardNo", SqlDbType.VarChar, 250).Value = MD.PancardNo;
            cmd.Parameters.Add("@AadharcardNo", SqlDbType.VarChar, 250).Value = MD.AadharcardNo;
            cmd.Parameters.Add("@Address", SqlDbType.VarChar, 500).Value = MD.Address;
            cmd.Parameters.Add("@Profile_Image_Path", SqlDbType.NVarChar, 4000).Value = MD.Profile_Image_Path;

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

        public SnehDLL.Management_Dll Get(int managerid)
        {
            SnehDLL.Management_Dll MD = null;
            SqlCommand cmd = new SqlCommand("ManagerMast_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ManagerID", SqlDbType.Int).Value = managerid;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                MD = new SnehDLL.Management_Dll(); int i = 0;
                MD.ManagerID = int.Parse(dt.Rows[i]["ManagerID"].ToString());
                MD.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                MD.FullName = dt.Rows[i]["FullName"].ToString();
                MD.Designation = dt.Rows[i]["Designation"].ToString();
                MD.Qualifications = dt.Rows[i]["Qualifications"].ToString();
                MD.MailID = dt.Rows[i]["MailID"].ToString();
                MD.ContactNo = dt.Rows[i]["ContactNo"].ToString();
                MD.Emergency_ContactNO = dt.Rows[i]["Emergency_ContactNO"].ToString();
                DateTime BirthDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["BirthDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out BirthDate);
                MD.BirthDate = BirthDate;
                DateTime Anniversary_Date = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["Anniversary_Date"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out Anniversary_Date);
                MD.Anniversary_Date = Anniversary_Date;
                MD.Reference_Documents = dt.Rows[i]["Reference_Documents"].ToString();
                DateTime JoinDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["JoinDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out JoinDate);
                MD.JoinDate = JoinDate;
                MD.PancardNo = dt.Rows[i]["PancardNo"].ToString();
                MD.AadharcardNo = dt.Rows[i]["AadharcardNo"].ToString();
                MD.Address = dt.Rows[i]["Address"].ToString();
                MD.Profile_Image_Path = dt.Rows[i]["Profile_Image_Path"].ToString();
                TimeSpan ClinicShiftTimeFrom = new TimeSpan(); DateTime ClinicShiftTimeD = new DateTime();
                DateTime.TryParseExact(dt.Rows[i]["Clinic_Shift_TimeFrom"].ToString(), DbHelper.Configuration.timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ClinicShiftTimeD);
                if (ClinicShiftTimeD > DateTime.MinValue && ClinicShiftTimeD < DateTime.MaxValue)
                {
                    ClinicShiftTimeFrom = ClinicShiftTimeD.TimeOfDay;
                }
                MD.Clinic_Shift_TimeFrom = ClinicShiftTimeFrom;
                TimeSpan ClinicShiftTimeUpto = new TimeSpan(); DateTime ClinicShiftTimeUptoD = new DateTime();
                DateTime.TryParseExact(dt.Rows[i]["Clinic_Shift_TimeUpto"].ToString(), DbHelper.Configuration.timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ClinicShiftTimeUptoD);
                if (ClinicShiftTimeUptoD > DateTime.MinValue && ClinicShiftTimeUptoD < DateTime.MaxValue)
                {
                    ClinicShiftTimeUpto = ClinicShiftTimeUptoD.TimeOfDay;
                }
                MD.Clinic_Shift_TimeUpto = ClinicShiftTimeUpto;
                MD.Clinic_Shift_TimeFromChar = dt.Rows[i]["Clinic_Shift_TimeFromChar"].ToString();
                MD.Clinic_Shift_TimeUptoChar = dt.Rows[i]["Clinic_Shift_TimeUptoChar"].ToString();
                MD.BloodGroup = dt.Rows[i]["BloodGroup"].ToString();
            }
            return MD;
        }

        public List<SnehDLL.Management_Dll> GetList(int id, string _fullName, DateTime _fromDate, DateTime _uptoDate)
        {
            List<SnehDLL.Management_Dll> MD = new List<SnehDLL.Management_Dll>();
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
                SnehDLL.Management_Dll D = new SnehDLL.Management_Dll();
                D.TYPEID = int.Parse(dt.Rows[i]["TYPEID"].ToString());
                D.ManagerID = int.Parse(dt.Rows[i]["ManagerID"].ToString());
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
                MD.Add(D);
            }
            return MD;
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

        public int Delete(int managerid)
        {
            SqlCommand cmd = new SqlCommand("Management_Mast_Delete"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ManagerID", SqlDbType.Int).Value = managerid;

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
            SqlCommand cmd = new SqlCommand("Get_ManagerList"); cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = db.DbRead(cmd); return dt;
        }
    }
}
