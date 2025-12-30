using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace SnehBLL
{
    public class PatientSessionPackage_Bll
    {
        DbHelper.SqlDb db;

        public PatientSessionPackage_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public SnehDLL.PatientSessionPackage_Dll Get(int _bookingID)
        {
            SnehDLL.PatientSessionPackage_Dll D = null;
            SqlCommand cmd = new SqlCommand("PatientPackage_GetForSessionEntrySingle"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@BookingID", SqlDbType.Int).Value = _bookingID;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                D = new SnehDLL.PatientSessionPackage_Dll(); int i = 0;
                D.BookingID = int.Parse(dt.Rows[i]["BookingID"].ToString());
                D.PackageCode = dt.Rows[i]["PackageCode"].ToString();
                float AppointmentCharge = 0; float.TryParse(dt.Rows[i]["AppointmentCharge"].ToString(), out AppointmentCharge); D.AppointmentCharge = AppointmentCharge;
                int AppointmentCount = 0; int.TryParse(dt.Rows[i]["AppointmentCount"].ToString(), out AppointmentCount); D.AppointmentCount = AppointmentCount;
                float PackageAmount = 0; float.TryParse(dt.Rows[i]["PackageAmount"].ToString(), out PackageAmount); D.PackageAmount = PackageAmount;
                float PackageBalance = 0; float.TryParse(dt.Rows[i]["PackageBalance"].ToString(), out PackageBalance); D.PackageBalance = PackageBalance;
                int ValidityDays = 0; int.TryParse(dt.Rows[i]["ValidityDays"].ToString(), out ValidityDays); D.ValidityDays = ValidityDays;
                float OneTimeAmt = 0; float.TryParse(dt.Rows[i]["OneTimeAmt"].ToString(), out OneTimeAmt); D.OneTimeAmt = OneTimeAmt;
                int MaximumTime = 0; int.TryParse(dt.Rows[i]["MaximumTime"].ToString(), out MaximumTime); D.MaximumTime = MaximumTime;
            }
            return D;
        }

        public List<SnehDLL.PatientSessionPackage_Dll> GetList(int _sessionID, int _patientID)
        {
            List<SnehDLL.PatientSessionPackage_Dll> DL = new List<SnehDLL.PatientSessionPackage_Dll>();
            SqlCommand cmd = new SqlCommand("PatientPackage_GetForSessionEntryAll"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@SessionID", SqlDbType.Int).Value = _sessionID;
            cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = _patientID;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.PatientSessionPackage_Dll D = new SnehDLL.PatientSessionPackage_Dll();
                D.BookingID = int.Parse(dt.Rows[i]["BookingID"].ToString());
                D.PackageCode = dt.Rows[i]["PackageCode"].ToString();
                float AppointmentCharge = 0; float.TryParse(dt.Rows[i]["AppointmentCharge"].ToString(), out AppointmentCharge); D.AppointmentCharge = AppointmentCharge;
                int AppointmentCount = 0; int.TryParse(dt.Rows[i]["AppointmentCount"].ToString(), out AppointmentCount); D.AppointmentCount = AppointmentCount;
                float PackageAmount = 0; float.TryParse(dt.Rows[i]["PackageAmount"].ToString(), out PackageAmount); D.PackageAmount = PackageAmount;
                float PackageBalance = 0; float.TryParse(dt.Rows[i]["PackageBalance"].ToString(), out PackageBalance); D.PackageBalance = PackageBalance;
                int ValidityDays = 0; int.TryParse(dt.Rows[i]["ValidityDays"].ToString(), out ValidityDays); D.ValidityDays = ValidityDays;
                float OneTimeAmt = 0; float.TryParse(dt.Rows[i]["OneTimeAmt"].ToString(), out OneTimeAmt); D.OneTimeAmt = OneTimeAmt;
                int MaximumTime = 0; int.TryParse(dt.Rows[i]["MaximumTime"].ToString(), out MaximumTime); D.MaximumTime = MaximumTime;

                DL.Add(D);
            }
            return DL;
        }

        public List<SnehDLL.PatientSessionPackage_Dll> GetList(int _patientID)
        {
            List<SnehDLL.PatientSessionPackage_Dll> DL = new List<SnehDLL.PatientSessionPackage_Dll>();
            SqlCommand cmd = new SqlCommand("PatientPackage_GetForSessionEntryOther"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = _patientID;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.PatientSessionPackage_Dll D = new SnehDLL.PatientSessionPackage_Dll();
                D.BookingID = int.Parse(dt.Rows[i]["BookingID"].ToString());
                D.PackageCode = dt.Rows[i]["PackageCode"].ToString();
                float AppointmentCharge = 0; float.TryParse(dt.Rows[i]["AppointmentCharge"].ToString(), out AppointmentCharge); D.AppointmentCharge = AppointmentCharge;
                int AppointmentCount = 0; int.TryParse(dt.Rows[i]["AppointmentCount"].ToString(), out AppointmentCount); D.AppointmentCount = AppointmentCount;
                float PackageAmount = 0; float.TryParse(dt.Rows[i]["PackageAmount"].ToString(), out PackageAmount); D.PackageAmount = PackageAmount;
                float PackageBalance = 0; float.TryParse(dt.Rows[i]["PackageBalance"].ToString(), out PackageBalance); D.PackageBalance = PackageBalance;
                int ValidityDays = 0; int.TryParse(dt.Rows[i]["ValidityDays"].ToString(), out ValidityDays); D.ValidityDays = ValidityDays;
                float OneTimeAmt = 0; float.TryParse(dt.Rows[i]["OneTimeAmt"].ToString(), out OneTimeAmt); D.OneTimeAmt = OneTimeAmt;
                int MaximumTime = 0; int.TryParse(dt.Rows[i]["MaximumTime"].ToString(), out MaximumTime); D.MaximumTime = MaximumTime;

                DL.Add(D);
            }
            return DL;
        }

        public List<SnehDLL.PatientSessionPackage_Dll> GetList_ForEdit(int AppointmentID, int _sessionID, int _patientID)
        {
            List<SnehDLL.PatientSessionPackage_Dll> DL = new List<SnehDLL.PatientSessionPackage_Dll>();
            SqlCommand cmd = new SqlCommand("PatientPackage_GetForSessionEntryAllEdit"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = AppointmentID;
            cmd.Parameters.Add("@SessionID", SqlDbType.Int).Value = _sessionID;
            cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = _patientID;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.PatientSessionPackage_Dll D = new SnehDLL.PatientSessionPackage_Dll();
                D.BookingID = int.Parse(dt.Rows[i]["BookingID"].ToString());
                D.PackageCode = dt.Rows[i]["PackageCode"].ToString();
                float AppointmentCharge = 0; float.TryParse(dt.Rows[i]["AppointmentCharge"].ToString(), out AppointmentCharge); D.AppointmentCharge = AppointmentCharge;
                int AppointmentCount = 0; int.TryParse(dt.Rows[i]["AppointmentCount"].ToString(), out AppointmentCount); D.AppointmentCount = AppointmentCount;
                float PackageAmount = 0; float.TryParse(dt.Rows[i]["PackageAmount"].ToString(), out PackageAmount); D.PackageAmount = PackageAmount;
                float PackageBalance = 0; float.TryParse(dt.Rows[i]["PackageBalance"].ToString(), out PackageBalance); D.PackageBalance = PackageBalance;
                int ValidityDays = 0; int.TryParse(dt.Rows[i]["ValidityDays"].ToString(), out ValidityDays); D.ValidityDays = ValidityDays;
                float OneTimeAmt = 0; float.TryParse(dt.Rows[i]["OneTimeAmt"].ToString(), out OneTimeAmt); D.OneTimeAmt = OneTimeAmt;
                int MaximumTime = 0; int.TryParse(dt.Rows[i]["MaximumTime"].ToString(), out MaximumTime); D.MaximumTime = MaximumTime;

                DL.Add(D);
            }
            return DL;
        }

        public SnehDLL.PatientSessionPackage_Dll Get_ForEdit(int AppointmentID, int _bookingID)
        {
            SnehDLL.PatientSessionPackage_Dll D = null;
            SqlCommand cmd = new SqlCommand("PatientPackage_GetForSessionEntrySingleEdit"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = AppointmentID;
            cmd.Parameters.Add("@BookingID", SqlDbType.Int).Value = _bookingID;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                D = new SnehDLL.PatientSessionPackage_Dll(); int i = 0;
                D.BookingID = int.Parse(dt.Rows[i]["BookingID"].ToString());
                D.PackageCode = dt.Rows[i]["PackageCode"].ToString();
                float AppointmentCharge = 0; float.TryParse(dt.Rows[i]["AppointmentCharge"].ToString(), out AppointmentCharge); D.AppointmentCharge = AppointmentCharge;
                int AppointmentCount = 0; int.TryParse(dt.Rows[i]["AppointmentCount"].ToString(), out AppointmentCount); D.AppointmentCount = AppointmentCount;
                float PackageAmount = 0; float.TryParse(dt.Rows[i]["PackageAmount"].ToString(), out PackageAmount); D.PackageAmount = PackageAmount;
                float PackageBalance = 0; float.TryParse(dt.Rows[i]["PackageBalance"].ToString(), out PackageBalance); D.PackageBalance = PackageBalance;
                int ValidityDays = 0; int.TryParse(dt.Rows[i]["ValidityDays"].ToString(), out ValidityDays); D.ValidityDays = ValidityDays;
                float OneTimeAmt = 0; float.TryParse(dt.Rows[i]["OneTimeAmt"].ToString(), out OneTimeAmt); D.OneTimeAmt = OneTimeAmt;
                int MaximumTime = 0; int.TryParse(dt.Rows[i]["MaximumTime"].ToString(), out MaximumTime); D.MaximumTime = MaximumTime;
            }
            return D;
        }

        public List<SnehDLL.PatientSessionPackage_Dll> GetList_ForEdit(int AppointmentID, int _patientID)
        {
            List<SnehDLL.PatientSessionPackage_Dll> DL = new List<SnehDLL.PatientSessionPackage_Dll>();
            SqlCommand cmd = new SqlCommand("PatientPackage_GetForSessionEntryOtherEdit"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = AppointmentID;
            cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = _patientID;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.PatientSessionPackage_Dll D = new SnehDLL.PatientSessionPackage_Dll();
                D.BookingID = int.Parse(dt.Rows[i]["BookingID"].ToString());
                D.PackageCode = dt.Rows[i]["PackageCode"].ToString();
                float AppointmentCharge = 0; float.TryParse(dt.Rows[i]["AppointmentCharge"].ToString(), out AppointmentCharge); D.AppointmentCharge = AppointmentCharge;
                int AppointmentCount = 0; int.TryParse(dt.Rows[i]["AppointmentCount"].ToString(), out AppointmentCount); D.AppointmentCount = AppointmentCount;
                float PackageAmount = 0; float.TryParse(dt.Rows[i]["PackageAmount"].ToString(), out PackageAmount); D.PackageAmount = PackageAmount;
                float PackageBalance = 0; float.TryParse(dt.Rows[i]["PackageBalance"].ToString(), out PackageBalance); D.PackageBalance = PackageBalance;
                int ValidityDays = 0; int.TryParse(dt.Rows[i]["ValidityDays"].ToString(), out ValidityDays); D.ValidityDays = ValidityDays;
                float OneTimeAmt = 0; float.TryParse(dt.Rows[i]["OneTimeAmt"].ToString(), out OneTimeAmt); D.OneTimeAmt = OneTimeAmt;
                int MaximumTime = 0; int.TryParse(dt.Rows[i]["MaximumTime"].ToString(), out MaximumTime); D.MaximumTime = MaximumTime;

                DL.Add(D);
            }
            return DL;
        }

        public List<SnehDLL.PatientSessionPackage_Dll> GetList(int AppointmentID, int _sessionID, int _patientID)
        {
            List<SnehDLL.PatientSessionPackage_Dll> DL = new List<SnehDLL.PatientSessionPackage_Dll>();
            SqlCommand cmd = new SqlCommand("PatientPackage_GetForSessionEntryAll"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = AppointmentID;
            cmd.Parameters.Add("@SessionID", SqlDbType.Int).Value = _sessionID;
            cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = _patientID;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.PatientSessionPackage_Dll D = new SnehDLL.PatientSessionPackage_Dll();
                D.BookingID = int.Parse(dt.Rows[i]["BookingID"].ToString());
                D.PackageCode = dt.Rows[i]["PackageCode"].ToString();
                float AppointmentCharge = 0; float.TryParse(dt.Rows[i]["AppointmentCharge"].ToString(), out AppointmentCharge); D.AppointmentCharge = AppointmentCharge;
                int AppointmentCount = 0; int.TryParse(dt.Rows[i]["AppointmentCount"].ToString(), out AppointmentCount); D.AppointmentCount = AppointmentCount;
                float PackageAmount = 0; float.TryParse(dt.Rows[i]["PackageAmount"].ToString(), out PackageAmount); D.PackageAmount = PackageAmount;
                float PackageBalance = 0; float.TryParse(dt.Rows[i]["PackageBalance"].ToString(), out PackageBalance); D.PackageBalance = PackageBalance;
                int ValidityDays = 0; int.TryParse(dt.Rows[i]["ValidityDays"].ToString(), out ValidityDays); D.ValidityDays = ValidityDays;
                float OneTimeAmt = 0; float.TryParse(dt.Rows[i]["OneTimeAmt"].ToString(), out OneTimeAmt); D.OneTimeAmt = OneTimeAmt;
                int MaximumTime = 0; int.TryParse(dt.Rows[i]["MaximumTime"].ToString(), out MaximumTime); D.MaximumTime = MaximumTime;

                DL.Add(D);
            }
            return DL;
        }
    }
}
