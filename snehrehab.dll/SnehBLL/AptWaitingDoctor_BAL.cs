using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SnehBLL
{
    public class AptWaitingDoctor_BAL
    {
        DbHelper.SqlDb db;

        public AptWaitingDoctor_BAL()
        {
            db = new DbHelper.SqlDb();
        }

        public int setNew(SnehDLL.AptWaitingDoctor_DAL D)
        {
            SqlCommand cmd = new SqlCommand("AptWaitingDoctor_Set"); cmd.CommandType = CommandType.StoredProcedure;
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
    }
}
