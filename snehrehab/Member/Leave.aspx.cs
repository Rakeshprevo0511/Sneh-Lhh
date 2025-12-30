using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Globalization;
using System.Data.SqlClient;
using System.Text;
using System.Web.Script.Serialization;

public partial class Member_Leave : System.Web.UI.Page
{
    int _loginID = 0; int _leaveID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        _loginID = SnehBLL.UserAccount_Bll.IsLogin();
        if (_loginID <= 0)
        {
            Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
        }
        if (!IsPostBack)
        {
            tb_FullDay.Visible = true; tb_HalfDay.Visible = false;

            SnehBLL.LeaveTypes_Bll LTB = new SnehBLL.LeaveTypes_Bll();
            txtLeaveTypes.Items.Clear(); txtLeaveTypes.Items.Add(new ListItem("Select Leave Type", "-1"));
            foreach (SnehDLL.LeaveTypes_Dll LTD in LTB.GetList())
            {
                txtLeaveTypes.Items.Add(new ListItem(LTD.TypeName, LTD.TypeID.ToString()));
            }

            SnehBLL.AppointmentTime_Bll ATB = new SnehBLL.AppointmentTime_Bll();
            txtFromTime.Items.Clear(); txtFromTime.Items.Add(new ListItem("Select Time", "-1"));
            txtUptoTime.Items.Clear(); txtUptoTime.Items.Add(new ListItem("Select Time", "-1"));
            foreach (SnehDLL.AppointmentTime_Dll ATD in ATB.GetList())
            {
                txtFromTime.Items.Add(new ListItem(ATD.TimeName, ATD.TimeID.ToString()));
                txtUptoTime.Items.Add(new ListItem(ATD.TimeName, ATD.TimeID.ToString()));
            }
        }
    }

    protected void txtLeaveTypes_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtBalance.Text = ""; txtFromDate.Text = ""; txtUptoDate.Text = ""; txtLeaveDate.Text = "";
        if (txtFromTime.Items.Count > 0) { txtFromTime.SelectedIndex = 0; }
        if (txtUptoTime.Items.Count > 0) { txtUptoTime.SelectedIndex = 0; }
        int _leaveType = 0; if (txtLeaveTypes.SelectedItem != null) { int.TryParse(txtLeaveTypes.SelectedItem.Value, out _leaveType); }

        if (_leaveType == 4)
        {
            tb_FullDay.Visible = false; tb_HalfDay.Visible = true;
        }
        else
        {
            tb_FullDay.Visible = true; tb_HalfDay.Visible = false;
        }
        LoadBalance(_leaveType);
    }

    private void LoadBalance(int _leaveType)
    {
        if (_leaveType != 4)
        {
            SnehBLL.LeaveApplications_Bll LAB = new SnehBLL.LeaveApplications_Bll();
            txtBalance.Text = LAB.Balance(_loginID, _leaveType).ToString();
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int _leaveType = 0; if (txtLeaveTypes.SelectedItem != null) { int.TryParse(txtLeaveTypes.SelectedItem.Value, out _leaveType); }
        if (_leaveType <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please select leave type...", 2); return;
        }
        DateTime _fromDate = new DateTime(); DateTime _uptoDate = new DateTime();
        TimeSpan _fromTime = new TimeSpan(); _fromTime = TimeSpan.MinValue;
        TimeSpan _uptoTime = new TimeSpan(); _uptoTime = TimeSpan.MinValue;
        
        if (_leaveType == 4)
        {
            if (txtLeaveDate.Text.Trim().Length <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please select leave date...", 2); return;
            }
            DateTime.TryParseExact(txtLeaveDate.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            if (_fromDate <= DateTime.MinValue)
            {
                DbHelper.Configuration.setAlert(Page, "Please select correct leave date...", 2); return;
            }
            if (_fromDate >= DateTime.MaxValue)
            {
                DbHelper.Configuration.setAlert(Page, "Please select correct leave date...", 2); return;
            }
            _uptoDate = _fromDate;

            int _fromTimeID = 0; if (txtFromTime.SelectedItem != null) { int.TryParse(txtFromTime.SelectedItem.Value, out _fromTimeID); }
            if (_fromTimeID <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please select half day time from...", 2); return;
            }
            SnehDLL.AppointmentTime_Dll ATD; SnehBLL.AppointmentTime_Bll ATB = new SnehBLL.AppointmentTime_Bll();
            ATD = ATB.Get(_fromTimeID);
            if (ATD != null)
            {
                _fromTime = ATD.TimeHour;
            }
            if (_fromTime <= TimeSpan.MinValue)
            {
                DbHelper.Configuration.setAlert(Page, "Please select correct half day time from...", 2); return;
            }
            int _uptoTimeID = 0; if (txtUptoTime.SelectedItem != null) { int.TryParse(txtUptoTime.SelectedItem.Value, out _uptoTimeID); }
            if (_uptoTimeID <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please select half day time upto...", 2); return;
            }
            ATD = ATB.Get(_uptoTimeID);
            if (ATD != null)
            {
                _uptoTime = ATD.TimeHour;
            }
            if (_uptoTime <= TimeSpan.MinValue)
            {
                DbHelper.Configuration.setAlert(Page, "Please select correct half day time upto...", 2); return;
            }
            if (_uptoTime < _fromTime)
            {
                DbHelper.Configuration.setAlert(Page, "Please select correct half day time...", 2); return;
            }
        }
        else
        {
            if (txtFromDate.Text.Trim().Length <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please select leave from date...", 2); return;
            }
            DateTime.TryParseExact(txtFromDate.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
            if (_fromDate <= DateTime.MinValue)
            {
                DbHelper.Configuration.setAlert(Page, "Please select correct leave from date...", 2); return;
            }
            if (_fromDate >= DateTime.MaxValue)
            {
                DbHelper.Configuration.setAlert(Page, "Please select correct leave from date...", 2); return;
            }

            if (txtUptoDate.Text.Trim().Length <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please select leave upto date...", 2); return;
            }
            DateTime.TryParseExact(txtUptoDate.Text.Trim(), DbHelper.Configuration.showDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);
            if (_uptoDate <= DateTime.MinValue)
            {
                DbHelper.Configuration.setAlert(Page, "Please select correct leave upto date...", 2); return;
            }
            if (_uptoDate >= DateTime.MaxValue)
            {
                DbHelper.Configuration.setAlert(Page, "Please select correct leave upto date...", 2); return;
            }

            if (_uptoDate < _fromDate)
            {
                DbHelper.Configuration.setAlert(Page, "Please select correct leave date...", 2); return;
            }
            TimeSpan ts = _uptoDate.Subtract(_fromDate);
            int days = ts.Days;
            int baldays=0;int.TryParse(txtBalance.Text, out baldays);
            if (days > baldays)
            {
                DbHelper.Configuration.setAlert(Page, "Balance leave is less...", 2); return;
            }
        }
        if (txtReason.Text.Trim().Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter reason for leave...", 2); return;
        }

        SnehDLL.LeaveApplications_Dll LAD = new SnehDLL.LeaveApplications_Dll();
        LAD.LeaveID = _leaveID;
        LAD.UniqueID = "";
        LAD.UserID = _loginID;
        LAD.FromDate = _fromDate;
        LAD.UptoDate = _uptoDate;
        LAD.FromTime = _fromTime;
        LAD.UptoTime = _uptoTime;
        LAD.TypeID = _leaveType;
        LAD.Reason = txtReason.Text.Trim();
        LAD.cAddress = txtAddress.Text.Trim();
        LAD.cNumber = txtNumber.Text.Trim();
        LAD.LeaveStatus = 0;
        LAD.StatusDate = DateTime.UtcNow.AddMinutes(330);
        LAD.AddedDate = DateTime.UtcNow.AddMinutes(330);
        LAD.ModifyDate = DateTime.UtcNow.AddMinutes(330);
        LAD.AddedBy = _loginID;
        LAD.ModifyBy = _loginID;

        SnehBLL.LeaveApplications_Bll LAB = new SnehBLL.LeaveApplications_Bll();
        int i = LAB.Set(LAD);
        if (i > 0)
        {
            SnehBLL.UserAccount_Bll UB = new SnehBLL.UserAccount_Bll();
            SnehDLL.UserAccount_Dll UD = UB.Get(_loginID);
            if (UD != null)
            {
                SqlCommand cmd = new SqlCommand("SELECT BranchName, LeaveMsgOn, LeaveMailOn, LeaveMailFt FROM SettingsMst;"); cmd.CommandType = CommandType.Text;
                DbHelper.SqlDb db = new DbHelper.SqlDb(); DataTable dt = db.DbRead(cmd);
                if (dt.Rows.Count > 0)
                {
                    string _leaveTypeName = txtLeaveTypes.SelectedItem.Text.Trim();
                    int _days = (_uptoDate - _fromDate).Days + 1;
                    string _date = " on date ";
                    string _time = ""; string _uptodate = "";
                    if (_leaveType == 4) { _time = " " + txtFromTime.SelectedItem.Text + "-" + txtUptoTime.SelectedItem.Text; }
                    else if (_days > 1) { _date = " from date "; _uptodate = " to " + _uptoDate.ToString(DbHelper.Configuration.showDateFormat); }
                    #region LEAVE MESSAGE
                    try
                    {
                        string[] mobileNos = dt.Rows[0]["LeaveMsgOn"].ToString().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        string _messageStr = "Leave Application of " + UD.FullName.Trim() + " " + _leaveTypeName + " for " + _days.ToString() + " days" + _date + _fromDate.ToString(DbHelper.Configuration.showDateFormat) + _time + _uptodate +
                            ". " + dt.Rows[0]["BranchName"].ToString().Trim() + " Center ";
                        if (mobileNos.Length > 0)
                        {
                            SnehBLL.ApiSms_Bll SMSB = new SnehBLL.ApiSms_Bll();
                            SMSB.Send(mobileNos, _messageStr);
                        }
                    }
                    catch
                    {
                    }
                    #endregion
                    #region LEAVE EMAIL
                    try
                    {
                        string siteUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/');
                        string[] mailIds = dt.Rows[0]["LeaveMailOn"].ToString().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        StringBuilder _mailStr = new StringBuilder("<div style=\"width:100%;max-width:600px;margin:0 auto;/*box-shadow: 0px 0px 3px 3px #39b3ca;*/border: 3px solid #39b3ca;font-family:Trebuchet MS;color: #636363;font-size: 15px;\">" +
                                                                    "<table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:100%;line-height: 28px;\">" +
                                                                        "<tr>" +
                                                                            "<td align=\"center\" style=\"padding:10px;background:#FFF;\">" +
                                                                                "<img src=\"" + siteUrl + "/images/snehlogin.png\" alt=\"\" style=\"max-width:339px;max-height:85px;\"/>" +
                                                                            "</td>" +
                                                                        "</tr>" +
                                                                        "<tr>" +
                                                                            "<td>" +
                                                                                "<hr style=\"border: 0px;border-bottom: 1px solid #e6e6e6;\"/>" +
                                                                            "</td>" +
                                                                        "</tr>" +
                                                                        "<tr>" +
                                                                            "<td align=\"justify\" style=\"padding: 10px; background: #FFF;\">" +
                                                                                "<p style=\"margin-bottom: 0px;\">" +
                                                                                   "Leave Application of <b>" + UD.FullName.Trim() + "</b>,<br />" + _leaveTypeName + " for " + _days.ToString() + " days" + _date + _fromDate.ToString(DbHelper.Configuration.showDateFormat) + _time + _uptodate + ". " +
                                                                                "</p>" +
                                                                            "</td>" +
                                                                        "</tr>" +
                                                                        "<tr>" +
                                                                           "<td style=\"padding: 10px; background: #FFF;text-align:justify;\"> " +
                                                                               "<div><b>Leave Reason : </b>" + txtReason.Text.Trim() + "</div>" +
                                                                               "<div><b>Contact Address : </b>" + txtAddress.Text.Trim() + "</div>" +
                                                                               "<div><b>Contact Number : </b>" + txtNumber.Text.Trim() + "</div>" +
                                                                            "</td>" +
                                                                        "</tr>" +
                                                                        "<tr>" +
                                                                           "<td  align=\"center\" style=\"padding: 10px; background: #FFF;\"> " +
                                                                                "<a href=\"((APPROVE_LINK_URL))\" style=\"display: inline-block; color: white; padding: 5px 25px;border-radius: 4px; text-decoration: none; background: #71bc37; border: 1px solid #428211;\">Approve</a>" +
                                                                                "&nbsp;&nbsp;&nbsp;" +
                                                                                "<a href=\"((REJECT_LINK_URL))\" style=\"display: inline-block; color: white; padding: 5px 25px;border-radius: 4px; text-decoration: none; background: #fb6b6b; border: 1px solid #f13737;\">Reject</a>" +
                                                                            "</td>" +
                                                                        "</tr>" +
                                                                        "<tr>" +
                                                                            "<td  align=\"justify\" style=\"padding: 10px; background: #FFF;\"> " +
                                                                                "<br />" +
                                                                                "Regards,<br />" +
                                                                                dt.Rows[0]["BranchName"].ToString().Trim() + " Center." +
                                                                                "<br /><br />" +
                                                                            "</td>" +
                                                                        "</tr>" +
                                                                        "<tr>" +
                                                                            "<td style=\"background: #000; padding: 15px; text-align: center;color: #FFF;line-height: 23px;font-size: 13px;\">" +
                                                                                 dt.Rows[0]["LeaveMailFt"].ToString().Trim().Replace(Environment.NewLine, "<br>") +
                                                                            "</td>" +
                                                                        "</tr>" +
                                                                    "</table>" +
                                                                "</div>" +
                                                                "<br />");
                        if (mailIds.Length > 0)
                        {
                            SnehBLL.ApiMail_Bll MALB = new SnehBLL.ApiMail_Bll(); snehrehab.rModel r = new snehrehab.rModel();
                            string leave_id = SnehBLL.LeaveApplications_Bll.Check(i);
                            foreach (string item in mailIds)
                            {
                                string lData = DbHelper.Configuration.Encrypt(new JavaScriptSerializer().Serialize(new
                                {
                                    leave_id = leave_id,
                                    account_id = UD.UserID,
                                    mail_id = item.Trim(),
                                }));
                                lData = HttpUtility.UrlEncode(r.Encode(lData));
                                string approveLink = siteUrl + "/LeaveA.aspx?data=" + lData;
                                string rejectLink = siteUrl + "/LeaveR.aspx?data=" + lData;
                                MALB.send(item.Trim(),
                                    _mailStr.Replace("((APPROVE_LINK_URL))", approveLink)
                                    .Replace("((REJECT_LINK_URL))", rejectLink).ToString(),
                                    "Leave Application");
                            }
                        }
                    }
                    catch
                    {
                    }
                    #endregion
                }
            }

            Session[DbHelper.Configuration.messageTextSession] = "Leave request submitted successfully..";
            Session[DbHelper.Configuration.messageTypeSession] = "1";
            Response.Redirect(ResolveClientUrl("~/Member/Leave.aspx"), true);
        }
        else
        {
            DbHelper.Configuration.setAlert(Page, "Unable to process your request, please try again...", 2);
        }
    }
}
