using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SnehBLL
{
    public class Degree_Bll
    {
        DbHelper.SqlDb db = null;
        public Degree_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public int set(SnehDLL.Degree_Dll DD)
        {
            SqlCommand cmd = new SqlCommand("Set_Deg_Img_Mast"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ReceiptionID", SqlDbType.Int).Value = DD.ReceiptionID;
            cmd.Parameters.Add("@ManagerID", SqlDbType.Int).Value = DD.ManagerID;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = DD.DoctorID;
            cmd.Parameters.Add("@Image_Path", SqlDbType.NVarChar, 4000).Value = DD.Image_Path;
            cmd.Parameters.Add("@Filename", SqlDbType.NVarChar, 4000).Value = DD.Filename;

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

        public DataTable Get_CerImage(int doctorid, int managerid, int receiptionid)
        {
            SqlCommand cmd = new SqlCommand("Get_Certicate"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = doctorid;
            cmd.Parameters.Add("@ManagerID", SqlDbType.Int).Value = managerid;
            cmd.Parameters.Add("@ReceiptionID", SqlDbType.Int).Value = receiptionid;
            DataTable dt = db.DbRead(cmd);
            return dt;
        }

        public DataTable Get_CerData(int idnew, int rec, int man, int doc)
        {
            SqlCommand cmd = new SqlCommand("Get_CerticateImage"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@IDNew", SqlDbType.Int).Value = idnew;
            cmd.Parameters.Add("@ReceiptionID", SqlDbType.Int).Value = rec;
            cmd.Parameters.Add("@ManagerID", SqlDbType.Int).Value = man;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = doc;
            DataTable dt = db.DbRead(cmd);
            return dt;
        }

        public int Update(string uniqueid, string imagepath, string filename)
        {
            SqlCommand cmd = new SqlCommand("UPDATE Deg_Img_Mast SET Image_Path=@Image_Path,Filename=@Filename WHERE UniqueID=@UniqueID"); cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = uniqueid;
            cmd.Parameters.Add("@Image_Path", SqlDbType.VarChar, 50).Value = imagepath;
            cmd.Parameters.Add("@Filename", SqlDbType.VarChar, 50).Value = filename;
            int i = db.DbUpdate(cmd); return i;
        }
    }
}
