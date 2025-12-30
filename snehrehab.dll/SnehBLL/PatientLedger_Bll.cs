using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace SnehBLL
{
    public class PatientLedger_Bll
    {
        DbHelper.SqlDb db;

        public enum LedgerHead
        {
            REGISTRATION_CHARGE,
            PACKAGE_CHARGE,
            UNKNOWN
        }

        private string LocationTypeGet(LedgerHead _ledgerHead)
        {
            string _type = "";
            switch (_ledgerHead)
            {
                case LedgerHead.REGISTRATION_CHARGE: _type = "REGISTRATION CHARGE"; break;
                case LedgerHead.PACKAGE_CHARGE: _type = "PACKAGE CHARGE"; break;
                default: _type = "UNKNOWN"; break;
            }
            return _type;
        }

        public PatientLedger_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public static int Check(string _uniqueID)
        {
            int _ledgerID = 0;
            SqlCommand cmd = new SqlCommand("PatientLedger_Check"); cmd.CommandType = CommandType.StoredProcedure;
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
            SqlCommand cmd = new SqlCommand("PatientLedger_Check"); cmd.CommandType = CommandType.StoredProcedure;
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

        public int NewRegistration(int _patientID, int _paymentType, int _bankID, string BankBranch,
            string ChequeTxnNo, DateTime _chequeDate, string _narration, int _patientTypeID)
        {
            SqlCommand cmd = new SqlCommand("PatientLedger_NewRegistration"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = _patientID;
            cmd.Parameters.Add("@PaymentType", SqlDbType.Int).Value = _paymentType;
            cmd.Parameters.Add("@BankID", SqlDbType.Int).Value = _bankID;
            cmd.Parameters.Add("@BankBranch", SqlDbType.NVarChar, 500).Value = BankBranch;
            cmd.Parameters.Add("@ChequeTxnNo", SqlDbType.NVarChar, 500).Value = ChequeTxnNo;
            if (_chequeDate > DateTime.MinValue)
                cmd.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = _chequeDate;
            else
                cmd.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@Narration", SqlDbType.VarChar, 4000).Value = _narration;
            cmd.Parameters.Add("@PatientTypeID", SqlDbType.Int).Value = _patientTypeID;

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

        public DataTable RegistrationBalance(string _search)
        {
            SqlCommand cmd = new SqlCommand("PatientLedger_BalanceRegistration"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 50).Value = _search;

            return db.DbRead(cmd);
        }

        public DataTable PackageBalance(string _search, DateTime _fromDate, DateTime _uptoDate)
        {
            SqlCommand cmd = new SqlCommand("PatientLedger_BalancePackage"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 50).Value = _search;
            if (_fromDate > DateTime.MinValue)
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = _fromDate;
            else
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = DBNull.Value;
            if (_uptoDate > DateTime.MinValue)
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = _uptoDate;
            else
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = DBNull.Value;

            return db.DbRead(cmd);
        }

        public int PayRegistration(int _patientID, float _amount, DateTime _payDate, string ChequeTxnNo, int _paymentMode, long bulkbookingid, string _narration)
        {
            SqlCommand cmd = new SqlCommand("PatientLedger_PayRegistration"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = _patientID;
            cmd.Parameters.Add("@PayAmt", SqlDbType.Decimal).Value = _amount;
            cmd.Parameters.Add("@ChequeTxnNo", SqlDbType.NVarChar, 500).Value = ChequeTxnNo;
            cmd.Parameters.Add("@PayMode", SqlDbType.Int).Value = _paymentMode;
            cmd.Parameters.Add("@BulkBookingID", SqlDbType.BigInt).Value = bulkbookingid;
            cmd.Parameters.Add("@Narration", SqlDbType.VarChar, 4000).Value = _narration;
            if (_payDate > DateTime.MinValue)
                cmd.Parameters.Add("@PayDate", SqlDbType.DateTime).Value = _payDate;
            else
                cmd.Parameters.Add("@PayDate", SqlDbType.DateTime).Value = DBNull.Value;

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

        public int NewPackageBooking(int _bookingID)
        {
            SqlCommand cmd = new SqlCommand("PatientLedger_NewPackage"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@BookingID", SqlDbType.Int).Value = _bookingID;
            
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

        public int PayPackage(int _ledgerID, float _amount, DateTime _payDate, int _paymentMode, int _bankID, string BankBranch, string ChequeTxnNo, DateTime _chequeDate, string _narration)
        {
            SqlCommand cmd = new SqlCommand("PatientLedger_PayPackages"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@LedgerID", SqlDbType.Int).Value = _ledgerID;
            cmd.Parameters.Add("@PayAmt", SqlDbType.Decimal).Value = _amount;
            if (_payDate > DateTime.MinValue)
                cmd.Parameters.Add("@PayDate", SqlDbType.DateTime).Value = _payDate;
            else
                cmd.Parameters.Add("@PayDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@PayMode", SqlDbType.Int).Value = _paymentMode;
            cmd.Parameters.Add("@BankID", SqlDbType.Int).Value = _bankID;
            cmd.Parameters.Add("@BankBranch", SqlDbType.NVarChar, 500).Value = BankBranch;
            cmd.Parameters.Add("@ChequeTxnNo", SqlDbType.NVarChar, 500).Value = ChequeTxnNo;
            if (_chequeDate > DateTime.MinValue)
                cmd.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = _chequeDate;
            else
                cmd.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@Narration", SqlDbType.VarChar, 4000).Value = _narration;

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

        public long NewBulkBooking(long BulkID)
        {
            SqlCommand cmd = new SqlCommand("PatientLedger_NewBulk"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@BulkID", SqlDbType.BigInt).Value = BulkID;

            SqlParameter Param = new SqlParameter(); Param.ParameterName = "@RetVal";
            Param.DbType = DbType.Int64; Param.Direction = ParameterDirection.Output;
            Param.Value = 0; cmd.Parameters.Add(Param);

            db.DbUpdate(cmd);

            long i = 0;
            if (cmd.Parameters["@RetVal"].Value != null)
            {
                long.TryParse(cmd.Parameters["@RetVal"].Value.ToString(), out i);
            }
            return i;
        }
    }
}
