using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SnehBLL
{
    public class MeetngScheTime_Bll
    {
        DbHelper.SqlDb db;
        public MeetngScheTime_Bll()
        {
            db = new DbHelper.SqlDb();
        }
        public SnehDLL.MeetingScheTime_Dll GetMeetScheTime(int _timeID)
        {
            SnehDLL.MeetingScheTime_Dll D = null;
            SqlCommand cmd = new SqlCommand("MeetScheTime_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@TimeID", SqlDbType.Int).Value = _timeID;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                D = new SnehDLL.MeetingScheTime_Dll(); int i = 0;
                D.TimeID = int.Parse(dt.Rows[i]["TimeID"].ToString());
                D.TimeFrom = dt.Rows[i]["TimeFrom"].ToString();
                D.TimeUpto = dt.Rows[i]["TimeUpto"].ToString();
                TimeSpan TimeHour = new TimeSpan(); DateTime TimeHourD = new DateTime();
                DateTime.TryParseExact(dt.Rows[i]["TimeHour"].ToString(), DbHelper.Configuration.timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out TimeHourD);
                if (TimeHourD > DateTime.MinValue && TimeHourD < DateTime.MaxValue)
                {
                    TimeHour = TimeHourD.TimeOfDay;
                }
                D.TimeHour = TimeHour;
            }
            return D;
        }
        public DataTable SearchDrMeetingSche(int _statusID, int _doctorID, DateTime _fromDate, DateTime _uptoDate, int _duration)
        {
            SqlCommand cmd = new SqlCommand("Appointments_SearchDrMeetingSchedule"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentStatus", SqlDbType.Int).Value = _statusID;
            cmd.Parameters.Add("@DoctorID", SqlDbType.VarChar).Value = _doctorID;
            cmd.Parameters.Add("@DURATION", SqlDbType.Int).Value = _duration;
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
        public SnehDLL.MeetingScheTime_Dll Getting(int _appointmentID)
        {
            SnehDLL.MeetingScheTime_Dll D = null;
            SqlCommand cmd = new SqlCommand("AppointmentsDrMeetSche_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                D = new SnehDLL.MeetingScheTime_Dll(); int i = 0;
                D.AppointmentID = int.Parse(dt.Rows[i]["AppointmentID"].ToString());
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                bool IsDeleted = false; bool.TryParse(dt.Rows[i]["IsDeleted"].ToString(), out IsDeleted); D.IsDeleted = IsDeleted;
                DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                D.AddedDate = AddedDate;
                DateTime AppointmentDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AppointmentDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AppointmentDate);
                D.AppointmentDate = AppointmentDate;
                D.Available1FromChar = dt.Rows[i]["Available1FromChar"].ToString();
                D.Available1UptoChar = dt.Rows[i]["Available1UptoChar"].ToString();
                DateTime ModifyDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["ModifyDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ModifyDate);
                D.ModifyDate = ModifyDate;
                int AddedBy = 0; int.TryParse(dt.Rows[i]["AddedBy"].ToString(), out AddedBy); D.AddedBy = AddedBy;
                int ModifyBy = 0; int.TryParse(dt.Rows[i]["ModifyBy"].ToString(), out ModifyBy); D.ModifyBy = ModifyBy;
            }
            return D;
        }
        public int setDrMeetSche(SnehDLL.MeetingScheTime_Dll D)
        {
            SqlCommand cmd = new SqlCommand("Appointments_SetDrMeetSch"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = D.AppointmentID;
            if (D.AppointmentDate > DateTime.MinValue)
                cmd.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = D.AppointmentDate;
            else
                cmd.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = DBNull.Value;
           
            if (D.AppointmentFrom > DateTime.MinValue)
                cmd.Parameters.Add("@AppointmentFrom", SqlDbType.DateTime).Value = D.AppointmentFrom;
            else
                cmd.Parameters.Add("@AppointmentFrom", SqlDbType.DateTime).Value = DBNull.Value;

            if (D.AppointmentUpto > DateTime.MinValue)
                cmd.Parameters.Add("@AppointmentUpto", SqlDbType.DateTime).Value = D.AppointmentUpto;
            else
                cmd.Parameters.Add("@AppointmentUpto", SqlDbType.DateTime).Value = DBNull.Value;
            if (D.Available1From > TimeSpan.MinValue && D.Available1From < TimeSpan.MaxValue && D.Available1FromChar.Length > 0)
                cmd.Parameters.Add("@Available1From", SqlDbType.Time).Value = D.Available1From;
            else
                cmd.Parameters.Add("@Available1From", SqlDbType.Time).Value = DBNull.Value;

            if (D.Available1Upto > TimeSpan.MinValue && D.Available1Upto < TimeSpan.MaxValue && D.Available1UptoChar.Length > 0)
                cmd.Parameters.Add("@Available1Upto", SqlDbType.Time).Value = D.Available1Upto;
            else
                cmd.Parameters.Add("@Available1Upto", SqlDbType.Time).Value = DBNull.Value;
            if (D.ModifyDate > DateTime.MinValue)
                cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = D.ModifyDate;
            else
                cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@ScheduleType", SqlDbType.VarChar, 250).Value = D.ScheduleType;
            cmd.Parameters.Add("@ModifyBy", SqlDbType.Int).Value = D.ModifyBy;
            cmd.Parameters.Add("@Available1FromChar", SqlDbType.VarChar, 50).Value = D.Available1FromChar;
            cmd.Parameters.Add("@Available1UptoChar", SqlDbType.VarChar, 50).Value = D.Available1UptoChar;

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
        public List<SnehDLL.MeetingScheTime_Dll> GetList()
        {
            List<SnehDLL.MeetingScheTime_Dll> DL = new List<SnehDLL.MeetingScheTime_Dll>();
            SqlCommand cmd = new SqlCommand("MeetingScheTime_GetList"); cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.MeetingScheTime_Dll D = new SnehDLL.MeetingScheTime_Dll();
                D.TimeID = int.Parse(dt.Rows[i]["TimeID"].ToString());
                D.TimeFrom = dt.Rows[i]["TimeFrom"].ToString();
                D.TimeUpto = dt.Rows[i]["TimeUpto"].ToString();
                TimeSpan TimeHour = new TimeSpan(); DateTime TimeHourD = new DateTime();
                DateTime.TryParseExact(dt.Rows[i]["TimeHour"].ToString(), DbHelper.Configuration.timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out TimeHourD);
                if (TimeHourD > DateTime.MinValue && TimeHourD < DateTime.MaxValue)
                {
                    TimeHour = TimeHourD.TimeOfDay;
                }
                D.TimeHour = TimeHour;

                DL.Add(D);
            }
            return DL;
        }
    }
}
