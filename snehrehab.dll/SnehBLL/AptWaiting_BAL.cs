using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SnehBLL
{
    public class AptWaiting_BAL
    {
        DbHelper.SqlDb db;

        public AptWaiting_BAL()
        {
            db = new DbHelper.SqlDb();
        }

        public static int Check(string _uniqueID)
        {
            int _appointmentID = 0;
            SqlCommand cmd = new SqlCommand("AptWaiting_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = _uniqueID;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = 0;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["AppointmentID"].ToString(), out _appointmentID);
            }
            return _appointmentID;
        }

        public static string Check(int _appointmentID)
        {
            string _uniqueID = "";
            SqlCommand cmd = new SqlCommand("AptWaiting_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = "";
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                _uniqueID = dt.Rows[0]["UniqueID"].ToString();
            }
            return _uniqueID;
        }

        public int Set(SnehDLL.AptWaiting_DAL D)
        {
            SqlCommand cmd = new SqlCommand("AptWaiting_Set"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = D.AppointmentID;
            cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = D.PatientID;
            cmd.Parameters.Add("@SessionID", SqlDbType.Int).Value = D.SessionID;
            if (D.AppointmentDate > DateTime.MinValue)
                cmd.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = D.AppointmentDate;
            else
                cmd.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = DBNull.Value;
            if (D.AppointmentTime > TimeSpan.MinValue && D.AppointmentTime < TimeSpan.MaxValue)
                cmd.Parameters.Add("@AppointmentTime", SqlDbType.Time).Value = D.AppointmentTime;
            else
                cmd.Parameters.Add("@AppointmentTime", SqlDbType.Time).Value = DBNull.Value;
            if (D.AppointmentFrom > DateTime.MinValue)
                cmd.Parameters.Add("@AppointmentFrom", SqlDbType.DateTime).Value = D.AppointmentFrom;
            else
                cmd.Parameters.Add("@AppointmentFrom", SqlDbType.DateTime).Value = DBNull.Value;

            if (D.AppointmentUpto > DateTime.MinValue)
                cmd.Parameters.Add("@AppointmentUpto", SqlDbType.DateTime).Value = D.AppointmentUpto;
            else
                cmd.Parameters.Add("@AppointmentUpto", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@Duration", SqlDbType.Int).Value = D.Duration;
            cmd.Parameters.Add("@Narration", SqlDbType.VarChar, 4000).Value = D.Narration;

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

        public DataTable Search(int _statusID, int _SessionID, int _doctorID, string _searchText, DateTime _fromDate, DateTime _uptoDate)
        {
            SqlCommand cmd = new SqlCommand("AptWaiting_Search"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentStatus", SqlDbType.Int).Value = _statusID;
            cmd.Parameters.Add("@SessionID", SqlDbType.Int).Value = _SessionID;
            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 50).Value = _searchText;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = _doctorID;
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


        public int Status(int AppointmentID, int AppointmentStatus)
        {
            SqlCommand cmd = new SqlCommand("AptWaiting_Status"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = AppointmentID;
            cmd.Parameters.Add("@AppointmentStatus", SqlDbType.Int).Value = AppointmentStatus;

            SqlParameter Param = new SqlParameter(); Param.ParameterName = "@RetVal";
            Param.DbType = DbType.Int64; Param.Direction = ParameterDirection.Output;
            Param.Value = 0; cmd.Parameters.Add(Param);

            DbHelper.SqlDb db = new DbHelper.SqlDb(); db.DbUpdate(cmd);

            int i = 0;
            if (cmd.Parameters["@RetVal"].Value != null)
            {
                int.TryParse(cmd.Parameters["@RetVal"].Value.ToString(), out i);
            }
            return i;
        }

        public DataTable AptDetail(int _appointmentID)
        {
            SqlCommand cmd = new SqlCommand(@"SELECT
		                                    AM.AppointmentID, 
		                                    AM.UniqueID, 
                                            PM.UniqueID AS PatUniqueID,
		                                    PM.PreFix, 
		                                    PM.FullName, 
		                                    SM.SessionName, 
		                                    CONVERT(VARCHAR(50), AM.AppointmentDate, 20) AS AppointmentDate, 
		                                    AM.Duration AS Duration, 
		                                    CONVERT(VARCHAR(50), AM.AppointmentTime, 20) AS AppointmentTime, 
		                                    CONVERT(VARCHAR(50), AM.AppointmentFrom, 20) AS AppointmentFrom, 
		                                    CONVERT(VARCHAR(50), AM.AppointmentUpto, 20) AS AppointmentUpto, 
		                                    COALESCE(AM.AppointmentStatus, 0) AS AppointmentStatus, 
                                            PT.PatientTypeID,
		                                    PT.PatientType,
		                                    (SELECT TOP (1) DM.DoctorID FROM AptWaitingDoctor AS AD INNER JOIN DoctorMast AS DM ON AD.DoctorID = DM.DoctorID WHERE (COALESCE (AD.IsMain, CAST('False' AS BIT)) = CAST('True' AS BIT)) AND (AD.AppointmentID = AM.AppointmentID)) AS TherapistID,
		                                    (SELECT TOP 1 DM.PreFix + ' ' + DM.FullName FROM AptWaitingDoctor AS AD INNER JOIN DoctorMast AS DM ON AD.DoctorID = DM.DoctorID
		                                    WHERE (COALESCE (AD.IsMain, CAST('False' AS BIT)) = CAST('True' AS BIT)) AND (AD.AppointmentID = AM.AppointmentID)) AS Therapist,
		                                    '' AS Remark
		                                    FROM PatientTypes AS PT RIGHT OUTER JOIN PatientMast AS PM ON PT.PatientTypeID = PM.PatientTypeID RIGHT OUTER JOIN
		                                    AptWaiting AS AM ON PM.PatientID = AM.PatientID LEFT OUTER JOIN SessionMast AS SM ON AM.SessionID = SM.SessionID
		                                    WHERE COALESCE(AM.IsDeleted, CAST('False' AS BIT)) <> CAST('True' AS BIT) 
                                            AND AM.AppointmentID = @AppointmentID"); cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;

            return db.DbRead(cmd);
        }

        public string GetActucalID(int appointmentID)
        {
            SqlCommand cmd = new SqlCommand(@"SELECT UniqueID FROM Appointments WHERE WaitingID = @WaitingID"); cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@WaitingID", SqlDbType.Int).Value = appointmentID;

            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["UniqueID"].ToString();
            }
            return string.Empty;
        }
    }
}
