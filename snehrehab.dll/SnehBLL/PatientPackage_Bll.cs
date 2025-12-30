using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace SnehBLL
{
    public class PatientPackage_Bll
    {
        DbHelper.SqlDb db;

        public PatientPackage_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public static int Check(string _uniqueID)
        {
            int _bookingID = 0;
            SqlCommand cmd = new SqlCommand("PatientPackage_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = _uniqueID;
            cmd.Parameters.Add("@BookingID", SqlDbType.Int).Value = 0;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["BookingID"].ToString(), out _bookingID);
            }
            return _bookingID;
        }

        public static string Check(int _bookingID)
        {
            string _uniqueID = "";
            SqlCommand cmd = new SqlCommand("PatientPackage_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = "";
            cmd.Parameters.Add("@BookingID", SqlDbType.Int).Value = _bookingID;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                _uniqueID = dt.Rows[0]["UniqueID"].ToString();
            }
            return _uniqueID;
        }
        public DataTable GetPacakgeUsage(int _bookingID)
        {
            SqlCommand cmd = new SqlCommand("GetBookingUsageDetails"); cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@BookingID", SqlDbType.Int).Value = _bookingID;

            return db.DbRead(cmd);
        }
        public DataTable GetBulkPacakgeUsage(int _BulkID)
        {
            SqlCommand cmd = new SqlCommand("GetBulkBookingUsageDetails"); cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@BulkID", SqlDbType.Int).Value = _BulkID;

            return db.DbRead(cmd);
        }
        public DataSet PatientDetail(int _patientID)
        {
            SqlCommand cmd = new SqlCommand("PatientPackage_PatientDetail"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = _patientID;

            return db.DbFetch(cmd);
        }

        public SnehDLL.PatientPackage_Dll Get(int _bookingID)
        {
            SnehDLL.PatientPackage_Dll D = null;
            SqlCommand cmd = new SqlCommand("PatientPackage_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@BookingID", SqlDbType.Int).Value = _bookingID;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                D = new SnehDLL.PatientPackage_Dll(); int i = 0;
                D.BookingID = int.Parse(dt.Rows[i]["BookingID"].ToString());
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                int PatientID = 0; int.TryParse(dt.Rows[i]["PatientID"].ToString(), out PatientID); D.PatientID = PatientID;
                int SessionID = 0; int.TryParse(dt.Rows[i]["SessionID"].ToString(), out SessionID); D.SessionID = SessionID;
                int PackageID = 0; int.TryParse(dt.Rows[i]["PackageID"].ToString(), out PackageID); D.PackageID = PackageID;
                float AppointmentCharge = 0; float.TryParse(dt.Rows[i]["AppointmentCharge"].ToString(), out AppointmentCharge); D.AppointmentCharge = AppointmentCharge;
                float AppointmentCount = 0; float.TryParse(dt.Rows[i]["AppointmentCount"].ToString(), out AppointmentCount); D.AppointmentCount = AppointmentCount;
                float ExtraCharge = 0; float.TryParse(dt.Rows[i]["ExtraCharge"].ToString(), out ExtraCharge); D.ExtraCharge = ExtraCharge;
                float PackageAmount = 0; float.TryParse(dt.Rows[i]["PackageAmount"].ToString(), out PackageAmount); D.PackageAmount = PackageAmount;
                int ModePayment = 0; int.TryParse(dt.Rows[i]["ModePayment"].ToString(), out ModePayment); D.ModePayment = ModePayment;
                int BankID = 0; int.TryParse(dt.Rows[i]["BankID"].ToString(), out BankID); D.BankID = BankID;
                D.Narration = dt.Rows[i]["Narration"].ToString();
                DateTime ChequeDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["ChequeDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ChequeDate);
                D.ChequeDate = ChequeDate;
                DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                D.AddedDate = AddedDate;
                DateTime ModifyDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["ModifyDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ModifyDate);
                D.ModifyDate = ModifyDate;
                int AddedBy = 0; int.TryParse(dt.Rows[i]["AddedBy"].ToString(), out AddedBy); D.AddedBy = AddedBy;
                int ModifyBy = 0; int.TryParse(dt.Rows[i]["ModifyBy"].ToString(), out ModifyBy); D.ModifyBy = ModifyBy;
            }
            return D;
        }

        public int Set(SnehDLL.PatientPackage_Dll D)
        {
            SqlCommand cmd = new SqlCommand("PatientPackage_Set"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@BookingID", SqlDbType.Int).Value = D.BookingID;
            cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = D.PatientID;
            cmd.Parameters.Add("@SessionID", SqlDbType.Int).Value = D.SessionID;
            cmd.Parameters.Add("@PackageID", SqlDbType.Int).Value = D.PackageID;
            cmd.Parameters.Add("@AppointmentCharge", SqlDbType.Decimal).Value = D.AppointmentCharge;
            cmd.Parameters.Add("@AppointmentCount", SqlDbType.Decimal).Value = D.AppointmentCount;
            cmd.Parameters.Add("@ExtraCharge", SqlDbType.Decimal).Value = D.ExtraCharge;
            cmd.Parameters.Add("@PackageAmount", SqlDbType.Decimal).Value = D.PackageAmount;
            cmd.Parameters.Add("@ModePayment", SqlDbType.Int).Value = D.ModePayment;
            cmd.Parameters.Add("@BankID", SqlDbType.Int).Value = D.BankID;
            cmd.Parameters.Add("@BankBranch", SqlDbType.NVarChar, 500).Value = D.BankBranch;
            cmd.Parameters.Add("@Narration", SqlDbType.VarChar, 4000).Value = D.Narration;
            cmd.Parameters.Add("@ChequeTxnNo", SqlDbType.NVarChar, 500).Value = D.ChequeTxnNo;
            if (D.ChequeDate > DateTime.MinValue)
                cmd.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = D.ChequeDate;
            else
                cmd.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = DBNull.Value;
            if (D.ModifyDate > DateTime.MinValue)
                cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = D.ModifyDate;
            else
                cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@ModifyBy", SqlDbType.Int).Value = D.ModifyBy;
            cmd.Parameters.Add("@IsDiscounted", SqlDbType.Bit).Value = D.IsDiscounted;
            cmd.Parameters.Add("@DiscountType", SqlDbType.Int).Value = D.DiscountType;
            cmd.Parameters.Add("@DiscountValue", SqlDbType.Decimal).Value = D.DiscountValue;
            cmd.Parameters.Add("@DiscountAmt", SqlDbType.Decimal).Value = D.DiscountAmt;
            cmd.Parameters.Add("@NetAmt", SqlDbType.Decimal).Value = D.NetAmt;
            cmd.Parameters.Add("@DiscountedOn", SqlDbType.Int).Value = D.DiscountedOn;
            cmd.Parameters.Add("@NewSessionCharge", SqlDbType.Decimal).Value = D.NewSessionCharge;
            cmd.Parameters.Add("@HospitalReceiptID", SqlDbType.VarChar, 100)
             .Value = string.IsNullOrEmpty(D.HospitalReceiptID) ? (object)DBNull.Value : D.HospitalReceiptID;

            if (D.HospitalReceiptDate > DateTime.MinValue)
                cmd.Parameters.Add("@HospitalReceiptDate", SqlDbType.DateTime).Value = D.HospitalReceiptDate;
            else
                cmd.Parameters.Add("@HospitalReceiptDate", SqlDbType.DateTime).Value = DBNull.Value;

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

        public DataTable Search(string _searchText, DateTime _fromDate, DateTime _uptoDate)
        {
            SqlCommand cmd = new SqlCommand("PatientPackage_Search"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 50).Value = _searchText;
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

        public int Delete(int _bookingID)
        {
            SqlCommand cmd = new SqlCommand("PatientPackage_Delete"); cmd.CommandType = CommandType.StoredProcedure;
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

        public bool IsUsed(int _bookingID)
        {
            SqlCommand cmd = new SqlCommand("PatientPackage_IsUsed"); cmd.CommandType = CommandType.StoredProcedure;
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
            return i > 0;
        }
    }
}