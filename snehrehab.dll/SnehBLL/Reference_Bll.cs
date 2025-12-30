using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SnehBLL
{
    public class Reference_Bll
    {
        DbHelper.SqlDb db;

        public Reference_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public List<SnehDLL.Reference_Dll> Get_Reference_DrList()
        {
            List<SnehDLL.Reference_Dll> RD = new List<SnehDLL.Reference_Dll>();
            SqlCommand cmd = new SqlCommand("Get_Refence_DoctorList"); cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SnehDLL.Reference_Dll RDN = new SnehDLL.Reference_Dll();
                    int DoctorID = 0; int.TryParse(dt.Rows[i]["DoctorID"].ToString(), out DoctorID);
                    RDN.DoctorID = DoctorID;
                    RDN.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                    RDN.Prefix = dt.Rows[i]["Prefix"].ToString();
                    RDN.Name = dt.Rows[i]["Name"].ToString();
                    RDN.MobileNo = dt.Rows[i]["MobileNo"].ToString();
                    RDN.EmailID = dt.Rows[i]["EmailID"].ToString();
                    RDN.Website = dt.Rows[i]["Website"].ToString();
                    RDN.Address = dt.Rows[i]["Address"].ToString();
                    DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                    RDN.AddedDate = AddedDate;
                    int ReferredBy = 0; int.TryParse(dt.Rows[i]["ReferredBy"].ToString(), out ReferredBy);
                    RDN.ReferredBy = ReferredBy;
                    RD.Add(RDN);
                }
            }
            return RD;
        }

        public List<SnehDLL.Reference_Dll> Get_Reference_HospitalList()
        {
            List<SnehDLL.Reference_Dll> RD = new List<SnehDLL.Reference_Dll>();
            SqlCommand cmd = new SqlCommand("Get_Refence_HospitalList"); cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SnehDLL.Reference_Dll RDN = new SnehDLL.Reference_Dll();
                    int HospitalID = 0; int.TryParse(dt.Rows[i]["HospitalID"].ToString(), out HospitalID);
                    RDN.HospitalID = HospitalID;
                    RDN.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                    RDN.Name = dt.Rows[i]["Name"].ToString();
                    RDN.MobileNo = dt.Rows[i]["MobileNo"].ToString();
                    RDN.EmailID = dt.Rows[i]["EmailID"].ToString();
                    RDN.Website = dt.Rows[i]["Website"].ToString();
                    RDN.Address = dt.Rows[i]["Address"].ToString();
                    DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                    RDN.AddedDate = AddedDate;
                    int ReferredBy = 0; int.TryParse(dt.Rows[i]["ReferredBy"].ToString(), out ReferredBy);
                    RDN.ReferredBy = ReferredBy;
                    RD.Add(RDN);
                }
            }
            return RD;
        }

        public List<SnehDLL.Reference_Dll> Get_Reference_OnlineList()
        {
            List<SnehDLL.Reference_Dll> RD = new List<SnehDLL.Reference_Dll>();
            SqlCommand cmd = new SqlCommand("Get_Refence_OnlineList"); cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SnehDLL.Reference_Dll RDN = new SnehDLL.Reference_Dll();
                    int OnlineID = 0; int.TryParse(dt.Rows[i]["OnlineID"].ToString(), out OnlineID);
                    RDN.OnlineID = OnlineID;
                    RDN.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                    RDN.Name = dt.Rows[i]["Name"].ToString();
                    RDN.MobileNo = dt.Rows[i]["MobileNo"].ToString();
                    RDN.EmailID = dt.Rows[i]["EmailID"].ToString();
                    RDN.Website = dt.Rows[i]["Website"].ToString();
                    RDN.Address = dt.Rows[i]["Address"].ToString();
                    DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                    RDN.AddedDate = AddedDate;
                    int ReferredBy = 0; int.TryParse(dt.Rows[i]["ReferredBy"].ToString(), out ReferredBy);
                    RDN.ReferredBy = ReferredBy;
                    RD.Add(RDN);
                }
            }
            return RD;
        }

        public List<SnehDLL.Reference_Dll> Get_Reference_OtherList()
        {
            List<SnehDLL.Reference_Dll> RD = new List<SnehDLL.Reference_Dll>();
            SqlCommand cmd = new SqlCommand("Get_Refence_OtherList"); cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SnehDLL.Reference_Dll RDN = new SnehDLL.Reference_Dll();
                    int OtherID = 0; int.TryParse(dt.Rows[i]["OtherID"].ToString(), out OtherID);
                    RDN.OtherID = OtherID;
                    RDN.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                    RDN.Name = dt.Rows[i]["Name"].ToString();
                    RDN.MobileNo = dt.Rows[i]["MobileNo"].ToString();
                    RDN.EmailID = dt.Rows[i]["EmailID"].ToString();
                    RDN.Website = dt.Rows[i]["Website"].ToString();
                    RDN.Address = dt.Rows[i]["Address"].ToString();
                    DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                    RDN.AddedDate = AddedDate;
                    int ReferredBy = 0; int.TryParse(dt.Rows[i]["ReferredBy"].ToString(), out ReferredBy);
                    RDN.ReferredBy = ReferredBy;
                    RD.Add(RDN);
                }
            }
            return RD;
        }

        public List<SnehDLL.Reference_Dll> Get_Reference_SchoolList()
        {
            List<SnehDLL.Reference_Dll> RD = new List<SnehDLL.Reference_Dll>();
            SqlCommand cmd = new SqlCommand("Get_Refence_SchoolList"); cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SnehDLL.Reference_Dll RDN = new SnehDLL.Reference_Dll();
                    int SchoolID = 0; int.TryParse(dt.Rows[i]["SchoolID"].ToString(), out SchoolID);
                    RDN.SchoolID = SchoolID;
                    RDN.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                    RDN.Name = dt.Rows[i]["Name"].ToString();
                    RDN.MobileNo = dt.Rows[i]["MobileNo"].ToString();
                    RDN.EmailID = dt.Rows[i]["EmailID"].ToString();
                    RDN.Website = dt.Rows[i]["Website"].ToString();
                    RDN.Address = dt.Rows[i]["Address"].ToString();
                    DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                    RDN.AddedDate = AddedDate;
                    int ReferredBy = 0; int.TryParse(dt.Rows[i]["ReferredBy"].ToString(), out ReferredBy);
                    RDN.ReferredBy = ReferredBy;
                    RD.Add(RDN);
                }
            }
            return RD;
        }

        public List<SnehDLL.Reference_Dll> Get_Reference_TeacherList()
        {
            List<SnehDLL.Reference_Dll> RD = new List<SnehDLL.Reference_Dll>();
            SqlCommand cmd = new SqlCommand("Get_Refence_TeacherList"); cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SnehDLL.Reference_Dll RDN = new SnehDLL.Reference_Dll();
                    int TeacherID = 0; int.TryParse(dt.Rows[i]["TeacherID"].ToString(), out TeacherID);
                    RDN.TeacherID = TeacherID;
                    RDN.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                    RDN.Name = dt.Rows[i]["Name"].ToString();
                    RDN.MobileNo = dt.Rows[i]["MobileNo"].ToString();
                    RDN.EmailID = dt.Rows[i]["EmailID"].ToString();
                    RDN.Website = dt.Rows[i]["Website"].ToString();
                    RDN.Address = dt.Rows[i]["Address"].ToString();
                    DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                    RDN.AddedDate = AddedDate;
                    int ReferredBy = 0; int.TryParse(dt.Rows[i]["ReferredBy"].ToString(), out ReferredBy);
                    RDN.ReferredBy = ReferredBy;
                    RD.Add(RDN);
                }
            }
            return RD;
        }

        public int Set_RefernceDr(SnehDLL.Reference_Dll RD)
        {
            SqlCommand cmd = new SqlCommand("Set_Dr_Reference"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = RD.DoctorID;
            cmd.Parameters.Add("@Prefix", SqlDbType.VarChar, 50).Value = RD.Prefix;
            cmd.Parameters.Add("@Name", SqlDbType.VarChar, 250).Value = RD.Name;
            cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar, 50).Value = RD.MobileNo;
            cmd.Parameters.Add("@EmailID", SqlDbType.VarChar, 500).Value = RD.EmailID;
            cmd.Parameters.Add("@Website", SqlDbType.VarChar, 500).Value = RD.Website;
            cmd.Parameters.Add("@Address", SqlDbType.VarChar, 500).Value = RD.Address;
            if (RD.AddedDate > DateTime.MinValue)
                cmd.Parameters.Add("@AddedDate", SqlDbType.DateTime).Value = RD.AddedDate;
            else
                cmd.Parameters.Add("@AddedDate", SqlDbType.DateTime).Value = DBNull.Value;

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

        public int Set_RefernceSchool(SnehDLL.Reference_Dll RD)
        {
            SqlCommand cmd = new SqlCommand("Set_School_Reference"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@SchoolID", SqlDbType.Int).Value = RD.SchoolID;
            cmd.Parameters.Add("@Name", SqlDbType.VarChar, 250).Value = RD.Name;
            cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar, 50).Value = RD.MobileNo;
            cmd.Parameters.Add("@EmailID", SqlDbType.VarChar, 500).Value = RD.EmailID;
            cmd.Parameters.Add("@Website", SqlDbType.VarChar, 500).Value = RD.Website;
            cmd.Parameters.Add("@Address", SqlDbType.VarChar, 500).Value = RD.Address;
            if (RD.AddedDate > DateTime.MinValue)
                cmd.Parameters.Add("@AddedDate", SqlDbType.DateTime).Value = RD.AddedDate;
            else
                cmd.Parameters.Add("@AddedDate", SqlDbType.DateTime).Value = DBNull.Value;

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

        public int Set_RefernceHospital(SnehDLL.Reference_Dll RD)
        {
            SqlCommand cmd = new SqlCommand("Set_Hospital_Reference"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@HospitalID", SqlDbType.Int).Value = RD.HospitalID;
            cmd.Parameters.Add("@Name", SqlDbType.VarChar, 250).Value = RD.Name;
            cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar, 50).Value = RD.MobileNo;
            cmd.Parameters.Add("@EmailID", SqlDbType.VarChar, 500).Value = RD.EmailID;
            cmd.Parameters.Add("@Website", SqlDbType.VarChar, 500).Value = RD.Website;
            cmd.Parameters.Add("@Address", SqlDbType.VarChar, 500).Value = RD.Address;
            if (RD.AddedDate > DateTime.MinValue)
                cmd.Parameters.Add("@AddedDate", SqlDbType.DateTime).Value = RD.AddedDate;
            else
                cmd.Parameters.Add("@AddedDate", SqlDbType.DateTime).Value = DBNull.Value;

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

        public int Set_RefernceTeacher(SnehDLL.Reference_Dll RD)
        {
            SqlCommand cmd = new SqlCommand("Set_Teacher_Reference"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@TeacherID", SqlDbType.Int).Value = RD.TeacherID;
            cmd.Parameters.Add("@Name", SqlDbType.VarChar, 250).Value = RD.Name;
            cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar, 50).Value = RD.MobileNo;
            cmd.Parameters.Add("@EmailID", SqlDbType.VarChar, 500).Value = RD.EmailID;
            cmd.Parameters.Add("@Website", SqlDbType.VarChar, 500).Value = RD.Website;
            cmd.Parameters.Add("@Address", SqlDbType.VarChar, 500).Value = RD.Address;
            if (RD.AddedDate > DateTime.MinValue)
                cmd.Parameters.Add("@AddedDate", SqlDbType.DateTime).Value = RD.AddedDate;
            else
                cmd.Parameters.Add("@AddedDate", SqlDbType.DateTime).Value = DBNull.Value;

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

        public int Set_RefernceOther(SnehDLL.Reference_Dll RD)
        {
            SqlCommand cmd = new SqlCommand("Set_Other_Reference"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@OtherID", SqlDbType.Int).Value = RD.OtherID;
            cmd.Parameters.Add("@Name", SqlDbType.VarChar, 250).Value = RD.Name;
            cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar, 50).Value = RD.MobileNo;
            cmd.Parameters.Add("@EmailID", SqlDbType.VarChar, 500).Value = RD.EmailID;
            cmd.Parameters.Add("@Website", SqlDbType.VarChar, 500).Value = RD.Website;
            cmd.Parameters.Add("@Address", SqlDbType.VarChar, 500).Value = RD.Address;
            if (RD.AddedDate > DateTime.MinValue)
                cmd.Parameters.Add("@AddedDate", SqlDbType.DateTime).Value = RD.AddedDate;
            else
                cmd.Parameters.Add("@AddedDate", SqlDbType.DateTime).Value = DBNull.Value;

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

        public int Set_RefernceOnlie(SnehDLL.Reference_Dll RD)
        {
            SqlCommand cmd = new SqlCommand("Set_Online_Reference"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@OnlineID", SqlDbType.Int).Value = RD.OnlineID;
            cmd.Parameters.Add("@Name", SqlDbType.VarChar, 250).Value = RD.Name;
            cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar, 50).Value = RD.MobileNo;
            cmd.Parameters.Add("@EmailID", SqlDbType.VarChar, 500).Value = RD.EmailID;
            cmd.Parameters.Add("@Website", SqlDbType.VarChar, 500).Value = RD.Website;
            cmd.Parameters.Add("@Address", SqlDbType.VarChar, 500).Value = RD.Address;
            if (RD.AddedDate > DateTime.MinValue)
                cmd.Parameters.Add("@AddedDate", SqlDbType.DateTime).Value = RD.AddedDate;
            else
                cmd.Parameters.Add("@AddedDate", SqlDbType.DateTime).Value = DBNull.Value;

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

        public DataTable Get_RefDR(int doctorid)
        {
            SqlCommand cmd = new SqlCommand("Get_Refence_Doctor"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = doctorid;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SnehDLL.Reference_Dll RDN = new SnehDLL.Reference_Dll();
                    int DoctorID = 0; int.TryParse(dt.Rows[i]["DoctorID"].ToString(), out DoctorID);
                    RDN.DoctorID = DoctorID;
                    RDN.Prefix = dt.Rows[i]["Prefix"].ToString();
                    RDN.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                    RDN.Name = dt.Rows[i]["Name"].ToString();
                    RDN.MobileNo = dt.Rows[i]["MobileNo"].ToString();
                    RDN.EmailID = dt.Rows[i]["EmailID"].ToString();
                    RDN.Website = dt.Rows[i]["Website"].ToString();
                    RDN.Address = dt.Rows[i]["Address"].ToString();
                    DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                    RDN.AddedDate = AddedDate;
                    int ReferredBy = 0; int.TryParse(dt.Rows[i]["ReferredBy"].ToString(), out ReferredBy);
                    RDN.ReferredBy = ReferredBy;
                }
            }
            return dt;
        }

        public DataTable Get_RefSchool(int schoolid)
        {
            SqlCommand cmd = new SqlCommand("Get_Refence_School"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@SchoolID", SqlDbType.Int).Value = schoolid;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SnehDLL.Reference_Dll RDN = new SnehDLL.Reference_Dll();
                    int SchoolID = 0; int.TryParse(dt.Rows[i]["SchoolID"].ToString(), out SchoolID);
                    RDN.SchoolID = SchoolID;
                    RDN.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                    RDN.Name = dt.Rows[i]["Name"].ToString();
                    RDN.MobileNo = dt.Rows[i]["MobileNo"].ToString();
                    RDN.EmailID = dt.Rows[i]["EmailID"].ToString();
                    RDN.Website = dt.Rows[i]["Website"].ToString();
                    RDN.Address = dt.Rows[i]["Address"].ToString();
                    DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                    RDN.AddedDate = AddedDate;
                    int ReferredBy = 0; int.TryParse(dt.Rows[i]["ReferredBy"].ToString(), out ReferredBy);
                    RDN.ReferredBy = ReferredBy;
                }
            }
            return dt;
        }

        public DataTable Get_RefTeacher(int teacherID)
        {
            SqlCommand cmd = new SqlCommand("Get_Refence_Teacher"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@TeacherID", SqlDbType.Int).Value = teacherID;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SnehDLL.Reference_Dll RDN = new SnehDLL.Reference_Dll();
                    int TeacherID = 0; int.TryParse(dt.Rows[i]["TeacherID"].ToString(), out TeacherID);
                    RDN.TeacherID = TeacherID;
                    RDN.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                    RDN.Name = dt.Rows[i]["Name"].ToString();
                    RDN.MobileNo = dt.Rows[i]["MobileNo"].ToString();
                    RDN.EmailID = dt.Rows[i]["EmailID"].ToString();
                    RDN.Website = dt.Rows[i]["Website"].ToString();
                    RDN.Address = dt.Rows[i]["Address"].ToString();
                    DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                    RDN.AddedDate = AddedDate;
                    int ReferredBy = 0; int.TryParse(dt.Rows[i]["ReferredBy"].ToString(), out ReferredBy);
                    RDN.ReferredBy = ReferredBy;
                }
            }
            return dt;
        }

        public DataTable Get_RefHospital(int hospitalid)
        {
            SqlCommand cmd = new SqlCommand("Get_Refence_Hospital"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@HospitalID", SqlDbType.Int).Value = hospitalid;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SnehDLL.Reference_Dll RDN = new SnehDLL.Reference_Dll();
                    int HospitalID = 0; int.TryParse(dt.Rows[i]["HospitalID"].ToString(), out HospitalID);
                    RDN.HospitalID = HospitalID;
                    RDN.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                    RDN.Name = dt.Rows[i]["Name"].ToString();
                    RDN.MobileNo = dt.Rows[i]["MobileNo"].ToString();
                    RDN.EmailID = dt.Rows[i]["EmailID"].ToString();
                    RDN.Website = dt.Rows[i]["Website"].ToString();
                    RDN.Address = dt.Rows[i]["Address"].ToString();
                    DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                    RDN.AddedDate = AddedDate;
                    int ReferredBy = 0; int.TryParse(dt.Rows[i]["ReferredBy"].ToString(), out ReferredBy);
                    RDN.ReferredBy = ReferredBy;
                }
            }
            return dt;
        }

        public DataTable Get_RefOther(int otherid)
        {
            SqlCommand cmd = new SqlCommand("Get_Refence_Other"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@OtherID", SqlDbType.Int).Value = otherid;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SnehDLL.Reference_Dll RDN = new SnehDLL.Reference_Dll();
                    int OtherID = 0; int.TryParse(dt.Rows[i]["OtherID"].ToString(), out OtherID);
                    RDN.OtherID = OtherID;
                    RDN.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                    RDN.Name = dt.Rows[i]["Name"].ToString();
                    RDN.MobileNo = dt.Rows[i]["MobileNo"].ToString();
                    RDN.EmailID = dt.Rows[i]["EmailID"].ToString();
                    RDN.Website = dt.Rows[i]["Website"].ToString();
                    RDN.Address = dt.Rows[i]["Address"].ToString();
                    DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                    RDN.AddedDate = AddedDate;
                    int ReferredBy = 0; int.TryParse(dt.Rows[i]["ReferredBy"].ToString(), out ReferredBy);
                    RDN.ReferredBy = ReferredBy;
                }
            }
            return dt;
        }

        public DataTable Get_RefOnline(int onlineid)
        {
            SqlCommand cmd = new SqlCommand("Get_Refence_Online"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@OnlineID", SqlDbType.Int).Value = onlineid;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SnehDLL.Reference_Dll RDN = new SnehDLL.Reference_Dll();
                    int OnlineID = 0; int.TryParse(dt.Rows[i]["OnlineID"].ToString(), out OnlineID);
                    RDN.OnlineID = OnlineID;
                    RDN.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                    RDN.Name = dt.Rows[i]["Name"].ToString();
                    RDN.MobileNo = dt.Rows[i]["MobileNo"].ToString();
                    RDN.EmailID = dt.Rows[i]["EmailID"].ToString();
                    RDN.Website = dt.Rows[i]["Website"].ToString();
                    RDN.Address = dt.Rows[i]["Address"].ToString();
                    DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                    RDN.AddedDate = AddedDate;
                    int ReferredBy = 0; int.TryParse(dt.Rows[i]["ReferredBy"].ToString(), out ReferredBy);
                    RDN.ReferredBy = ReferredBy;
                }
            }
            return dt;
        }

        public int DeleteRef(int ReferredBy, int Ref_Selected)
        {
            SqlCommand cmd = new SqlCommand("Delete_ReferenceAll"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ReferredBy", SqlDbType.Int).Value = ReferredBy;
            cmd.Parameters.Add("@Ref_Selected", SqlDbType.Int).Value = Ref_Selected;

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

        public DataTable Search(int ReferredBy, int Ref_Selected, string _searchText, DateTime _fromDate, DateTime _uptoDate)
        {
            SqlCommand cmd = new SqlCommand("Ref_RepotsAll"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ReferredBy", SqlDbType.Int).Value = ReferredBy;
            cmd.Parameters.Add("@Ref_Selected", SqlDbType.Int).Value = Ref_Selected;
            if (_fromDate > DateTime.MinValue)
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = _fromDate;
            else
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = DBNull.Value;
            if (_uptoDate > DateTime.MinValue)
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = _uptoDate;
            else
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 50).Value = _searchText;

            return db.DbRead(cmd);
        }

        public SnehDLL.Reference_Dll GetRef_Single(int referedby, int referselected)
        {
            SnehDLL.Reference_Dll RD = new SnehDLL.Reference_Dll();
            SqlCommand cmd = new SqlCommand("Ref_Repotrts_Single"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ReferredBy", SqlDbType.Int).Value = referedby;
            cmd.Parameters.Add("@Ref_Selected", SqlDbType.Int).Value = referselected;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                int referedid = 0; int refselectid = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int.TryParse(dt.Rows[i]["ReferredID"].ToString(), out referedid);
                    int.TryParse(dt.Rows[i]["Reference_Selected"].ToString(), out refselectid);
                    RD.ReferredBy = referedid;
                    RD.Ref_SelectID = refselectid;
                    RD.Name = dt.Rows[i]["Name"].ToString();
                    RD.MobileNo = dt.Rows[i]["MobileNo"].ToString();
                    RD.EmailID = dt.Rows[i]["EmailID"].ToString();
                    RD.Website = dt.Rows[i]["Website"].ToString();
                    RD.Address = dt.Rows[i]["Address"].ToString();
                    DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                    RD.AddedDate = AddedDate;
                }

            }
            return RD;
        }


    }
}
