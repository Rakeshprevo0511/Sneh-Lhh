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

public partial class Member_UserAccd : System.Web.UI.Page
{
    int _loginID = 0; int _userID = 0;

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
                _userID = SnehBLL.UserAccount_Bll.Check(Request.QueryString["record"].ToString());
            }
        }
        if (_userID > 0)
        {
            if (!IsPostBack)
            {
                if (SnehBLL.UserAccount_Bll.IsMain(_userID))
                {
                    Session[DbHelper.Configuration.messageTextSession] = "Master user account account can not deleted.";
                    Session[DbHelper.Configuration.messageTypeSession] = "3";

                    Response.Redirect(ResolveClientUrl("~/Member/UserAccs.aspx"), true);
                }
            }
        }
        else
        {
            Response.Redirect(ResolveClientUrl("~/Member/UserAccs.aspx"), true);
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        SnehBLL.UserAccount_Bll DB = new SnehBLL.UserAccount_Bll();
        int i = DB.Delete(_userID);
        if (i > 0)
        {
            if (i == _loginID)
            {
                Response.Redirect(ResolveClientUrl("~/logout.ashx"), true);
            }
            Session[DbHelper.Configuration.messageTextSession] = "User account entry deleted successfully.";
            Session[DbHelper.Configuration.messageTypeSession] = "1";

            Response.Redirect(ResolveClientUrl("~/Member/UserAccs.aspx"), true);
        }
        else
        {
            DbHelper.Configuration.setAlert(this.Page, "Unable to process your request, please try again.", 2);
        }
    }
}
