using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace SnehBLL
{
    public class SessionMast_Bll
    {
        DbHelper.SqlDb db;

        public SessionMast_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public static int Check(string _uniqueID)
        {
            int _sessionID = 0;
            SqlCommand cmd = new SqlCommand("SessionMast_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = _uniqueID;
            cmd.Parameters.Add("@SessionID", SqlDbType.Int).Value = 0;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["SessionID"].ToString(), out _sessionID);
            }
            return _sessionID;
        }

        public static string Check(int _sessionID)
        {
            string _uniqueID = "";
            SqlCommand cmd = new SqlCommand("SessionMast_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = "";
            cmd.Parameters.Add("@SessionID", SqlDbType.Int).Value = _sessionID;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                _uniqueID = dt.Rows[0]["UniqueID"].ToString();
            }
            return _uniqueID;
        }

        public SnehDLL.SessionMast_Dll Get(int _sessionID)
        {
            SnehDLL.SessionMast_Dll D = null;
            SqlCommand cmd = new SqlCommand("SessionMast_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@SessionID", SqlDbType.Int).Value = _sessionID;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                D = new SnehDLL.SessionMast_Dll(); int i = 0;
                D.SessionID = int.Parse(dt.Rows[i]["SessionID"].ToString());
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                D.SessionCode = dt.Rows[i]["SessionCode"].ToString();
                D.SessionName = dt.Rows[i]["SessionName"].ToString();
                bool IsPackage = false; bool.TryParse(dt.Rows[i]["IsPackage"].ToString(), out IsPackage); D.IsPackage = IsPackage;
                bool IsEvaluation = false; bool.TryParse(dt.Rows[i]["IsEvaluation"].ToString(), out IsEvaluation); D.IsEvaluation = IsEvaluation;
                bool MultipleDoctor = true; bool.TryParse(dt.Rows[i]["MultipleDoctor"].ToString(), out MultipleDoctor); D.MultipleDoctor = MultipleDoctor;
                int SessionGroupID = 0; int.TryParse(dt.Rows[i]["SessionGroupID"].ToString(), out SessionGroupID); D.SessionGroupID = SessionGroupID;
                int ChargeType = 0; int.TryParse(dt.Rows[i]["ChargeType"].ToString(), out ChargeType); D.ChargeType = ChargeType;
                DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                D.AddedDate = AddedDate;
                DateTime ModifyDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["ModifyDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ModifyDate);
                D.ModifyDate = ModifyDate;
                int AddedBy = 0; int.TryParse(dt.Rows[i]["AddedBy"].ToString(), out AddedBy); D.AddedBy = AddedBy;
                int ModifyBy = 0; int.TryParse(dt.Rows[i]["ModifyBy"].ToString(), out ModifyBy); D.ModifyBy = ModifyBy;
                bool TimeWise = false; bool.TryParse(dt.Rows[i]["TimeWise"].ToString(), out TimeWise); D.TimeWise = TimeWise;
            }
            return D;
        }

        public List<SnehDLL.SessionMast_Dll> GetList()
        {
            List<SnehDLL.SessionMast_Dll> DL = new List<SnehDLL.SessionMast_Dll>();
            SqlCommand cmd = new SqlCommand("SessionMast_GetList"); cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.SessionMast_Dll D = new SnehDLL.SessionMast_Dll();
                D.SessionID = int.Parse(dt.Rows[i]["SessionID"].ToString());
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                D.SessionCode = dt.Rows[i]["SessionCode"].ToString();
                D.SessionName = dt.Rows[i]["SessionName"].ToString();
                bool IsPackage = false; bool.TryParse(dt.Rows[i]["IsPackage"].ToString(), out IsPackage); D.IsPackage = IsPackage;
                bool IsEvaluation = false; bool.TryParse(dt.Rows[i]["IsEvaluation"].ToString(), out IsEvaluation); D.IsEvaluation = IsEvaluation;
                bool MultipleDoctor = true; bool.TryParse(dt.Rows[i]["MultipleDoctor"].ToString(), out MultipleDoctor); D.MultipleDoctor = MultipleDoctor;
                int SessionGroupID = 0; int.TryParse(dt.Rows[i]["SessionGroupID"].ToString(), out SessionGroupID); D.SessionGroupID = SessionGroupID;
                DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                D.AddedDate = AddedDate;
                DateTime ModifyDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["ModifyDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ModifyDate);
                D.ModifyDate = ModifyDate;
                int AddedBy = 0; int.TryParse(dt.Rows[i]["AddedBy"].ToString(), out AddedBy); D.AddedBy = AddedBy;
                int ModifyBy = 0; int.TryParse(dt.Rows[i]["ModifyBy"].ToString(), out ModifyBy); D.ModifyBy = ModifyBy;

                DL.Add(D);
            }
            return DL;
        }

        public List<SnehDLL.SessionMast_Dll> GetPackageList()
        {
            List<SnehDLL.SessionMast_Dll> DL = new List<SnehDLL.SessionMast_Dll>();
            SqlCommand cmd = new SqlCommand("SessionMast_GetPackageList"); cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.SessionMast_Dll D = new SnehDLL.SessionMast_Dll();
                D.SessionID = int.Parse(dt.Rows[i]["SessionID"].ToString());
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                D.SessionCode = dt.Rows[i]["SessionCode"].ToString();
                D.SessionName = dt.Rows[i]["SessionName"].ToString();
                bool IsPackage = false; bool.TryParse(dt.Rows[i]["IsPackage"].ToString(), out IsPackage); D.IsPackage = IsPackage;
                bool IsEvaluation = false; bool.TryParse(dt.Rows[i]["IsEvaluation"].ToString(), out IsEvaluation); D.IsEvaluation = IsEvaluation;
                bool MultipleDoctor = true; bool.TryParse(dt.Rows[i]["MultipleDoctor"].ToString(), out MultipleDoctor); D.MultipleDoctor = MultipleDoctor;
                int SessionGroupID = 0; int.TryParse(dt.Rows[i]["SessionGroupID"].ToString(), out SessionGroupID); D.SessionGroupID = SessionGroupID;
                DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                D.AddedDate = AddedDate;
                DateTime ModifyDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["ModifyDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ModifyDate);
                D.ModifyDate = ModifyDate;
                int AddedBy = 0; int.TryParse(dt.Rows[i]["AddedBy"].ToString(), out AddedBy); D.AddedBy = AddedBy;
                int ModifyBy = 0; int.TryParse(dt.Rows[i]["ModifyBy"].ToString(), out ModifyBy); D.ModifyBy = ModifyBy;

                DL.Add(D);
            }
            return DL;
        }

        public DataTable getCharges()
        {
            SqlCommand cmd = new SqlCommand("SessionMast_ChargesGetList"); cmd.CommandType = CommandType.StoredProcedure;
            return db.DbRead(cmd);
        }

        public int setCharges(int _sessionID, int _type, float _amount1, float _amount2)
        {
            SqlCommand cmd = new SqlCommand("SessionMast_ChargesSet"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@SessionID", SqlDbType.Int).Value = _sessionID;
            cmd.Parameters.Add("@ChargeType", SqlDbType.Int).Value = _type;
            cmd.Parameters.Add("@Doctor1", SqlDbType.Decimal).Value = _amount1;
            cmd.Parameters.Add("@Doctor2", SqlDbType.Decimal).Value = _amount2;

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

        public bool chargeIsUpdated(int _sessionID)
        {
            SqlCommand cmd = new SqlCommand("SessionMast_ChargeIsUpdated"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@SessionID", SqlDbType.Int).Value = _sessionID;

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
    }
}
