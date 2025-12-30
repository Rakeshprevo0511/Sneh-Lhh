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

namespace snehrehab.Member
{
    public partial class LeaveDel : System.Web.UI.Page
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
                    
                }
            }
            else
            {
                Response.Redirect(ResolveClientUrl("~/Member/LeaveAll.aspx"), true);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            SnehBLL.LeaveApplications_Bll DB = new SnehBLL.LeaveApplications_Bll();
            int i = DB.Delete(_leaveID);
            if (i > 0)
            {
                Session[DbHelper.Configuration.messageTextSession] = "Leave application entry deleted successfully.";
                Session[DbHelper.Configuration.messageTypeSession] = "1";

                Response.Redirect(ResolveClientUrl("~/Member/LeaveAll.aspx"), true);
            }
            else
            {
                DbHelper.Configuration.setAlert(this.Page, "Unable to process your request, please try again.", 2);
            }
        }
    }
}
