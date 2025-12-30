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
    public class UserAccount_Bll
    {
        DbHelper.SqlDb db;
        static DateTime lastReqDT = new DateTime(2022, 03, 07, 11, 00, 00);
        public UserAccount_Bll()
        {
            db = new DbHelper.SqlDb();
        }

        public static int Check(string _uniqueID)
        {
            int _userID = 0;
            SqlCommand cmd = new SqlCommand("UserAccount_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = _uniqueID;
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = 0;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["UserID"].ToString(), out _userID);
            }
            return _userID;
        }

        public static string Check(int _userID)
        {
            string _uniqueID = "";
            SqlCommand cmd = new SqlCommand("UserAccount_Check"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UniqueID", SqlDbType.VarChar, 50).Value = "";
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                _uniqueID = dt.Rows[0]["UniqueID"].ToString();
            }
            return _uniqueID;
        }

        public static int checkIsMember(int _userID)
        {
            int IsMember = 0;
            SqlCommand cmd = new SqlCommand("UserAccount_Check_IsMember"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["IsManager"].ToString(), out IsMember);
            }
            return IsMember;
        }
        public static bool IsAdmin()
        {
            int _catID = SnehBLL.UserAccount_Bll.getCategory();
            if (_catID == 1) return true;
            return false;
        }

       

        public SnehDLL.UserAccount_Dll Get(int _userID)
        {
            SnehDLL.UserAccount_Dll D = null;
            SqlCommand cmd = new SqlCommand("UserAccount_Get"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
            DataTable dt = db.DbRead(cmd);
            if (dt.Rows.Count > 0)
            {
                D = new SnehDLL.UserAccount_Dll(); int i = 0;
                D.UserID = int.Parse(dt.Rows[i]["UserID"].ToString());
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                int DoctorID = 0; int.TryParse(dt.Rows[i]["DoctorID"].ToString(), out DoctorID); D.DoctorID = DoctorID;
                D.FullName = dt.Rows[i]["FullName"].ToString();
                D.MobileNo = dt.Rows[i]["MobileNo"].ToString();
                D.MailID = dt.Rows[i]["MailID"].ToString();
                D.LoginName = dt.Rows[i]["LoginName"].ToString();
                D.LoginPwd = dt.Rows[i]["LoginPwd"].ToString();
                D.LastLogin = dt.Rows[i]["LastLogin"].ToString();
                int UserCatID = 0; int.TryParse(dt.Rows[i]["UserCatID"].ToString(), out UserCatID); D.UserCatID = UserCatID;
                bool IsMain = false; bool.TryParse(dt.Rows[i]["IsMain"].ToString(), out IsMain); D.IsMain = IsMain;
                bool IsEnabled = true; bool.TryParse(dt.Rows[i]["IsEnabled"].ToString(), out IsEnabled); D.IsEnabled = IsEnabled;
                DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                D.AddedDate = AddedDate;
                DateTime ModifyDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["ModifyDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ModifyDate);
                D.ModifyDate = ModifyDate;
                int AddedBy = 0; int.TryParse(dt.Rows[i]["AddedBy"].ToString(), out AddedBy); D.AddedBy = AddedBy;
                int ModifyBy = 0; int.TryParse(dt.Rows[i]["ModifyBy"].ToString(), out ModifyBy); D.ModifyBy = ModifyBy;
                int IsLoggedIn = 0; int.TryParse(dt.Rows[i]["IsLoggedIn"].ToString(), out IsLoggedIn); D.IsLoggedIn = IsLoggedIn;
                int IsPwdReset = 0; int.TryParse(dt.Rows[i]["IsPwdReset"].ToString(), out IsPwdReset); D.IsPwdReset = IsPwdReset;
            }
            return D;
        }

        public List<SnehDLL.UserAccount_Dll> GetList()
        {
            List<SnehDLL.UserAccount_Dll> DL = new List<SnehDLL.UserAccount_Dll>();
            SqlCommand cmd = new SqlCommand("UserAccount_GetList"); cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = db.DbRead(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SnehDLL.UserAccount_Dll D = new SnehDLL.UserAccount_Dll();
                D.UserID = int.Parse(dt.Rows[i]["UserID"].ToString());
                D.UniqueID = dt.Rows[i]["UniqueID"].ToString();
                int DoctorID = 0; int.TryParse(dt.Rows[i]["DoctorID"].ToString(), out DoctorID); D.DoctorID = DoctorID;
                D.FullName = dt.Rows[i]["FullName"].ToString();
                D.MobileNo = dt.Rows[i]["MobileNo"].ToString();
                D.MailID = dt.Rows[i]["MailID"].ToString();
                D.LoginName = dt.Rows[i]["LoginName"].ToString();
                D.LoginPwd = dt.Rows[i]["LoginPwd"].ToString();
                D.LastLogin = dt.Rows[i]["LastLogin"].ToString();
                int UserCatID = 0; int.TryParse(dt.Rows[i]["UserCatID"].ToString(), out UserCatID); D.UserCatID = UserCatID;
                bool IsMain = false; bool.TryParse(dt.Rows[i]["IsMain"].ToString(), out IsMain); D.IsMain = IsMain;
                bool IsEnabled = true; bool.TryParse(dt.Rows[i]["IsEnabled"].ToString(), out IsEnabled); D.IsEnabled = IsEnabled;
                DateTime AddedDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["AddedDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out AddedDate);
                D.AddedDate = AddedDate;
                DateTime ModifyDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["ModifyDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out ModifyDate);
                D.ModifyDate = ModifyDate;
                int AddedBy = 0; int.TryParse(dt.Rows[i]["AddedBy"].ToString(), out AddedBy); D.AddedBy = AddedBy;
                int ModifyBy = 0; int.TryParse(dt.Rows[i]["ModifyBy"].ToString(), out ModifyBy); D.ModifyBy = ModifyBy;
                int IsLoggedIn = 0; int.TryParse(dt.Rows[i]["IsLoggedIn"].ToString(), out IsLoggedIn); D.IsLoggedIn = IsLoggedIn;
                int IsPwdReset = 0; int.TryParse(dt.Rows[i]["IsPwdReset"].ToString(), out IsPwdReset); D.IsPwdReset = IsPwdReset;

                DL.Add(D);
            }
            return DL;
        }

        public SnehDLL.UserAccount_Dll Login(string _loginName, string _password, int _categoryID)
        {
            SnehDLL.UserAccount_Dll D = null;
            SqlCommand cmd = new SqlCommand("UserAccount_Login"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@LoginName", SqlDbType.VarChar, 50).Value = _loginName;
            cmd.Parameters.Add("@LoginPwd", SqlDbType.VarChar, 50).Value = _password;
            cmd.Parameters.Add("@UserCatID", SqlDbType.Int).Value = _categoryID;

            DataTable dt = db.DbRead(cmd);

            if (dt.Rows.Count == 1)
            {
                int _userID = 0; int.TryParse(dt.Rows[0]["UserID"].ToString(), out _userID);
                if (_userID > 0)
                {
                    D = Get(_userID);
                }
            }
            return D;
        }

        public static void LogOut()
        {
            int _userID = IsLogin();
            if (_userID > 0)
            {
                UserActivity_Bll AB = new UserActivity_Bll(); AB.LogOut(_userID, setTokenID(""));
            }

            HttpContext.Current.Session[DbHelper.Configuration.loginFullName] = null;
            HttpContext.Current.Session[DbHelper.Configuration.loginUserID] = null;
            HttpContext.Current.Session[DbHelper.Configuration.loginName] = null;
            HttpContext.Current.Session[DbHelper.Configuration.loginUserCat] = null;

            HttpCookie C = new HttpCookie(DbHelper.Configuration.cookieUserID, "");
            C.Expires = DateTime.UtcNow.AddMinutes(330).AddDays(-99);
            HttpContext.Current.Response.Cookies.Add(C);

            C = new HttpCookie(DbHelper.Configuration.cookieCompID, "");
            C.Expires = DateTime.UtcNow.AddMinutes(330).AddDays(-99);
            HttpContext.Current.Response.Cookies.Add(C);

            setTokenID();
        }

        public static void setLogin(int _userID, string _loginName, string _fullName, int _categoryID)
        {
            HttpContext.Current.Session[DbHelper.Configuration.loginFullName] = _fullName;
            HttpContext.Current.Session[DbHelper.Configuration.loginUserID] = _userID.ToString();
            HttpContext.Current.Session[DbHelper.Configuration.loginName] = _loginName;
            HttpContext.Current.Session[DbHelper.Configuration.loginUserCat] = _categoryID.ToString();

            string _encVal = DbHelper.Configuration.Encrypt(_userID.ToString());
            HttpCookie C = new HttpCookie(DbHelper.Configuration.cookieUserID, _encVal);
            C.Expires = DateTime.UtcNow.AddMinutes(330).AddDays(99);
            HttpContext.Current.Response.Cookies.Add(C);
        }

        public static string setTokenID(string _tokenID)
        {
            if (_tokenID.Length > 0)
            {
                HttpCookie C = new HttpCookie(DbHelper.Configuration.cookieCompID, _tokenID);
                C.Expires = DateTime.UtcNow.AddMinutes(330).AddDays(99);
                HttpContext.Current.Response.Cookies.Add(C);
            }
            else
            {
                if (HttpContext.Current.Request.Cookies[DbHelper.Configuration.cookieCompID] != null)
                {
                    HttpCookie C = HttpContext.Current.Request.Cookies[DbHelper.Configuration.cookieCompID];
                    if (C != null)
                    {
                        _tokenID = C.Value;
                    }
                }
            }
            return _tokenID;
        }

        public static void setTokenID()
        {
            HttpCookie C = new HttpCookie(DbHelper.Configuration.cookieCompID, "");
            C.Expires = DateTime.UtcNow.AddMinutes(330).AddDays(-99);
            HttpContext.Current.Response.Cookies.Add(C);
        }

        public static int IsLogin()
        {
            int _userID = 0;
            if (HttpContext.Current.Session[DbHelper.Configuration.loginUserID] != null)
            {
                int.TryParse(HttpContext.Current.Session[DbHelper.Configuration.loginUserID].ToString(), out _userID);
            }
            if (_userID <= 0)
            {
                if (HttpContext.Current.Request.Cookies[DbHelper.Configuration.cookieUserID] != null)
                {
                    HttpCookie C = HttpContext.Current.Request.Cookies[DbHelper.Configuration.cookieUserID];
                    if (C != null)
                    {
                        string _decVal = DbHelper.Configuration.Decrypt(C.Value);
                        int.TryParse(_decVal, out _userID);
                        if (_userID > 0)
                        {
                            UserAccount_Bll UAB = new UserAccount_Bll();
                            SnehDLL.UserAccount_Dll UAD = UAB.Get(_userID);
                            if (UAD != null)
                            {
                                setLogin(UAD.UserID, UAD.FullName, UAD.LoginName, UAD.UserCatID);
                            }
                            else
                            {
                                _userID = 0;
                            }
                        }
                    }
                }
            }
            if (_userID > 0)
            {
                if (PwdForced(_userID)) { _userID = 0; }
            }
          

            return _userID;
        }        
       

        

        public static int getCategory()
        {
            int _categoryID = 0;
            if (HttpContext.Current.Session[DbHelper.Configuration.loginUserCat] != null)
            {
                int.TryParse(HttpContext.Current.Session[DbHelper.Configuration.loginUserCat].ToString(), out _categoryID);
            }

            if (_categoryID <= 0)
            {
                HttpContext.Current.Session[DbHelper.Configuration.loginUserID] = null; IsLogin();

                if (HttpContext.Current.Session[DbHelper.Configuration.loginUserCat] != null)
                {
                    int.TryParse(HttpContext.Current.Session[DbHelper.Configuration.loginUserCat].ToString(), out _categoryID);
                }
            }
            return _categoryID;
        }

        public int Set(SnehDLL.UserAccount_Dll D)
        {
            SqlCommand cmd = new SqlCommand("UserAccount_Set"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = D.UserID;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = D.DoctorID;
            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 250).Value = D.FullName;
            cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar, 150).Value = D.MobileNo;
            cmd.Parameters.Add("@MailID", SqlDbType.VarChar, 250).Value = D.MailID;
            cmd.Parameters.Add("@LoginName", SqlDbType.VarChar, 50).Value = D.LoginName;
            cmd.Parameters.Add("@LoginPwd", SqlDbType.VarChar, 50).Value = D.LoginPwd;
            cmd.Parameters.Add("@UserCatID", SqlDbType.Int).Value = D.UserCatID;
            cmd.Parameters.Add("@IsEnabled", SqlDbType.Bit).Value = D.IsEnabled;
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

        public static bool IsMain(int _userID)
        {
            SqlCommand cmd = new SqlCommand("UserAccount_IsMain"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

            DbHelper.SqlDb db = new DbHelper.SqlDb();

            DataTable dt = db.DbRead(cmd);

            bool i = false;
            if (dt.Rows.Count > 0)
            {
                bool.TryParse(dt.Rows[0]["IsMain"].ToString(), out i);
            }
            return i;
        }

        public int Delete(int _userID)
        {
            SqlCommand cmd = new SqlCommand("UserAccount_Delete"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

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

        public bool LoginChk(int _userID, string _loginName)
        {
            SqlCommand cmd = new SqlCommand("UserAccount_LoginChk"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
            cmd.Parameters.Add("@LoginName", SqlDbType.VarChar, 50).Value = _loginName;

            SqlParameter Param = new SqlParameter(); Param.ParameterName = "@RetVal";
            Param.DbType = DbType.Int64; Param.Direction = ParameterDirection.Output;
            Param.Value = 0; cmd.Parameters.Add(Param);

            db.DbUpdate(cmd);

            int i = 0;
            if (cmd.Parameters["@RetVal"].Value != null)
            {
                int.TryParse(cmd.Parameters["@RetVal"].Value.ToString(), out i);
            }
            return i > 0;
        }

        public DataTable Search()
        {
            SqlCommand cmd = new SqlCommand("UserAccount_Search"); cmd.CommandType = CommandType.StoredProcedure;
            return db.DbRead(cmd);
        }

        public int Update(SnehDLL.UserAccount_Dll D)
        {
            SqlCommand cmd = new SqlCommand("UserAccount_Update"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = D.UserID;
            cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 250).Value = D.FullName;
            cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar, 150).Value = D.MobileNo;
            cmd.Parameters.Add("@MailID", SqlDbType.VarChar, 250).Value = D.MailID;
            cmd.Parameters.Add("@LoginName", SqlDbType.VarChar, 50).Value = D.LoginName;
            cmd.Parameters.Add("@LoginPwd", SqlDbType.VarChar, 50).Value = D.LoginPwd;
            cmd.Parameters.Add("@IsEnabled", SqlDbType.Bit).Value = D.IsEnabled;
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

        public int Password(int _userID, string _oldPwd, string _newPwd)
        {
            SqlCommand cmd = new SqlCommand("UserAccount_NewPass"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
            cmd.Parameters.Add("@ModifyDate", SqlDbType.DateTime).Value = DateTime.UtcNow.AddMinutes(330);
            cmd.Parameters.Add("@OldPwd", SqlDbType.VarChar, 50).Value = _oldPwd;
            cmd.Parameters.Add("@NewPwd", SqlDbType.VarChar, 50).Value = _newPwd;
            cmd.Parameters.Add("@TokenID", SqlDbType.VarChar, 250).Value = setTokenID("");

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

        public static int log_LoggedIn(int _userID, string _tokenID)
        {
            SqlCommand cmd = new SqlCommand("UserAccountLog_LoggedIn"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
            cmd.Parameters.Add("@TokenID", SqlDbType.VarChar, 250).Value = _tokenID;

            SqlParameter Param = new SqlParameter(); Param.ParameterName = "@RetVal";
            Param.DbType = DbType.Int64; Param.Direction = ParameterDirection.Output;
            Param.Value = 0; cmd.Parameters.Add(Param);

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            db.DbUpdate(cmd);

            int i = 0;
            if (cmd.Parameters["@RetVal"].Value != null)
            {
                int.TryParse(cmd.Parameters["@RetVal"].Value.ToString(), out i);
            }
            return i;
        }

        public static int log_LoggedOut(int _userID, string _tokenID)
        {
            SqlCommand cmd = new SqlCommand("UserAccountLog_LoggedOut"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
            cmd.Parameters.Add("@TokenID", SqlDbType.VarChar, 250).Value = _tokenID;

            SqlParameter Param = new SqlParameter(); Param.ParameterName = "@RetVal";
            Param.DbType = DbType.Int64; Param.Direction = ParameterDirection.Output;
            Param.Value = 0; cmd.Parameters.Add(Param);

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            db.DbUpdate(cmd);

            int i = 0;
            if (cmd.Parameters["@RetVal"].Value != null)
            {
                int.TryParse(cmd.Parameters["@RetVal"].Value.ToString(), out i);
            }
            return i;
        }

        public static bool PwdForced(int _userID)
        {
            SqlCommand cmd = new SqlCommand("UserSession_IsForced"); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
            cmd.Parameters.Add("@TokenID", SqlDbType.VarChar, 250).Value = setTokenID("");

            SqlParameter Param = new SqlParameter(); Param.ParameterName = "@RetVal";
            Param.DbType = DbType.Int64; Param.Direction = ParameterDirection.Output;
            Param.Value = 0; cmd.Parameters.Add(Param);

            DbHelper.SqlDb db = new DbHelper.SqlDb();
            db.DbUpdate(cmd);

            int i = 0;
            if (cmd.Parameters["@RetVal"].Value != null)
            {
                int.TryParse(cmd.Parameters["@RetVal"].Value.ToString(), out i);
            }
            return i > 0;
        }

        public static void CheckAuth()
        {
            bool _auth = true;
            if (!_auth)
            {
                HttpContext.Current.Response.Redirect("/Member/NoAuth.aspx", true);
            }
        }

        public int AuthManager()
        {
            int AuthManagerId = 0;
            SqlCommand cmd = new SqlCommand("GetAuthManagerId");
            cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = db.DbRead(cmd);
            int _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int.TryParse(dt.Rows[i]["UserID"].ToString(), out AuthManagerId);
                int AuthManagerId1 = 0;
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    int.TryParse(dt.Rows[j]["UserID"].ToString(), out AuthManagerId);

                    if (_loginID == AuthManagerId)
                    {
                        return AuthManagerId;

                    }
                }
                //if (dt.Rows.Count > 0)
                //{
                //    int.TryParse(dt.Rows[0]["UserID"].ToString(), out AuthManagerId);
                //}
            }
            return AuthManagerId;
        }

        public static bool IsAdminOrReception()
        {
            int _catID = SnehBLL.UserAccount_Bll.getCategory();
            if (_catID == 1) return true;
            if (_catID == 2) return true;
            return false;
        }

    }
}
