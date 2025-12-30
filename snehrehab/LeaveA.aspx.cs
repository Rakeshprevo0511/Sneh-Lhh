using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Globalization;

namespace snehrehab
{
    public partial class LeaveA : System.Web.UI.Page
    {
        int leaveID = 0; int userID = 0; string mail_id = string.Empty;
        public string fullName = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["data"] != null)
            {
                snehrehab.rModel r = new snehrehab.rModel();
                try
                {
                    dynamic data = JsonConvert.DeserializeObject(DbHelper.Configuration.Decrypt(r.Decode(HttpUtility.UrlDecode(Request.QueryString["data"].ToString()))));
                    string str = (string)data.leave_id;
                    if (DbHelper.Configuration.IsGuid(str))
                    {
                        leaveID = SnehBLL.LeaveApplications_Bll.Check(str);
                    }
                    userID = (int)data.account_id;
                    mail_id = (string)data.mail_id;
                }
                catch
                {
                }
            }
            if (leaveID > 0 && userID > 0 && !string.IsNullOrEmpty(mail_id))
            {
                SnehBLL.LeaveApplications_Bll LB = new SnehBLL.LeaveApplications_Bll();
                DataTable dt = LB.Get(leaveID);
                if (dt.Rows.Count > 0)
                {
                    int _userID = 0; int.TryParse(dt.Rows[0]["UserID"].ToString(), out _userID);
                    if (_userID == userID)
                    {
                        int LeaveStatus = 0; int.TryParse(dt.Rows[0]["LeaveStatus"].ToString(), out LeaveStatus);
                        fullName = dt.Rows[0]["FullName"].ToString();
                        if (LeaveStatus == 0)
                        { 
                            int i = LB.SetStatus(leaveID, 1, mail_id);
                            if (i > 0)
                            {
                                LB.ApproveMail(leaveID);
                                pnlSuccess.Visible = true;
                            }
                            else
                            {
                                pnlFailed.Visible = true;
                            }
                        }
                        else
                        {
                            if (LeaveStatus == 1)
                            {
                                pnlPlaced.Visible = true;
                            }
                            else if (LeaveStatus == 2)
                            {
                                pnlPlaced.Visible = true;
                            }
                            else
                            {
                                pnlInvalid.Visible = true;
                            }
                        }
                    }
                    else
                    {
                        pnlInvalid.Visible = true;
                    }
                }
                else
                {
                    pnlInvalid.Visible = true;
                }
            }
            else
            {
                pnlInvalid.Visible = true;
            }
        } 
    }
}