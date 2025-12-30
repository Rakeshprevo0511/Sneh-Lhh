using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace SnehBLL
{
    public class Packages_Bll
    {
        DbHelper.SqlDb db;

        public Packages_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public static int Check(string _uniqueID)
        {
            int _packageID = 0;
            SqlCommand cmd = new SqlCommand("Packages_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = _uniqueID;
            cmd.Parameters.Add("@PackageID", SqlDbType.Int).Value = 0;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["PackageID"].ToString(), out _packageID);
            }
            return _packageID;
        }

        public static string Check(int _packageID)
        {
            string _uniqueID = "";
            SqlCommand cmd = new SqlCommand("Packages_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = "";
            cmd.Parameters.Add("@PackageID", SqlDbType.Int).Value = _packageID;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                _uniqueID = dt.Rows[0]["UniqueID"].ToString();
            }
            return _uniqueID;
        }

        public SnehDLL.Packages_Dll Get(int _packageID)
        {
            SnehDLL.Packages_Dll D = null;
            SqlCommand cmd = new SqlCommand("Packages_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PackageID", SqlDbType.Int).Value = _packageID;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                D = new SnehDLL.Packages_Dll(); int i = 0;
                D.PackageID = int.Parse(dt.Rows[i]["PackageID"].ToString());
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                D.PackageCode = dt.Rows[i]["PackageCode"].ToString();
                D.cDescription = dt.Rows[i]["cDescription"].ToString();
                int PatientTypeID = 0; int.TryParse(dt.Rows[i]["PatientTypeID"].ToString(), out PatientTypeID); D.PatientTypeID = PatientTypeID;
                int CategoryID = 0; int.TryParse(dt.Rows[i]["CategoryID"].ToString(), out CategoryID); D.CategoryID = CategoryID;
                float PackageAmt = 0; float.TryParse(dt.Rows[i]["PackageAmt"].ToString(), out PackageAmt); D.PackageAmt = PackageAmt;
                float OneTimeAmt = 0; float.TryParse(dt.Rows[i]["OneTimeAmt"].ToString(), out OneTimeAmt); D.OneTimeAmt = OneTimeAmt;
                int Appointments = 0; int.TryParse(dt.Rows[i]["Appointments"].ToString(), out Appointments); D.Appointments = Appointments;
                int ValidityDays = 0; int.TryParse(dt.Rows[i]["ValidityDays"].ToString(), out ValidityDays); D.ValidityDays = ValidityDays;
                int MaximumTime = 0; int.TryParse(dt.Rows[i]["MaximumTime"].ToString(), out MaximumTime); D.MaximumTime = MaximumTime;

            }
            return D;
        }

        public List<SnehDLL.Packages_Dll> GetSessionPackage(int _sessionID, int _patientID)
        {
            List<SnehDLL.Packages_Dll> DL = new List<SnehDLL.Packages_Dll>();
            SqlCommand cmd = new SqlCommand("Packages_SessionPackages"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@SessionID", SqlDbType.Int).Value = _sessionID;
            cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = _patientID;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.Packages_Dll D = new SnehDLL.Packages_Dll();
                D.PackageID = int.Parse(dt.Rows[i]["PackageID"].ToString());
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                D.PackageCode = dt.Rows[i]["PackageCode"].ToString();
                D.cDescription = dt.Rows[i]["cDescription"].ToString();
                int PatientTypeID = 0; int.TryParse(dt.Rows[i]["PatientTypeID"].ToString(), out PatientTypeID); D.PatientTypeID = PatientTypeID;
                int CategoryID = 0; int.TryParse(dt.Rows[i]["CategoryID"].ToString(), out CategoryID); D.CategoryID = CategoryID;
                float PackageAmt = 0; float.TryParse(dt.Rows[i]["PackageAmt"].ToString(), out PackageAmt); D.PackageAmt = PackageAmt;
                float OneTimeAmt = 0; float.TryParse(dt.Rows[i]["OneTimeAmt"].ToString(), out OneTimeAmt); D.OneTimeAmt = OneTimeAmt;
                int Appointments = 0; int.TryParse(dt.Rows[i]["Appointments"].ToString(), out Appointments); D.Appointments = Appointments;
                int ValidityDays = 0; int.TryParse(dt.Rows[i]["ValidityDays"].ToString(), out ValidityDays); D.ValidityDays = ValidityDays;
                int MaximumTime = 0; int.TryParse(dt.Rows[i]["MaximumTime"].ToString(), out MaximumTime); D.MaximumTime = MaximumTime;

                DL.Add(D);
            }
            return DL;
        }

        public DataTable Search(string _search)
        {
            SqlCommand cmd = new SqlCommand("Packages_Search"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PackageCode", SqlDbType.VarChar, 50).Value = _search;

            return db.DbRead(cmd);
        }

        public DataTable Search(string _search, int _sessionID)
        {
            SqlCommand cmd = new SqlCommand("Packages_SessionWiseSearch"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PackageCode", SqlDbType.VarChar, 50).Value = _search;
            cmd.Parameters.Add("@SessionID", SqlDbType.Int).Value = _sessionID;

            return db.DbRead(cmd);
        }

        public int Set(SnehDLL.Packages_Dll D)
        {
            SqlCommand cmd = new SqlCommand("Packages_Set"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PackageID", SqlDbType.Int).Value = D.PackageID;
            cmd.Parameters.Add("@PackageCode", SqlDbType.VarChar, 50).Value = D.PackageCode;
            cmd.Parameters.Add("@cDescription", SqlDbType.VarChar, 250).Value = D.cDescription;
            cmd.Parameters.Add("@PatientTypeID", SqlDbType.Int).Value = D.PatientTypeID;
            cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = D.CategoryID;
            cmd.Parameters.Add("@PackageAmt", SqlDbType.Decimal).Value = D.PackageAmt;
            cmd.Parameters.Add("@OneTimeAmt", SqlDbType.Decimal).Value = D.OneTimeAmt;
            cmd.Parameters.Add("@Appointments", SqlDbType.Int).Value = D.Appointments;
            cmd.Parameters.Add("@ValidityDays", SqlDbType.Int).Value = D.ValidityDays;
            cmd.Parameters.Add("@MaximumTime", SqlDbType.Int).Value = D.MaximumTime;

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

        public int Delete(int _packageID)
        {
            SqlCommand cmd = new SqlCommand("Packages_Delete"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PackageID", SqlDbType.Int).Value = _packageID;

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

        public bool IsUsed(int _packageID)
        {
            SqlCommand cmd = new SqlCommand("Packages_IsUsed"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PackageID", SqlDbType.Int).Value = _packageID;

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

        public bool IsCodeAvailable(int _packageID, string _codeNo)
        {
            SqlCommand cmd = new SqlCommand("Packages_CodeChk"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PackageID", SqlDbType.Int).Value = _packageID;
            cmd.Parameters.Add("@PackageCode", SqlDbType.VarChar, 50).Value = _codeNo;

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

        public int Revise(SnehDLL.Packages_Dll D)
        {
            SqlCommand cmd = new SqlCommand("Packages_Revise"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PackageID", SqlDbType.Int).Value = D.PackageID;
            //cmd.Parameters.Add("@PackageCode", SqlDbType.VarChar, 50).Value = D.PackageCode;
            //cmd.Parameters.Add("@cDescription", SqlDbType.VarChar, 250).Value = D.cDescription;
            //cmd.Parameters.Add("@PatientTypeID", SqlDbType.Int).Value = D.PatientTypeID;
            //cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = D.CategoryID;
            cmd.Parameters.Add("@PackageAmt", SqlDbType.Decimal).Value = D.PackageAmt;
            cmd.Parameters.Add("@OneTimeAmt", SqlDbType.Decimal).Value = D.OneTimeAmt;
            cmd.Parameters.Add("@Appointments", SqlDbType.Int).Value = D.Appointments;
            cmd.Parameters.Add("@ValidityDays", SqlDbType.Int).Value = D.ValidityDays;
            cmd.Parameters.Add("@MaximumTime", SqlDbType.Int).Value = D.MaximumTime;

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

        public int Toggle(int PackageID)
        {
            SqlCommand cmd = new SqlCommand("UPDATE Packages SET IsEnabled = CASE WHEN COALESCE(IsEnabled, CAST('True' AS BIT)) = CAST('True' AS BIT) THEN CAST('False' AS BIT) ELSE CAST('True' AS BIT) END WHERE PackageID = @PackageID"); cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@PackageID", SqlDbType.Int).Value = PackageID;

            return db.DbUpdate(cmd);
        }

        public List<SnehDLL.Packages_Dll> GetSessionPackage(int AppointmentID, int _sessionID, int _patientID)
        {
            List<SnehDLL.Packages_Dll> DL = new List<SnehDLL.Packages_Dll>();
            SqlCommand cmd = new SqlCommand("Packages_SessionPackages"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = AppointmentID;
            cmd.Parameters.Add("@SessionID", SqlDbType.Int).Value = _sessionID;
            cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = _patientID;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.Packages_Dll D = new SnehDLL.Packages_Dll();
                D.PackageID = int.Parse(dt.Rows[i]["PackageID"].ToString());
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                D.PackageCode = dt.Rows[i]["PackageCode"].ToString();
                D.cDescription = dt.Rows[i]["cDescription"].ToString();
                int PatientTypeID = 0; int.TryParse(dt.Rows[i]["PatientTypeID"].ToString(), out PatientTypeID); D.PatientTypeID = PatientTypeID;
                int CategoryID = 0; int.TryParse(dt.Rows[i]["CategoryID"].ToString(), out CategoryID); D.CategoryID = CategoryID;
                float PackageAmt = 0; float.TryParse(dt.Rows[i]["PackageAmt"].ToString(), out PackageAmt); D.PackageAmt = PackageAmt;
                float OneTimeAmt = 0; float.TryParse(dt.Rows[i]["OneTimeAmt"].ToString(), out OneTimeAmt); D.OneTimeAmt = OneTimeAmt;
                int Appointments = 0; int.TryParse(dt.Rows[i]["Appointments"].ToString(), out Appointments); D.Appointments = Appointments;
                int ValidityDays = 0; int.TryParse(dt.Rows[i]["ValidityDays"].ToString(), out ValidityDays); D.ValidityDays = ValidityDays;
                int MaximumTime = 0; int.TryParse(dt.Rows[i]["MaximumTime"].ToString(), out MaximumTime); D.MaximumTime = MaximumTime;

                DL.Add(D);
            }
            return DL;
        }

        public List<SnehDLL.Packages_Dll> GetSessionPackageNewFilter(int AppointmentID, int _sessionID, int _patientID)
        {
            List<SnehDLL.Packages_Dll> DL = new List<SnehDLL.Packages_Dll>();
            SqlCommand cmd = new SqlCommand("Packages_SessionPackagesNewFilter"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = AppointmentID;
            cmd.Parameters.Add("@SessionID", SqlDbType.Int).Value = _sessionID;
            cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = _patientID;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.Packages_Dll D = new SnehDLL.Packages_Dll();
                D.PackageID = int.Parse(dt.Rows[i]["PackageID"].ToString());
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                D.PackageCode = dt.Rows[i]["PackageCode"].ToString();
                D.cDescription = dt.Rows[i]["cDescription"].ToString();
                int PatientTypeID = 0; int.TryParse(dt.Rows[i]["PatientTypeID"].ToString(), out PatientTypeID); D.PatientTypeID = PatientTypeID;
                int CategoryID = 0; int.TryParse(dt.Rows[i]["CategoryID"].ToString(), out CategoryID); D.CategoryID = CategoryID;
                float PackageAmt = 0; float.TryParse(dt.Rows[i]["PackageAmt"].ToString(), out PackageAmt); D.PackageAmt = PackageAmt;
                float OneTimeAmt = 0; float.TryParse(dt.Rows[i]["OneTimeAmt"].ToString(), out OneTimeAmt); D.OneTimeAmt = OneTimeAmt;
                int Appointments = 0; int.TryParse(dt.Rows[i]["Appointments"].ToString(), out Appointments); D.Appointments = Appointments;
                int ValidityDays = 0; int.TryParse(dt.Rows[i]["ValidityDays"].ToString(), out ValidityDays); D.ValidityDays = ValidityDays;
                int MaximumTime = 0; int.TryParse(dt.Rows[i]["MaximumTime"].ToString(), out MaximumTime); D.MaximumTime = MaximumTime;

                DL.Add(D);
            }
            return DL;
        }
    }
}