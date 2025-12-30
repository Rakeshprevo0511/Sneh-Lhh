using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SnehBLL
{
    public class UserCategory_Bll
    {
        DbHelper.SqlDb db;

        public UserCategory_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public List<SnehDLL.UserCategory_Dll> GetList()
        {
            List<SnehDLL.UserCategory_Dll> DL = new List<SnehDLL.UserCategory_Dll>();
            SqlCommand cmd = new SqlCommand("UserCategory_GetList"); cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.UserCategory_Dll D = new SnehDLL.UserCategory_Dll();
                D.UserCatID = int.Parse(dt.Rows[i]["UserCatID"].ToString());
                D.CategoryName = dt.Rows[i]["CategoryName"].ToString();
                int SpecialityID = 0; int.TryParse(dt.Rows[i]["SpecialityID"].ToString(), out SpecialityID); D.SpecialityID = SpecialityID;

                DL.Add(D);
            }
            return DL;
        }
    }
}
