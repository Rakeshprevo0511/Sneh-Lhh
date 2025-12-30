using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SnehBLL
{
    public class FeedBackType_Bll
    {
        DbHelper.SqlDb db;

        public FeedBackType_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public List<SnehDLL.FeedBackType_Dll> GetList()
        {
            List<SnehDLL.FeedBackType_Dll> DL = new List<SnehDLL.FeedBackType_Dll>();
            SqlCommand cmd = new SqlCommand("FeedBack_Type_GetList"); cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.FeedBackType_Dll D = new SnehDLL.FeedBackType_Dll();
                D.TypeID = int.Parse(dt.Rows[i]["TypeID"].ToString());
                D.TypeName = dt.Rows[i]["TypeName"].ToString();
                bool IsEnabled = true; bool.TryParse(dt.Rows[i]["IsEnabled"].ToString(), out IsEnabled); D.IsEnabled = IsEnabled;

                DL.Add(D);
            }
            return DL;
        }

        public SnehDLL.FeedBackType_Dll Get(int TypeID)
        {
            SnehDLL.FeedBackType_Dll D = null;
            SqlCommand cmd = new SqlCommand("FeedBack_Type_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@TypeID", SqlDbType.Int).Value = TypeID;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                int i = 0;
                D = new SnehDLL.FeedBackType_Dll();
                D.TypeID = int.Parse(dt.Rows[i]["TypeID"].ToString());
                D.TypeName = dt.Rows[i]["TypeName"].ToString();
                bool IsEnabled = true; bool.TryParse(dt.Rows[i]["IsEnabled"].ToString(), out IsEnabled); D.IsEnabled = IsEnabled;
            }
            return D;
        }
    }
}
