using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace SnehBLL
{
    public class LeaveTypes_Bll
    {
        DbHelper.SqlDb db;

        public LeaveTypes_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public SnehDLL.LeaveTypes_Dll Get(int _typeID)
        {
            SnehDLL.LeaveTypes_Dll D = null;
            SqlCommand cmd = new SqlCommand("LeaveTypes_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@TypeID", SqlDbType.Int).Value = _typeID;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                D = new SnehDLL.LeaveTypes_Dll(); int i = 0;
                D.TypeID = int.Parse(dt.Rows[i]["TypeID"].ToString());
                D.TypeName = dt.Rows[i]["TypeName"].ToString();
                int tDays = 0; int.TryParse(dt.Rows[i]["tDays"].ToString(), out tDays); D.tDays = tDays;
            }
            return D;
        }

        public List<SnehDLL.LeaveTypes_Dll> GetList()
        {
            List<SnehDLL.LeaveTypes_Dll> DL = new List<SnehDLL.LeaveTypes_Dll>();
            SqlCommand cmd = new SqlCommand("LeaveTypes_GetList"); cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.LeaveTypes_Dll D = new SnehDLL.LeaveTypes_Dll();
                D.TypeID = int.Parse(dt.Rows[i]["TypeID"].ToString());
                D.TypeName = dt.Rows[i]["TypeName"].ToString();
                int tDays = 0; int.TryParse(dt.Rows[i]["tDays"].ToString(), out tDays); D.tDays = tDays;

                DL.Add(D);
            }
            return DL;
        }
    }
}
