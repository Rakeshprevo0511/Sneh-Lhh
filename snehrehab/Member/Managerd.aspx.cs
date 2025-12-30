using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace snehrehab.Member
{
    public partial class Managerd : System.Web.UI.Page
    {
        int _loginID = 0; int managerid = 0;
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
                    managerid = SnehBLL.Management_Bll.Check(Request.QueryString["record"].ToString());
                }
            }
            if (managerid > 0)
            {
                if (!IsPostBack)
                {

                }
            }
            else
            {
                Response.Redirect(ResolveClientUrl("~/Member/ViewList.aspx"), true);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            SnehBLL.Management_Bll RB = new SnehBLL.Management_Bll();
            int i = RB.Delete(managerid);
            if (i > 0)
            {
                Session[DbHelper.Configuration.messageTextSession] = "Management entry deleted successfully.";
                Session[DbHelper.Configuration.messageTypeSession] = "1";

                Response.Redirect(ResolveClientUrl("~/Member/ViewList.aspx"), true);
            }
            else
            {
                DbHelper.Configuration.setAlert(this.Page, "Unable to process your request, please try again.", 2);
            }
        }
    }
}