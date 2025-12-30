using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace SnehBLL
{
    public class DoctorLedger_Bll
    {
        DbHelper.SqlDb db;

        public DoctorLedger_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public static int Check(string _uniqueID)
        {
            int _ledgerID = 0;
            SqlCommand cmd = new SqlCommand("DoctorLedger_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = _uniqueID;
            cmd.Parameters.Add("@LedgerID", SqlDbType.Int).Value = 0;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["LedgerID"].ToString(), out _ledgerID);
            }
            return _ledgerID;
        }

        public static string Check(int _ledgerID)
        {
            string _uniqueID = "";
            SqlCommand cmd = new SqlCommand("DoctorLedger_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = "";
            cmd.Parameters.Add("@LedgerID", SqlDbType.Int).Value = _ledgerID;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                _uniqueID = dt.Rows[0]["UniqueID"].ToString();
            }
            return _uniqueID;
        }

        public int NewExpense(int _doctorID, float _amount, DateTime _payDate, int _paymentID, string _narration,
            int _bankID, DateTime _chequeDate, int _loginID)
        {
            SqlCommand cmd = new SqlCommand("DoctorLedger_NewPayment"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = _doctorID;
            cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = _amount;
            if (_payDate > DateTime.MinValue)
                cmd.Parameters.Add("@PayDate", SqlDbType.DateTime).Value = _payDate;
            else
                cmd.Parameters.Add("@PayDate", SqlDbType.DateTime).Value = DBNull.Value;

            cmd.Parameters.Add("@PaymentType", SqlDbType.Int).Value = _paymentID;
            cmd.Parameters.Add("@Narration", SqlDbType.VarChar, 4000).Value = _narration;
            cmd.Parameters.Add("@BankID", SqlDbType.Int).Value = _bankID;
            if (_chequeDate > DateTime.MinValue)
                cmd.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = _chequeDate;
            else
                cmd.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@ModifyBy", SqlDbType.Int).Value = _loginID;

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
