using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace snehrehab.Member
{
    public partial class Receiptiond : System.Web.UI.Page
    {
        int _loginID = 0; int receiptionid = 0;

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
                    receiptionid = SnehBLL.Receiption_Bll.Check(Request.QueryString["record"].ToString());
                }
            }
            if (receiptionid > 0)
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
            SnehBLL.Receiption_Bll RB = new SnehBLL.Receiption_Bll();
            int i = RB.Delete(receiptionid);
            if (i > 0)
            {
                Session[DbHelper.Configuration.messageTextSession] = "Receiptionist entry deleted successfully.";
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