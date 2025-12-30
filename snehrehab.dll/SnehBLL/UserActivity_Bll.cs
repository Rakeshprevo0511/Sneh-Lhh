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
    public class UserActivity_Bll
    {
        DbHelper.SqlDb db;

        public UserActivity_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public List<SnehDLL.UserActivity_Dll> GetList(int _userID)
        {
            List<SnehDLL.UserActivity_Dll> DL = new List<SnehDLL.UserActivity_Dll>();
            SqlCommand cmd = new SqlCommand("UserActivity_GetList"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.UserActivity_Dll D = new SnehDLL.UserActivity_Dll();
                D.ActivityID = int.Parse(dt.Rows[i]["ActivityID"].ToString());
                int UserID = 0; int.TryParse(dt.Rows[i]["UserID"].ToString(), out UserID); D.UserID = UserID;
                D.OldValue = dt.Rows[i]["OldValue"].ToString();
                D.NewValue = dt.Rows[i]["NewValue"].ToString();
                D.Remark = dt.Rows[i]["Remark"].ToString();
                DateTime aDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["aDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out aDate);
                D.aDate = aDate;

                DL.Add(D);
            }
            return DL;
        }

        public int Set(SnehDLL.UserActivity_Dll D)
        {
            SqlCommand cmd = new SqlCommand("UserActivity_SetNew"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = D.UserID;
            cmd.Parameters.Add("@OldValue", SqlDbType.VarChar, -1).Value = D.OldValue;
            cmd.Parameters.Add("@NewValue", SqlDbType.VarChar, -1).Value = D.NewValue;
            cmd.Parameters.Add("@Remark", SqlDbType.VarChar, -1).Value = D.Remark;

            return db.DbUpdate(cmd);
        }

        public DataTable Search(int _userID, DateTime _fromDate, DateTime _uptoDate)
        {
            SqlCommand cmd = new SqlCommand("UserActivity_Search"); cmd.CommandType = CommandType.StoredProcedure;
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

        public int LogIn(int _userID, string _tokenID)
        {
            string _remark = "Logged In : ";
            if (HttpContext.Current.Request.Browser.IsMobileDevice)
                _remark += "Mobile ";
            else
                _remark += "Browser ";
            if (string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
                _remark += "(" + HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] + ")";
            else
                _remark += HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            _remark += " " + HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];

            SnehDLL.UserActivity_Dll D = new SnehDLL.UserActivity_Dll()
            {
                ActivityID = 0,
                UserID = _userID,
                OldValue = "",
                NewValue = "",
                Remark = _remark,
                aDate = DateTime.UtcNow.AddMinutes(330)
            };
            if (SnehBLL.UserAccount_Bll.log_LoggedIn(_userID,  _tokenID) > 0)
            {
                SnehBLL.UserActivity_Bll B = new UserActivity_Bll();
                return B.Set(D);
            }
            return 0;
        }

        public int LogIn(int _userID, string _tokenID, string _remark)
        {
            SnehDLL.UserActivity_Dll D = new SnehDLL.UserActivity_Dll()
            {
                ActivityID = 0,
                UserID = _userID,
                OldValue = "",
                NewValue = "",
                Remark = _remark,
                aDate = DateTime.UtcNow.AddMinutes(330)
            };
            if (SnehBLL.UserAccount_Bll.log_LoggedIn(_userID, _tokenID) > 0)
            {
                SnehBLL.UserActivity_Bll B = new UserActivity_Bll();
                return B.Set(D);
            }
            return 0;
        }

        public int LogOut(int _userID, string _tokenID)
        {
            string _remark = "Logged Out : ";
            if (HttpContext.Current.Request.Browser.IsMobileDevice)
                _remark += "Mobile ";
            else
                _remark += "Browser ";
            if (string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
                _remark += "(" + HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] + ")";
            else
                _remark += HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            _remark += " " + HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];

            SnehDLL.UserActivity_Dll D = new SnehDLL.UserActivity_Dll()
            {
                ActivityID = 0,
                UserID = _userID,
                OldValue = "",
                NewValue = "",
                Remark = _remark,
                aDate = DateTime.UtcNow.AddMinutes(330)
            };
            if (SnehBLL.UserAccount_Bll.log_LoggedOut(_userID, _tokenID) > 0)
            {
                SnehBLL.UserActivity_Bll B = new UserActivity_Bll();
                return B.Set(D);
            }
            return 0;
        }

        public int LogOut(int _userID, string _tokenID, string _remark)
        {
            SnehDLL.UserActivity_Dll D = new SnehDLL.UserActivity_Dll()
            {
                ActivityID = 0,
                UserID = _userID,
                OldValue = "",
                NewValue = "",
                Remark = _remark,
                aDate = DateTime.UtcNow.AddMinutes(330)
            };
            if (SnehBLL.UserAccount_Bll.log_LoggedOut(_userID, _tokenID) > 0)
            {
                SnehBLL.UserActivity_Bll B = new UserActivity_Bll();
                return B.Set(D);
            }
            return 0;
        }
    }
}