using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace SnehBLL
{
    public class PatientCategory_Bll
    {
        DbHelper.SqlDb db;

        public PatientCategory_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public SnehDLL.PatientCategory_Dll Get(int _categoryID)
        {
            SnehDLL.PatientCategory_Dll D = null;
            SqlCommand cmd = new SqlCommand("PatientCategory_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                D = new SnehDLL.PatientCategory_Dll(); int i = 0;
                D.CategoryID = int.Parse(dt.Rows[i]["CategoryID"].ToString());
                D.CategoryName = dt.Rows[i]["CategoryName"].ToString();
                
            }
            return D;
        }

        public List<SnehDLL.PatientCategory_Dll> GetList()
        {
            List<SnehDLL.PatientCategory_Dll> DL = new List<SnehDLL.PatientCategory_Dll>();
            SqlCommand cmd = new SqlCommand("PatientCategory_GetList"); cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.PatientCategory_Dll D = new SnehDLL.PatientCategory_Dll();
                D.CategoryID = int.Parse(dt.Rows[i]["CategoryID"].ToString());
                D.CategoryName = dt.Rows[i]["CategoryName"].ToString();

                DL.Add(D);
            }
            return DL;
        }
    }
}
