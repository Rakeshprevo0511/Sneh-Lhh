using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace SnehBLL
{
    public class PatientTypes_Bll
    {
        DbHelper.SqlDb db;

        public PatientTypes_Bll()
        {
            db = new DbHelper.SqlDb(); 
        }

        public static int Check(string _uniqueID)
        {
            int _patientTypeID = 0;
            SqlCommand cmd = new SqlCommand("PatientTypes_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = _uniqueID;
            cmd.Parameters.Add("@PatientTypeID", SqlDbType.Int).Value = 0;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["PatientTypeID"].ToString(), out _patientTypeID);
            }
            return _patientTypeID;
        }

        public static string Check(int _patientTypeID)
        {
            string _uniqueID = "";
            SqlCommand cmd = new SqlCommand("PatientTypes_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = "";
            cmd.Parameters.Add("@PatientTypeID", SqlDbType.Int).Value = _patientTypeID;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                _uniqueID = dt.Rows[0]["UniqueID"].ToString();
            }
            return _uniqueID;
        }

        public SnehDLL.PatientTypes_Dll Get(int _patientTypeID)
        {
            SnehDLL.PatientTypes_Dll D = null;
            SqlCommand cmd = new SqlCommand("PatientTypes_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PatientTypeID", SqlDbType.Int).Value = _patientTypeID;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                D = new SnehDLL.PatientTypes_Dll(); int i = 0;
                D.PatientTypeID = int.Parse(dt.Rows[i]["PatientTypeID"].ToString());
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                D.PatientType = dt.Rows[i]["PatientType"].ToString();
                DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                D.AddedDate = AddedDate;
                DateTime ModifyDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["ModifyDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ModifyDate);
                D.ModifyDate = ModifyDate;
                int AddedBy = 0; int.TryParse(dt.Rows[i]["AddedBy"].ToString(), out AddedBy); D.AddedBy = AddedBy;
                int ModifyBy = 0; int.TryParse(dt.Rows[i]["ModifyBy"].ToString(), out ModifyBy); D.ModifyBy = ModifyBy;
            }
            return D;
        }

        public List<SnehDLL.PatientTypes_Dll> GetList()
        {
            List<SnehDLL.PatientTypes_Dll> DL = new List<SnehDLL.PatientTypes_Dll>();
            SqlCommand cmd = new SqlCommand("PatientTypes_GetList"); cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.PatientTypes_Dll D = new SnehDLL.PatientTypes_Dll();
                D.PatientTypeID = int.Parse(dt.Rows[i]["PatientTypeID"].ToString());
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                D.PatientType = dt.Rows[i]["PatientType"].ToString();
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
    }
}
