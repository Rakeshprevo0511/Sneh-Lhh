using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace SnehBLL
{
    public class AppointmentSession_Bll
    {
        DbHelper.SqlDb db;

        public AppointmentSession_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public int Delete(int _appointmentID)
        {
            SqlCommand cmd = new SqlCommand("AppointmentSession_Delete"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;

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

        public int Set(int AppointmentID, float sessionCharges, long bulkID, int BulkSelected, DateTime EntryDate, int ModifyBy,
            string Narration, int _appointmentStatus, int BulkPackageID, int patientID)
        {
            SqlCommand cmd = new SqlCommand("AppointmentSession_SetBulk"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = AppointmentID;
            cmd.Parameters.Add("@SessionCharges", SqlDbType.Decimal).Value = sessionCharges;
            cmd.Parameters.Add("@BulkID", SqlDbType.BigInt).Value = bulkID;
            cmd.Parameters.Add("@BulkSelected", SqlDbType.Int).Value = BulkSelected;
            cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = patientID;
            if (EntryDate > DateTime.MinValue)
                cmd.Parameters.Add("@EntryDate", SqlDbType.DateTime).Value = EntryDate;
            else
                cmd.Parameters.Add("@EntryDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@ModifyBy", SqlDbType.Int).Value = ModifyBy;
            cmd.Parameters.Add("@Narration", SqlDbType.VarChar, 4000).Value = Narration;
            cmd.Parameters.Add("@AppointmentStatus", SqlDbType.Int).Value = _appointmentStatus;
            cmd.Parameters.Add("@BulkPackageID", SqlDbType.Int).Value = BulkPackageID;

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

        public int SetBulkPayment(int _appointmentID)
        {
            SqlCommand cmd = new SqlCommand("PatientLedger_NewBulkSession"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;

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

        public int Set(SnehDLL.AppointmentSession_Dll D, int _appointmentStatus)
        {
            SqlCommand cmd = new SqlCommand("AppointmentSession_Set"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = D.AppointmentID;
            cmd.Parameters.Add("@BookingID", SqlDbType.Int).Value = D.BookingID;
            cmd.Parameters.Add("@PackageID", SqlDbType.Int).Value = D.PackageID;
            cmd.Parameters.Add("@PackageTotalBalance", SqlDbType.Decimal).Value = D.PackageTotalBalance;
            cmd.Parameters.Add("@AppointmentCharge", SqlDbType.Decimal).Value = D.AppointmentCharge;
            cmd.Parameters.Add("@PackageNewBalance", SqlDbType.Decimal).Value = D.PackageNewBalance;
            cmd.Parameters.Add("@PaymentType", SqlDbType.Int).Value = D.PaymentType;
            cmd.Parameters.Add("@AmountToPay", SqlDbType.Decimal).Value = D.AmountToPay;
            cmd.Parameters.Add("@BankID", SqlDbType.Int).Value = D.BankID;
            if (D.ChequeDate > DateTime.MinValue)
                cmd.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = D.ChequeDate;
            else
                cmd.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@Narration", SqlDbType.VarChar, 4000).Value = D.Narration;
            cmd.Parameters.Add("@OtherBookingID", SqlDbType.Int).Value = D.OtherBookingID;
            cmd.Parameters.Add("@OtherBookingTotalBalance", SqlDbType.Int).Value = D.OtherBookingTotalBalance;
            cmd.Parameters.Add("@OtherBookingNewBalance", SqlDbType.Int).Value = D.OtherBookingNewBalance;            
            if (D.ModifyDate > DateTime.MinValue)
                cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = D.ModifyDate;
            else
                cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = DBNull.Value;

            cmd.Parameters.Add("@ModifyBy", SqlDbType.Int).Value = D.ModifyBy;
            cmd.Parameters.Add("@AppointmentStatus", SqlDbType.Int).Value = _appointmentStatus;

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

        public int SetEvalutionPayment(int _appointmentID)
        {
            SqlCommand cmd = new SqlCommand("PatientLedger_NewEvaluation"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;

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

        public int SetOtherPayment(int _appointmentID)
        {
            SqlCommand cmd = new SqlCommand("PatientLedger_NewOtherSession"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;

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

        public int SetPackagePayment(int _appointmentID, int _deductBookingID)
        {
            SqlCommand cmd = new SqlCommand("PatientLedger_NewPackageSession"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            cmd.Parameters.Add("@DeductBookingID", SqlDbType.Int).Value = _deductBookingID;

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

        public int SetDoctorPayment(int _appointmentID)
        {
            SqlCommand cmd = new SqlCommand("DoctorLedger_NewAppointment"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;

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

        public SnehDLL.AppointmentSession_Dll Get(int AppointmentID)
        {
            SnehDLL.AppointmentSession_Dll D = null;
            SqlCommand cmd = new SqlCommand("AppointmentSession_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = AppointmentID;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                D = new SnehDLL.AppointmentSession_Dll(); int i = 0;
                D.AppointmentID = int.Parse(dt.Rows[i]["AppointmentID"].ToString());
                int BookingID = 0; int.TryParse(dt.Rows[i]["BookingID"].ToString(), out BookingID); D.BookingID = BookingID;
                int PackageID = 0; int.TryParse(dt.Rows[i]["PackageID"].ToString(), out PackageID); D.PackageID = PackageID;
                float PackageTotalBalance = 0; float.TryParse(dt.Rows[i]["PackageTotalBalance"].ToString(), out PackageTotalBalance); D.PackageTotalBalance = PackageTotalBalance;
                float AppointmentCharge = 0; float.TryParse(dt.Rows[i]["AppointmentCharge"].ToString(), out AppointmentCharge); D.AppointmentCharge = AppointmentCharge;
                float PackageNewBalance = 0; float.TryParse(dt.Rows[i]["PackageNewBalance"].ToString(), out PackageNewBalance); D.PackageNewBalance = PackageNewBalance;
                int PaymentType = 0; int.TryParse(dt.Rows[i]["PaymentType"].ToString(), out PaymentType); D.PaymentType = PaymentType;
                float AmountToPay = 0; float.TryParse(dt.Rows[i]["AmountToPay"].ToString(), out AmountToPay); D.AmountToPay = AmountToPay;
                int BankID = 0; int.TryParse(dt.Rows[i]["BankID"].ToString(), out BankID); D.BankID = BankID;
                DateTime ChequeDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["ChequeDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ChequeDate);
                D.ChequeDate = ChequeDate;
                D.Narration = dt.Rows[i]["Narration"].ToString();
                int OtherBookingID = 0; int.TryParse(dt.Rows[i]["OtherBookingID"].ToString(), out OtherBookingID); D.OtherBookingID = OtherBookingID;
                float OtherBookingTotalBalance = 0; float.TryParse(dt.Rows[i]["OtherBookingTotalBalance"].ToString(), out OtherBookingTotalBalance); D.OtherBookingTotalBalance = OtherBookingTotalBalance;
                float OtherBookingNewBalance = 0; float.TryParse(dt.Rows[i]["OtherBookingNewBalance"].ToString(), out OtherBookingNewBalance); D.OtherBookingNewBalance = OtherBookingNewBalance;
                DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                D.AddedDate = AddedDate;
                DateTime ModifyDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["ModifyDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ModifyDate);
                D.ModifyDate = ModifyDate;
                int AddedBy = 0; int.TryParse(dt.Rows[i]["AddedBy"].ToString(), out AddedBy); D.AddedBy = AddedBy;
                int ModifyBy = 0; int.TryParse(dt.Rows[i]["ModifyBy"].ToString(), out ModifyBy); D.ModifyBy = ModifyBy;
                int DeductBookingID = 0; int.TryParse(dt.Rows[i]["DeductBookingID"].ToString(), out DeductBookingID); 
                D.DeductBookingID = DeductBookingID;
                D.ChequeTxnNo = dt.Rows[i]["ChequeTxnNo"].ToString();
                D.BankBranch = dt.Rows[i]["BankBranch"].ToString();
                int BulkBookingID = 0; int.TryParse(dt.Rows[i]["BulkBookingID"].ToString(), out BulkBookingID);
                D.BulkBookingID = BulkBookingID;
            }
            return D;
        }

        public int AppointmentPaidEdit_Delete(int _appointmentID)
        {
            SqlCommand cmd = new SqlCommand("AppoinmentPaidEdit_AppointmentSession_Delete"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;

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

        public DataSet Hospital_And_Patient(int _appointmentID)
        {
            SqlCommand cmd = new SqlCommand("Hospital_And_Patient_Ledger"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            DataSet DS = new DataSet();
            DS = db.DbFetch(cmd);
            return DS;
        }

        public void Update_HospitalLeger(int ledgerheadid)
        {
            SqlCommand cmd = new SqlCommand("UPDATE HospitalLedger SET IsDeleted=CAST('True'AS BIT) where LedgerHeadID=@LedgerHeadID");
            cmd.Parameters.Add("@LedgerHeadID", SqlDbType.Int).Value = ledgerheadid;
            int i = 0;
            i = db.DbUpdate(cmd);
        }

        public void Update_PatientLeger(int ledgerheadid)
        {
            SqlCommand cmd = new SqlCommand("UPDATE PatientLedger SET IsDeleted=CAST('True'AS BIT) where LedgerHeadID=@LedgerHeadID");
            cmd.Parameters.Add("@LedgerHeadID", SqlDbType.Int).Value = ledgerheadid;
            int i = 0;
            i = db.DbUpdate(cmd);
        }

        public int DeleteDoctorPayment(int _appointmentID)
        {
            SqlCommand cmd = new SqlCommand("DoctorLedger_DeleteAppointment"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;

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

        public int UpdateOtherPayment(int _appointmentID)
        {
            SqlCommand cmd = new SqlCommand("PatientLedger_UpdateNewOtherSession"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;

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
