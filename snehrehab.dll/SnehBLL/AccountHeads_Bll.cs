using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace SnehBLL
{
    public class AccountHeads_Bll
    {
        DbHelper.SqlDb db;

        public AccountHeads_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public List<SnehDLL.AccountHeads_Dll> GetList()
        {
            List<SnehDLL.AccountHeads_Dll> DL = new List<SnehDLL.AccountHeads_Dll>();
            SqlCommand cmd = new SqlCommand("AccountHeads_GetList"); cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.AccountHeads_Dll D = new SnehDLL.AccountHeads_Dll();
                D.HeadID = int.Parse(dt.Rows[i]["HeadID"].ToString());
                D.HeadName = dt.Rows[i]["HeadName"].ToString();
                int ParentHeadID = 0; int.TryParse(dt.Rows[i]["ParentHeadID"].ToString(), out ParentHeadID); D.ParentHeadID = ParentHeadID;

                DL.Add(D);
            }
            return DL;
        }
    }
}
