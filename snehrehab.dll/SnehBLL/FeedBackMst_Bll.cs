using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SnehBLL
{
    public class FeedBackMst_Bll
    {
        DbHelper.SqlDb db;

        public FeedBackMst_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public int New(SnehDLL.FeedBackMst_Dll D)
        {
            SqlCommand cmd = new SqlCommand("FeedBack_Mst_New"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@TypeID", SqlDbType.Int).Value = D.TypeID;
            cmd.Parameters.Add("@OtherTypeID", SqlDbType.NVarChar, 500).Value = D.OtherTypeID;
            cmd.Parameters.Add("@ToID", SqlDbType.Int).Value = D.ToID;
            cmd.Parameters.Add("@OtherToID", SqlDbType.NVarChar, 500).Value = D.OtherToID;
            cmd.Parameters.Add("@cMessage", SqlDbType.NVarChar, -1).Value = D.cMessage;
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

        public DataTable GetMyList(int _addedBy, string _search)
        {
            SqlCommand cmd = new SqlCommand("FeedBack_Mst_GetMyList"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@AddedBy", SqlDbType.Int).Value = _addedBy;
            cmd.Parameters.Add("@cMessage", SqlDbType.NVarChar, 50).Value = _search;

            return db.DbRead(cmd);
        }

        public DataTable GetAllList(string _search, DateTime _fromDate, DateTime _uptoDate)
        {
            SqlCommand cmd = new SqlCommand("FeedBack_Mst_GetAllList"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Search", SqlDbType.NVarChar, 50).Value = _search;
            if (_fromDate > DateTime.MinValue)
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = _fromDate;
            else
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = DBNull.Value;
            if (_uptoDate > DateTime.MinValue)
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = _uptoDate;
            else
                cmd.Parameters.Add("@UptoDate", SqlDbType.DateTime).Value = DBNull.Value;

            return db.DbRead(cmd);
        }
    }
}
