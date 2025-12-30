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

public partial class Member_UserPwd : System.Web.UI.Page
{
    int _loginID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        _loginID = SnehBLL.UserAccount_Bll.IsLogin();
        if (_loginID <= 0)
        {
            Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (txtOld.Text.Trim().Length <= 0 || txtNew.Text.Trim().Length <= 0 || txtConfirm.Text.Trim().Length <= 0)
        {
            DbHelper.Configuration.setAlert(Page, "Please enter required fields...", 2); return;
        }
        if (txtNew.Text != txtConfirm.Text)
        {
            DbHelper.Configuration.setAlert(Page, "Confirm password mismatch...", 2); return;
        }
        SnehBLL.UserAccount_Bll UB = new SnehBLL.UserAccount_Bll();
        int i = UB.Password(_loginID, txtOld.Text, txtNew.Text);
        if (i > 0)
        {
            DbHelper.Configuration.setAlert(Page, "Your login password changed successfully...", 1);
        }
        else if (i == -1)
        {
            DbHelper.Configuration.setAlert(Page, "Enter correct old password and try again...", 2);
        }
        else
        {
            DbHelper.Configuration.setAlert(Page, "Unable to process your request, please try again...", 2);
        }
    }
}