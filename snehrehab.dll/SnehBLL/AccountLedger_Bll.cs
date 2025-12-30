using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace SnehBLL
{
    public class AccountLedger_Bll
    {
        DbHelper.SqlDb db;

        public AccountLedger_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public int NewExpense(int _headID, float _amount, DateTime _payDate, int _paymentID, string _narration,
            int _bankID, DateTime _chequeDate, int _loginID)
        {
            SqlCommand cmd = new SqlCommand("AccountLedger_NewPayment"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@HeadID", SqlDbType.Int).Value = _headID;
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
