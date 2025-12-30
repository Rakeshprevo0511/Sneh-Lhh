using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace SnehBLL
{
    public class AppointmentChangeRequest_Bll
    {
        DbHelper.SqlDb db;

        public AppointmentChangeRequest_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public static int Check(string _uniqueID)
        {
            int _requestID = 0;
            SqlCommand cmd = new SqlCommand("AppointmentChangeRequest_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = _uniqueID;
            cmd.Parameters.Add("@RequestID", SqlDbType.Int).Value = 0;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["RequestID"].ToString(), out _requestID);
            }
            return _requestID;
        }

        public static string Check(int _requestID)
        {
            string _uniqueID = "";
            SqlCommand cmd = new SqlCommand("AppointmentChangeRequest_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = "";
            cmd.Parameters.Add("@RequestID", SqlDbType.Int).Value = _requestID;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                _uniqueID = dt.Rows[0]["UniqueID"].ToString();
            }
            return _uniqueID;
        }

        public SnehDLL.AppointmentChangeRequest_Dll Get(int _requestID)
        {
            SnehDLL.AppointmentChangeRequest_Dll D = null;
            SqlCommand cmd = new SqlCommand("AppointmentChangeRequest_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@RequestID", SqlDbType.Int).Value = _requestID;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                D = new SnehDLL.AppointmentChangeRequest_Dll(); int i = 0;
                D.RequestID = int.Parse(dt.Rows[i]["RequestID"].ToString());
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                int AppointmentID = 0; int.TryParse(dt.Rows[i]["AppointmentID"].ToString(), out AppointmentID); D.AppointmentID = AppointmentID;
                int ReqDoctorID = 0; int.TryParse(dt.Rows[i]["ReqDoctorID"].ToString(), out ReqDoctorID); D.ReqDoctorID = ReqDoctorID;
                int AssignToDoctorID = 0; int.TryParse(dt.Rows[i]["AssignToDoctorID"].ToString(), out AssignToDoctorID); D.AssignToDoctorID = AssignToDoctorID;
                D.Remarks = dt.Rows[i]["Remarks"].ToString();
                DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                D.AddedDate = AddedDate;
                DateTime ModifyDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["ModifyDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ModifyDate);
                D.ModifyDate = ModifyDate;
                int AddedBy = 0; int.TryParse(dt.Rows[i]["AddedBy"].ToString(), out AddedBy); D.AddedBy = AddedBy;
                int ModifyBy = 0; int.TryParse(dt.Rows[i]["ModifyBy"].ToString(), out ModifyBy); D.ModifyBy = ModifyBy;
                int RequestStatus = 0; int.TryParse(dt.Rows[i]["RequestStatus"].ToString(), out RequestStatus); D.RequestStatus = RequestStatus;
                DateTime StatusDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["StatusDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out StatusDate);
                D.StatusDate = StatusDate;
                D.StatusRemark = dt.Rows[i]["StatusRemark"].ToString();
            }
            return D;
        }

        public int New(SnehDLL.AppointmentChangeRequest_Dll D)
        {
            SqlCommand cmd = new SqlCommand("AppointmentChangeRequest_New"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = D.AppointmentID;
            cmd.Parameters.Add("@ReqDoctorID", SqlDbType.Int).Value = D.ReqDoctorID;
            cmd.Parameters.Add("@AssignToDoctorID", SqlDbType.Int).Value = D.AssignToDoctorID;
            cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 4000).Value = D.Remarks;
            if (D.ModifyDate > DateTime.MinValue)
                cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = D.ModifyDate;
            else
                cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@ModifyBy", SqlDbType.Int).Value = D.ModifyBy;

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

        public int Delete(int _requestID)
        {
            SqlCommand cmd = new SqlCommand("AppointmentChangeRequest_Delete"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@RequestID", SqlDbType.Int).Value = _requestID;

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

        public int Reject(int _requestID)
        {
            SqlCommand cmd = new SqlCommand("AppointmentChangeRequest_Reject"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@RequestID", SqlDbType.Int).Value = _requestID;

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

        public int getDoctorID(int _loginID)
        {
            int _doctorID = 0;
            SqlCommand cmd = new SqlCommand("SELECT DoctorID FROM UserAccount WHERE UserID = @UserID "); cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _loginID;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
                int.TryParse(dt.Rows[0]["DoctorID"].ToString(), out _doctorID);
            return _doctorID;
        }

        public static int pending_count(out string _dateS)
        {
            int _count = 0; _dateS = "";
            SqlCommand cmd = new SqlCommand("SELECT COUNT(1) AS Pending, CONVERT(VARCHAR(50), MIN(AddedDate), 20) AS AddedDate FROM AppointmentChangeRequest WHERE COALESCE(RequestStatus, 0) = 0 "); cmd.CommandType = CommandType.Text;
            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0][0].ToString(), out _count);
                DateTime _date = new DateTime(); DateTime.TryParseExact(dt.Rows[0][1].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _date);
                if (_date > DateTime.MinValue)
                    _dateS = _date.ToString(DbHelper.Configuration.showDateFormat);
                if (_dateS == DateTime.UtcNow.AddMinutes(330).ToString(DbHelper.Configuration.showDateFormat))
                {
                    _dateS = "";
                }
            }
            return _count;
        }
    }
}
