using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SnehBLL
{

    public class OtherActCategory_BLL
    {
        DbHelper.SqlDb db;

        public OtherActCategory_BLL()
        {
            db = new DbHelper.SqlDb();
        }

        public static int Check(string _uniqueID)
        {
            int _categoryID = 0;
            SqlCommand cmd = new SqlCommand("OtherAct_Category_Check");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = _uniqueID;
            cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = 0;
            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["CategoryID"].ToString(), out _categoryID);
            }
            return _categoryID;
        }

        public List<SnehDLL.OtherActCategory_DLL> GetList()
        {
            List<SnehDLL.OtherActCategory_DLL> DL = new List<SnehDLL.OtherActCategory_DLL>();
            SqlCommand cmd = new SqlCommand("OtherAct_Category_GetList");
            cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SnehDLL.OtherActCategory_DLL D = new SnehDLL.OtherActCategory_DLL();
                    D.CategoryID = Convert.ToInt64(dt.Rows[i]["CategoryID"].ToString());
                    D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                    D.CategoryName = dt.Rows[i]["CategoryName"].ToString();
                    DL.Add(D);
                }
            }
            return DL;
        }

        public int Set(SnehDLL.OtherActCategory_DLL OCD)
        {
            SqlCommand cmd = new SqlCommand("OtherAct_Category_Set"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = OCD.CategoryID;
            cmd.Parameters.Add("@Categoryname", SqlDbType.VarChar, 150).Value = OCD.CategoryName;

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

        public SnehDLL.OtherActCategory_DLL Get(int _categoryID)
        {
            SnehDLL.OtherActCategory_DLL OCD = null;
            SqlCommand cmd = new SqlCommand("OtherAct_Category_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                OCD = new SnehDLL.OtherActCategory_DLL(); int i = 0;
                OCD.CategoryID = int.Parse(dt.Rows[i]["CategoryID"].ToString());
                OCD.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                OCD.CategoryName = dt.Rows[i]["CategoryName"].ToString();
                OCD.IsEnabled = Convert.ToBoolean(dt.Rows[i]["IsEnabled"].ToString());
                OCD.IsDeleted = Convert.ToBoolean(dt.Rows[i]["IsDeleted"].ToString());
            }
            return OCD;
        }

        public int Delete(int _categoryID)
        {
            SqlCommand cmd = new SqlCommand("OtherAct_Category_Delete"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;

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