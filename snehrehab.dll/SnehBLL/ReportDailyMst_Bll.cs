using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace SnehBLL
{
    public class ReportDailyMst_Bll
    {
        DbHelper.SqlDb db;

        public ReportDailyMst_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public bool IsValid(int _appointmentID)
        {
            SqlCommand cmd = new SqlCommand("ReportMst_IsValid"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            cmd.Parameters.Add("@ReportID", SqlDbType.Int).Value = 1;


            SqlParameter Param = new SqlParameter(); Param.ParameterName = "@RetVal";
            Param.DbType = DbType.Int64; Param.Direction = ParameterDirection.Output;
            Param.Value = 0; cmd.Parameters.Add(Param);

            db.DbUpdate(cmd);

            int i = 0;
            if (cmd.Parameters["@RetVal"].Value != null)
            {
                int.TryParse(cmd.Parameters["@RetVal"].Value.ToString(), out i);
            }
            return (i > 0);
        }

        public int _impairementTypeID = 1;

        public DataTable impairement_GetList()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ImpairementID", typeof(int));
            dt.Columns.Add("Impairements", typeof(string));

            dt.Rows.Add(1, "Neuromotor System");
            dt.Rows.Add(2, "Musculoskeletal System");
            dt.Rows.Add(3, "Somatosensory System");
            dt.Rows.Add(4, "Visual System");
            dt.Rows.Add(5, "Proprioceptive System");
            dt.Rows.Add(6, "Tactile System");
            dt.Rows.Add(7, "Auditory System");
            dt.Rows.Add(8, "Vestibular System");
            dt.Rows.Add(9, "Orosensory System");
            dt.Rows.Add(10, "Oromotor System");

            return dt;
        }

        public DataTable SearchByStatus(int _doctorID, string _fullName, DateTime _fromDate, DateTime _uptoDate, bool _isDoctor, int statusid)
        {
            SqlCommand cmd = new SqlCommand("ReportDailyMst_Search_Status"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = _doctorID;
            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 50).Value = _fullName;
            if (_fromDate > DateTime.MinValue)
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = _fromDate;
            else
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = DBNull.Value;
            if (_uptoDate > DateTime.MinValue)
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = _uptoDate;
            else
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@IsDoctor", SqlDbType.Bit).Value = _isDoctor;
            cmd.Parameters.Add("@StatusID", SqlDbType.Int).Value = statusid;
            return db.DbRead(cmd);
        }

        public string impairement_Get(int _impairementID)
        {
            string _impairements = "";
            foreach (DataRow dr in impairement_GetList().Rows)
            {
                int _tmp = 0; int.TryParse(dr["ImpairementID"].ToString(), out _tmp);
                if (_tmp == _impairementID)
                {
                    _impairements = dr["Impairements"].ToString();
                    break;
                }
            }
            return _impairements;
        }

        public DataTable Performance_GetList()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PerformanceID", typeof(int));
            dt.Columns.Add("Performance", typeof(string));

            dt.Rows.Add(1, "Fair");
            dt.Rows.Add(2, "Good");
            dt.Rows.Add(3, "Poor");


            return dt;
        }

        public string Performance_Get(int _performanceID)
        {
            string _performance = "";
            foreach (DataRow dr in Performance_GetList().Rows)
            {
                int _tmp = 0; int.TryParse(dr["PerformanceID"].ToString(), out _tmp);
                if (_tmp == _performanceID)
                {
                    _performance = dr["Performance"].ToString();
                    break;
                }
            }
            return _performance;
        }

        public DataTable GoalScale_GetList()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ScaleID", typeof(int));
            dt.Columns.Add("Scale", typeof(string));

            dt.Rows.Add(1, "GAS: -2");
            dt.Rows.Add(2, "GAS: -1");
            dt.Rows.Add(3, "GAS: 0");
            dt.Rows.Add(3, "GAS: +1");
            dt.Rows.Add(3, "GAS: +2");

            return dt;
        }

        public string GoalScale_Get(int _scaleID)
        {
            string _scale = "";
            foreach (DataRow dr in GoalScale_GetList().Rows)
            {
                int _tmp = 0; int.TryParse(dr["ScaleID"].ToString(), out _tmp);
                if (_tmp == _scaleID)
                {
                    _scale = dr["Scale"].ToString();
                    break;
                }
            }
            return _scale;
        }
        public int Set(int _appointmentID, string Observation, string Suggestion, DateTime ModifyDate, int _loginID, string DiagnosisIDs, string DiagnosisOther)
        {
            SqlCommand cmd = new SqlCommand("ReportDailyMst_Set1"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            cmd.Parameters.Add("@Observation", SqlDbType.VarChar, -1).Value = Observation;
            cmd.Parameters.Add("@Suggestion", SqlDbType.VarChar, -1).Value = Suggestion;
            if (ModifyDate > DateTime.MinValue)
                cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = ModifyDate;
            else
                cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@ModifyBy", SqlDbType.Int).Value = _loginID;

            cmd.Parameters.Add("@DiagnosisID", SqlDbType.VarChar, -1).Value = DiagnosisIDs;
            cmd.Parameters.Add("@DiagnosisOther", SqlDbType.VarChar, -1).Value = DiagnosisOther;

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
        public DataTable Patient_SearchByStatus(string _fullName, DateTime _fromDate, DateTime _uptoDate, bool _isDoctor, int statusid)
        {
            SqlCommand cmd = new SqlCommand("ReportDailyMst_Search_Status_ByPatient"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 50).Value = _fullName;
            if (_fromDate > DateTime.MinValue)
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = _fromDate;
            else
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = DBNull.Value;
            if (_uptoDate > DateTime.MinValue)
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = _uptoDate;
            else
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@IsDoctor", SqlDbType.Bit).Value = _isDoctor;
            cmd.Parameters.Add("@StatusID", SqlDbType.Int).Value = statusid;
            return db.DbRead(cmd);
        }
        public DataTable PatientSearch(string _fullName, DateTime _fromDate, DateTime _uptoDate, bool _isDoctor)
        {
            SqlCommand cmd = new SqlCommand("Demo_ReportDailyMst_Search"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 50).Value = _fullName;
            if (_fromDate > DateTime.MinValue)
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = _fromDate;
            else
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = DBNull.Value;
            if (_uptoDate > DateTime.MinValue)
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = _uptoDate;
            else
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@IsDoctor", SqlDbType.Bit).Value = _isDoctor;

            return db.DbRead(cmd);
        }
        public int SET1(int _appointmentID, string _sessionGoal, string _activity, /*string _equipments,*/ /*int _performanceID,*/
            int _goalAssScaleID, /*string _longTermGoals, string _shortTermGoals,*/ string _suggestions, DateTime ModifyDate, int _loginID,
           string DiagnosisIDs, string DiagnosisOther)
        {
            SqlCommand cmd = new SqlCommand("ReportDailyMst_Set"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            cmd.Parameters.Add("@SessionGoal", SqlDbType.VarChar, -1).Value = _sessionGoal;
            cmd.Parameters.Add("@Activity", SqlDbType.VarChar, -1).Value = _activity;
            // cmd.Parameters.Add("@Equipments", SqlDbType.VarChar, -1).Value = _equipments;
            //cmd.Parameters.Add("@PerformanceID", SqlDbType.Int).Value = _performanceID;
            cmd.Parameters.Add("@GoalAssScaleID", SqlDbType.Int).Value = _goalAssScaleID;
            //cmd.Parameters.Add("@LongTermGoals", SqlDbType.VarChar, -1).Value = _longTermGoals;
            //cmd.Parameters.Add("@ShortTermGoals", SqlDbType.VarChar, -1).Value = _shortTermGoals;
            cmd.Parameters.Add("@Suggestions", SqlDbType.VarChar, -1).Value = _suggestions;
            if (ModifyDate > DateTime.MinValue)
                cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = ModifyDate;
            else
                cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@ModifyBy", SqlDbType.Int).Value = _loginID;
            cmd.Parameters.Add("@DiagnosisID", SqlDbType.VarChar, -1).Value = DiagnosisIDs;
            cmd.Parameters.Add("@DiagnosisOther", SqlDbType.VarChar, -1).Value = DiagnosisOther;

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

        public int DeleteAttr(int _appointmentID, int _attributeTypeID)
        {
            SqlCommand cmd = new SqlCommand("ReportDailyChld_Delete"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            cmd.Parameters.Add("@AttributeTypeID", SqlDbType.Int).Value = _attributeTypeID;

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

        public int SetAttr(int _appointmentID, int _attributeTypeID, int _attributeID)
        {
            SqlCommand cmd = new SqlCommand("ReportDailyChld_Set"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            cmd.Parameters.Add("@AttributeTypeID", SqlDbType.Int).Value = _attributeTypeID;
            cmd.Parameters.Add("@AttributeID", SqlDbType.Int).Value = _attributeID;

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

        public DataSet Get(int _appointmentID)
        {
            SqlCommand cmd = new SqlCommand("ReportDailyMst_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;

            return db.DbFetch(cmd);
        }

        public DataTable GetAttr(int _appointmentID, int _attributeTypeID)
        {
            SqlCommand cmd = new SqlCommand("ReportDailyChld_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            cmd.Parameters.Add("@AttributeTypeID", SqlDbType.Int).Value = _attributeTypeID;

            return db.DbRead(cmd);
        }

        public DataTable Search(int _doctorID, string _fullName, DateTime _fromDate, DateTime _uptoDate, bool _isDoctor)
        {
            SqlCommand cmd = new SqlCommand("ReportDailyMst_Search"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = _doctorID;
            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 50).Value = _fullName;
            if (_fromDate > DateTime.MinValue)
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = _fromDate;
            else
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = DBNull.Value;
            if (_uptoDate > DateTime.MinValue)
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = _uptoDate;
            else
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@IsDoctor", SqlDbType.Bit).Value = _isDoctor;

            return db.DbRead(cmd);
        }
        public bool IsDailyRpt_Type1(int AppointmentID)
        {
            SqlCommand cmd = new SqlCommand("SELECT 1 FROM AppointmentDoctor AD INNER JOIN DoctorMast DM ON AD.DoctorID = DM.DoctorID WHERE AD.AppointmentID = @AppointmentID AND COALESCE(DM.DailyRptTypeID, 0) = 1 ");
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = AppointmentID;

            return db.DbRead(cmd).Rows.Count > 0;
        }
        public bool IsDailyRpt_Type0(int AppointmentID)
        {
            SqlCommand cmd = new SqlCommand("SELECT 1 FROM AppointmentDoctor AD INNER JOIN DoctorMast DM ON AD.DoctorID = DM.DoctorID WHERE AD.AppointmentID = @AppointmentID AND COALESCE(DM.DailyRptTypeID, 0) = 0 ");
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = AppointmentID;

            return db.DbRead(cmd).Rows.Count > 0;
        }
        public DataTable GetTherapist(int _appointmentID)
        {
            SqlCommand cmd = new SqlCommand(" SELECT DM.Prefix +' '+ DM.FullName AS FullName FROM AppointmentDoctor AD INNER JOIN DoctorMast DM ON AD.DoctorID = DM.DoctorID WHERE AD.AppointmentID = @AppointmentID ");
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;

            return db.DbRead(cmd);
        }
    }
}
