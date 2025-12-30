using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace SnehBLL
{
    public class Appointments_Bll
    {
        DbHelper.SqlDb db;

        public Appointments_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public static int Check(string _uniqueID)
        {
            int _appointmentID = 0;
            SqlCommand cmd = new SqlCommand("Appointments_Check"); cmd.CommandType = CommandType.StoredProcedure;
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

        public static int GetPatientID(int _appointmentID)
        {
            SqlCommand cmd = new SqlCommand("SELECT PatientID FROM Appointments WHERE AppointmentID=@AppointmentID"); cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            DbHelper.SqlDb db = new DbHelper.SqlDb(); DataTable dt = db.DbRead(cmd); int patientid = 0;
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["PatientID"].ToString(), out patientid);
            }
            return patientid;
        }

        public static string Check(int _appointmentID)
        {
            string _uniqueID = "";
            SqlCommand cmd = new SqlCommand("Appointments_Check"); cmd.CommandType = CommandType.StoredProcedure;
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

        public DataSet PatientDetail(int _patientID)
        {
            SqlCommand cmd = new SqlCommand("Appointments_PatientDetail"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = _patientID;

            return db.DbFetch(cmd);
        }

        public DataSet SessionDetail(int _appointmentID)
        {
            SqlCommand cmd = new SqlCommand("Appointments_SessionDetail"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;

            return db.DbFetch(cmd);
        }

        public List<SnehDLL.DoctorMast_Dll> getTherapist(int _sessionID, DateTime _appointmentDate)
        {
            List<SnehDLL.DoctorMast_Dll> DL = new List<SnehDLL.DoctorMast_Dll>();
            SqlCommand cmd = new SqlCommand("Appointments_getTherapist"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@SessionID", SqlDbType.Int).Value = _sessionID;
            if (_appointmentDate > DateTime.MinValue)
                cmd.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = _appointmentDate;
            else
                cmd.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = DBNull.Value;

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

        public int Set(SnehDLL.Appointments_Dll D)
        {
            SqlCommand cmd = new SqlCommand("Appointments_Set"); cmd.CommandType = CommandType.StoredProcedure;
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

        public int Update(SnehDLL.Appointments_Dll D, int _requestID)
        {
            SqlCommand cmd = new SqlCommand("Appointments_Update"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = D.AppointmentID;
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
            cmd.Parameters.Add("@RequestID", SqlDbType.Int).Value = _requestID;

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

        public List<SnehDLL.Appointments_Dll> GetPatientSchedule(int _patientID, DateTime _appointmentDate)
        {
            List<SnehDLL.Appointments_Dll> DL = new List<SnehDLL.Appointments_Dll>();
            SqlCommand cmd = new SqlCommand("Appointments_PatientSchedule"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = _patientID;
            if (_appointmentDate > DateTime.MinValue)
                cmd.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = _appointmentDate;
            else
                cmd.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = DBNull.Value;

            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.Appointments_Dll D = new SnehDLL.Appointments_Dll();
                D.AppointmentID = int.Parse(dt.Rows[i]["AppointmentID"].ToString());
                TimeSpan AppointmentTime = new TimeSpan(); DateTime AppointmentTimeD = new DateTime();
                DateTime.TryParseExact(dt.Rows[i]["AppointmentTime"].ToString(), DbHelper.Configuration.timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AppointmentTimeD);
                if (AppointmentTimeD > DateTime.MinValue && AppointmentTimeD < DateTime.MaxValue)
                {
                    AppointmentTime = AppointmentTimeD.TimeOfDay;
                }
                D.AppointmentTime = AppointmentTime;
                int Duration = 0; int.TryParse(dt.Rows[i]["Duration"].ToString(), out Duration); D.Duration = Duration;

                DL.Add(D);
            }
            return DL;
        }

        public List<SnehDLL.Appointments_Dll> GetDoctorSchedule(int _doctorID, DateTime _appointmentDate)
        {
            List<SnehDLL.Appointments_Dll> DL = new List<SnehDLL.Appointments_Dll>();
            SqlCommand cmd = new SqlCommand("Appointments_DoctorSchedule"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = _doctorID;
            if (_appointmentDate > DateTime.MinValue)
                cmd.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = _appointmentDate;
            else
                cmd.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = DBNull.Value;

            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.Appointments_Dll D = new SnehDLL.Appointments_Dll();
                D.AppointmentID = int.Parse(dt.Rows[i]["AppointmentID"].ToString());
                TimeSpan AppointmentTime = new TimeSpan(); DateTime AppointmentTimeD = new DateTime();
                DateTime.TryParseExact(dt.Rows[i]["AppointmentTime"].ToString(), DbHelper.Configuration.timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AppointmentTimeD);
                if (AppointmentTimeD > DateTime.MinValue && AppointmentTimeD < DateTime.MaxValue)
                {
                    AppointmentTime = AppointmentTimeD.TimeOfDay;
                }
                D.AppointmentTime = AppointmentTime;
                int Duration = 0; int.TryParse(dt.Rows[i]["Duration"].ToString(), out Duration); D.Duration = Duration;

                DL.Add(D);
            }
            return DL;
        }
        public DataTable SearchDur(int _statusID, int _SessionID, int _doctorID, string _searchText, DateTime _fromDate, DateTime _uptoDate, int _duration, int PageIndex, out int PagingSize, out long TotalRecord)
        {
            TotalRecord = 0; PagingSize = 0;
            //SqlCommand cmd = new SqlCommand("Appointments_Search"); cmd.CommandType = CommandType.StoredProcedure;
            SqlCommand cmd = new SqlCommand("Appointments_Search_test_time_new"); cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@AppointmentStatus", SqlDbType.Int).Value = _statusID;
            cmd.Parameters.Add("@SessionID", SqlDbType.Int).Value = _SessionID;
            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 50).Value = _searchText;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = _doctorID;
            cmd.Parameters.Add("@DURATION", SqlDbType.Int).Value = _duration;
            if (_fromDate > DateTime.MinValue)
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = _fromDate;
            else
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = DBNull.Value;
            if (_uptoDate > DateTime.MinValue)
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = _uptoDate;
            else
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@PageIndex", SqlDbType.Int).Value = PageIndex;


            SqlParameter Param = new SqlParameter(); Param.ParameterName = "@TotalRecord";
            Param.DbType = DbType.Int64; Param.Direction = ParameterDirection.Output;
            Param.Value = 0; cmd.Parameters.Add(Param);

            Param = new SqlParameter(); Param.ParameterName = "@PagingSize";
            Param.DbType = DbType.Int64; Param.Direction = ParameterDirection.Output;
            Param.Value = 0; cmd.Parameters.Add(Param);


            DataTable dt = db.DbRead_TimeOut(cmd);

            if (cmd.Parameters["@TotalRecord"].Value != null)
            {
                long.TryParse(cmd.Parameters["@TotalRecord"].Value.ToString(), out TotalRecord);
            }
            if (cmd.Parameters["@PagingSize"].Value != null)
            {
                int.TryParse(cmd.Parameters["@PagingSize"].Value.ToString(), out PagingSize);
            }

            return dt;

           // return db.DbRead(cmd);
        }

        public DataTable SearchDur_Export(int _statusID, int _SessionID, int _doctorID, string _searchText, DateTime _fromDate, DateTime _uptoDate, int _duration, int PageIndex, out int PagingSize, out long TotalRecord)
        {
            TotalRecord = 0; PagingSize = 0;
            //SqlCommand cmd = new SqlCommand("Appointments_Search"); cmd.CommandType = CommandType.StoredProcedure;
            SqlCommand cmd = new SqlCommand("Appointments_Search_test_Export"); cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@AppointmentStatus", SqlDbType.Int).Value = _statusID;
            cmd.Parameters.Add("@SessionID", SqlDbType.Int).Value = _SessionID;
            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 50).Value = _searchText;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = _doctorID;
            cmd.Parameters.Add("@DURATION", SqlDbType.Int).Value = _duration;
            if (_fromDate > DateTime.MinValue)
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = _fromDate;
            else
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = DBNull.Value;
            if (_uptoDate > DateTime.MinValue)
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = _uptoDate;
            else
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@PageIndex", SqlDbType.Int).Value = PageIndex;


            SqlParameter Param = new SqlParameter(); Param.ParameterName = "@TotalRecord";
            Param.DbType = DbType.Int64; Param.Direction = ParameterDirection.Output;
            Param.Value = 0; cmd.Parameters.Add(Param);

            Param = new SqlParameter(); Param.ParameterName = "@PagingSize";
            Param.DbType = DbType.Int64; Param.Direction = ParameterDirection.Output;
            Param.Value = 0; cmd.Parameters.Add(Param);


            DataTable dt = db.DbRead_TimeOut(cmd);

            if (cmd.Parameters["@TotalRecord"].Value != null)
            {
                long.TryParse(cmd.Parameters["@TotalRecord"].Value.ToString(), out TotalRecord);
            }
            if (cmd.Parameters["@PagingSize"].Value != null)
            {
                int.TryParse(cmd.Parameters["@PagingSize"].Value.ToString(), out PagingSize);
            }

            return dt;

            //return db.DbRead(cmd);
        }
        public DataTable Search(int _statusID, int _SessionID, int _doctorID, string _searchText, DateTime _fromDate, DateTime _uptoDate)
        {
            SqlCommand cmd = new SqlCommand("Appointments_Search"); cmd.CommandType = CommandType.StoredProcedure;
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

        public DataTable AptChart(int _statusID, int _SessionID, int _doctorID, string _searchText, DateTime _fromDate, DateTime _uptoDate)
        {
            SqlCommand cmd = new SqlCommand("Appointments_Chart"); cmd.CommandType = CommandType.StoredProcedure;
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

        public DataTable MySearch(int _doctorID, string _searchText, DateTime _fromDate, DateTime _uptoDate)
        {
            SqlCommand cmd = new SqlCommand("Appointments_MySearch"); cmd.CommandType = CommandType.StoredProcedure;
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
        public DataTable MySearchNew(int _statusID, int _sessionid, int _doctorID, string _searchText, DateTime _fromDate, DateTime _uptoDate)
        {
            SqlCommand cmd = new SqlCommand("Appointments_MySearchNew"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentStatus", SqlDbType.Int).Value = _statusID;
            cmd.Parameters.Add("@SessionID", SqlDbType.Int).Value = _sessionid;
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
        public SnehDLL.Appointments_Dll Get(int _appointmentID)
        {
            SnehDLL.Appointments_Dll D = null;
            SqlCommand cmd = new SqlCommand("Appointments_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                D = new SnehDLL.Appointments_Dll(); int i = 0;
                D.AppointmentID = int.Parse(dt.Rows[i]["AppointmentID"].ToString());
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                int PatientID = 0; int.TryParse(dt.Rows[i]["PatientID"].ToString(), out PatientID); D.PatientID = PatientID;
                int SessionID = 0; int.TryParse(dt.Rows[i]["SessionID"].ToString(), out SessionID); D.SessionID = SessionID;
                DateTime AppointmentDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AppointmentDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AppointmentDate);
                D.AppointmentDate = AppointmentDate;
                TimeSpan AppointmentTime = new TimeSpan(); DateTime AppointmentTimeD = new DateTime();
                DateTime.TryParseExact(dt.Rows[i]["AppointmentTime"].ToString(), DbHelper.Configuration.timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AppointmentTimeD);
                if (AppointmentTimeD > DateTime.MinValue && AppointmentTimeD < DateTime.MaxValue)
                {
                    AppointmentTime = AppointmentTimeD.TimeOfDay;
                }
                D.AppointmentTime = AppointmentTime;
                DateTime AppointmentFrom = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AppointmentFrom"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AppointmentFrom);
                D.AppointmentFrom = AppointmentFrom;
                DateTime AppointmentUpto = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AppointmentUpto"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AppointmentUpto);
                D.AppointmentUpto = AppointmentUpto;
                int Duration = 0; int.TryParse(dt.Rows[i]["Duration"].ToString(), out Duration); D.Duration = Duration;
                D.Narration = dt.Rows[i]["Narration"].ToString();
                int AppointmentStatus = 0; int.TryParse(dt.Rows[i]["AppointmentStatus"].ToString(), out AppointmentStatus); D.AppointmentStatus = AppointmentStatus;
                DateTime StatusDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["StatusDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out StatusDate);
                D.StatusDate = StatusDate;
                bool IsDeleted = false; bool.TryParse(dt.Rows[i]["IsDeleted"].ToString(), out IsDeleted); D.IsDeleted = IsDeleted;
                DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                D.AddedDate = AddedDate;
                DateTime ModifyDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["ModifyDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ModifyDate);
                D.ModifyDate = ModifyDate;
                int AddedBy = 0; int.TryParse(dt.Rows[i]["AddedBy"].ToString(), out AddedBy); D.AddedBy = AddedBy;
                int ModifyBy = 0; int.TryParse(dt.Rows[i]["ModifyBy"].ToString(), out ModifyBy); D.ModifyBy = ModifyBy;

            }
            return D;
        }

        public int CancelAppointment(int _appointmentID, string Remark)
        {
            SqlCommand cmd = new SqlCommand("Appointments_Cancel"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            cmd.Parameters.Add("@Remark", SqlDbType.NVarChar, 4000).Value = Remark;

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

        public int AbsentAppointment(int _appointmentID)
        {
            SqlCommand cmd = new SqlCommand("Appointments_Absent"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;

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

        public static string SessionViewLink(string _uniqueID, string _isUpdated)
        {
            int isUpdated = 0; int.TryParse(_isUpdated, out isUpdated);
            if (isUpdated > 0)
            {
                //return "<a href=\"javascript:;\" data-toggle=\"modal\" data-target=\"#myModal\" onclick=\"LoadReport('" + _uniqueID + "')\">View</a>&nbsp;";
                return "<a href=\"/SessionRpt/CreateRpt.ashx?record=" + _uniqueID + "\" target=\"_blank\" class=\"btn-print btn-success\">Print</a></br>";
            }
            return "Pending</br>";
        }
        public static string SessionDownloadViewLink(string _uniqueID, string _isUpdated)
        {
            int isUpdated = 0; int.TryParse(_isUpdated, out isUpdated);
            if (isUpdated > 0)
            {
                return "<a href=\"/SessionRpt/DownloadCreateRpt.ashx?record=" + _uniqueID + "\" target=\"_blank\" class=\"btn-print btn-success\">Download</a></br>";
            }
            return "Pending</br>";
        }

        public static string SessionRptLink(string _uniqueID, string _isUpdated)
        {
            int isUpdated = 0; int.TryParse(_isUpdated, out isUpdated);
            if (isUpdated > 0)
            {
                return "<a href=\"/SessionRpt/CreateRpt.ashx?record=" + _uniqueID + "\" target=\"_blank\" class=\"btn-print btn-success\">Print</a></br>";
            }
            return "Pending</br>";
        }

        public static string SessionDownloadRptLink(string _uniqueID, string _isUpdated)
        {
            int isUpdated = 0; int.TryParse(_isUpdated, out isUpdated);
            if (isUpdated > 0)
            {
                return "<a href=\"/SessionRpt/DownloadCreateRpt.ashx?record=" + _uniqueID + "\" target=\"_blank\" class=\"btn-print btn-success\">Download</a></br>";
            }
            return "Pending</br>";
        }

        public static int SessionRptType(int _appointmentID)
        {
            SqlCommand cmd = new SqlCommand("ReportMst_GetType"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;

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
		                                    AM.AppointmentID, AM.ScheduleType,
		                                    AM.UniqueID, 
                                            PM.UniqueID AS PatUniqueID,
		                                    PM.PreFix, 
		                                    PM.FullName, 
		                                    SM.SessionName, AM.Available1FromChar, AM.Available1UptoChar,
		                                    CONVERT(VARCHAR(50), AM.AppointmentDate, 20) AS AppointmentDate, 
		                                    AM.Duration AS Duration, 
		                                    CONVERT(VARCHAR(50), AM.AppointmentTime, 20) AS AppointmentTime, 
		                                    CONVERT(VARCHAR(50), AM.AppointmentFrom, 20) AS AppointmentFrom, 
		                                    CONVERT(VARCHAR(50), AM.AppointmentUpto, 20) AS AppointmentUpto, 
		                                    COALESCE(AM.AppointmentStatus, 0) AS AppointmentStatus, 
                                            PT.PatientTypeID,
		                                    PT.PatientType,
		                                    (SELECT TOP (1) DM.DoctorID FROM AppointmentDoctor AS AD INNER JOIN DoctorMast AS DM ON AD.DoctorID = DM.DoctorID WHERE (COALESCE (AD.IsMain, CAST('False' AS BIT)) = CAST('True' AS BIT)) AND (AD.AppointmentID = AM.AppointmentID)) AS TherapistID,
                                            (SELECT TOP (1) DM.DoctorID FROM AppointDrmeetSchedule AS ADS INNER JOIN DoctorMast AS DM ON ADS.DoctorID = DM.DoctorID WHERE (COALESCE (ADS.IsMain, CAST('False' AS BIT)) = CAST('True' AS BIT)) AND (ADS.AppointmentID = AM.AppointmentID)) AS DoctorID,
		                                    (SELECT TOP 1 DM.PreFix + ' ' + DM.FullName FROM AppointmentDoctor AS AD INNER JOIN DoctorMast AS DM ON AD.DoctorID = DM.DoctorID
		                                    WHERE (COALESCE (AD.IsMain, CAST('False' AS BIT)) = CAST('True' AS BIT)) AND (AD.AppointmentID = AM.AppointmentID)) AS Therapist,
                                            (SELECT TOP 1 DM.PreFix + ' ' + DM.FullName FROM AppointDrmeetSchedule AS ADS INNER JOIN DoctorMast AS DM ON ADS.DoctorID = DM.DoctorID
		                                    WHERE (COALESCE (ADS.IsMain, CAST('False' AS BIT)) = CAST('True' AS BIT)) AND (ADS.AppointmentID = AM.AppointmentID)) AS Doctor,
		                                    CASE WHEN AM.AppointmentStatus = 10 THEN AM.CancelRemark WHEN AM.AppointmentStatus = 1 THEN COALESCE((SELECT TOP 1 Narration FROM AppointmentSession ASS WHERE ASS.AppointmentID = AM.AppointmentID),'') ELSE '' END AS Remark
		                                    FROM PatientTypes AS PT RIGHT OUTER JOIN PatientMast AS PM ON PT.PatientTypeID = PM.PatientTypeID RIGHT OUTER JOIN
		                                    Appointments AS AM ON PM.PatientID = AM.PatientID LEFT OUTER JOIN SessionMast AS SM ON AM.SessionID = SM.SessionID
		                                    WHERE COALESCE(AM.IsDeleted, CAST('False' AS BIT)) <> CAST('True' AS BIT) 
                                            AND AM.AppointmentID = @AppointmentID"); cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;

            return db.DbRead(cmd);
        }

        public DataTable fill_Session()
        {
            SqlCommand cmd = new SqlCommand("FillSession"); cmd.CommandType = CommandType.StoredProcedure;

            return db.DbRead(cmd);
        }
        public DataTable SearchYestAppointRecord(int _status, int _doctorID, DateTime _fromDate, DateTime _uptoDate)
        {
            SqlCommand cmd = new SqlCommand("YesterdayAppointRecord_Search"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Yes_No", SqlDbType.Int).Value = _status;
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
        public int EditAppointment(int _appointmentid, int _sessionid, int duration, DateTime _appointmentdate, TimeSpan _appointmenttime)
        {
            SqlCommand cmd = new SqlCommand("EditAppointment"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentid;
            cmd.Parameters.Add("@SessionID", SqlDbType.Int).Value = _sessionid;
            cmd.Parameters.Add("@Duration", SqlDbType.Int).Value = duration;
            cmd.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = _appointmentdate;
            cmd.Parameters.Add("@AppointmentTime", SqlDbType.Time).Value = _appointmenttime;

            SqlParameter Param = new SqlParameter(); Param.ParameterName = "@Retval";
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
        public DataTable YesterDayAppointments(int _doctorID)
        {
            SqlCommand cmd = new SqlCommand("Appointments_Alert"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = _doctorID;
            return db.DbRead(cmd);
        }
        public DataTable SaveNarrat(int _doctorid)
        {
            SqlCommand cmd = new SqlCommand(@"SELECT * FROM YesterdayAppoint WHERE UserID=@DoctorID"); cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int, 250).Value = _doctorid;
            db.DbUpdate(cmd);
            return db.DbRead(cmd);
        }
        public int SaveNarrat_New(SnehDLL.SupportTicket_Dll D)
        {
            SqlCommand cmd = new SqlCommand("YesterdayAppointNarration_New"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@TicketID", SqlDbType.Int).Value = D.TicketID;
            cmd.Parameters.Add("@Yes_No", SqlDbType.VarChar).Value = D.yes_no;
            cmd.Parameters.Add("@Yes_No_Value", SqlDbType.VarChar).Value = D.yes_no_value;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int, 250).Value = D.UserID;
            cmd.Parameters.Add("@yNarration", SqlDbType.VarChar, 250).Value = D.yNarration;
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
        public int SaveWithoutNarrat_New(SnehDLL.SupportTicket_Dll D)
        {
            SqlCommand cmd = new SqlCommand("YesterdayAppoint_New"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@TicketID", SqlDbType.Int).Value = D.TicketID;
            cmd.Parameters.Add("@Yes_No", SqlDbType.VarChar).Value = D.yes_no;
            cmd.Parameters.Add("@Yes_No_Value", SqlDbType.VarChar).Value = D.yes_no_value;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int, 250).Value = D.UserID;
            // cmd.Parameters.Add("@yNarration", SqlDbType.VarChar, 250).Value = D.yNarration;
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
        public int CancelDrMeetSched(int _appointmentID)
        {
            SqlCommand cmd = new SqlCommand("AppointmentsDrmeetSche_Cancel"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
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
        public DataTable DrYesterdayAppoint(string _fullName, DateTime _fromDate, DateTime _uptoDate)
        {
            SqlCommand cmd = new SqlCommand("YesterdayAppoint_Search"); cmd.CommandType = CommandType.StoredProcedure;
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
        public int setDrMeetSche(SnehDLL.Appointments_Dll D)
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
            if (D.Available1From > DateTime.MinValue && D.Available1From < DateTime.MaxValue && D.Available1FromChar.Length > 0)
                cmd.Parameters.Add("@Available1From", SqlDbType.DateTime).Value = D.Available1From;
            else
                cmd.Parameters.Add("@Available1From", SqlDbType.DateTime).Value = DBNull.Value;

            if (D.Available1Upto > DateTime.MinValue && D.Available1Upto < DateTime.MaxValue && D.Available1UptoChar.Length > 0)
                cmd.Parameters.Add("@Available1Upto", SqlDbType.DateTime).Value = D.Available1Upto;
            else
                cmd.Parameters.Add("@Available1Upto", SqlDbType.DateTime).Value = DBNull.Value;
            if (D.ModifyDate > DateTime.MinValue)
                cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = D.ModifyDate;
            else
                cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = DBNull.Value;
            if (D.Available1FromTime > TimeSpan.MinValue && D.Available1FromTime < TimeSpan.MaxValue)
                cmd.Parameters.Add("@Available1FromTime", SqlDbType.Time).Value = D.Available1FromTime;
            else
                cmd.Parameters.Add("@Available1FromTime", SqlDbType.Time).Value = DBNull.Value;
            if (D.Available1UptoTime > TimeSpan.MinValue && D.Available1UptoTime < TimeSpan.MaxValue)
                cmd.Parameters.Add("@Available1UptoTime", SqlDbType.Time).Value = D.Available1UptoTime;
            else
                cmd.Parameters.Add("@Available1UptoTime", SqlDbType.Time).Value = DBNull.Value;
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
        public DataTable DrYesterdayAppointMy(int _userID, DateTime _fromDate, DateTime _uptoDate)
        {
            SqlCommand cmd = new SqlCommand("YesterdayAppoint_MYSearch"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
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
        public DataTable ScheDetail(int _appointmentID, int _resourceID)
        {
            SqlCommand cmd = new SqlCommand(@"select AM.AppointmentID, AM.DoctorID,
            (DM.PreFix + ' ' + DM.FullName) as Doctor from DoctorMast as DM inner join 
            AppointDrmeetSchedule as AM on DM.DoctorID=AM.DoctorID 
            where AM.AppointmentID = @AppointmentID and AM.DoctorID=@ResourceID");
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            cmd.Parameters.Add("@ResourceID", SqlDbType.Int).Value = _resourceID;

            return db.DbRead(cmd);
        }
    }
}
