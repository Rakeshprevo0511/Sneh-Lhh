using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace SnehBLL
{
    public class ProductMst_Bll
    {
        DbHelper.SqlDb db;

        public ProductMst_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public static int Check(string _uniqueID)
        {
            int _productID = 0;
            SqlCommand cmd = new SqlCommand("Product_Mst_Check"); cmd.CommandType = CommandType.StoredProcedure;
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

        public static string Check(int _productID)
        {
            string _uniqueID = "";
            SqlCommand cmd = new SqlCommand("Product_Mst_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = "";
            cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = _productID;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                _uniqueID = dt.Rows[0]["UniqueID"].ToString();
            }
            return _uniqueID;
        }

        public int Delete(int _productID)
        {
            SqlCommand cmd = new SqlCommand("Product_Mst_Delete"); cmd.CommandType = CommandType.StoredProcedure;
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

        public SnehDLL.ProductMst_Dll Get(int _productID)
        {
            SnehDLL.ProductMst_Dll D = null;
            SqlCommand cmd = new SqlCommand("Product_Mst_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = _productID;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                D = new SnehDLL.ProductMst_Dll(); int i = 0;
                D.ProductID = int.Parse(dt.Rows[i]["ProductID"].ToString());
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                D.ProductName = dt.Rows[i]["ProductName"].ToString();
                float UnitPrice = 0; float.TryParse(dt.Rows[i]["UnitPrice"].ToString(), out UnitPrice); D.UnitPrice = UnitPrice;
                int CategoryID = 0; int.TryParse(dt.Rows[i]["CategoryID"].ToString(), out CategoryID); D.CategoryID = CategoryID;
                DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                D.AddedDate = AddedDate;
                DateTime ModifyDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["ModifyDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ModifyDate);
                D.ModifyDate = ModifyDate;
                int AddedBy = 0; int.TryParse(dt.Rows[i]["AddedBy"].ToString(), out AddedBy); D.AddedBy = AddedBy;
                int ModifyBy = 0; int.TryParse(dt.Rows[i]["ModifyBy"].ToString(), out ModifyBy); D.ModifyBy = ModifyBy;
            }
            return D;
        }

        public List<SnehDLL.ProductMst_Dll> GetList()
        {
            List<SnehDLL.ProductMst_Dll> DL = new List<SnehDLL.ProductMst_Dll>();
            SqlCommand cmd = new SqlCommand("Product_Mst_GetList"); cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.ProductMst_Dll D = new SnehDLL.ProductMst_Dll();
                D.ProductID = int.Parse(dt.Rows[i]["ProductID"].ToString());
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                D.ProductName = dt.Rows[i]["ProductName"].ToString();
                float UnitPrice = 0; float.TryParse(dt.Rows[i]["UnitPrice"].ToString(), out UnitPrice); D.UnitPrice = UnitPrice;
                int CategoryID = 0; int.TryParse(dt.Rows[i]["CategoryID"].ToString(), out CategoryID); D.CategoryID = CategoryID;
                DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                D.AddedDate = AddedDate;
                DateTime ModifyDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["ModifyDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ModifyDate);
                D.ModifyDate = ModifyDate;
                int AddedBy = 0; int.TryParse(dt.Rows[i]["AddedBy"].ToString(), out AddedBy); D.AddedBy = AddedBy;
                int ModifyBy = 0; int.TryParse(dt.Rows[i]["ModifyBy"].ToString(), out ModifyBy); D.ModifyBy = ModifyBy;

                DL.Add(D);
            }
            return DL;
        }

        public List<SnehDLL.ProductMst_Dll> GetList(int _categoryID)
        {
            List<SnehDLL.ProductMst_Dll> DL = new List<SnehDLL.ProductMst_Dll>();
            SqlCommand cmd = new SqlCommand("Product_Mst_GetCatList"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.ProductMst_Dll D = new SnehDLL.ProductMst_Dll();
                D.ProductID = int.Parse(dt.Rows[i]["ProductID"].ToString());
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                D.ProductName = dt.Rows[i]["ProductName"].ToString();
                float UnitPrice = 0; float.TryParse(dt.Rows[i]["UnitPrice"].ToString(), out UnitPrice); D.UnitPrice = UnitPrice;
                int CategoryID = 0; int.TryParse(dt.Rows[i]["CategoryID"].ToString(), out CategoryID); D.CategoryID = CategoryID;
                DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                D.AddedDate = AddedDate;
                DateTime ModifyDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["ModifyDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ModifyDate);
                D.ModifyDate = ModifyDate;
                int AddedBy = 0; int.TryParse(dt.Rows[i]["AddedBy"].ToString(), out AddedBy); D.AddedBy = AddedBy;
                int ModifyBy = 0; int.TryParse(dt.Rows[i]["ModifyBy"].ToString(), out ModifyBy); D.ModifyBy = ModifyBy;

                DL.Add(D);
            }
            return DL;
        }

        public int Set(SnehDLL.ProductMst_Dll D)
        {
            SqlCommand cmd = new SqlCommand("Product_Mst_Set"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = D.ProductID;
            cmd.Parameters.Add("@ProductName", SqlDbType.VarChar, 150).Value = D.ProductName;
            cmd.Parameters.Add("@UnitPrice", SqlDbType.Decimal).Value = D.UnitPrice;
            cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = D.CategoryID;
            if (D.ModifyDate > DateTime.MinValue)
                cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = D.ModifyDate;
            else
                cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@ModifyBy", SqlDbType.Int).Value = D.ModifyBy;

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

        public DataTable Search(int _categoryID, string _searchText)
        {
            SqlCommand cmd = new SqlCommand("Product_Mst_GetSearch"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = _categoryID;
            cmd.Parameters.Add("@ProductName", SqlDbType.VarChar, 150).Value = _searchText;

            return db.DbRead(cmd);
        }
    }
}
