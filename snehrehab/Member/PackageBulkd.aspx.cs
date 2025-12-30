using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace snehrehab.Member
{
    public partial class PackageBulkd : System.Web.UI.Page
    {
        int _loginID = 0; long BulkID = 0;
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
                    BulkID = SnehBLL.PatientBulk_Bll.Check(Request.QueryString["record"].ToString());
                }
            }
            if (BulkID > 0)
            {
                SnehBLL.PatientBulk_Bll DB = new SnehBLL.PatientBulk_Bll();
                if (DB.IsUsed(BulkID))
                {
                    Session[DbHelper.Configuration.messageTextSession] = "Patient bulk booking is already in use.";
                    Session[DbHelper.Configuration.messageTypeSession] = "3";

                    Response.Redirect(ResolveClientUrl("~/Member/PackageBulks.aspx"), true);
                }
            }
            else
            {
                Response.Redirect(ResolveClientUrl("~/Member/PackageBulks.aspx"), true);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            SnehBLL.PatientBulk_Bll DB = new SnehBLL.PatientBulk_Bll();
            long i = DB.Delete(BulkID);
            if (i > 0)
            {
                Session[DbHelper.Configuration.messageTextSession] = "Patient bulk booking entry deleted successfully.";
                Session[DbHelper.Configuration.messageTypeSession] = "1";

                Response.Redirect(ResolveClientUrl("~/Member/PackageBulks.aspx"), true);
            }
            else if (i == -10)
            {
                Session[DbHelper.Configuration.messageTextSession] = "Patient bulk booking is already in use.";
                Session[DbHelper.Configuration.messageTypeSession] = "3";

                Response.Redirect(ResolveClientUrl("~/Member/PackageBulks.aspx"), true);
            }
            else
            {
                DbHelper.Configuration.setAlert(this.Page, "Unable to process your request, please try again.", 2);
            }
        }
    }
}