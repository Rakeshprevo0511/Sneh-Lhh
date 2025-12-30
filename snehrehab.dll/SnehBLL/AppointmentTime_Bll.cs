using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace SnehBLL
{
    public class AppointmentTime_Bll
    {
        DbHelper.SqlDb db;

        public AppointmentTime_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public SnehDLL.AppointmentTime_Dll Get(int _timeID)
        {
            SnehDLL.AppointmentTime_Dll D = null;
            SqlCommand cmd = new SqlCommand("AppointmentTime_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@TimeID", SqlDbType.Int).Value = _timeID;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                D = new SnehDLL.AppointmentTime_Dll(); int i = 0;
                D.TimeID = int.Parse(dt.Rows[i]["TimeID"].ToString());
                D.TimeName = dt.Rows[i]["TimeName"].ToString();
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

        public List<SnehDLL.AppointmentTime_Dll> GetList()
        {
            List<SnehDLL.AppointmentTime_Dll> DL = new List<SnehDLL.AppointmentTime_Dll>();
            SqlCommand cmd = new SqlCommand("AppointmentTime_GetList"); cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.AppointmentTime_Dll D = new SnehDLL.AppointmentTime_Dll();
                D.TimeID = int.Parse(dt.Rows[i]["TimeID"].ToString());
                D.TimeName = dt.Rows[i]["TimeName"].ToString();
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

        public List<SnehDLL.AppointmentTime_Dll> PatientSchedule(TimeSpan timeSpan, int _duration)
        {
            List<SnehDLL.AppointmentTime_Dll> DL = new List<SnehDLL.AppointmentTime_Dll>();
            SqlCommand cmd = new SqlCommand("AppointmentTime_GetPatientSchedule"); cmd.CommandType = CommandType.StoredProcedure;
            if (timeSpan > TimeSpan.MinValue && timeSpan < TimeSpan.MaxValue)
                cmd.Parameters.Add("@TimeHour", SqlDbType.Time).Value = timeSpan;
            else
                cmd.Parameters.Add("@TimeHour", SqlDbType.Time).Value = DBNull.Value;

            cmd.Parameters.Add("@Duration", SqlDbType.Int).Value = _duration;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.AppointmentTime_Dll D = new SnehDLL.AppointmentTime_Dll();
                D.TimeID = int.Parse(dt.Rows[i]["TimeID"].ToString());
                D.TimeName = dt.Rows[i]["TimeName"].ToString();
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

        public List<SnehDLL.AppointmentTime_Dll> DoctorSchedule(TimeSpan timeSpan, int _duration)
        {
            List<SnehDLL.AppointmentTime_Dll> DL = new List<SnehDLL.AppointmentTime_Dll>();
            SqlCommand cmd = new SqlCommand("AppointmentTime_GetDoctorSchedule"); cmd.CommandType = CommandType.StoredProcedure;
            if (timeSpan > TimeSpan.MinValue && timeSpan < TimeSpan.MaxValue)
                cmd.Parameters.Add("@TimeHour", SqlDbType.Time).Value = timeSpan;
            else
                cmd.Parameters.Add("@TimeHour", SqlDbType.Time).Value = DBNull.Value;

            cmd.Parameters.Add("@Duration", SqlDbType.Int).Value = _duration;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.AppointmentTime_Dll D = new SnehDLL.AppointmentTime_Dll();
                D.TimeID = int.Parse(dt.Rows[i]["TimeID"].ToString());
                D.TimeName = dt.Rows[i]["TimeName"].ToString();
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
        public List<SnehDLL.Appointments_Dll> GetWaitingListDr(int _appointmentID)
        {
            List<SnehDLL.Appointments_Dll> DL = new List<SnehDLL.Appointments_Dll>();
            SqlCommand cmd = new SqlCommand("AppointmentTime_GetWaitingList"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("AppointmentID", SqlDbType.Int).Value = _appointmentID;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.Appointments_Dll D = new SnehDLL.Appointments_Dll();
                DateTime AppointmentDate = new DateTime();
                DateTime.TryParseExact(dt.Rows[i]["AppointmentDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AppointmentDate);
                D.AppointmentDate = AppointmentDate;
                DateTime AppointmentFrom = new DateTime();
                DateTime.TryParseExact(dt.Rows[i]["AppointmentFrom"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AppointmentFrom);
                D.AppointmentFrom = AppointmentFrom;
                DateTime AppointmentUpto = new DateTime();
                DateTime.TryParseExact(dt.Rows[i]["AppointmentUpto"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AppointmentUpto);
                D.AppointmentUpto = AppointmentUpto;
                int doctorId = 0; int.TryParse(dt.Rows[i]["DoctorID"].ToString(), out doctorId);
                D.doctorId = doctorId;
                DL.Add(D);
            }
            return DL;
        }
        public List<SnehDLL.AppointmentTime_Dll> GetListDrByDate(DateTime _appointmentDate, int _doctorId)
        {
            List<SnehDLL.AppointmentTime_Dll> DL = new List<SnehDLL.AppointmentTime_Dll>();
            SqlCommand cmd = new SqlCommand("AppointmentTime_GetListByDAte"); cmd.CommandType = CommandType.StoredProcedure;

            if (_appointmentDate > DateTime.MinValue)
                cmd.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = _appointmentDate;
            else
                cmd.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = _doctorId;

            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.AppointmentTime_Dll D = new SnehDLL.AppointmentTime_Dll();
                D.AppointmentID = int.Parse(dt.Rows[i]["AppointmentID"].ToString());
                DateTime AppointmentFrom = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AppointmentFrom"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AppointmentFrom);
                D.AppointmentFrom = AppointmentFrom;
                DateTime AppointmentUpto = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AppointmentUpto"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AppointmentUpto);
                D.AppointmentUpto = AppointmentUpto;
                DL.Add(D);
            }
            return DL;
        }
        public List<SnehDLL.Appointments_Dll> GetListDrByDate_new(DateTime _appointmentDate, int _doctorId)
        {
            List<SnehDLL.Appointments_Dll> DL = new List<SnehDLL.Appointments_Dll>();
            SqlCommand cmd = new SqlCommand("AppointmentTime_GetListByDAte_NEW"); cmd.CommandType = CommandType.StoredProcedure;

            if (_appointmentDate > DateTime.MinValue)
                cmd.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = _appointmentDate;
            else
                cmd.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = _doctorId;

            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.Appointments_Dll D = new SnehDLL.Appointments_Dll();
                D.AppointmentID = int.Parse(dt.Rows[i]["AppointmentID"].ToString());
                DateTime AvailableFrom = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["Available1From"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AvailableFrom);
                D.Available1From = AvailableFrom;
                DateTime AvailableUpto = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["Available1Upto"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AvailableUpto);
                D.Available1Upto = AvailableUpto;
                DL.Add(D);
            }
            return DL;
        }
        public List<SnehDLL.AppointmentTime_Dll> DoctorMettingSchedule(int _doctorID, DateTime _appointmentDate)
        {
            List<SnehDLL.AppointmentTime_Dll> DL = new List<SnehDLL.AppointmentTime_Dll>();
            SqlCommand cmd = new SqlCommand("AppointmentTime_GetDOCTORMeetingSchedule"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = _doctorID;
            if (_appointmentDate > DateTime.MinValue)
                cmd.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = _appointmentDate;
            else
                cmd.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = DBNull.Value;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.AppointmentTime_Dll D = new SnehDLL.AppointmentTime_Dll();
                D.TimeID = int.Parse(dt.Rows[i]["TimeID"].ToString());
                D.TimeID2 = int.Parse(dt.Rows[i]["TimeID2"].ToString());

                D.Available1FromChar = dt.Rows[i]["Available1FromChar"].ToString();

                TimeSpan Available1FromTime = new TimeSpan(); DateTime Available1FromTimeD = new DateTime();
                DateTime.TryParseExact(dt.Rows[i]["Available1FromTime"].ToString(), DbHelper.Configuration.timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out Available1FromTimeD);
                if (Available1FromTimeD > DateTime.MinValue && Available1FromTimeD < DateTime.MaxValue)
                {
                    Available1FromTime = Available1FromTimeD.TimeOfDay;
                }
                D.Available1FromTime = Available1FromTime;

                D.Available1UptoChar = dt.Rows[i]["Available1UptoChar"].ToString();
                TimeSpan Available1UptoTime = new TimeSpan(); DateTime Available1UptoTimeD = new DateTime();
                DateTime.TryParseExact(dt.Rows[i]["Available1UptoTime"].ToString(), DbHelper.Configuration.timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out Available1UptoTimeD);
                if (Available1UptoTimeD > DateTime.MinValue && Available1UptoTimeD < DateTime.MaxValue)
                {
                    Available1UptoTime = Available1UptoTimeD.TimeOfDay;
                }
                D.Available1UptoTime = Available1UptoTime;

                DL.Add(D);
            }
            return DL;
        }
        public List<SnehDLL.Appointments_Dll> GetListAppointDr(DateTime _appointmentDate, int _doctorId)
        {
            List<SnehDLL.Appointments_Dll> DL = new List<SnehDLL.Appointments_Dll>();
            SqlCommand cmd = new SqlCommand("AppointmentTime_GetAppointListDr"); cmd.CommandType = CommandType.StoredProcedure;

            if (_appointmentDate > DateTime.MinValue)
                cmd.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = _appointmentDate;
            else
                cmd.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@DoctorId", SqlDbType.Int).Value = _doctorId;

            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.Appointments_Dll D = new SnehDLL.Appointments_Dll();
                D.AppointmentID = int.Parse(dt.Rows[i]["AppointmentID"].ToString());
                DateTime AppointmentFrom = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["Available1From"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AppointmentFrom);
                D.Available1From = AppointmentFrom;

                DateTime AppointmentUpto = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["Available1Upto"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AppointmentUpto);
                D.Available1Upto = AppointmentUpto;
                DL.Add(D);
            }
            return DL;
        }
        public List<SnehDLL.AppointmentTime_Dll> GetListDr(DateTime _appointmentDate, int _doctorId)
        {
            List<SnehDLL.AppointmentTime_Dll> DL = new List<SnehDLL.AppointmentTime_Dll>();
            SqlCommand cmd = new SqlCommand("AppointmentTime_GetListDr"); cmd.CommandType = CommandType.StoredProcedure;

            if (_appointmentDate > DateTime.MinValue)
                cmd.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = _appointmentDate;
            else
                cmd.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = _doctorId;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.AppointmentTime_Dll D = new SnehDLL.AppointmentTime_Dll();
                D.AppointmentID = int.Parse(dt.Rows[i]["AppointmentID"].ToString());
                DateTime AppointmentFrom = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AppointmentFrom"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AppointmentFrom);
                D.AppointmentFrom = AppointmentFrom;
                DateTime AppointmentUpto = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AppointmentUpto"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AppointmentUpto);
                D.AppointmentUpto = AppointmentUpto;
                DL.Add(D);
            }
            return DL;
        }
        public List<SnehDLL.AppointmentTime_Dll> DoctorSchedule(int _leaveID)
        {
            List<SnehDLL.AppointmentTime_Dll> DL = new List<SnehDLL.AppointmentTime_Dll>();
            SqlCommand cmd = new SqlCommand("AppointmentTime_GetDoctorLeave"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@LeaveID", SqlDbType.Int).Value = _leaveID;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.AppointmentTime_Dll D = new SnehDLL.AppointmentTime_Dll();
                D.TimeID = int.Parse(dt.Rows[i]["TimeID"].ToString());
                D.TimeName = dt.Rows[i]["TimeName"].ToString();
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

        public SnehDLL.AppointmentTime_Dll Get(TimeSpan _timeHour)
        {
            SnehDLL.AppointmentTime_Dll D = null;
            SqlCommand cmd = new SqlCommand("AppointmentTime_GetID"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@TimeHour", SqlDbType.Time).Value = _timeHour;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                D = new SnehDLL.AppointmentTime_Dll(); int i = 0;
                D.TimeID = int.Parse(dt.Rows[i]["TimeID"].ToString());
                D.TimeName = dt.Rows[i]["TimeName"].ToString();
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
    }
}
