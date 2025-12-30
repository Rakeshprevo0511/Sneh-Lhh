using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace SnehBLL
{
    public class CashEntries_Bll
    {
        DbHelper.SqlDb db;

        public CashEntries_Bll()
        {
            db = new DbHelper.SqlDb(); 
        }

        public int NewCash(SnehDLL.CashEntries_Dll SD)
        {
            SqlCommand cmd = new SqlCommand("CashEntries_New"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AccountType", SqlDbType.Int).Value = SD.AccountType;
            cmd.Parameters.Add("@AccountNameID", SqlDbType.Int).Value = SD.AccountNameID;
            cmd.Parameters.Add("@CreditAmt", SqlDbType.Decimal).Value = SD.CreditAmt;
            cmd.Parameters.Add("@DebitAmt", SqlDbType.Decimal).Value = SD.DebitAmt;
            cmd.Parameters.Add("@PayMode", SqlDbType.Int).Value = SD.PayMode;
            cmd.Parameters.Add("@Narration", SqlDbType.VarChar, 4000).Value = SD.Narration;
            if (SD.PayDate > DateTime.MinValue)
                cmd.Parameters.Add("@PayDate", SqlDbType.DateTime).Value = SD.PayDate;
            else
                cmd.Parameters.Add("@PayDate", SqlDbType.DateTime).Value = DBNull.Value;
            if (SD.ModifyDate > DateTime.MinValue)
                cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = SD.ModifyDate;
            else
                cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@ModifyBy", SqlDbType.Int).Value = SD.ModifyBy;
            cmd.Parameters.Add("@BankID", SqlDbType.Int).Value = SD.BankID;
            if (SD.ChequeDate > DateTime.MinValue)
                cmd.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = SD.ChequeDate;
            else
                cmd.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@HeadID", SqlDbType.Int).Value = SD.HeadID;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = SD.DoctorID;
            cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = SD.PatientID;
            cmd.Parameters.Add("@ProductCatID", SqlDbType.Int).Value = SD.ProductCatID;
            cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = SD.ProductID;

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

        public DataTable Search(int _accountType, int _accountID, DateTime _fromDate, DateTime _uptoDate, string _searchText)
        {
            SqlCommand cmd = new SqlCommand("CashEntries_Search"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AccountType", SqlDbType.Int).Value = _accountType;
            cmd.Parameters.Add("@AccountNameID", SqlDbType.Int).Value = _accountID; 
            if (_fromDate > DateTime.MinValue)
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = _fromDate;
            else
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = DBNull.Value;
            if (_uptoDate > DateTime.MinValue)
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = _uptoDate;
            else
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@SearchText", SqlDbType.VarChar, 50).Value = _searchText;

            return db.DbRead(cmd);
        }
    }
}
