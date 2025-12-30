using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Globalization;

namespace snehrehab.Member
{
    public partial class LeaveEdit : System.Web.UI.Page
    {
        int _loginID = 0; int _leaveID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            _loginID = SnehBLL.UserAccount_Bll.IsLogin();
            if (_loginID <= 0)
            {
                Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
            }
            if (Request.QueryString["record"] != null)
            {
                if (DbHelper.Configuration.IsGuid(Request.QueryString["record"].ToString()))
                {
                    _leaveID = SnehBLL.LeaveApplications_Bll.Check(Request.QueryString["record"].ToString());
                }
            }
            if (_leaveID > 0)
            {
                if (!IsPostBack)
                {
                    SnehBLL.LeaveApplications_Bll LA = new SnehBLL.LeaveApplications_Bll();
                    DataTable LD = LA.Get(_leaveID);
                    if (LD.Rows.Count > 0)
                    {
                        if (LD.Rows[0]["LeaveStatus"].ToString() != "0")
                        {
                            Response.Redirect(ResolveClientUrl("~/Member/LeaveAll.aspx"), true); return;
                        }
                        txtFullName.Text = LD.Rows[0]["FullName"].ToString();
                        txtLeaveTypes.Text = LD.Rows[0]["TypeName"].ToString();
                        DateTime _fromDate = new DateTime(); DateTime.TryParseExact(LD.Rows[0]["HalfFromDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _fromDate);
                        DateTime _uptoDate = new DateTime(); DateTime.TryParseExact(LD.Rows[0]["HalfUptoDate"].ToString(), DbHelper.Configuration.dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _uptoDate);

                        if (LD.Rows[0]["TypeID"].ToString() == "4")
                        {
                            txtLeaveFrom.Text = _fromDate.ToString("dd/MM/yyyy hh:mm tt");
                            txtLeaveUpto.Text = _uptoDate.ToString("dd/MM/yyyy hh:mm tt");
                        }
                        else
                        {
                            txtLeaveFrom.Text = _fromDate.ToString("dd/MM/yyyy");
                            txtLeaveUpto.Text = _uptoDate.ToString("dd/MM/yyyy");
                        }
                        txtReason.Text = LD.Rows[0]["Reason"].ToString();
                    }
                    else
                    {
                        Session[DbHelper.Configuration.messageTextSession] = "Unable to find leave details, please try again...";
                        Session[DbHelper.Configuration.messageTypeSession] = "2";

                        Response.Redirect(ResolveClientUrl("~/Member/LeaveAll.aspx"), true);
                    }
                }
            }
            else
            {
                Response.Redirect(ResolveClientUrl("~/Member/LeaveAll.aspx"), true);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int _status = 0; if (txtAction.SelectedItem != null) { int.TryParse(txtAction.SelectedItem.Value, out _status); }
            if (_status <= 0)
            {
                DbHelper.Configuration.setAlert(Page, "Please select leave status...", 2); return;
            }
            SnehBLL.LeaveApplications_Bll LA = new SnehBLL.LeaveApplications_Bll();
            int i = LA.SetStatus(_leaveID, _status);
            if (i > 0)
            {
                if (_status == 1)
                {
                    LA.ApproveMail(_leaveID);
                    Session[DbHelper.Configuration.messageTextSession] = "Leave application approved sucessfully...";
                }
                if (_status == 2)
                {
                    Session[DbHelper.Configuration.messageTextSession] = "Leave application rejected sucessfully...";
                }
                Session[DbHelper.Configuration.messageTypeSession] = "1";

                Response.Redirect(ResolveClientUrl("~/Member/LeaveAll.aspx"), true);
            }
            else
            {
                DbHelper.Configuration.setAlert(Page, "Unable to process your request, please try again...", 2);
            }
        }

    
    }
}
