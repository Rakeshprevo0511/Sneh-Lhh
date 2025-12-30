using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SnehBLL
{
    public class OtherAct_CashEntry_BLL
    {
        DbHelper.SqlDb db;

        public OtherAct_CashEntry_BLL()
        {
            db = new DbHelper.SqlDb();
        }
        public int set(SnehDLL.OtherAct_CashEntry_DLL OCD)
        {
            SqlCommand cmd = new SqlCommand("OtherActivity_CashEntries"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AccountType", SqlDbType.Int).Value = OCD.AccountType;
            cmd.Parameters.Add("@AccountNameID", SqlDbType.Int).Value = OCD.AccountNameID;
            cmd.Parameters.Add("@CreditAmt", SqlDbType.Decimal).Value = OCD.CreditAmt;
            cmd.Parameters.Add("@DebitAmt", SqlDbType.Decimal).Value = OCD.DebitAmt;
            cmd.Parameters.Add("@PayMode", SqlDbType.Int).Value = OCD.PayMode;
            cmd.Parameters.Add("@Narration", SqlDbType.VarChar, 4000).Value = OCD.Narration;
            cmd.Parameters.Add("@ChequeTxnNo", SqlDbType.NVarChar, 500).Value = OCD.ChequeTxnNo;
            if (OCD.PayDate > DateTime.MinValue)
                cmd.Parameters.Add("@PayDate", SqlDbType.DateTime).Value = OCD.PayDate;
            else
                cmd.Parameters.Add("@PayDate", SqlDbType.DateTime).Value = DBNull.Value;
            if (OCD.ModifyDate > DateTime.MinValue)
                cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = OCD.ModifyDate;
            else
                cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@ModifyBy", SqlDbType.Int).Value = OCD.ModifyBy;
            cmd.Parameters.Add("@BankID", SqlDbType.Int).Value = OCD.BankID;
            if (OCD.ChequeDate > DateTime.MinValue)
                cmd.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = OCD.ChequeDate;
            else
                cmd.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@HeadID", SqlDbType.Int).Value = OCD.HeadID;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = OCD.DoctorID;
            cmd.Parameters.Add("@Ass_DoctorID", SqlDbType.Int).Value = OCD.Ass_DoctorID;
            cmd.Parameters.Add("@AccountNamePatientID", SqlDbType.Int).Value = OCD.Account_NamePatientID;
            cmd.Parameters.Add("@AccountNameDoctorID", SqlDbType.Int).Value = OCD.Account_NameDoctorID;
            cmd.Parameters.Add("@ProductCatID", SqlDbType.Int).Value = OCD.ProductCatID;
            cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = OCD.ProductID;
            cmd.Parameters.Add("@AccountName", SqlDbType.NVarChar, 250).Value = OCD.AccountName;
            cmd.Parameters.Add("@ProductCategory", SqlDbType.NVarChar, 250).Value = OCD.ProductCategory;
            cmd.Parameters.Add("@ProductName", SqlDbType.NVarChar, 250).Value = OCD.ProductName;
            cmd.Parameters.Add("@Doctor", SqlDbType.NVarChar, 250).Value = OCD.Doctor;
            cmd.Parameters.Add("@Ass_Doctor", SqlDbType.NVarChar, 250).Value = OCD.Ass_Doctor;
            if (OCD.Online_TransactionID != "" && OCD.Online_TransactionDate != null)
                cmd.Parameters.Add("@OnlineTransactionID", SqlDbType.NVarChar, 500).Value = OCD.Online_TransactionID;
            else
                cmd.Parameters.Add("@OnlineTransactionID", SqlDbType.NVarChar, 500).Value = DBNull.Value;
            if (OCD.Online_TransactionDate > DateTime.MinValue)
                cmd.Parameters.Add("@OnlineTransactionDate", SqlDbType.DateTime).Value = OCD.Online_TransactionDate;
            else
                cmd.Parameters.Add("@OnlineTransactionDate", SqlDbType.DateTime).Value = DBNull.Value;
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
