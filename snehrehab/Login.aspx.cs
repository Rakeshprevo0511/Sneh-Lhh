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

public partial class Login : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SnehBLL.UserAccount_Bll.LogOut();
            txtCategory.Items.Clear(); txtCategory.Items.Add(new ListItem("Select Category", "-1"));
            SnehBLL.UserCategory_Bll UCB = new SnehBLL.UserCategory_Bll();
            foreach (SnehDLL.UserCategory_Dll UCD in UCB.GetList())
            {
                txtCategory.Items.Add(new ListItem(UCD.CategoryName, UCD.UserCatID.ToString()));
            }
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (txtUsername.Text.Trim().Length <= 0)
        {
            DbHelper.Configuration.setAlert(MsgPlace, "Enter your login user name.", 2); return;
        }
        if (txtPassword.Text.Length <= 0)
        {
            DbHelper.Configuration.setAlert(MsgPlace, "Enter your login password.", 2); return;
        }
        int _categoryID = 0; if (txtCategory.SelectedItem != null) { int.TryParse(txtCategory.SelectedItem.Value, out _categoryID); }
        if (_categoryID <= 0)
        {
            DbHelper.Configuration.setAlert(MsgPlace, "Select your login category.", 2); return;
        }

        string _token = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString();

        SnehBLL.UserAccount_Bll UAB = new SnehBLL.UserAccount_Bll();
        SnehDLL.UserAccount_Dll UAD = UAB.Login(txtUsername.Text.Trim(), txtPassword.Text, _categoryID);
        if (UAD != null)
        {
            if (UAD.IsEnabled)
            {
                
                SnehBLL.UserAccount_Bll.setLogin(UAD.UserID, UAD.FullName, UAD.LoginName, UAD.UserCatID);
                SnehBLL.UserAccount_Bll.setTokenID(_token);
                SnehBLL.UserActivity_Bll AC = new SnehBLL.UserActivity_Bll(); AC.LogIn(UAD.UserID, _token);
                Session["pwd_alert_show"] = 0;
                Session["bday_alert_show"] = 0;
                Session["reval_alert_show"] = 0;

                Response.Redirect(ResolveClientUrl("~/Member/"), true);
            }
            else
            {
                DbHelper.Configuration.setAlert(MsgPlace, "Your login name is blocked.", 2);
            }
        }
        else
        {
            DbHelper.Configuration.setAlert(MsgPlace, "Invalid user name or password.", 2);
        }
    }
}
