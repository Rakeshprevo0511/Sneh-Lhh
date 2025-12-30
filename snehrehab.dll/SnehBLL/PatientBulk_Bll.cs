using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SnehBLL
{
    public class PatientBulk_Bll
    {
        DbHelper.SqlDb db;

        public PatientBulk_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public static long Check(string UniqueID)
        {
            long BulkID = 0;
            SqlCommand cmd = new SqlCommand("PatientBulk_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = UniqueID;
            cmd.Parameters.Add("@BulkID", SqlDbType.Int).Value = 0;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                long.TryParse(dt.Rows[0]["BulkID"].ToString(), out BulkID);
            }
            return BulkID;
        }

        public static string Check(long BulkID)
        {
            string UniqueID = "";
            SqlCommand cmd = new SqlCommand("PatientBulk_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = "";
            cmd.Parameters.Add("@BulkID", SqlDbType.Int).Value = BulkID;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                UniqueID = dt.Rows[0]["UniqueID"].ToString();
            }
            return UniqueID;
        }

        public long Set(SnehDLL.PatientBulk_Dll D)
        {
            SqlCommand cmd = new SqlCommand("PatientBulk_Set"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@BulkID", SqlDbType.Int).Value = D.BulkID;
            cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = D.PatientID;
            cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = D.Amount;
            cmd.Parameters.Add("@ModePayment", SqlDbType.Int).Value = D.ModePayment;
            if (D.PaidDate > DateTime.MinValue)
                cmd.Parameters.Add("@PaidDate", SqlDbType.DateTime).Value = D.PaidDate;
            else
                cmd.Parameters.Add("@PaidDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@BankID", SqlDbType.Int).Value = D.BankID;
            cmd.Parameters.Add("@BankBranch", SqlDbType.NVarChar, 500).Value = D.BankBranch;
            cmd.Parameters.Add("@ChequeTxnNo", SqlDbType.NVarChar, 500).Value = D.ChequeTxnNo;
            if (D.ChequeDate > DateTime.MinValue)
                cmd.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = D.ChequeDate;
            else
                cmd.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@Narration", SqlDbType.VarChar, 4000).Value = D.Narration;
            cmd.Parameters.Add("@ModifyBy", SqlDbType.Int).Value = D.ModifyBy;
            cmd.Parameters.Add("@IsPackage", SqlDbType.Bit).Value = D.IsPackage;

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

        public DataTable Search(string _searchText, int ModePayment, DateTime _fromDate, DateTime _uptoDate)
        {
            SqlCommand cmd = new SqlCommand("PatientBulk_Search"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 50).Value = _searchText;
            cmd.Parameters.Add("@ModePayment", SqlDbType.Int).Value = ModePayment;
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

        public DataSet Data(long BulkID)
        {
            SqlCommand cmd = new SqlCommand("PatientBulk_Data"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@BulkID", SqlDbType.BigInt).Value = BulkID;
            return db.DbFetch(cmd);
        }

        public bool IsUsed(long BulkID)
        {
            SqlCommand cmd = new SqlCommand("PatientBulk_IsUsed"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@BulkID", SqlDbType.BigInt).Value = BulkID;

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

        public long Delete(long BulkID)
        {
            SqlCommand cmd = new SqlCommand("PatientBulk_Delete"); cmd.CommandType = CommandType.StoredProcedure;
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

        public List<SnehDLL.PatientBulk_Dll> ListPackage(int _patientID)
        {
            List<SnehDLL.PatientBulk_Dll> DL = new List<SnehDLL.PatientBulk_Dll>();
            SqlCommand cmd = new SqlCommand("PatientBulk_GetForSessionEntry"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = _patientID;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.PatientBulk_Dll D = new SnehDLL.PatientBulk_Dll();
                D.BulkID = long.Parse(dt.Rows[i]["BulkID"].ToString());
                float Amount = 0; float.TryParse(dt.Rows[i]["Amount"].ToString(), out Amount); D.Amount = Amount;
                DateTime PaidDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["PaidDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out PaidDate);
                D.PaidDate = PaidDate;
                DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                D.AddedDate = AddedDate;
                int ModePayment = 0; int.TryParse(dt.Rows[i]["ModePayment"].ToString(), out ModePayment); D.ModePayment = ModePayment;
                D.ChequeTxnNo = dt.Rows[i]["ChequeTxnNo"].ToString();
                float BalanceAmount = 0; float.TryParse(dt.Rows[i]["BalanceAmount"].ToString(), out BalanceAmount); D.BalanceAmount = BalanceAmount;

                DL.Add(D);
            }
            return DL;
        }

        public List<SnehDLL.PatientBulk_Dll> ListPackage_ForEdit(int _patientID, int AppointmentID)
        {
            List<SnehDLL.PatientBulk_Dll> DL = new List<SnehDLL.PatientBulk_Dll>();
            SqlCommand cmd = new SqlCommand("PatientBulk_GetForSessionEntryEdit"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = _patientID;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = AppointmentID;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.PatientBulk_Dll D = new SnehDLL.PatientBulk_Dll();
                D.BulkID = long.Parse(dt.Rows[i]["BulkID"].ToString());
                float Amount = 0; float.TryParse(dt.Rows[i]["Amount"].ToString(), out Amount); D.Amount = Amount;
                DateTime PaidDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["PaidDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out PaidDate);
                D.PaidDate = PaidDate;
                DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                D.AddedDate = AddedDate;
                int ModePayment = 0; int.TryParse(dt.Rows[i]["ModePayment"].ToString(), out ModePayment); D.ModePayment = ModePayment;
                D.ChequeTxnNo = dt.Rows[i]["ChequeTxnNo"].ToString();
                float BalanceAmount = 0; float.TryParse(dt.Rows[i]["BalanceAmount"].ToString(), out BalanceAmount); D.BalanceAmount = BalanceAmount;

                DL.Add(D);
            }
            return DL;
        }

        public long SetBulkPackage(SnehDLL.PatientBulkPackage_Dll D)
        {
            SqlCommand cmd = new SqlCommand("PatientBulkPackage_Set"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@BookingID", SqlDbType.BigInt).Value = D.BookingID;
            cmd.Parameters.Add("@BulkID", SqlDbType.Int).Value = D.BulkID;
            cmd.Parameters.Add("@SessionID", SqlDbType.Int).Value = D.SessionID;
            cmd.Parameters.Add("@PackageID", SqlDbType.Int).Value = D.PackageID;
            cmd.Parameters.Add("@AppointmentCharge", SqlDbType.Decimal).Value = D.AppointmentCharge;
            cmd.Parameters.Add("@AppointmentCount", SqlDbType.Int).Value = D.AppointmentCount;
            cmd.Parameters.Add("@PackageAmount", SqlDbType.Decimal).Value = D.PackageAmount;

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

        public DataTable GetBulkPackage(long BulkID)
        {
            SqlCommand cmd = new SqlCommand("PatientBulkPackage_GetSessionID"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@BulkID", SqlDbType.Int).Value = BulkID;
            DataTable dt = db.DbRead(cmd);
            return dt;
        }

        public int UpdateBulkPackage(int SessionID, int PackageID, long booking, float onetimeamount, int appointmentcount)
        {
            SqlCommand cmd = new SqlCommand("UPDATE PatientBulkPkg SET SessionID=@SessionID,AppointmentCharge=@AppointmentCharge,AppointmentCount=@AppointmentCount WHERE PackageID=@PackageID AND BookingID=@BookingID"); cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@PackageID", SqlDbType.Int).Value = PackageID;
            cmd.Parameters.Add("@SessionID", SqlDbType.Int).Value = SessionID;
            cmd.Parameters.Add("@BookingID", SqlDbType.BigInt).Value = booking;
            cmd.Parameters.Add("@AppointmentCharge", SqlDbType.Decimal).Value = onetimeamount;
            cmd.Parameters.Add("@AppointmentCount", SqlDbType.Int).Value = appointmentcount;
            int i = db.DbUpdate(cmd);
            return i;
        }

        public SnehDLL.PatientBulkPackage_Dll GetPatientBulkPackage(long bulkid)
        {
            SnehDLL.PatientBulkPackage_Dll PBD = new SnehDLL.PatientBulkPackage_Dll();
            SqlCommand cmd = new SqlCommand("PatientBulkPackage_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@BulkID", SqlDbType.Int).Value = bulkid;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                int BulkId = 0; int.TryParse(dt.Rows[0]["BulkID"].ToString(), out BulkId);
                float AppointmentCharge = 0; float.TryParse(dt.Rows[0]["AppointmentCharge"].ToString(), out AppointmentCharge);
                int AppointmentCount = 0; int.TryParse(dt.Rows[0]["AppointmentCount"].ToString(), out AppointmentCount);
                float PackageAmount = 0; float.TryParse(dt.Rows[0]["PackageAmount"].ToString(), out PackageAmount);
                int PatientID = 0; int.TryParse(dt.Rows[0]["PatientID"].ToString(), out PatientID);
                int bankid = 0; int.TryParse(dt.Rows[0]["BankID"].ToString(), out bankid);
                int ModePayment = 0; int.TryParse(dt.Rows[0]["ModePayment"].ToString(), out ModePayment);
                bool ispackage = false; bool.TryParse(dt.Rows[0]["IsPackage"].ToString(), out ispackage);
                PBD.BulkID = BulkId;
                PBD.AppointmentCharge = AppointmentCharge;
                PBD.AppointmentCount = AppointmentCount;
                PBD.PackageAmount = PackageAmount;
                PBD.PatientID = PatientID;
                PBD.BankID = bankid;
                PBD.ModePayment = ModePayment;
                PBD.BankBranch = dt.Rows[0]["BankBranch"].ToString();
                PBD.ChequeTxnNo = dt.Rows[0]["ChequeTxnNo"].ToString();
                DateTime ChequeDate = new DateTime(); DateTime.TryParseExact(dt.Rows[0]["ChequeDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ChequeDate);
                DateTime PaidDate = new DateTime(); DateTime.TryParseExact(dt.Rows[0]["PaidDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out PaidDate);
                PBD.PaidDate = PaidDate;
                PBD.ChequeDate = ChequeDate;
                PBD.Narration = dt.Rows[0]["Narration"].ToString();
                PBD.IsPackage = ispackage;
            }
            return PBD;
        }

        public List<SnehDLL.PatientBulkPackage_Dll> GetSessionList(long bulkid)
        {
            List<SnehDLL.PatientBulkPackage_Dll> PBD = new List<SnehDLL.PatientBulkPackage_Dll>();
            SqlCommand cmd = new SqlCommand("PatientBulkPackageSession_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@BulkID", SqlDbType.Int).Value = bulkid;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.PatientBulkPackage_Dll PD = new SnehDLL.PatientBulkPackage_Dll();
                int sessionid = 0; int.TryParse(dt.Rows[i]["SessionID"].ToString(), out sessionid);
                int packageid = 0; int.TryParse(dt.Rows[i]["PackageID"].ToString(), out packageid);
                PD.SessionID = sessionid;
                PD.PackageID = packageid;
                PBD.Add(PD);
            }
            return PBD;
        }

        public int DeletePatientBulkPackage(long bulkid)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM PatientBulkPkg WHERE BulkID=@BulkID"); cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@BulkID", SqlDbType.Int).Value = bulkid;
            int i = db.DbUpdate(cmd);
            return i;
        }

        public long DeletePatientHospitalLedger(long bulkid, int patientid, int modepayment)
        {
            SqlCommand cmd = new SqlCommand("DeletePatienTLedger_BulkPak"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@BulkID", SqlDbType.Int).Value = bulkid;
            cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = patientid;
            cmd.Parameters.Add("@ModePayment", SqlDbType.Int).Value = modepayment;

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

        public SnehDLL.PatientBulk_Dll GetPatientBulk(long bulkid)
        {
            SnehDLL.PatientBulk_Dll PBD = new SnehDLL.PatientBulk_Dll();
            SqlCommand cmd = new SqlCommand("PatientBulk_GetNew"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@BulkID", SqlDbType.Int).Value = bulkid;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                int BulkId = 0; int.TryParse(dt.Rows[0]["BulkID"].ToString(), out BulkId);
                float Amount = 0; float.TryParse(dt.Rows[0]["Amount"].ToString(), out Amount);
                int PatientID = 0; int.TryParse(dt.Rows[0]["PatientID"].ToString(), out PatientID);
                int bankid = 0; int.TryParse(dt.Rows[0]["BankID"].ToString(), out bankid);
                int ModePayment = 0; int.TryParse(dt.Rows[0]["ModePayment"].ToString(), out ModePayment);
                bool ispackage = false; bool.TryParse(dt.Rows[0]["IsPackage"].ToString(), out ispackage);
                PBD.BulkID = BulkId;
                PBD.UniqueID = dt.Rows[0]["UniqueID"].ToString();
                PBD.PatientID = PatientID;
                PBD.Amount = Amount;
                PBD.BankID = bankid;
                PBD.ModePayment = ModePayment;
                PBD.BankBranch = dt.Rows[0]["BankBranch"].ToString();
                PBD.ChequeTxnNo = dt.Rows[0]["ChequeTxnNo"].ToString();
                DateTime ChequeDate = new DateTime(); DateTime.TryParseExact(dt.Rows[0]["ChequeDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ChequeDate);
                DateTime PAIDDATE = new DateTime(); DateTime.TryParseExact(dt.Rows[0]["PAIDDATE"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out PAIDDATE);
                PBD.PaidDate = PAIDDATE;
                PBD.ChequeDate = ChequeDate;
                PBD.Narration = dt.Rows[0]["Narration"].ToString();
                PBD.IsPackage = ispackage;
            }
            return PBD;
        }

        public bool GetIspackage(long bulkid)
        {
            SqlCommand cmd = new SqlCommand("SELECT COALESCE(IsPackage,'False') AS IsPackage from PatientBulk where BulkID=@BulkID"); cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@BulkID", SqlDbType.Int).Value = bulkid;
            DataTable dt = db.DbRead(cmd);
            bool IsPackage = false;
            if (dt.Rows.Count > 0)
            {
                bool.TryParse(dt.Rows[0]["IsPackage"].ToString(), out IsPackage);
            }
            return IsPackage;
        }

        public bool GetIspackageByPatientID(int patientid)
        {
            SqlCommand cmd = new SqlCommand("SELECT COALESCE(IsPackage,'False') AS IsPackage from PatientBulk where PatientID=@PatientID"); cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = patientid;
            DataTable dt = db.DbRead(cmd);
            bool IsPackage = false;
            if (dt.Rows.Count > 0)
            {
                bool.TryParse(dt.Rows[0]["IsPackage"].ToString(), out IsPackage);
            }
            return IsPackage;
        }

        public List<SnehDLL.PatientBulkPackage_Dll> GetPatientBulkPackageNew(int BulkId)
        {
            List<SnehDLL.PatientBulkPackage_Dll> PB = new List<SnehDLL.PatientBulkPackage_Dll>();
            SqlCommand cmd = new SqlCommand("PatientBulkPackage_GetList"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@BulkID", SqlDbType.Int).Value = BulkId;
            DataTable dt = db.DbRead(cmd);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.PatientBulkPackage_Dll PBD = new SnehDLL.PatientBulkPackage_Dll();
                int BookingID = 0; int.TryParse(dt.Rows[i]["BookingID"].ToString(), out BookingID);
                int BulkID = 0; int.TryParse(dt.Rows[i]["BulkID"].ToString(), out BulkID);
                int SessionID = 0; int.TryParse(dt.Rows[i]["SessionID"].ToString(), out SessionID);
                int PackageID = 0; int.TryParse(dt.Rows[i]["PackageID"].ToString(), out PackageID);
                float AppointmentCharge = 0; float.TryParse(dt.Rows[i]["AppointmentCharge"].ToString(), out AppointmentCharge);
                int AppointmentCount = 0; int.TryParse(dt.Rows[i]["AppointmentCount"].ToString(), out AppointmentCount);
                float PackageAmount = 0; float.TryParse(dt.Rows[i]["PackageAmount"].ToString(), out PackageAmount);
                int UsedAppointmentCount = 0; int.TryParse(dt.Rows[i]["UsedAppointmentCount"].ToString(), out UsedAppointmentCount);
                float UsedAppointmentCharge = 0; float.TryParse(dt.Rows[i]["UsedAppointmentCharge"].ToString(), out UsedAppointmentCharge);
                bool ispackage = false; bool.TryParse(dt.Rows[0]["IsPackage"].ToString(), out ispackage);
                PBD.BookingID = BookingID;
                PBD.BulkID = BulkID;
                PBD.SessionID = SessionID;
                PBD.PackageID = PackageID;
                PBD.AppointmentCharge = AppointmentCharge;
                PBD.AppointmentCount = AppointmentCount;
                PBD.PackageAmount = PackageAmount;
                PBD.UsedAppointmentCount = UsedAppointmentCount;
                PBD.UsedAppointmentCharge = UsedAppointmentCharge;
                PBD.PackageCode = dt.Rows[i]["PackageCode"].ToString();
                PBD.IsPackage = ispackage;
                PB.Add(PBD);
            }

            return PB;
        }

        public List<SnehDLL.PatientBulkPackage_Dll> GetPatientBulkPackageFull(int BulkId)
        {
            List<SnehDLL.PatientBulkPackage_Dll> PB = new List<SnehDLL.PatientBulkPackage_Dll>();
            SqlCommand cmd = new SqlCommand("PatientBulkPackage_GetListNew"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@BulkID", SqlDbType.Int).Value = BulkId;
            DataTable dt = db.DbRead(cmd);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.PatientBulkPackage_Dll PBD = new SnehDLL.PatientBulkPackage_Dll();
                int BookingID = 0; int.TryParse(dt.Rows[i]["BookingID"].ToString(), out BookingID);
                int BulkID = 0; int.TryParse(dt.Rows[i]["BulkID"].ToString(), out BulkID);
                int SessionID = 0; int.TryParse(dt.Rows[i]["SessionID"].ToString(), out SessionID);
                int PackageID = 0; int.TryParse(dt.Rows[i]["PackageID"].ToString(), out PackageID);
                float AppointmentCharge = 0; float.TryParse(dt.Rows[i]["AppointmentCharge"].ToString(), out AppointmentCharge);
                int AppointmentCount = 0; int.TryParse(dt.Rows[i]["AppointmentCount"].ToString(), out AppointmentCount);
                float PackageAmount = 0; float.TryParse(dt.Rows[i]["PackageAmount"].ToString(), out PackageAmount);
                int UsedAppointmentCount = 0; int.TryParse(dt.Rows[i]["UsedAppointmentCount"].ToString(), out UsedAppointmentCount);
                float UsedAppointmentCharge = 0; float.TryParse(dt.Rows[i]["UsedAppointmentCharge"].ToString(), out UsedAppointmentCharge);
                bool ispackage = false; bool.TryParse(dt.Rows[0]["IsPackage"].ToString(), out ispackage);
                PBD.BookingID = BookingID;
                PBD.BulkID = BulkID;
                PBD.SessionID = SessionID;
                PBD.PackageID = PackageID;
                PBD.AppointmentCharge = AppointmentCharge;
                PBD.AppointmentCount = AppointmentCount;
                PBD.PackageAmount = PackageAmount;
                PBD.UsedAppointmentCount = UsedAppointmentCount;
                PBD.UsedAppointmentCharge = UsedAppointmentCharge;
                PBD.PackageCode = dt.Rows[i]["PackageCode"].ToString();
                PBD.IsPackage = ispackage;
                PB.Add(PBD);
            }

            return PB;
        }

        public SnehDLL.PatientBulkPackage_Dll GetPatientBulkPackageByPackage(int BulkId, int PackageID)
        {
            SnehDLL.PatientBulkPackage_Dll PBD = new SnehDLL.PatientBulkPackage_Dll();
            SqlCommand cmd = new SqlCommand("PatientBulkPackage_GetBy_Package"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@BulkID", SqlDbType.Int).Value = BulkId;
            cmd.Parameters.Add("@PackageID", SqlDbType.Int).Value = PackageID;
            DataTable dt = db.DbRead(cmd);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int BookingID = 0; int.TryParse(dt.Rows[i]["BookingID"].ToString(), out BookingID);
                int BulkID = 0; int.TryParse(dt.Rows[i]["BulkID"].ToString(), out BulkID);
                int SessionID = 0; int.TryParse(dt.Rows[i]["SessionID"].ToString(), out SessionID);
                int packageID = 0; int.TryParse(dt.Rows[i]["PackageID"].ToString(), out packageID);
                float AppointmentCharge = 0; float.TryParse(dt.Rows[i]["AppointmentCharge"].ToString(), out AppointmentCharge);
                int AppointmentCount = 0; int.TryParse(dt.Rows[i]["AppointmentCount"].ToString(), out AppointmentCount);
                float PackageAmount = 0; float.TryParse(dt.Rows[i]["PackageAmount"].ToString(), out PackageAmount);
                int UsedAppointmentCount = 0; int.TryParse(dt.Rows[i]["UsedAppointmentCount"].ToString(), out UsedAppointmentCount);
                float UsedAppointmentCharge = 0; float.TryParse(dt.Rows[i]["UsedAppointmentCharge"].ToString(), out UsedAppointmentCharge);
                PBD.BookingID = BookingID;
                PBD.BulkID = BulkID;
                PBD.SessionID = SessionID;
                PBD.PackageID = packageID;
                PBD.AppointmentCharge = AppointmentCharge;
                PBD.AppointmentCount = AppointmentCount;
                PBD.PackageAmount = PackageAmount;
                PBD.UsedAppointmentCount = UsedAppointmentCount;
                PBD.UsedAppointmentCharge = UsedAppointmentCharge;
            }

            return PBD;
        }

        public SnehDLL.PatientBulkPackage_Dll GetPatientBulkPackageByPackageNew(int BulkId, int PackageID)
        {
            SnehDLL.PatientBulkPackage_Dll PBD = new SnehDLL.PatientBulkPackage_Dll();
            SqlCommand cmd = new SqlCommand("PatientBulkPackage_GetBy_PackageNew"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@BulkID", SqlDbType.Int).Value = BulkId;
            cmd.Parameters.Add("@PackageID", SqlDbType.Int).Value = PackageID;
            DataTable dt = db.DbRead(cmd);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int BookingID = 0; int.TryParse(dt.Rows[i]["BookingID"].ToString(), out BookingID);
                int BulkID = 0; int.TryParse(dt.Rows[i]["BulkID"].ToString(), out BulkID);
                int SessionID = 0; int.TryParse(dt.Rows[i]["SessionID"].ToString(), out SessionID);
                int packageID = 0; int.TryParse(dt.Rows[i]["PackageID"].ToString(), out packageID);
                float AppointmentCharge = 0; float.TryParse(dt.Rows[i]["AppointmentCharge"].ToString(), out AppointmentCharge);
                int AppointmentCount = 0; int.TryParse(dt.Rows[i]["AppointmentCount"].ToString(), out AppointmentCount);
                float PackageAmount = 0; float.TryParse(dt.Rows[i]["PackageAmount"].ToString(), out PackageAmount);
                float UsedAppointmentCount = 0; float.TryParse(dt.Rows[i]["UsedAppointmentCount"].ToString(), out UsedAppointmentCount);
                float UsedAppointmentCharge = 0; float.TryParse(dt.Rows[i]["UsedAppointmentCharge"].ToString(), out UsedAppointmentCharge);
                int maximumtime = 0; int.TryParse(dt.Rows[i]["MaximumTime"].ToString(), out maximumtime);
                float onetimeamt = 0; float.TryParse(dt.Rows[i]["OneTimeAmt"].ToString(), out onetimeamt);
                PBD.BookingID = BookingID;
                PBD.BulkID = BulkID;
                PBD.SessionID = SessionID;
                PBD.PackageID = packageID;
                PBD.AppointmentCharge = AppointmentCharge;
                PBD.AppointmentCount = AppointmentCount;
                PBD.PackageAmount = PackageAmount;
                PBD.UsedAppointmentCount = UsedAppointmentCount;
                PBD.UsedAppointmentCharge = UsedAppointmentCharge;
                PBD.MaximumTime = maximumtime;
                PBD.OneTimeAmt = onetimeamt;
            }

            return PBD;
        }

        public int UpdatePatientBulkPackage(float UsedAppointmentCharge, double UsedAppointmentCount, long BookingID)
        {
            SqlCommand cmd = new SqlCommand("UPDATE PatientBulkPkg SET UsedAppointmentCharge=@UsedAppointmentCharge,UsedAppointmentCount=@UsedAppointmentCount WHERE BookingID=@BookingID"); cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@UsedAppointmentCharge", SqlDbType.Decimal).Value = UsedAppointmentCharge;
            cmd.Parameters.Add("@UsedAppointmentCount", SqlDbType.Decimal).Value = UsedAppointmentCount;
            cmd.Parameters.Add("@BookingID", SqlDbType.BigInt).Value = BookingID;
            int i = db.DbUpdate(cmd);
            return i;
        }

        public DataTable GetSessionTime(int _appointmentID)
        {
            SnehDLL.PatientBulkPackage_Dll PBD = new SnehDLL.PatientBulkPackage_Dll();
            SqlCommand cmd = new SqlCommand("Get_SessionTime"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            DataTable dt = db.DbRead(cmd);
            return dt;
        }

        public DataTable GetSingleBulkPackage(long bulkid, int packageid)
        {
            SnehDLL.PatientBulkPackage_Dll PBD = new SnehDLL.PatientBulkPackage_Dll();
            SqlCommand cmd = new SqlCommand("PatientBulkPackage_GetSingle"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@BulkID", SqlDbType.Int).Value = bulkid;
            cmd.Parameters.Add("@PackageID", SqlDbType.Int).Value = packageid;
            DataTable dt = db.DbRead(cmd);
            return dt;
        }

        public float GetBalAmount(long bulkid)
        {
            SqlCommand cmd = new SqlCommand("SELECT SUM(UsedAppointmentCharge)AS BalAmount FROM PatientBulkPkg WHERE BulkID=@BulkID"); cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@BulkID", SqlDbType.Decimal).Value = bulkid;
            DataTable dt = db.DbRead(cmd);
            float BalAmount = 0;
            if (dt.Rows.Count > 0)
            {
                float.TryParse(dt.Rows[0]["BalAmount"].ToString(), out BalAmount);
            }
            return BalAmount;
        }
    }
}
