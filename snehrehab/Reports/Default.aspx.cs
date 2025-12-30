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

public partial class Reports_Default : System.Web.UI.Page
{
    int _loginID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        _loginID = SnehBLL.UserAccount_Bll.IsLogin();
        if (_loginID <= 0)
        {
            Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
        }
        tb_manager.Visible = false; tb_reception.Visible = false; tb_admin.Visible = false;
        int _catID = SnehBLL.UserAccount_Bll.getCategory();
        if (_catID == 1 || _catID==6)
        {
            tb_manager.Visible = true;
        }
        else if (_catID == 2)
        {
            tb_reception.Visible = true;
        }
        else if (_catID == 4)
        {
            tb_admin.Visible = true;
        }
        else
        {
            Response.Redirect("~/Member/", true);
        }
    }
}
