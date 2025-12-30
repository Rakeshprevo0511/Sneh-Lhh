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

public partial class Member_UserAcce : System.Web.UI.Page
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
                LoadData();
            }
        }
        else
        {
            Response.Redirect(ResolveClientUrl("~/Member/UserAccs.aspx"), true);
        }
    }

    private void LoadData()
    {
        SnehBLL.UserAccount_Bll UB = new SnehBLL.UserAccount_Bll();
        SnehDLL.UserAccount_Dll UD = UB.Get(_userID);
        if (UD != null)
        {
            txtFullName.Text = UD.FullName;
            txtMobileNo.Text = UD.MobileNo;
            txtMail.Text = UD.MailID;
            txtLoginName.Text = UD.LoginName;
            txtPassword.Text = UD.LoginPwd;
        }
        else
        {
            Session[DbHelper.Configuration.messageTextSession] = "User account detail not found.";
            Session[DbHelper.Configuration.messageTypeSession] = "2";

            Response.Redirect(ResolveClientUrl("~/Member/UserAccs.aspx"), true);
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (txtFullName.Text.Trim().Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter full name of account holder...", 2); return;
        }
        if (txtLoginName.Text.Trim().Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter account login name...", 2); return;
        }
        SnehBLL.UserAccount_Bll UB = new SnehBLL.UserAccount_Bll();
        if (UB.LoginChk(_userID, txtLoginName.Text.Trim()))
        {
            DbHelper.Configuration.setAlert(Page, "Enter login name is not available, please try another...", 2); return;
        }
        if (txtPassword.Text.Trim().Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter account password...", 2); return;
        }
        if (txtPassword.Text.Trim().Length <= 3)
        {
            DbHelper.Configuration.setAlert(Page, "Account password should be minimum 4 character...", 2); return;
        }
        SnehDLL.UserAccount_Dll UD = new SnehDLL.UserAccount_Dll();
        UD.UserID = _userID; UD.UniqueID = ""; UD.DoctorID = 0;
        UD.FullName = txtFullName.Text.Trim();
        UD.MobileNo = txtMobileNo.Text.Trim();
        UD.MailID = txtMail.Text.Trim();
        UD.LoginName = txtLoginName.Text.Trim();
        UD.LoginPwd = txtPassword.Text;
        UD.LastLogin = ""; UD.UserCatID = -1;
        UD.IsMain = false; UD.IsEnabled = true;
        UD.AddedDate = DateTime.UtcNow.AddMinutes(330);
        UD.ModifyDate = DateTime.UtcNow.AddMinutes(330);
        UD.AddedBy = _loginID; UD.ModifyBy = _loginID;

        int i = UB.Update(UD);
        if (i > 0)
        {
            Session[DbHelper.Configuration.messageTextSession] = "User account details saved successfully...";
            Session[DbHelper.Configuration.messageTypeSession] = "1";

            Response.Redirect(ResolveClientUrl("~/Member/UserAccs.aspx"), true);
        }
        else
        {
            DbHelper.Configuration.setAlert(Page, "Unable to process your request, please try again...", 2);
        }
    }
}
