using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SnehBLL
{
    public class FeedBackTo_Bll
    {
        DbHelper.SqlDb db;

        public FeedBackTo_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public List<SnehDLL.FeedBackTo_Dll> GetList()
        {
            List<SnehDLL.FeedBackTo_Dll> DL = new List<SnehDLL.FeedBackTo_Dll>();
            SqlCommand cmd = new SqlCommand("FeedBack_To_GetList"); cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.FeedBackTo_Dll D = new SnehDLL.FeedBackTo_Dll();
                D.ToID = int.Parse(dt.Rows[i]["ToID"].ToString());
                D.ToName = dt.Rows[i]["ToName"].ToString();

                DL.Add(D);
            }
            return DL;
        }

        public SnehDLL.FeedBackTo_Dll Get(int ToID)
        {
            SnehDLL.FeedBackTo_Dll D = null;
            SqlCommand cmd = new SqlCommand("FeedBack_To_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ToID", SqlDbType.Int).Value = ToID;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                int i = 0;
                D = new SnehDLL.FeedBackTo_Dll();
                D.ToID = int.Parse(dt.Rows[i]["ToID"].ToString());
                D.ToName = dt.Rows[i]["ToName"].ToString();
            }
            return D;
        }
    }
}
