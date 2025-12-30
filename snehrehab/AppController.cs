using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace snehrehab
{
    public class AppController : ApiController
    {
        private int UserCatID = 3;

        private sModel Token_Data(string token)
        {
            sModel s = null;
            if (!string.IsNullOrEmpty(token))
            {
                SqlCommand cmd = new SqlCommand("UserSession_Data"); cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TokenID", SqlDbType.VarChar, 250).Value = token;
                DbHelper.SqlDb db = new DbHelper.SqlDb(); DataTable dt = db.DbRead(cmd);
                if (dt.Rows.Count > 0)
                {
                    s = new sModel(); int i = 0;
                    s.TokenID = dt.Rows[i]["TokenID"].ToString();
                    int UserID = 0; int.TryParse(dt.Rows[i]["UserID"].ToString(), out UserID); s.UserID = UserID;
                    DateTime StartDate = new DateTime(); DateTime.TryParseExact(dt.Rows[i]["StartDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out StartDate);
                    s.StartDate = StartDate;
                    bool IsPwdReset = false; bool.TryParse(dt.Rows[i]["IsPwdReset"].ToString(), out IsPwdReset);
                    s.IsPwdReset = IsPwdReset;
                    bool IsForced = false; bool.TryParse(dt.Rows[i]["IsForced"].ToString(), out IsForced);
                    s.IsForced = IsForced;
                    int DoctorID = 0; int.TryParse(dt.Rows[i]["DoctorID"].ToString(), out DoctorID); s.DoctorID = DoctorID;
                    s.FullName = dt.Rows[i]["FullName"].ToString();
                    s.MailID = dt.Rows[i]["MailID"].ToString();
                    s.MobileNo = dt.Rows[i]["MobileNo"].ToString();
                    bool IsEnabled = false; bool.TryParse(dt.Rows[i]["IsEnabled"].ToString(), out IsEnabled);
                    s.IsEnabled = IsEnabled;
                }
            }
            return s;
        }

        private bool Token_Check(ref sModel SD)
        {
            bool isValid = true;
            if (SD == null)
            {
                SD = new sModel();
                isValid = false; SD.Msg = "Your session is expired, Please login again.";
            }
            else
            {
                if (SD.UserID <= 0)
                {
                    isValid = false; SD.Msg = "Your session is expired, Please login again.";
                }
                if (!SD.IsEnabled)
                {
                    isValid = false; SD.Msg = "Your account is blocked, Please contact to admin.";
                }
            }
            return isValid;
        }

        [System.Web.Http.HttpPost]
        public rModel login(JObject jObject)
        {
            rModel r = new rModel();
            try
            {
                string uname = (string)((dynamic)jObject).uname; if (string.IsNullOrEmpty(uname)) { uname = string.Empty; }
                string password = (string)((dynamic)jObject).password; if (string.IsNullOrEmpty(password)) { password = string.Empty; }
                string mobile_ip = (string)((dynamic)jObject).mobile_ip; if (string.IsNullOrEmpty(mobile_ip)) { mobile_ip = string.Empty; }
                string mobile_brand = (string)((dynamic)jObject).mobile_brand; if (string.IsNullOrEmpty(mobile_brand)) { mobile_brand = string.Empty; }
                string mobile_model = (string)((dynamic)jObject).mobile_model; if (string.IsNullOrEmpty(mobile_model)) { mobile_model = string.Empty; }
                string mobile_remark = (string)((dynamic)jObject).mobile_remark; if (string.IsNullOrEmpty(mobile_remark)) { mobile_remark = string.Empty; }

                SnehBLL.UserAccount_Bll UAB = new SnehBLL.UserAccount_Bll();
                SnehDLL.UserAccount_Dll UAD = UAB.Login(uname, password, UserCatID);
                if (UAD != null)
                {
                    if (UAD.IsEnabled)
                    {
                        string token = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString();
                        string _remark = "Logged In : Mobile App (" + mobile_ip + ") " + mobile_brand + " " + mobile_model + " " + mobile_remark;
                        SnehBLL.UserActivity_Bll AC = new SnehBLL.UserActivity_Bll(); int i = AC.LogIn(UAD.UserID, token, _remark);
                        if (i > 0)
                        {
                            r.status = true; r.msg = "Logged Successfully.";
                            r.data = new
                            {
                                full_name = UAD.FullName,
                                mail_id = UAD.MailID,
                                mobile_no = UAD.MobileNo,
                                last_login = UAD.LastLogin,
                                token = r.Encode(token),
                            };
                            return r;
                        }
                        else
                        {
                            r.msg = "Unable to process, Please try again."; return r;
                        }
                    }
                    else
                    {
                        r.msg = "Your account is blocked, Please contact to admin."; return r;
                    }
                }
                else
                {
                    r.msg = "Invalid username or password."; return r;
                }
            }
            catch (Exception ex)
            {
                r.msg = ex.Message;
            }
            return r;
        }

        [System.Web.Http.HttpPost]
        public rModel logout(JObject jObject)
        {
            rModel r = new rModel();
            try
            {
                sModel SD = Token_Data(r.Decode((string)((dynamic)jObject).token));
                if (!Token_Check(ref SD))
                {
                    r.msg = SD.Msg; return r;
                }
                string mobile_ip = (string)((dynamic)jObject).mobile_ip; if (string.IsNullOrEmpty(mobile_ip)) { mobile_ip = string.Empty; }
                string mobile_brand = (string)((dynamic)jObject).mobile_brand; if (string.IsNullOrEmpty(mobile_brand)) { mobile_brand = string.Empty; }
                string mobile_model = (string)((dynamic)jObject).mobile_model; if (string.IsNullOrEmpty(mobile_model)) { mobile_model = string.Empty; }
                string mobile_remark = (string)((dynamic)jObject).mobile_remark; if (string.IsNullOrEmpty(mobile_remark)) { mobile_remark = string.Empty; }
                string _remark = "Logged Out : Mobile App (" + mobile_ip + ") " + mobile_brand + " " + mobile_model + " " + mobile_remark;
                SnehBLL.UserActivity_Bll AB = new SnehBLL.UserActivity_Bll();
                AB.LogOut(SD.UserID, SD.TokenID, _remark);
                r.status = true; r.msg = "Logout Successfully";
            }
            catch (Exception ex)
            {
                r.msg = ex.Message;
            }
            return r;
        }
    }
}