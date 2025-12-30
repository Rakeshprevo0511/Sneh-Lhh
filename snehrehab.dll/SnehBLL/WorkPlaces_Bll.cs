using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace SnehBLL
{
    public class WorkPlaces_Bll
    {
        DbHelper.SqlDb db;

        public WorkPlaces_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public static int Check(string _uniqueID)
        {
            int _workplaceID = 0;
            SqlCommand cmd = new SqlCommand("WorkPlaces_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = _uniqueID;
            cmd.Parameters.Add("@WorkplaceID", SqlDbType.Int).Value = 0;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["WorkplaceID"].ToString(), out _workplaceID);
            }
            return _workplaceID;
        }

        public static string Check(int _workplaceID)
        {
            string _uniqueID = "";
            SqlCommand cmd = new SqlCommand("WorkPlaces_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = "";
            cmd.Parameters.Add("@WorkplaceID", SqlDbType.Int).Value = _workplaceID;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                _uniqueID = dt.Rows[0]["UniqueID"].ToString();
            }
            return _uniqueID;
        }

        public int Delete(int _workplaceID)
        {
            SqlCommand cmd = new SqlCommand("WorkPlaces_Delete"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@WorkplaceID", SqlDbType.Int).Value = _workplaceID;

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

        public SnehDLL.WorkPlaces_Dll Get(int _workplaceID)
        {
            SnehDLL.WorkPlaces_Dll D = null;
            SqlCommand cmd = new SqlCommand("WorkPlaces_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@WorkplaceID", SqlDbType.Int).Value = _workplaceID;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                D = new SnehDLL.WorkPlaces_Dll(); int i = 0;
                D.WorkplaceID = int.Parse(dt.Rows[i]["WorkplaceID"].ToString());
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                D.Workplace = dt.Rows[i]["Workplace"].ToString();
                DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                D.AddedDate = AddedDate;
                DateTime ModifyDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["ModifyDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ModifyDate);
                D.ModifyDate = ModifyDate;
                int AddedBy = 0; int.TryParse(dt.Rows[i]["AddedBy"].ToString(), out AddedBy); D.AddedBy = AddedBy;
                int ModifyBy = 0; int.TryParse(dt.Rows[i]["ModifyBy"].ToString(), out ModifyBy); D.ModifyBy = ModifyBy;
                bool IsEnabled = true; bool.TryParse(dt.Rows[i]["IsEnabled"].ToString(), out IsEnabled); D.IsEnabled = IsEnabled;
                D.Address = dt.Rows[i]["Address"].ToString();
            }
            return D;
        }

        public List<SnehDLL.WorkPlaces_Dll> GetList()
        {
            List<SnehDLL.WorkPlaces_Dll> DL = new List<SnehDLL.WorkPlaces_Dll>();
            SqlCommand cmd = new SqlCommand("WorkPlaces_GetList"); cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.WorkPlaces_Dll D = new SnehDLL.WorkPlaces_Dll();
                D.WorkplaceID = int.Parse(dt.Rows[i]["WorkplaceID"].ToString());
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                D.Workplace = dt.Rows[i]["Workplace"].ToString();
                DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                D.AddedDate = AddedDate;
                DateTime ModifyDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["ModifyDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ModifyDate);
                D.ModifyDate = ModifyDate;
                int AddedBy = 0; int.TryParse(dt.Rows[i]["AddedBy"].ToString(), out AddedBy); D.AddedBy = AddedBy;
                int ModifyBy = 0; int.TryParse(dt.Rows[i]["ModifyBy"].ToString(), out ModifyBy); D.ModifyBy = ModifyBy;
                bool IsEnabled = true; bool.TryParse(dt.Rows[i]["IsEnabled"].ToString(), out IsEnabled); D.IsEnabled = IsEnabled;

                DL.Add(D);
            }
            return DL;
        }

        public List<SnehDLL.WorkPlaces_Dll> Search(string _workPlace)
        {
            List<SnehDLL.WorkPlaces_Dll> DL = new List<SnehDLL.WorkPlaces_Dll>();
            SqlCommand cmd = new SqlCommand("WorkPlaces_GetSearch"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Workplace", SqlDbType.VarChar, 50).Value = _workPlace;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.WorkPlaces_Dll D = new SnehDLL.WorkPlaces_Dll();
                D.WorkplaceID = int.Parse(dt.Rows[i]["WorkplaceID"].ToString());
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                D.Workplace = dt.Rows[i]["Workplace"].ToString();
                DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                D.AddedDate = AddedDate;
                DateTime ModifyDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["ModifyDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ModifyDate);
                D.ModifyDate = ModifyDate;
                int AddedBy = 0; int.TryParse(dt.Rows[i]["AddedBy"].ToString(), out AddedBy); D.AddedBy = AddedBy;
                int ModifyBy = 0; int.TryParse(dt.Rows[i]["ModifyBy"].ToString(), out ModifyBy); D.ModifyBy = ModifyBy;
                bool IsEnabled = true; bool.TryParse(dt.Rows[i]["IsEnabled"].ToString(), out IsEnabled); D.IsEnabled = IsEnabled;

                DL.Add(D);
            }
            return DL;
        }

        public int Set(SnehDLL.WorkPlaces_Dll D)
        {
            SqlCommand cmd = new SqlCommand("WorkPlaces_Set"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@WorkplaceID", SqlDbType.Int).Value = D.WorkplaceID;
            cmd.Parameters.Add("@Workplace", SqlDbType.VarChar, 250).Value = D.Workplace;
            cmd.Parameters.Add("@IsEnabled", SqlDbType.Bit).Value = D.IsEnabled;
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
    }
}
