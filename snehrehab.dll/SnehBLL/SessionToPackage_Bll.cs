using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SnehBLL
{
    public class SessionToPackage_Bll
    {
        DbHelper.SqlDb db;

        public SessionToPackage_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public int Delete(int _packageID)
        {
            SqlCommand cmd = new SqlCommand("SessionToPackage_Delete"); cmd.CommandType = CommandType.StoredProcedure;
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

        public List<SnehDLL.SessionToPackage_Dll> GetList(int _packageID)
        {
            List<SnehDLL.SessionToPackage_Dll> DL = new List<SnehDLL.SessionToPackage_Dll>();
            SqlCommand cmd = new SqlCommand("SessionToPackage_GetList"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PackageID", SqlDbType.Int).Value = _packageID;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.SessionToPackage_Dll D = new SnehDLL.SessionToPackage_Dll();
                int SessionID = 0; int.TryParse(dt.Rows[i]["SessionID"].ToString(), out SessionID); D.SessionID = SessionID;
                int PackageID = 0; int.TryParse(dt.Rows[i]["PackageID"].ToString(), out PackageID); D.PackageID = PackageID;

                DL.Add(D);
            }
            return DL;
        }

        public int Set(SnehDLL.SessionToPackage_Dll D)
        {
            SqlCommand cmd = new SqlCommand("SessionToPackage_Set"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PackageID", SqlDbType.Int).Value = D.PackageID;
            cmd.Parameters.Add("@SessionID", SqlDbType.Int).Value = D.SessionID;

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
