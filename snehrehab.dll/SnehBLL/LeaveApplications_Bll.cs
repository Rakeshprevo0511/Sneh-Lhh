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
    public class LeaveApplications_Bll
    {
        DbHelper.SqlDb db;

        public LeaveApplications_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public static int Check(string _uniqueID)
        {
            int _leaveID = 0;
            SqlCommand cmd = new SqlCommand("LeaveApplications_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = _uniqueID;
            cmd.Parameters.Add("@LeaveID", SqlDbType.Int).Value = 0;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["LeaveID"].ToString(), out _leaveID);
            }
            return _leaveID;
        }

        public static string Check(int _leaveID)
        {
            string _uniqueID = "";
            SqlCommand cmd = new SqlCommand("LeaveApplications_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = "";
            cmd.Parameters.Add("@LeaveID", SqlDbType.Int).Value = _leaveID;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                _uniqueID = dt.Rows[0]["UniqueID"].ToString();
            }
            return _uniqueID;
        }

        public int Delete(int _leaveID)
        {
            SqlCommand cmd = new SqlCommand("LeaveApplications_Delete"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@LeaveID", SqlDbType.Int).Value = _leaveID;

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

        public int Set(SnehDLL.LeaveApplications_Dll D)
        {
            SqlCommand cmd = new SqlCommand("LeaveApplications_Set"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@LeaveID", SqlDbType.Int).Value = D.LeaveID;
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = D.UserID;
            if (D.FromDate > DateTime.MinValue)
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = D.FromDate;
            else
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = DBNull.Value;
            if (D.UptoDate > DateTime.MinValue)
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = D.UptoDate;
            else
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = DBNull.Value;

            if (D.FromTime > TimeSpan.MinValue && D.FromTime < TimeSpan.MaxValue)
                cmd.Parameters.Add("@FromTime", SqlDbType.Time).Value = D.FromTime;
            else
                cmd.Parameters.Add("@FromTime", SqlDbType.Time).Value = DBNull.Value;

            if (D.UptoTime > TimeSpan.MinValue && D.UptoTime < TimeSpan.MaxValue)
                cmd.Parameters.Add("@UptoTime", SqlDbType.Time).Value = D.UptoTime;
            else
                cmd.Parameters.Add("@UptoTime", SqlDbType.Time).Value = DBNull.Value;
            cmd.Parameters.Add("@TypeID", SqlDbType.Int).Value = D.TypeID;
            cmd.Parameters.Add("@Reason", SqlDbType.VarChar, 4000).Value = D.Reason;
            cmd.Parameters.Add("@cAddress", SqlDbType.VarChar, 4000).Value = D.cAddress;
            cmd.Parameters.Add("@cNumber", SqlDbType.VarChar, 250).Value = D.cNumber;

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

        public DataTable Search(int _statusID, string _fullName, DateTime _fromDate, DateTime _uptoDate)
        {
            SqlCommand cmd = new SqlCommand("LeaveApplications_Search"); cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@LeaveStatus", SqlDbType.Int).Value = _statusID;
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

        public DataTable Search(int _userID)
        {
            SqlCommand cmd = new SqlCommand("LeaveApplications_MySearch"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

            return db.DbRead(cmd);
        }

        public int Balance(int _userID, int _leaveType)
        {
            SqlCommand cmd = new SqlCommand("LeaveApplications_Balance"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
            cmd.Parameters.Add("@TypeID", SqlDbType.Int).Value = _leaveType;

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

        public DataTable Pending(string _fullName, DateTime _fromDate, DateTime _uptoDate)
        {
            SqlCommand cmd = new SqlCommand("LeaveApplications_Pending"); cmd.CommandType = CommandType.StoredProcedure;
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

        public DataTable Get(int _leaveID)
        {
            SqlCommand cmd = new SqlCommand("LeaveApplications_Detail"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@LeaveID", SqlDbType.Int).Value = _leaveID;

            return db.DbRead(cmd);
        }

        public int SetStatus(int _leaveID, int _status)
        {
            SqlCommand cmd = new SqlCommand("LeaveApplications_SetStatus"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@LeaveID", SqlDbType.Int).Value = _leaveID;
            cmd.Parameters.Add("@LeaveStatus", SqlDbType.Int).Value = _status;

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

        public int SetStatus(int _leaveID, int _status, string email)
        {
            SqlCommand cmd = new SqlCommand("LeaveApplications_SetByMail"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@LeaveID", SqlDbType.Int).Value = _leaveID;
            cmd.Parameters.Add("@LeaveStatus", SqlDbType.Int).Value = _status;
            cmd.Parameters.Add("@MailID", SqlDbType.NVarChar, 500).Value = email;

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

        public DataTable DoctorSchedule(int _doctorID, DateTime _appointmentDate)
        {
            SqlCommand cmd = new SqlCommand("LeaveApplications_DoctorSchedule"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = _doctorID;
            if (_appointmentDate > DateTime.MinValue)
                cmd.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = _appointmentDate;
            else
                cmd.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = DBNull.Value;

            return db.DbRead(cmd);
        }

        public void ApproveMail(int leaveID)
        {
            string[] LeaveAMailTo = null; string[] LeaveAMailBcc = null; string BranchName = string.Empty; string LeaveMailFt = string.Empty;
            SqlCommand cmd = new SqlCommand("SELECT BranchName, LeaveAMailTo, LeaveAMailBcc, LeaveMailFt FROM SettingsMst"); cmd.CommandType = CommandType.Text;
            DbHelper.SqlDb db = new DbHelper.SqlDb(); DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                BranchName = dt.Rows[0]["BranchName"].ToString();
                LeaveAMailTo = dt.Rows[0]["LeaveAMailTo"].ToString().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                LeaveAMailBcc = dt.Rows[0]["LeaveAMailBcc"].ToString().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                LeaveMailFt = dt.Rows[0]["LeaveMailFt"].ToString();
            }
            if (LeaveAMailTo != null && LeaveAMailTo.Length > 0)
            {
                List<string> mailTo = new List<string>();
                for (int i = 0; LeaveAMailTo != null && i < LeaveAMailTo.Length; i++)
                {
                    if (DbHelper.Configuration.isValidEmail(LeaveAMailTo[i].Trim()))
                    {
                        mailTo.Add(LeaveAMailTo[i].Trim());
                    }
                }
                List<string> mailBcc = new List<string>();
                for (int i = 0; LeaveAMailBcc != null && i < LeaveAMailBcc.Length; i++)
                {
                    if (DbHelper.Configuration.isValidEmail(LeaveAMailBcc[i].Trim()))
                    {
                        mailBcc.Add(LeaveAMailBcc[i].Trim());
                    }
                }
                if (mailTo.Count > 0)
                {
                    cmd = new SqlCommand("SELECT UA.UserID,	LA.UniqueID, UA.FullName, UA.LoginName, UC.CategoryName, S.Speciality, CONVERT(VARCHAR(50), LA.FromDate, 20) AS FromDate, CONVERT(VARCHAR(50), LA.UptoDate, 20) AS UptoDate, CONVERT(VARCHAR(50), LA.FromTime, 20) AS FromTime, CONVERT(VARCHAR(50), LA.UptoTime, 20) AS UptoTime, LA.TypeID,LT.TypeName, LA.Reason, LA.cAddress, " +
                        " LA.LeaveDays, LA.cNumber, LA.LeaveStatus, CASE WHEN COALESCE(LA.LeaveStatus, 0) = 0 THEN 'On Hold' WHEN COALESCE(LA.LeaveStatus, 0) = 1 THEN 'Approved' WHEN COALESCE(LA.LeaveStatus, 0) = 2 THEN 'Rejected' ELSE '- - -' END AS LeaveStatusName, CONVERT(VARCHAR(50), LA.StatusDate, 20) AS StatusDate, CONVERT(VARCHAR(50), LA.AddedDate, 20) AS AddedDate, " +
                        " CONVERT(VARCHAR(50), LA.ModifyDate, 20) AS ModifyDate FROM UserCategory AS UC LEFT OUTER JOIN Specialities AS S ON UC.SpecialityID = S.SpecialityID RIGHT OUTER JOIN UserAccount AS UA ON UC.UserCatID = UA.UserCatID RIGHT OUTER JOIN LeaveApplications AS LA ON UA.UserID = LA.UserID LEFT OUTER JOIN LeaveTypes AS LT ON LA.TypeID = LT.TypeID " +
                        " WHERE LA.LeaveID = @LeaveID"); cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@LeaveID", SqlDbType.Int).Value = leaveID;
                    DataTable dtL = db.DbRead(cmd);
                    if (dtL.Rows.Count > 0)
                    {
                        int _days = 0; int.TryParse(dtL.Rows[0]["LeaveDays"].ToString(), out _days);
                        int _leaveType = 0; int.TryParse(dtL.Rows[0]["TypeID"].ToString(), out _leaveType);
                        DateTime _fromDate = new DateTime(); DateTime.TryParseExact(dtL.Rows[0]["FromDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
                        DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(dtL.Rows[0]["UptoDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
                        string _leaveTypeName = dtL.Rows[0]["TypeName"].ToString();
                        string _date = " on date ";
                        string _time = ""; string _uptodate = "";
                        if (_leaveType == 4)
                        {
                            string fromTimeText = string.Empty; string uptoTimeText = string.Empty;
                            DateTime FromTimeD = new DateTime(); DateTime.TryParseExact(dtL.Rows[0]["FromTime"].ToString(), DbHelper.Configuration.timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out FromTimeD);
                            if (FromTimeD > DateTime.MinValue && FromTimeD < DateTime.MaxValue)
                            {
                                fromTimeText = FromTimeD.ToString(DbHelper.Configuration.showTimeFormat);
                            }
                            DateTime UptoTimeD = new DateTime(); DateTime.TryParseExact(dtL.Rows[0]["UptoTime"].ToString(), DbHelper.Configuration.timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out UptoTimeD);
                            if (UptoTimeD > DateTime.MinValue && UptoTimeD < DateTime.MaxValue)
                            {
                                uptoTimeText = UptoTimeD.ToString(DbHelper.Configuration.showTimeFormat);
                            }
                            _time = " " + fromTimeText + "-" + uptoTimeText;
                        }
                        else if (_days > 1) { _date = " from date "; _uptodate = " to " + _uptoDate.ToString(DbHelper.Configuration.showDateFormat); }

                        string siteUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath.TrimEnd('/');
                        StringBuilder _mailStr = new StringBuilder("<div style=\"width:100%;max-width:600px;margin:0 auto;/*box-shadow: 0px 0px 3px 3px #39b3ca;*/border: 3px solid #39b3ca;font-family:Trebuchet MS;color: #636363;font-size: 15px;\">" +
                                                                    "<table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:100%;line-height: 28px;\">" +
                                                                        "<tr>" +
                                                                            "<td align=\"center\" style=\"padding:10px;background:#FFF;\">" +
                                                                                "<img src=\"" + siteUrl + "/images/snehlogin.png\" alt=\"\" style=\"max-width:339px;max-height:85px;\"/>" +
                                                                            "</td>" +
                                                                        "</tr>" +
                                                                        "<tr>" +
                                                                            "<td>" +
                                                                                "<hr style=\"border: 0px;border-bottom: 1px solid #e6e6e6;\"/>" +
                                                                            "</td>" +
                                                                        "</tr>" +
                                                                        "<tr>" +
                                                                            "<td align=\"justify\" style=\"padding: 10px; background: #FFF;\">" +
                                                                                "<p style=\"margin-bottom: 0px;\">" +
                                                                                   "Leave Application of <b>" + dtL.Rows[0]["FullName"].ToString() + "</b>,<br />" + _leaveTypeName + " for " + _days.ToString() + " days" + _date + _fromDate.ToString(DbHelper.Configuration.showDateFormat) + _time + _uptodate + ". " +
                                                                                "</p>" +
                                                                            "</td>" +
                                                                        "</tr>" +
                                                                        "<tr>" +
                                                                           "<td style=\"padding: 10px; background: #FFF;text-align:justify;\"> " +
                                                                               "<div><b>Leave Reason : </b>" + dtL.Rows[0]["Reason"].ToString() + "</div>" +
                                                                               "<div><b>Contact Address : </b>" + dtL.Rows[0]["cAddress"].ToString() + "</div>" +
                                                                               "<div><b>Contact Number : </b>" + dtL.Rows[0]["cNumber"].ToString() + "</div>" +
                                                                            "</td>" +
                                                                        "</tr>" +
                                                                        "<tr>" +
                                                                            "<td  align=\"justify\" style=\"padding: 10px; background: #FFF;\"> " +
                                                                                "<br />" +
                                                                                "Regards,<br />" +
                                                                                BranchName.Trim() + " Center." +
                                                                                "<br /><br />" +
                                                                            "</td>" +
                                                                        "</tr>" +
                                                                        "<tr>" +
                                                                            "<td style=\"background: #000; padding: 15px; text-align: center;color: #FFF;line-height: 23px;font-size: 13px;\">" +
                                                                                 LeaveMailFt.Trim().Replace(Environment.NewLine, "<br>") +
                                                                            "</td>" +
                                                                        "</tr>" +
                                                                    "</table>" +
                                                                "</div>" +
                                                                "<br />");

                        string _subject = "Leave Application Accepted";
                        SnehBLL.ApiMail_Bll MALB = new SnehBLL.ApiMail_Bll();
                        MALB.send(mailTo.ToArray(), mailBcc.ToArray(), _mailStr.ToString(), _subject);
                    }
                }
            }
        }
    }
}