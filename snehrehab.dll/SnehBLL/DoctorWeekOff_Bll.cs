using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SnehBLL
{
    public class DoctorWeekOff_Bll
    {
        DbHelper.SqlDb db;

        public DoctorWeekOff_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public int Delete(int _doctorID)
        {
            SqlCommand cmd = new SqlCommand("DoctorWeekOff_DeleteAll"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = _doctorID;

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

        public List<SnehDLL.DoctorWeekOff_Dll> GetList(int _doctorID)
        {
            List<SnehDLL.DoctorWeekOff_Dll> DL = new List<SnehDLL.DoctorWeekOff_Dll>();
            SqlCommand cmd = new SqlCommand("DoctorWeekOff_GetList"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = _doctorID;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.DoctorWeekOff_Dll D = new SnehDLL.DoctorWeekOff_Dll();
                int DoctorID = 0; int.TryParse(dt.Rows[i]["DoctorID"].ToString(), out DoctorID); D.DoctorID = DoctorID;
                int DayID = 0; int.TryParse(dt.Rows[i]["DayID"].ToString(), out DayID); D.DayID = DayID;

                DL.Add(D);
            }
            return DL;
        }

        public int Set(SnehDLL.DoctorWeekOff_Dll D)
        {
            SqlCommand cmd = new SqlCommand("DoctorWeekOff_Set"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = D.DoctorID;
            cmd.Parameters.Add("@DayID", SqlDbType.Int).Value = D.DayID;

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
