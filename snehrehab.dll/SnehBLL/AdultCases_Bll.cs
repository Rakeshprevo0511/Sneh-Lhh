using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace SnehBLL
{
    public class AdultCases_Bll
    {
        DbHelper.SqlDb db;

        public AdultCases_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public static int Check(string _uniqueID)
        {
            int _adultCaseID = 0;
            SqlCommand cmd = new SqlCommand("AdultCases_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = _uniqueID;
            cmd.Parameters.Add("@AdultCaseID", SqlDbType.Int).Value = 0;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["AdultCaseID"].ToString(), out _adultCaseID);
            }
            return _adultCaseID;
        }

        public static string Check(int _adultCaseID)
        {
            string _uniqueID = "";
            SqlCommand cmd = new SqlCommand("AdultCases_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = "";
            cmd.Parameters.Add("@AdultCaseID", SqlDbType.Int).Value = _adultCaseID;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                _uniqueID = dt.Rows[0]["UniqueID"].ToString();
            }
            return _uniqueID;
        }

        public SnehDLL.AdultCases_Dll Get(int _adultCaseID)
        {
            SnehDLL.AdultCases_Dll D = null;
            SqlCommand cmd = new SqlCommand("AdultCases_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AdultCaseID", SqlDbType.Int).Value = _adultCaseID;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                D = new SnehDLL.AdultCases_Dll(); int i = 0;
                D.AdultCaseID = int.Parse(dt.Rows[i]["AdultCaseID"].ToString());
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                D.AdultCase = dt.Rows[i]["AdultCase"].ToString();
                DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                D.AddedDate = AddedDate;
                DateTime ModifyDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["ModifyDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ModifyDate);
                D.ModifyDate = ModifyDate;
                int AddedBy = 0; int.TryParse(dt.Rows[i]["AddedBy"].ToString(), out AddedBy); D.AddedBy = AddedBy;
                int ModifyBy = 0; int.TryParse(dt.Rows[i]["ModifyBy"].ToString(), out ModifyBy); D.ModifyBy = ModifyBy;
            }
            return D;
        }

        public List<SnehDLL.AdultCases_Dll> GetList()
        {
            List<SnehDLL.AdultCases_Dll> DL = new List<SnehDLL.AdultCases_Dll>();
            SqlCommand cmd = new SqlCommand("AdultCases_GetList"); cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.AdultCases_Dll D = new SnehDLL.AdultCases_Dll();
                D.AdultCaseID = int.Parse(dt.Rows[i]["AdultCaseID"].ToString());
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                D.AdultCase = dt.Rows[i]["AdultCase"].ToString();
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
