using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace SnehBLL
{
    public class Specialities_Bll
    {
        DbHelper.SqlDb db;

        public Specialities_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public static int Check(string _uniqueID)
        {
            int _specialityID = 0;
            SqlCommand cmd = new SqlCommand("Specialities_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = _uniqueID;
            cmd.Parameters.Add("@SpecialityID", SqlDbType.Int).Value = 0;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["SpecialityID"].ToString(), out _specialityID);
            }
            return _specialityID;
        }

        public static string Check(int _specialityID)
        {
            string _uniqueID = "";
            SqlCommand cmd = new SqlCommand("Specialities_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = "";
            cmd.Parameters.Add("@SpecialityID", SqlDbType.Int).Value = _specialityID;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                _uniqueID = dt.Rows[0]["UniqueID"].ToString();
            }
            return _uniqueID;
        }

        public int Delete(int _specialityID)
        {
            SqlCommand cmd = new SqlCommand("Specialities_Delete"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@SpecialityID", SqlDbType.Int).Value = _specialityID;

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

        public SnehDLL.Specialities_Dll Get(int _specialityID)
        {
            SnehDLL.Specialities_Dll D = null;
            SqlCommand cmd = new SqlCommand("Specialities_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@SpecialityID", SqlDbType.Int).Value = _specialityID;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                D = new SnehDLL.Specialities_Dll(); int i = 0;
                D.SpecialityID = int.Parse(dt.Rows[i]["SpecialityID"].ToString());
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                D.Speciality = dt.Rows[i]["Speciality"].ToString();
                bool IsEnabled = true; bool.TryParse(dt.Rows[i]["IsEnabled"].ToString(), out IsEnabled); D.IsEnabled = IsEnabled;
                DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                D.AddedDate = AddedDate;
                DateTime ModifyDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["ModifyDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ModifyDate);
                D.ModifyDate = ModifyDate;
                int AddedBy = 0; int.TryParse(dt.Rows[i]["AddedBy"].ToString(), out AddedBy); D.AddedBy = AddedBy;
                int ModifyBy = 0; int.TryParse(dt.Rows[i]["ModifyBy"].ToString(), out ModifyBy); D.ModifyBy = ModifyBy;
            }
            return D;
        }

        public List<SnehDLL.Specialities_Dll> GetList()
        {
            List<SnehDLL.Specialities_Dll> DL = new List<SnehDLL.Specialities_Dll>();
            SqlCommand cmd = new SqlCommand("Specialities_GetList"); cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.Specialities_Dll D = new SnehDLL.Specialities_Dll();
                D.SpecialityID = int.Parse(dt.Rows[i]["SpecialityID"].ToString());
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                D.Speciality = dt.Rows[i]["Speciality"].ToString();
                bool IsEnabled = true; bool.TryParse(dt.Rows[i]["IsEnabled"].ToString(), out IsEnabled); D.IsEnabled = IsEnabled;
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

        public List<SnehDLL.Specialities_Dll> Search(string _speciality)
        {
            List<SnehDLL.Specialities_Dll> DL = new List<SnehDLL.Specialities_Dll>();
            SqlCommand cmd = new SqlCommand("Specialities_GetSearch"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Speciality", SqlDbType.VarChar, 50).Value = _speciality;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.Specialities_Dll D = new SnehDLL.Specialities_Dll();
                D.SpecialityID = int.Parse(dt.Rows[i]["SpecialityID"].ToString());
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                D.Speciality = dt.Rows[i]["Speciality"].ToString();
                bool IsEnabled = true; bool.TryParse(dt.Rows[i]["IsEnabled"].ToString(), out IsEnabled); D.IsEnabled = IsEnabled;
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

        public int Set(SnehDLL.Specialities_Dll D)
        {
            SqlCommand cmd = new SqlCommand("Specialities_Set"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@SpecialityID", SqlDbType.Int).Value = D.SpecialityID;
            cmd.Parameters.Add("@Speciality", SqlDbType.VarChar, 250).Value = D.Speciality;
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
