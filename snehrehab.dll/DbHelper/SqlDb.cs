using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DbHelper
{
    public class SqlDb
    {
        SqlConnection con;

        public SqlDb()
        {
            con = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"].ToString());
        }

        public SqlConnection DbConnection()
        {
            return new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"].ToString());
        }

        public DataSet DbFetch(SqlCommand cmd)
        {
            DataSet ds = new DataSet();
            try
            {
                con.Open(); cmd.Connection = con; cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                con.Close();
            }

            return ds;
        }

        public DataTable DbRead(SqlCommand cmd)
        {
            DataTable dt = new DataTable();
            try
            {
                con.Open(); cmd.Connection = con; cmd.CommandTimeout = 0;
                SqlDataReader dr; dr = cmd.ExecuteReader();

                dt.Load(dr); con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                con.Close();
            }

            return dt;
        }

        public DataTable DbRead_TimeOut(SqlCommand cmd)
        {
            DataTable dt = new DataTable();
            try
            {
                con.Open(); cmd.Connection = con; cmd.CommandTimeout = 0;
                SqlDataReader dr; dr = cmd.ExecuteReader();
                dt.Load(dr); con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return dt;
        }

        public int DbUpdate(SqlCommand cmd)
        {
            int i = 0;
            try
            {
                con.Open(); cmd.Connection = con; cmd.CommandTimeout = 0;

                i = cmd.ExecuteNonQuery(); con.Close();
            }

            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                con.Close();

            }

            return i;
        }
    }
}
