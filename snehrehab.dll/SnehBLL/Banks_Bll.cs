using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace SnehBLL
{
    public class Banks_Bll
    {
        DbHelper.SqlDb db;

        public Banks_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public static int Check(string _uniqueID)
        {
            int _bankID = 0;
            SqlCommand cmd = new SqlCommand("Banks_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = _uniqueID;
            cmd.Parameters.Add("@BankID", SqlDbType.Int).Value = 0;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["BankID"].ToString(), out _bankID);
            }
            return _bankID;
        }

        public static string Check(int _bankID)
        {
            string _uniqueID = "";
            SqlCommand cmd = new SqlCommand("Banks_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = "";
            cmd.Parameters.Add("@BankID", SqlDbType.Int).Value = _bankID;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                _uniqueID = dt.Rows[0]["UniqueID"].ToString();
            }
            return _uniqueID;
        }

        public SnehDLL.Banks_Dll Get(int _bankID)
        {
            SnehDLL.Banks_Dll D = null;
            SqlCommand cmd = new SqlCommand("Banks_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@BankID", SqlDbType.Int).Value = _bankID;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                D = new SnehDLL.Banks_Dll(); int i = 0;
                D.BankID = int.Parse(dt.Rows[i]["BankID"].ToString());
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                D.BankName = dt.Rows[i]["BankName"].ToString();
                DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                D.AddedDate = AddedDate;
                DateTime ModifyDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["ModifyDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ModifyDate);
                D.ModifyDate = ModifyDate;
                int AddedBy = 0; int.TryParse(dt.Rows[i]["AddedBy"].ToString(), out AddedBy); D.AddedBy = AddedBy;
                int ModifyBy = 0; int.TryParse(dt.Rows[i]["ModifyBy"].ToString(), out ModifyBy); D.ModifyBy = ModifyBy;
            }
            return D;
        }

        public List<SnehDLL.Banks_Dll> GetList()
        {
            List<SnehDLL.Banks_Dll> DL = new List<SnehDLL.Banks_Dll>();
            SqlCommand cmd = new SqlCommand("Banks_GetList"); cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.Banks_Dll D = new SnehDLL.Banks_Dll();
                D.BankID = int.Parse(dt.Rows[i]["BankID"].ToString());
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                D.BankName = dt.Rows[i]["BankName"].ToString();
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

        public int Set(SnehDLL.Banks_Dll D)
        {
            SqlCommand cmd = new SqlCommand("Banks_Set"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@BankID", SqlDbType.Int).Value = D.BankID;
            cmd.Parameters.Add("@BankName", SqlDbType.VarChar, 250).Value = D.BankName;
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
