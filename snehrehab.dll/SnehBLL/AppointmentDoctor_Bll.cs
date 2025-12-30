using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace SnehBLL
{
   public class AppointmentDoctor_Bll
    {
       DbHelper.SqlDb db;

       public AppointmentDoctor_Bll()
       {
           db = new DbHelper.SqlDb();
       }

       public int setNew(SnehDLL.AppointmentDoctor_Dll D)
       {
           SqlCommand cmd = new SqlCommand("AppointmentDoctor_Set"); cmd.CommandType = CommandType.StoredProcedure;
           cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = D.AppointmentID;
           cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = D.DoctorID;
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

       public List<SnehDLL.AppointmentDoctor_Dll> GetList(int _appointmentID)
       {
           List<SnehDLL.AppointmentDoctor_Dll> DL = new List<SnehDLL.AppointmentDoctor_Dll>();
           SqlCommand cmd = new SqlCommand("AppointmentDoctor_GetList"); cmd.CommandType = CommandType.StoredProcedure;
           cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = _appointmentID;
           DataTable dt = db.DbRead(cmd);
           for (int i = 0; i < dt.Rows.Count; i++)
           {
               SnehDLL.AppointmentDoctor_Dll D = new SnehDLL.AppointmentDoctor_Dll();
               D.AppointmentID = _appointmentID;
               int DoctorID = 0; int.TryParse(dt.Rows[i]["DoctorID"].ToString(), out DoctorID); D.DoctorID = DoctorID;
               bool IsMain = false; bool.TryParse(dt.Rows[i]["IsMain"].ToString(), out IsMain); D.IsMain = IsMain;
               int ShareType = 0; int.TryParse(dt.Rows[i]["ShareType"].ToString(), out ShareType); D.ShareType = ShareType;
               float ShareAmount = 0; float.TryParse(dt.Rows[i]["ShareAmount"].ToString(), out ShareAmount); D.ShareAmount = ShareAmount;

               DL.Add(D);
           }
           return DL;
       }

       public void Delete(int AppointmentID)
       {
           SqlCommand cmd = new SqlCommand("DELETE FROM AppointmentDoctor WHERE AppointmentID = @AppointmentID"); cmd.CommandType = CommandType.Text;
           cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = AppointmentID;
           db.DbUpdate(cmd);
       }
    }
}
