using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace SnehBLL
{
    public class SettingsMst_Bll
    {
        DbHelper.SqlDb db;

        public SettingsMst_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public SnehDLL.SettingsMst_Dll Get()
        {
            SnehDLL.SettingsMst_Dll D = null;
            SqlCommand cmd = new SqlCommand("SettingsMst_Get"); cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                D = new SnehDLL.SettingsMst_Dll(); int i = 0;
                DateTime RptSentDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["RptSentDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out RptSentDate);
                D.RptSentDate = RptSentDate;
                D.RptMobileNo = dt.Rows[i]["RptMobileNo"].ToString();
                D.RptMailID = dt.Rows[i]["RptMailID"].ToString();
                bool IsMainServer = false; bool.TryParse(dt.Rows[i]["IsMainServer"].ToString(), out IsMainServer);
                D.IsMainServer = IsMainServer;
            }
            return D;
        }

        public static bool GetIsMainServer()
        {
            bool IsMainServer = false;
            SqlCommand cmd = new SqlCommand("SettingsMst_GetIsMainServer"); cmd.CommandType = CommandType.StoredProcedure;
            DbHelper.SqlDb db = new DbHelper.SqlDb(); DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                int i = 0; bool.TryParse(dt.Rows[i]["IsMainServer"].ToString(), out IsMainServer);
            }
            return IsMainServer;
        }

        public static DateTime GetRptSentDate()
        {
            DateTime RptSentDate = new DateTime();
            SqlCommand cmd = new SqlCommand("SettingsMst_GetRptSentDate"); cmd.CommandType = CommandType.StoredProcedure;
            DbHelper.SqlDb db = new DbHelper.SqlDb(); DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                int i = 0;
                DateTime.TryParseExact(dt.Rows[i]["RptSentDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out RptSentDate);
            }
            if (RptSentDate > DateTime.MinValue)
                return RptSentDate.Date;
            return DateTime.UtcNow.AddMinutes(330).Date;
        }

        public int SetRptMobileNo(string _mobileNo)
        {
            SqlCommand cmd = new SqlCommand("SettingsMst_SetRptMobileNo"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@RptMobileNo", SqlDbType.VarChar, 4000).Value = _mobileNo;

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

        public int SetRptMailID(string _mailID)
        {
            SqlCommand cmd = new SqlCommand("SettingsMst_SetRptMailID"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@RptMailID", SqlDbType.VarChar, 4000).Value = _mailID;

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

        public static bool SendReport(DateTime _reportDate)
        {
            SqlCommand cmd = new SqlCommand("SettingsMst_SetRptSentDate"); cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter Param = new SqlParameter(); Param.ParameterName = "@RetVal";
            Param.DbType = DbType.Int64; Param.Direction = ParameterDirection.Output;
            Param.Value = 0; cmd.Parameters.Add(Param);

            DbHelper.SqlDb db = new DbHelper.SqlDb(); db.DbUpdate(cmd);

            int i = 0;
            if (cmd.Parameters["@RetVal"].Value != null)
            {
                int.TryParse(cmd.Parameters["@RetVal"].Value.ToString(), out i);
            }

            return false;
        }
    }
}