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
using System.Globalization;

public partial class Member_UserAccs : System.Web.UI.Page
{
    int _loginID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        _loginID = SnehBLL.UserAccount_Bll.IsLogin();
        if (_loginID <= 0)
        {
            Response.Redirect(ResolveClientUrl(DbHelper.Configuration.SessionOutURL), true);
        }
        if (!IsPostBack)
        {            
            LoadData();            
        }
    }

    protected void AccountGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        AccountGV.PageIndex = e.NewPageIndex; LoadData();
    }

    private void LoadData()
    {
        SnehBLL.UserAccount_Bll DB = new SnehBLL.UserAccount_Bll();
        AccountGV.DataSource = DB.Search();
        AccountGV.DataBind();
        if (AccountGV.HeaderRow != null) { AccountGV.HeaderRow.TableSection = TableRowSection.TableHeader; }
        btn_create.Visible = false;
        AccountGV.Columns[7].Visible = false;
        if (_loginID == DbHelper.Configuration.managerLoginId)
        {
            btn_create.Visible = true; 
            AccountGV.Columns[7].Visible = true;
        }
    }  
}
