using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace SnehBLL
{
    public class PatientChrges_Bll
    {
        DbHelper.SqlDb db;

        public PatientChrges_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public static int Check(string _uniqueID)
        {
            int _chargeID = 0;
            SqlCommand cmd = new SqlCommand("PatientChrges_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = _uniqueID;
            cmd.Parameters.Add("@ChargeID", SqlDbType.Int).Value = 0;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["ChargeID"].ToString(), out _chargeID);
            }
            return _chargeID;
        }

        public static string Check(int _chargeID)
        {
            string _uniqueID = "";
            SqlCommand cmd = new SqlCommand("PatientChrges_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = "";
            cmd.Parameters.Add("@ChargeID", SqlDbType.Int).Value = _chargeID;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                _uniqueID = dt.Rows[0]["UniqueID"].ToString();
            }
            return _uniqueID;
        }

        public int Delete(int _chargeID)
        {
            SqlCommand cmd = new SqlCommand("PatientChrges_Delete"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ChargeID", SqlDbType.Int).Value = _chargeID;

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

        public SnehDLL.PatientChrges_Dll Get(int _chargeID)
        {
            SnehDLL.PatientChrges_Dll D = null;
            SqlCommand cmd = new SqlCommand("PatientChrges_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ChargeID", SqlDbType.Int).Value = _chargeID;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                D = new SnehDLL.PatientChrges_Dll(); int i = 0;
                D.ChargeID = int.Parse(dt.Rows[i]["ChargeID"].ToString());
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                int PatientTypeID = 0; int.TryParse(dt.Rows[i]["PatientTypeID"].ToString(), out PatientTypeID);
                D.PatientTypeID = PatientTypeID;
                float ChargeAmt = 0; float.TryParse(dt.Rows[i]["ChargeAmt"].ToString(), out ChargeAmt);
                D.ChargeAmt = ChargeAmt;
                DateTime ValidFrom = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["ValidFrom"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ValidFrom);
                D.ValidFrom = ValidFrom;
                DateTime ValidUpto = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["ValidUpto"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ValidUpto);
                D.ValidUpto = ValidUpto;
                DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                D.AddedDate = AddedDate;
                DateTime ModifyDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["ModifyDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ModifyDate);
                D.ModifyDate = ModifyDate;
                int AddedBy = 0; int.TryParse(dt.Rows[i]["AddedBy"].ToString(), out AddedBy); D.AddedBy = AddedBy;
                int ModifyBy = 0; int.TryParse(dt.Rows[i]["ModifyBy"].ToString(), out ModifyBy); D.ModifyBy = ModifyBy;
            }
            return D;
        }

        public List<SnehDLL.PatientChrges_Dll> GetList()
        {
            List<SnehDLL.PatientChrges_Dll> DL = new List<SnehDLL.PatientChrges_Dll>();
            SqlCommand cmd = new SqlCommand("PatientChrges_GetList"); cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.PatientChrges_Dll D = new SnehDLL.PatientChrges_Dll();
                D.ChargeID = int.Parse(dt.Rows[i]["ChargeID"].ToString());
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                int PatientTypeID = 0; int.TryParse(dt.Rows[i]["PatientTypeID"].ToString(), out PatientTypeID);
                D.PatientTypeID = PatientTypeID;
                float ChargeAmt = 0; float.TryParse(dt.Rows[i]["ChargeAmt"].ToString(), out ChargeAmt);
                D.ChargeAmt = ChargeAmt;
                DateTime ValidFrom = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["ValidFrom"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ValidFrom);
                D.ValidFrom = ValidFrom;
                DateTime ValidUpto = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["ValidUpto"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ValidUpto);
                D.ValidUpto = ValidUpto;
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

        public DataTable Search()
        {
            SqlCommand cmd = new SqlCommand("PatientChrges_GetSearch"); cmd.CommandType = CommandType.StoredProcedure;
            return db.DbRead(cmd);
        }

        public int Set(SnehDLL.PatientChrges_Dll D)
        {
            SqlCommand cmd = new SqlCommand("PatientChrges_Set"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ChargeID", SqlDbType.Int).Value = D.ChargeID;
            cmd.Parameters.Add("@PatientTypeID", SqlDbType.Int).Value = D.PatientTypeID;
            cmd.Parameters.Add("@ChargeAmt", SqlDbType.Decimal).Value = D.ChargeAmt;
            if (D.ValidFrom > DateTime.MinValue)
                cmd.Parameters.Add("@ValidFrom", SqlDbType.DateTime).Value = D.ValidFrom;
            else
                cmd.Parameters.Add("@ValidFrom", SqlDbType.DateTime).Value = DBNull.Value;
            if (D.ValidUpto > DateTime.MinValue)
                cmd.Parameters.Add("@ValidUpto", SqlDbType.DateTime).Value = D.ValidUpto;
            else
                cmd.Parameters.Add("@ValidUpto", SqlDbType.DateTime).Value = DBNull.Value;
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

        public int SetOnly(SnehDLL.PatientChrges_Dll D)
        {
            SqlCommand cmd = new SqlCommand("PatientChrges_SetOnly"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PatientTypeID", SqlDbType.Int).Value = D.PatientTypeID;
            cmd.Parameters.Add("@ChargeAmt", SqlDbType.Decimal).Value = D.ChargeAmt;
            if (D.ValidFrom > DateTime.MinValue)
                cmd.Parameters.Add("@ValidFrom", SqlDbType.DateTime).Value = D.ValidFrom;
            else
                cmd.Parameters.Add("@ValidFrom", SqlDbType.DateTime).Value = DBNull.Value;
            if (D.ValidUpto > DateTime.MinValue)
                cmd.Parameters.Add("@ValidUpto", SqlDbType.DateTime).Value = D.ValidUpto;
            else
                cmd.Parameters.Add("@ValidUpto", SqlDbType.DateTime).Value = DBNull.Value;
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
