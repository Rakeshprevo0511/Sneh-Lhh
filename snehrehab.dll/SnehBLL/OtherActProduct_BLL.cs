using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
namespace SnehBLL
{
    public class OtherActProduct_BLL
    {
        DbHelper.SqlDb db;

        public OtherActProduct_BLL()
        {
            db = new DbHelper.SqlDb();
        }

        public static int Check(string _uniqueID)
        {
            int _productID = 0;
            SqlCommand cmd = new SqlCommand("OtherAct_Product_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = _uniqueID;
            cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = 0;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["ProductID"].ToString(), out _productID);
            }
            return _productID;
        }

        public SnehDLL.OtherActProduct_DLL Get(int _productID)
        {
            SnehDLL.OtherActProduct_DLL OPD = new SnehDLL.OtherActProduct_DLL();
            SqlCommand cmd = new SqlCommand("OtherAct_Product_Get");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = _productID;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                OPD.ProductID = Convert.ToInt64(dt.Rows[0]["ProductID"].ToString());
                OPD.UniqueID = dt.Rows[0]["UniqueID"].ToString();
                OPD.ProductName = dt.Rows[0]["ProductName"].ToString();
                OPD.CategoryID = Convert.ToInt64(dt.Rows[0]["CategoryID"].ToString());
                float Unitprice = 0; float.TryParse(dt.Rows[0]["UnitPrice"].ToString(), out Unitprice);
                OPD.UnitPrice = Unitprice;
                OPD.IsEnabled = Convert.ToBoolean(dt.Rows[0]["IsEnabled"].ToString());
                OPD.IsDeleted = Convert.ToBoolean(dt.Rows[0]["IsDeleted"].ToString());
            }
            return OPD;
        }

        public int Set(SnehDLL.OtherActProduct_DLL OPD)
        {
            SqlCommand cmd = new SqlCommand("OtherAct_Product_Set"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = OPD.ProductID;
            cmd.Parameters.Add("@ProductName", SqlDbType.VarChar, 150).Value = OPD.ProductName;
            cmd.Parameters.Add("@UnitPrice", SqlDbType.Decimal).Value = OPD.UnitPrice;
            cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = OPD.CategoryID;


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

        public int Delete(int _productID)
        {
            SqlCommand cmd = new SqlCommand("OtherAct_Product_Delete"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = _productID;

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

        public List<SnehDLL.OtherActProduct_DLL> GetList(int _categoryid)
        {
            List<SnehDLL.OtherActProduct_DLL> OAD = new List<SnehDLL.OtherActProduct_DLL>();
            SqlCommand cmd = new SqlCommand("OtherAct_Product_GetList");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@CategoryID", SqlDbType.BigInt).Value = _categoryid;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SnehDLL.OtherActProduct_DLL D = new SnehDLL.OtherActProduct_DLL();
                    D.ProductID = Convert.ToInt64(dt.Rows[i]["ProductID"].ToString());
                    D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                    D.ProductName = dt.Rows[i]["ProductName"].ToString();
                    D.CategoryID = Convert.ToInt64(dt.Rows[i]["CategoryID"].ToString());
                    float unitprice = 0; float.TryParse(dt.Rows[i]["UnitPrice"].ToString(), out unitprice);
                    D.UnitPrice = unitprice;
                    D.IsEnabled = Convert.ToBoolean(dt.Rows[i]["IsEnabled"].ToString());
                    D.IsDeleted = Convert.ToBoolean(dt.Rows[i]["IsDeleted"].ToString());
                    OAD.Add(D);
                }
            }
            return OAD;
        }

        public DataTable Search(int _categoryID, string _searchText)
        {
            SqlCommand cmd = new SqlCommand("OtherAct_Product_Search");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
            cmd.Parameters.Add("@ProductName", SqlDbType.VarChar, 150).Value = _searchText;

            return db.DbRead(cmd);
        }
    }
}
