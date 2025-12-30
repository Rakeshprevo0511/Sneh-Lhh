using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;

namespace SnehBLL
{
    public class SupportTicket_Bll
    {
        DbHelper.SqlDb db;

        public SupportTicket_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public static int Check(string _uniqueID)
        {
            int _ticketID = 0;
            SqlCommand cmd = new SqlCommand("SupportTicket_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = _uniqueID;
            cmd.Parameters.Add("@TicketID", SqlDbType.Int).Value = 0;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["TicketID"].ToString(), out _ticketID);
            }
            return _ticketID;
        }

        public static string Check(int _ticketID)
        {
            string _uniqueID = "";
            SqlCommand cmd = new SqlCommand("SupportTicket_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = "";
            cmd.Parameters.Add("@TicketID", SqlDbType.Int).Value = _ticketID;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                _uniqueID = dt.Rows[0]["UniqueID"].ToString();
            }
            return _uniqueID;
        }

        public int Delete(int _ticketID)
        {
            SqlCommand cmd = new SqlCommand("SupportTicket_Delete"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@TicketID", SqlDbType.Int).Value = _ticketID;

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

        public SnehDLL.SupportTicket_Dll Get(int _ticketID)
        {
            SnehDLL.SupportTicket_Dll D = null;
            SqlCommand cmd = new SqlCommand("SupportTicket_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@TicketID", SqlDbType.Int).Value = _ticketID;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                D = new SnehDLL.SupportTicket_Dll(); int i = 0;
                D.TicketID = int.Parse(dt.Rows[i]["TicketID"].ToString());
                D.TicketCode = dt.Rows[i]["TicketCode"].ToString();
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                int UserID = 0; int.TryParse(dt.Rows[i]["UserID"].ToString(), out UserID); D.UserID = UserID;
                D.tMessage = dt.Rows[i]["tMessage"].ToString();
                D.aFile = dt.Rows[i]["aFile"].ToString();
                D.uFile = dt.Rows[i]["uFile"].ToString();
                DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                D.AddedDate = AddedDate;
                DateTime ModifyDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["ModifyDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ModifyDate);
                D.ModifyDate = ModifyDate;
                int AddedBy = 0; int.TryParse(dt.Rows[i]["AddedBy"].ToString(), out AddedBy); D.AddedBy = AddedBy;
                int ModifyBy = 0; int.TryParse(dt.Rows[i]["ModifyBy"].ToString(), out ModifyBy); D.ModifyBy = ModifyBy;
                int cStatus = 0; int.TryParse(dt.Rows[i]["cStatus"].ToString(), out cStatus); D.cStatus = cStatus;
                D.Remark = dt.Rows[i]["Remark"].ToString();
                DateTime StatusDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["StatusDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out StatusDate);
                D.StatusDate = StatusDate;
            }
            return D;
        }

        public List<SnehDLL.SupportTicket_Dll> GetList()
        {
            List<SnehDLL.SupportTicket_Dll> DL = new List<SnehDLL.SupportTicket_Dll>();
            SqlCommand cmd = new SqlCommand("SupportTicket_GetList"); cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.SupportTicket_Dll D = new SnehDLL.SupportTicket_Dll();
                D.TicketID = int.Parse(dt.Rows[i]["TicketID"].ToString());
                D.TicketCode = dt.Rows[i]["TicketCode"].ToString();
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                int UserID = 0; int.TryParse(dt.Rows[i]["UserID"].ToString(), out UserID); D.UserID = UserID;
                D.tMessage = dt.Rows[i]["tMessage"].ToString();
                D.aFile = dt.Rows[i]["aFile"].ToString();
                D.uFile = dt.Rows[i]["uFile"].ToString();
                DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                D.AddedDate = AddedDate;
                DateTime ModifyDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["ModifyDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ModifyDate);
                D.ModifyDate = ModifyDate;
                int AddedBy = 0; int.TryParse(dt.Rows[i]["AddedBy"].ToString(), out AddedBy); D.AddedBy = AddedBy;
                int ModifyBy = 0; int.TryParse(dt.Rows[i]["ModifyBy"].ToString(), out ModifyBy); D.ModifyBy = ModifyBy;
                int cStatus = 0; int.TryParse(dt.Rows[i]["cStatus"].ToString(), out cStatus); D.cStatus = cStatus;
                D.Remark = dt.Rows[i]["Remark"].ToString();
                DateTime StatusDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["StatusDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out StatusDate);
                D.StatusDate = StatusDate;

                DL.Add(D);
            }
            return DL;
        }

        public DataTable Search(string _fullName, DateTime _fromDate, DateTime _uptoDate)
        {
            SqlCommand cmd = new SqlCommand("SupportTicket_Search"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 50).Value = _fullName;
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

        public DataTable Search(int _userID, DateTime _fromDate, DateTime _uptoDate)
        {
            SqlCommand cmd = new SqlCommand("SupportTicket_MySearch"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
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

        public int Set(int UserID, string tMessage, string aFile, string uFile, DateTime ModifyDate, int ModifyBy)
        {
            SqlCommand cmd = new SqlCommand("SupportTicket_Set"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = UserID;
            cmd.Parameters.Add("@tMessage", SqlDbType.VarChar, -1).Value = tMessage;
            cmd.Parameters.Add("@aFile", SqlDbType.VarChar, -1).Value = aFile;
            cmd.Parameters.Add("@uFile", SqlDbType.VarChar, -1).Value = uFile;
            if (ModifyDate > DateTime.MinValue)
                cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = ModifyDate;
            else
                cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = DBNull.Value;
            cmd.Parameters.Add("@ModifyBy", SqlDbType.Int).Value = ModifyBy;

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

        public string FilePath(bool _full)
        {
            if (_full)
            {
                return HttpContext.Current.Server.MapPath(DbHelper.Configuration.FileFolder);
            }
            else
            {
                return DbHelper.Configuration.FileFolder;
            }
        }

        public bool Upload(ref System.Web.UI.WebControls.FileUpload txtFile, string _fileName)
        {
            try
            {
                txtFile.SaveAs(FilePath(true) + _fileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public int Update(int _ticketID, string _remark, int _cStatus)
        {
            SqlCommand cmd = new SqlCommand("SupportTicket_Update"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@TicketID", SqlDbType.Int).Value = _ticketID;
            cmd.Parameters.Add("@Remark", SqlDbType.VarChar, -1).Value = _remark;
            cmd.Parameters.Add("@cStatus", SqlDbType.Int).Value = _cStatus;

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
        public static int CheckYesAppoint(string _uniqueID)
        {
            int _ticketID = 0;
            SqlCommand cmd = new SqlCommand("YesterdayAppoint_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = _uniqueID;
            cmd.Parameters.Add("@TicketID", SqlDbType.Int).Value = 0;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["TicketID"].ToString(), out _ticketID);
            }
            return _ticketID;
        }
        public SnehDLL.SupportTicket_Dll GetYestAppoint(int _ticketID)
        {
            SnehDLL.SupportTicket_Dll D = null;
            SqlCommand cmd = new SqlCommand("YesterdayAppoint_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@TicketID", SqlDbType.Int).Value = _ticketID;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                D = new SnehDLL.SupportTicket_Dll(); int i = 0;
                D.TicketID = int.Parse(dt.Rows[i]["TicketID"].ToString());
                D.TicketCode = dt.Rows[i]["TicketCode"].ToString();
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                int UserID = 0; int.TryParse(dt.Rows[i]["UserID"].ToString(), out UserID); D.UserID = UserID;
                D.yNarration = dt.Rows[i]["yNarration"].ToString();
                D.yRemark = dt.Rows[i]["yRemark"].ToString();
                DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                D.AddedDate = AddedDate;
                DateTime ModifyDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["ModifyDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ModifyDate);
                D.ModifyDate = ModifyDate;
                int AddedBy = 0; int.TryParse(dt.Rows[i]["AddedBy"].ToString(), out AddedBy); D.AddedBy = AddedBy;
                int ModifyBy = 0; int.TryParse(dt.Rows[i]["ModifyBy"].ToString(), out ModifyBy); D.ModifyBy = ModifyBy;
                int yStatus = 0; int.TryParse(dt.Rows[i]["yStatus"].ToString(), out yStatus); D.yStatus = yStatus;
                D.Remark = dt.Rows[i]["yRemark"].ToString();
                DateTime StatusDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["StatusDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out StatusDate);
                D.StatusDate = StatusDate;
            }
            return D;
        }
        public int Update_DrYesterdayAppoint(int _ticketID, string _yremark, int _yStatus)
        {
            SqlCommand cmd = new SqlCommand("dRYesterdayAppoint_Update"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@TicketID", SqlDbType.Int).Value = _ticketID;
            cmd.Parameters.Add("@yRemark", SqlDbType.VarChar, -1).Value = _yremark;
            cmd.Parameters.Add("@yStatus", SqlDbType.Int).Value = _yStatus;

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
