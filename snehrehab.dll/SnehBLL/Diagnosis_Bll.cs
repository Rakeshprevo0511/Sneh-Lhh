using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SnehBLL
{
    public class Diagnosis_Bll
    {
        DbHelper.SqlDb db;

        public Diagnosis_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public List<SnehDLL.Diagnosis_Dll> Dropdown()
        {
            List<SnehDLL.Diagnosis_Dll> DL = new List<SnehDLL.Diagnosis_Dll>();
            SqlCommand cmd = new SqlCommand("Diagnosis_Dropdown"); cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.Diagnosis_Dll D = new SnehDLL.Diagnosis_Dll();
                D.DiagnosisID = long.Parse(dt.Rows[i]["DiagnosisID"].ToString());
                D.dName = dt.Rows[i]["dName"].ToString();

                DL.Add(D);
            }
            return DL;
        }

        public int setFromOther(string DiagnosisIDs, string DiagnosisOther, int PatientID)
        {
            //SqlCommand cmd = new SqlCommand("UPDATE PATIENTMAST SET DIAGNOSISOTHER = @DiagnosisOther, DiagnosisID = @DiagnosisIDs WHERE PATIENTID = @PatientID");
            //cmd.Parameters.Add("@DiagnosisIDs", SqlDbType.NVarChar).Value = DiagnosisIDs;
            //cmd.Parameters.Add("@DiagnosisOther", SqlDbType.NVarChar, 500).Value = DiagnosisOther;
            //cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = PatientID;
            //db.DbUpdate(cmd);

            SqlCommand cmd = new SqlCommand("UpatePatientMast_Set"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@DiagnosisIDs", SqlDbType.NVarChar).Value = DiagnosisIDs;
            cmd.Parameters.Add("@DiagnosisOther", SqlDbType.NVarChar, 500).Value = DiagnosisOther;
            cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = PatientID;

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

        public void setFromOther(string DiagnosisOther)
        {

        }
        public string getdiagnosis(string DiagnosisID)
        {
            string diagnosisname = "";
            DbHelper.SqlDb db = new DbHelper.SqlDb();
            SqlCommand cmd = new SqlCommand("Diagnosis_Get");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@DiagnosisID", SqlDbType.VarChar, 50).Value = DiagnosisID;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                diagnosisname = dt.Rows[0]["dName"].ToString();
            }
            return diagnosisname;

        }

        public DataTable Search(int patienttypeid, string fullname, string diagnosis, DateTime _from, DateTime _uptodate)
        {
            SqlCommand cmd = new SqlCommand("Patient_Diagnosis");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PatienttypeID", SqlDbType.Int).Value = patienttypeid;
            cmd.Parameters.Add("@Fullname", SqlDbType.NVarChar, 200).Value = fullname;
            cmd.Parameters.Add("@Diagnosis", SqlDbType.NVarChar, 200).Value = diagnosis;
            if (_from > DateTime.MinValue)
                cmd.Parameters.Add("@Fromdate", SqlDbType.DateTime).Value = _from;
            else
                cmd.Parameters.Add("@Fromdate", SqlDbType.DateTime).Value = DBNull.Value;
            if (_uptodate > DateTime.MinValue)
                cmd.Parameters.Add("@Uptodate", SqlDbType.DateTime).Value = _uptodate;
            else
                cmd.Parameters.Add("@Uptodate", SqlDbType.DateTime).Value = DBNull.Value;
            return db.DbRead(cmd);
        }
    }
}
