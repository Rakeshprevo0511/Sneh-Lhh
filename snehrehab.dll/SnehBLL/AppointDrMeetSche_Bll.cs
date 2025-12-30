using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SnehBLL
{
    public class AppointDrMeetSche_Bll
    {
        DbHelper.SqlDb db;

        public object DoctorID { get; set; }

        public AppointDrMeetSche_Bll()
        {
            db = new DbHelper.SqlDb();
        }
        public int setNew(SnehDLL.AppointmentDrMeetSch_Dll D)
        {
            SqlCommand cmd = new SqlCommand("AppointmentDrMeetsche_Set"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = D.ID;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = D.AppointmentID;
            cmd.Parameters.Add("@DoctorID", SqlDbType.VarChar).Value = D.DoctorID;
            cmd.Parameters.Add("@IsMain", SqlDbType.Bit).Value = D.IsMain;
            cmd.Parameters.Add("@ShareType", SqlDbType.Int).Value = D.ShareType;
            cmd.Parameters.Add("@ShareAmount", SqlDbType.Decimal).Value = D.ShareAmount;

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
        public int DeleteApp(SnehDLL.AppointmentDrMeetSch_Dll D)
        {
            SqlCommand cmd = new SqlCommand("AppointmentDrMeetsche_Delete");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = D.ID;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = D.AppointmentID;
            SqlParameter Param = new SqlParameter();
            Param.ParameterName = "@RetVal";
            Param.DbType = DbType.Int64;
            Param.Direction = ParameterDirection.Output;
            Param.Value = 0; cmd.Parameters.Add(Param);

            db.DbUpdate(cmd);

            int i = 0;
            if (cmd.Parameters["@RetVal"].Value != null)
            {
                int.TryParse(cmd.Parameters["@RetVal"].Value.ToString(), out i);
            }
            return i;
        }
        public List<SnehDLL.AppointmentDrMeetSch_Dll> GetList_new(int _appointmentID)
        {
            List<SnehDLL.AppointmentDrMeetSch_Dll> DL = new List<SnehDLL.AppointmentDrMeetSch_Dll>();
            SqlCommand cmd = new SqlCommand("AppointmentDoctor_GetList_new"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.AppointmentDrMeetSch_Dll D = new SnehDLL.AppointmentDrMeetSch_Dll();
                D.AppointmentID = _appointmentID;
                //int DoctorID = 0; int.TryParse(dt.Rows[i]["DoctorID"].ToString(), out DoctorID); D.DoctorID = DoctorID.ToString();
                D.DoctorID= dt.Rows[i]["DoctorID"].ToString();
                //D.DoctorName = dt.Rows[i]["Therapist"].ToString();
                bool IsMain = false; bool.TryParse(dt.Rows[i]["IsMain"].ToString(), out IsMain); D.IsMain = IsMain;
                int ShareType = 0; int.TryParse(dt.Rows[i]["ShareType"].ToString(), out ShareType); D.ShareType = ShareType;
                float ShareAmount = 0; float.TryParse(dt.Rows[i]["ShareAmount"].ToString(), out ShareAmount); D.ShareAmount = ShareAmount;

                DL.Add(D);
            }
            return DL;
        }
        public List<SnehDLL.AppointmentDrMeetSch_Dll> getlist_dr(int _appointmentID)
        {
            List<SnehDLL.AppointmentDrMeetSch_Dll> DL = new List<SnehDLL.AppointmentDrMeetSch_Dll>();
            SqlCommand cmd = new SqlCommand("DoctorMast_Get_new"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.AppointmentDrMeetSch_Dll D = new SnehDLL.AppointmentDrMeetSch_Dll();
                //D.AppointmentID = _appointmentID;
                D.AppointmentID = Convert.ToInt32(dt.Rows[i]["AppointmentID"]);
                //int DoctorID = 0; int.TryParse(dt.Rows[i]["DoctorID"].ToString(), out DoctorID); D.DoctorID = DoctorID.ToString();
                D.DoctorID = dt.Rows[i]["DoctorID"].ToString();
                D.DoctorName = dt.Rows[i]["Therapist"].ToString();
              //  bool IsMain = false; bool.TryParse(dt.Rows[i]["IsMain"].ToString(), out IsMain); D.IsMain = IsMain;
               // int ShareType = 0; int.TryParse(dt.Rows[i]["ShareType"].ToString(), out ShareType); D.ShareType = ShareType;
               // float ShareAmount = 0; float.TryParse(dt.Rows[i]["ShareAmount"].ToString(), out ShareAmount); D.ShareAmount = ShareAmount;

                DL.Add(D);
            }
            return DL;
        }
    }
}
